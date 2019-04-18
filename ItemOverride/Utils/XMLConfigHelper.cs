using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Collections.Generic;
using ODebug;
using ConfigData;

namespace ConfigHelpers
{
    public class XMLConfigHelper
    {
        // Default path is Outward/Config
        private string basePath = Path.Combine(Directory.GetCurrentDirectory(), "Config");
        private string configName;
        // Default mode is read only
        private ConfigModes mode = ConfigModes.ReadOnly;
        private XmlDocument configDoc;
        private string xmlConfigDefault;
        private string configDirectory = "\\Config";
        private bool m_hasConfigsBeenLoaded = false;
        private List<OutwardItemOverrides> m_OutwardItemOverrides;
        private FileSystemWatcher directoryWatcher = new FileSystemWatcher();
        private FileSystemWatcher fileWatcher = new FileSystemWatcher();
        private XmlSerializerNamespaces xns = new XmlSerializerNamespaces();

        #region Constructors
        public XMLConfigHelper() {
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }

        public XMLConfigHelper(ConfigModes mode)
        {
            this.mode = mode;
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }

        public XMLConfigHelper(ConfigModes mode, string configName)
        {
            this.mode = mode;
            this.configName = configName;
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }
        public XMLConfigHelper(string configName)
        {
            this.configName = configName;
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }

        public XMLConfigHelper(string configName, string basePath)
        {
            this.configName = configName;
            this.basePath = basePath;
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }

        public XMLConfigHelper(ConfigModes mode, string configName, string basePath)
        {
            this.mode = mode;
            this.basePath = basePath;
            this.configName = configName;
            this.m_OutwardItemOverrides = new List<OutwardItemOverrides>();
        }

        #endregion

        public void Init()
        {
            try {
                // Create the config directory if it doesn't exist
                if (!Directory.Exists(basePath))
                {
                    // File system operations are slow, so we can't guarantee that the directory will be created 
                    // before we try to create the file

                    directoryWatcher.Path = Directory.GetCurrentDirectory();
                    directoryWatcher.Created += WatcherCreated;
                    directoryWatcher.EnableRaisingEvents = true;

                    Directory.CreateDirectory(basePath);
                    return;
                }
                else
                {
                    directoryWatcher.Created -= WatcherCreated;
                }

                configDoc = new XmlDocument();

                //configDoc.Load(FullPath);
                // parse all the xml and return them

                var directoryInfo = new DirectoryInfo(basePath);
                var fileInfo = directoryInfo.GetFiles();
                foreach (FileInfo fInfo in fileInfo)
                {
                    var filename = Path.GetFileName(fInfo.FullName);
                    var fullPath = Path.Combine(basePath, filename);
                    m_OutwardItemOverrides.Add(ParseXml<OutwardItemOverrides>(fullPath));
                }

                if (m_OutwardItemOverrides.Count > 0)
                {
                    m_hasConfigsBeenLoaded = true;
                } else
                {
                    m_hasConfigsBeenLoaded = false;
                }
            }
            catch(DirectoryNotFoundException e)
            {
                OLogger.Log("FileNotFoundException: " + e.ToString());
                throw new FileNotFoundException("Can't find config folder: " + basePath);
                
            }
            catch(XmlException e)
            {
                OLogger.Log("XmlException :/");
                // Do nothing, which is bad practice
            }
            catch (Exception e)
            {           
                // Prob a bad idea, but kept for now
                OLogger.Log("Exception called :( ");
                OLogger.Log(e.Message);
                OLogger.Log(e.ToString());
                throw e;
            }
        }

        public List<OutwardItemOverrides> GetConfigData()
        {
            if (m_hasConfigsBeenLoaded)
            {
                return m_OutwardItemOverrides;
            } else
            {
                return null;
            }
        }

        private void WatcherCreated(object sender, FileSystemEventArgs e)
        {
            // Wait for the directory we want to create to actually be created
            if ((configDirectory.Contains(e.Name) || e.Name.Contains(configDirectory) || e.Name.Equals(configDirectory)))
                //configName.Contains(e.Name) || e.Name.Contains(configName) || e.Name.Equals(configName))
                // When that happens, re-call Init, since it will skip the creating this time
                Init();
        }

        public float ReadFloat(string xpath)
        {
            return float.Parse(ReadString(xpath));
        }

        public int ReadInt(string xpath)
        {
            return int.Parse(ReadString(xpath));
        }

        public string ReadString(string xpath)
        {
            XmlNode curNode = ReadNode(xpath);
            if (curNode == null)
                return null;
            return ReadNode(xpath).InnerText;
        }

        public XmlNode ReadNode(string xpath)
        {
            // If Init() hasn't been called by the time we try to read a value, call it
            if (configDoc == null)
                Init();
            return configDoc.SelectSingleNode(xpath);
        }

        public void WriteValue(string xpath, string value)
        {
            WriteToXml(xpath, value);
        }

        public void WriteToXml(string xpath, string value)
        {
            if (configDoc == null)
                Init();

            // Break up the path into parts
            string[] paths;
            if (xpath.Contains("/"))
                paths = xpath.Split('/');
            else
                paths = new string[] { xpath };

            XmlNode curNode = configDoc;
            XPathNavigator nav = configDoc.CreateNavigator();
            foreach(string s in paths)
            {
                // Skip blank paths
                if (s.Equals(""))
                    continue;

                // Try to move down the hierarchy, if we can't then create the nodes
                bool moveSuccess = nav.MoveToChild(s, "");
                if (!moveSuccess)
                {
                    XmlWriter pages = nav.AppendChild();
                    pages.WriteElementString(s, "");
                    pages.Close();
                    // Actually move to the new node
                    nav.MoveToChild(s, "");
                }
            }
            // Set the value at the end
            nav.SetValue(value);
            
            // Save the resulting changes
            configDoc.Save(FullPath);
        }

        /// <summary>
        /// Write the default xml to the config file, if specified.
        /// This allows easy storing of defaults, so users don't have to go look up 
        /// what the config file looks like.
        /// </summary>
        /// <param name="locBasePath"></param>
        /// <param name="configName"></param>
        private void CreateDefault()
        {
            try
            {
                // This is okay to do because we never get here unless the config file doesn't exist
                File.WriteAllText(Path.Combine(basePath, "default.xml"), xmlConfigDefault);
            }
            catch(Exception e)
            {
                // This is bad practice, but I want to make sure the exception gets passed up to the individual mod
                // so people don't think it's the API's fault.
                throw e;
            }
        }

        // Accessors
        public string XMLDefaultConfig
        {
            get { return xmlConfigDefault; }
            set { this.xmlConfigDefault = value; }
        }
        public string FullPath
        {
            get { return Path.Combine(basePath, configName); }
        }

        public ConfigModes Mode
        {
            get { return mode; }
        }
        /// <summary>
        /// Expose the actual XmlDocument to allow for people to implement their own methods if need be.
        /// </summary>
        public XmlDocument ConfigDoc
        {
            get { return configDoc; }
        }

        public enum ConfigModes
        {
            ReadOnly,
            CreateIfMissing
        }

        public T ParseXml<T>(string fullPath) where T : class
        {
            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // If the XML document has been altered with unknown
            // nodes or attributes, handles them with the
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(fullPath, FileMode.Open);
            // Declares an object variable of the type to be deserialized.
            T po = null;
            try
            {
                
                // Uses the Deserialize method to restore the object's state
                // with data from the XML document. */
                po = (T)serializer.Deserialize(fs);
            } catch (InvalidOperationException e)
            {
                OLogger.Log("Malformed xml elements or attributes or other error.");
                OLogger.Log(e.ToString());
                OLogger.Log("Inner Exception: " + e.InnerException.ToString());
            }
            fs.Close();
            return po;
        }

        private void CreateDefaultOverrides()
        {
            try
            {
                // Creates an instance of the XmlSerializer class;
                // specifies the type of object to serialize.
                XmlSerializer serializer =
                new XmlSerializer(typeof(OutwardItemOverrides));
                TextWriter writer = new StreamWriter(Path.Combine(basePath, "Default.xml"));
                OutwardItemOverrides po = new OutwardItemOverrides();


                // Creates an ItemOverride.
                ItemOverride i1 = new ItemOverride();
                i1.ItemID = 2000090;             

                // Creates an WeaponOverrideData
                WeaponOverrideData o1 = new WeaponOverrideData();
                o1.OverrideType = OverrideType.WEAPON;
                o1.WeaponStatType = WeaponStatType.DAMAGE;
                o1.DmgType = DamageType.Types.Fire;
                o1.Value = 200;

                // Creates an ItemOverrideData
                ItemOverrideData o2 = new ItemOverrideData();
                o2.OverrideType = OverrideType.ITEMSTAT;
                o2.ItemStatType = ItemStatType.MAXDURABILITY;
                o2.Value = 25.0f;

                // Add them to the list of overrides
                i1.Data = new List<OverrideData>();
                i1.Data.Add(o1);
                i1.Data.Add(o2);

                // Inserts the item into the array.
                po.ItemOverrides = i1;

                // Set DebugMode to default OFF
                po.DebugMode = false;

                // Serializes the WeaponOverrideData, and closes the TextWriter.
                serializer.Serialize(writer, po);
                writer.Close();
            } catch (Exception e)
            {
                throw e;
            }
        }
        protected void serializer_UnknownNode
         (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        protected void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}

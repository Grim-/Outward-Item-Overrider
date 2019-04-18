using Partiality.Modloader;
using UnityEngine;
using ODebug;
using ConfigHelpers;
using ConfigData;
using Harmony;
using System.Collections.Generic;
using System;

namespace ItemOverrideMod
{
    public class ItemOverrider : PartialityMod
    {

        public ItemOverrider()
        {
            this.ModID = "Outward Item Overrider";
            this.Version = "0110";
            this.author = "Emo, r4cken";
        }

        public static APILoad APILoader;

        List<OutwardItemOverrides> outwardItemOverrides;
        XMLConfigHelper xmlConfigHelper;

        public override void Init()
        {
            base.Init();
            OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default", true, true);
            //OLogger.Log("OnInit()");
            //FileLog.Log("OnInit()");
            outwardItemOverrides = new List<OutwardItemOverrides>();
            xmlConfigHelper = new XMLConfigHelper();

            // Load our configuration files
            try
            {
                xmlConfigHelper.Init();
                //OLogger.Log("xmlConfig Init called");
                //FileLog.Log("xmlConfig Init called");
                // Deserialize our XML to C# classes and objects directly, the parameter is the XMLRootAttribute, aka Document Root.
                outwardItemOverrides = xmlConfigHelper.GetConfigData();

                // Reads the order date.
                if (outwardItemOverrides.Count > 0)
                {
                    OLogger.Log("Sucessfully read the configuration file contents.");
                    FileLog.Log("Sucessfully read the configuration file contents.");
                }
                else
                {
                    OLogger.Log("Failed to read the configuration file contents.");
                    FileLog.Log("Failed to read the configuration file contents.");
                }

            }
            catch (Exception e)
            {
                // Prob a bad idea, but kept for now
                OLogger.Log("Exception called :( ");
                FileLog.Log("Exception called :( ");
                OLogger.Log(e.Message);
                FileLog.Log(e.Message);
                OLogger.Log(e.ToString());
                FileLog.Log(e.ToString());
                throw e;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();

            APILoad.api = this;

            GameObject obj = new GameObject();
            APILoader = obj.AddComponent<APILoad>();
            //FileLog.Log("OnEnable()");
            //OLogger.Log("OnEnable()");

            if (outwardItemOverrides.Count > 0)
            {
                APILoader.Initialise(outwardItemOverrides);
            } else
            {
                OLogger.Log("There was an error reading the configuration files, cant patch anything. Exiting");
                FileLog.Log("There was an error reading the configuration files, cant patch anything. Exiting");
            }
        }

        public override void OnLoad()
        {
            base.OnLoad();
            //NOTE: The in-game logger window will not be created and displayed unless we instantiate it in here...
           
            OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default", true, true);
            //OLogger.Log("OnLoad()");
            //FileLog.Log("OnLoad()");
        }
    }
}

using Partiality.Modloader;
using UnityEngine;
using ODebug;
using ConfigHelpers;
using ConfigData;
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

        public override void OnEnable()
        {
            base.OnEnable();
            APILoad.api = this;
            GameObject obj = new GameObject();
            APILoader = obj.AddComponent<APILoad>();

            //Used to set the parent of all debug boxes to obj
            OLogger.SetupObject(obj);

            // Load our configuration file
            XMLConfigHelper xmlConfigHelper = new XMLConfigHelper(XMLConfigHelper.ConfigModes.CreateIfMissing, "ItemOverrides.xml");
            xmlConfigHelper.Init();

            // Deserialize our XML to C# classes and objects directly, the parameter is the XMLRootAttribute, aka Document Root.
            OutwardItemOverrides outwardItemOverrides;
            try
            {
                outwardItemOverrides = xmlConfigHelper.ParseXml<OutwardItemOverrides>();
                OLogger.SetDebug(outwardItemOverrides.DebugMode);
                OLogger.SetUIPanelEnabled("Default", outwardItemOverrides.DebugMode);
                // Reads the order date.
                OLogger.Log("Sucessfully read the configuration file contents.");

            } catch (Exception e)
            {
                // Prob a bad idea, but kept for now
                throw e;
            }

            APILoader.Initialise(outwardItemOverrides);
        }

        public override void OnLoad()
        {
            base.OnLoad();
            //NOTE: The in-game logger window will not be created and displayed unless we instantiate it in here...
            OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default", true, true);            
        }
    }
}

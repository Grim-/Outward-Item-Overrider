using Partiality.Modloader;
using UnityEngine;
using ODebug;
using ConfigHelpers;
using ConfigData;
using System;
using System.Collections.Generic;

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
            XMLConfigHelper xmlConfigHelper = new XMLConfigHelper();
            OLogger.Log("xmlConfig ctor called");
            List<OutwardItemOverrides> outwardItemOverrides = new List<OutwardItemOverrides>();
            try
            {
                xmlConfigHelper.Init();
                OLogger.Log("xmlConfig Init called");
                // Deserialize our XML to C# classes and objects directly, the parameter is the XMLRootAttribute, aka Document Root.
                outwardItemOverrides = xmlConfigHelper.GetConfigData();

                OLogger.SetDebug(outwardItemOverrides.FirstOrDefault().DebugMode);
                OLogger.SetUIPanelEnabled("Default", outwardItemOverrides.FirstOrDefault().DebugMode);
                // Reads the order date.
                OLogger.Log("Sucessfully read the configuration file contents.");

            } catch (Exception e)
            {
                // Prob a bad idea, but kept for now
                OLogger.Log("Exception called :( ");
                OLogger.Log(e.Message);
                OLogger.Log(e.ToString());
                throw e;
            }

            APILoader.Initialise(outwardItemOverrides);
        }

        public override void OnLoad()
        {
            base.OnLoad();
            //NOTE: The in-game logger window will not be created and displayed unless we instantiate it in here...
            //OLogger.SetDebug(true);
            OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default", true, true);            
        }
    }
}

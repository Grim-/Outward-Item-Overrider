using Partiality.Modloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SimpleJSON;

namespace OutwardItemOverrider
{
    public class OutwardItemOverrider : PartialityMod
    {
        public static ScriptLoad script;

        private static string overrideDir = "Mods/Overrides/";

        public string[] fileNames;
        public List<ItemOverride> variablesToOverride = new List<ItemOverride>();


        public OutwardItemOverrider()
        {
            this.ModID = "Outward Item Overrider";
            this.Version = "0.1";
            this.author = "Author";
        }

        public override void OnEnable()
        {
            base.OnEnable();

            GameObject go = new GameObject();
            script = go.AddComponent<ScriptLoad>();

            script.outwardOverrider = this;

            script.Initialise();
            CheckOverrideFolder();
        }

        public override void OnDisable()
        {
            base.OnDisable();  
        }


        public void CheckOverrideFolder()
        {
            var dirInfo = new DirectoryInfo(overrideDir);
            var fileInfo = dirInfo.GetFiles();
            fileNames = new string[fileInfo.Length];


            Debug.Log("::: FILES HERE :::");
            for (int i = 0; i < fileInfo.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(fileInfo[i].FullName);
                fileNames[i] = fileName;
                Debug.Log("Attempting to Load JSON for " + fileName);
                Debug.Log("Attempting to Load JSON for " + fileInfo[i].Name);
                LoadOverrides(fileInfo[i].FullName);
            }
        }

        public void LoadOverrides(string fileLocation)
        {
            //Debug.Log(":: Loading Config ::");
            string json = File.ReadAllText(fileLocation);
            //RootObject itemOverrideInfo = JsonUtility.FromJson<RootObject>(json);

            var test = JSON.Parse(json);
            var itemID = test["itemID"];
            var _overrideType = (ItemOverrideType)Enum.Parse(typeof(ItemOverrideType), test["type"], true);

            for (int i = 0; i < test["overrides"].Count; i++)
            {
                var statOverride = test["overrides"][i];
                
               // Debug.Log(_overrideType);

                switch (_overrideType)
                {
                    case ItemOverrideType.NONE:
                        break;
                    case ItemOverrideType.WEAPON:
                        WeaponOverride overid = new WeaponOverride();

                        overid.itemID = itemID;
                        overid.overrideType = _overrideType;
                        overid.weaponStatType = (WeaponStatType)Enum.Parse(typeof(WeaponStatType), statOverride["weapon_stat_type"], true);
                        overid.stat = statOverride["stat"];
                        overid.value = statOverride["value"];

                        if (statOverride["weapon_damage_type"])
                        {
                            overid.weaponDamageType = (WeaponDamageType)Enum.Parse(typeof(WeaponDamageType), statOverride["weapon_damage_type"], true);
                        }

                        variablesToOverride.Add(overid);
                        break;
                    case ItemOverrideType.ARMOUR:
                        break;
                    case ItemOverrideType.SPELL:
                        break;
                    case ItemOverrideType.BAG:
                        break;
                }
                
            };          
        }

    }

    [Serializable]
    public class ItemOverride
    {
        public int itemID;
        public ItemOverrideType overrideType;
        public string stat;
        public string value_type;
        public float value;
    }

    public class WeaponOverride : ItemOverride
    {
        public WeaponStatType weaponStatType;
        public WeaponDamageType weaponDamageType;
    }

    public enum ItemOverrideType
    {
        NONE,
        WEAPON,
        ARMOUR,
        SPELL,
        BAG
    }

    public enum WeaponStatType
    {
        NONE,
        DAMAGE,
        IMPACT,
        STAMINA_COST,
        REACH,
        SPEED,
        HEALTH_BONUS,
        POUCH_BONUS,
        HEAT_PROTECTION,
        COLD_PROTECTION,
        IMPACT_PROTECTION,
        CORRUPTION_PROTECTION,
        WATER_PROOF,
        MOVEMENT_PENALTY,
        STAMINA_USE_PENALTY,
        HEAT_REGEN_PENALTY,
        MANA_USE_MODIFIER
    }

    public enum WeaponDamageType
    {
        Physical,
        Ethereal,
        Decay,
        Electric,
        Frost,
        Fire
    }
}

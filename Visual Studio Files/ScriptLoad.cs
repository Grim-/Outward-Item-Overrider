using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using On;
using UnityEngine;
using System.Reflection;
using static WeaponStats;

namespace OutwardItemOverrider
{
    public class ScriptLoad : MonoBehaviour
    {
        public OutwardItemOverrider outwardOverrider;

        private Dictionary<ItemOverride, Item> itemsToOverride = new Dictionary<ItemOverride, Item>();

        public void Initialise()
        {
            Patch();
        }

        public void Patch()
        {
            On.ResourcesPrefabManager.LoadItemPrefabs += new On.ResourcesPrefabManager.hook_LoadItemPrefabs(itemsLoadedHook);
        }

        private void itemsLoadedHook(On.ResourcesPrefabManager.orig_LoadItemPrefabs orig, ResourcesPrefabManager self)
        {
            orig(self);


            CheckVariablesToOverride();


            if (itemsToOverride.Count > 0)
            {

                foreach (var item in itemsToOverride)
                {
                    // parse the field or variable that wants to be changed, get the reference to whatever 
                    ParseItemOverride(item.Value, item.Key);
                }
               
            }

        }

        private void ParseItemOverride(Item itemToChange, ItemOverride itemOverride)
        {
            var overrideType = itemOverride.overrideType;



            switch (itemOverride.overrideType)
            {
                case ItemOverrideType.NONE:
                    break;
                case ItemOverrideType.WEAPON:
                    ParseWeaponChanges(itemToChange, itemOverride);              
                    break;
                case ItemOverrideType.ARMOUR:
                    break;
                case ItemOverrideType.SPELL:
                    break;
                case ItemOverrideType.BAG:
                    break;
            }
        }


        private void ParseWeaponChanges(Item itemToChange, ItemOverride itemOverride)
        {
            var weaponOverride = (WeaponOverride) itemOverride;
            var weaponStatComponent = itemToChange.GetComponent<WeaponStats>();

            Debug.Log("Parsing Weapon Changes for Item ID : " + itemOverride.itemID);

            switch (weaponOverride.weaponStatType)
            {
                case WeaponStatType.NONE:
                    break;
                case WeaponStatType.DAMAGE:
                    Debug.Log("Updating Damage Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);

                    ParseWeaponDamageChanges(itemToChange, itemOverride);
                    //weaponStatComponent.Impact = itemOverride.value;
                    break;
                case WeaponStatType.IMPACT:

                    Debug.Log("Updating Impact Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    weaponStatComponent.Impact = itemOverride.value;

                    break;
                case WeaponStatType.STAMINA_COST:
                    Debug.Log("Updating Stamina Cost Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    weaponStatComponent.StamCost = itemOverride.value;
                    break;
                case WeaponStatType.REACH:
                    Debug.Log("Updating Weapon Reach Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    weaponStatComponent.Reach = itemOverride.value;
                    break;
                case WeaponStatType.SPEED:
                    Debug.Log("Updating Weapon Attack Speed Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    weaponStatComponent.AttackSpeed = itemOverride.value;
                    break;
                case WeaponStatType.HEALTH_BONUS:

                    Debug.Log("Updating Weapon Health Bonus Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    var hpBonus = ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus");
                    Debug.Log("Max HP bonus " + hpBonus);
                    ReflectionSetValue(itemOverride.value, typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus" );

                    break;
                case WeaponStatType.POUCH_BONUS:

                    Debug.Log("Updating Weapon Pouch Bonus Stat for Item ID : " + itemOverride.itemID + " to " + itemOverride.value);
                    var pouchBonus = ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_pouchCapacityBonus");
                    Debug.Log("Pouch Bonus " + pouchBonus);
                    ReflectionSetValue(itemOverride.value, typeof(EquipmentStats), weaponStatComponent, "m_pouchCapacityBonus");

                    break;
                case WeaponStatType.HEAT_PROTECTION:
                    break;
                case WeaponStatType.COLD_PROTECTION:
                    break;
                case WeaponStatType.IMPACT_PROTECTION:
                    break;
                case WeaponStatType.CORRUPTION_PROTECTION:
                    break;
                case WeaponStatType.WATER_PROOF:
                    break;
                case WeaponStatType.MOVEMENT_PENALTY:
                    break;
                case WeaponStatType.STAMINA_USE_PENALTY:
                    break;
                case WeaponStatType.HEAT_REGEN_PENALTY:
                    break;
                case WeaponStatType.MANA_USE_MODIFIER:
                    break;
                default:
                    break;
            }

            Debug.Log("Parsing Complete");
        }

        public void ParseWeaponDamageChanges(Item itemToChange, ItemOverride itemOverride)
        {

            Debug.Log("Updating Weapon Damage");
            var weaponOverride = (WeaponOverride)itemOverride;
            var weaponStatComponent = itemToChange.GetComponent<WeaponStats>();
            var damageType = weaponOverride.weaponDamageType;

            ModifyWeaponDamage(itemToChange, damageType, itemOverride.value);

        }

        public void ModifyWeaponDamage(Item item, DamageType.Types weaponDamageType, float damageAmount)
        {
            WeaponStats weaponStatComponent = item.GetComponent<WeaponStats>();
            DamageType damageType = new DamageType(weaponDamageType, damageAmount);
            weaponStatComponent.BaseDamage.Add(damageType);
            SetAttackStepDamage(weaponStatComponent.Attacks, damageAmount);
        }

        //damageIndex is the value returned from BaseDamage
        public void SetAttackStepDamage(AttackData[] attackData, float damageValue)
        {
            Debug.Log("Setting Attack Step Damage for each step.");
            Debug.Log("  To " + damageValue);
            //iterate each attack step in attack data
            for (int i = 0; i < attackData.Length; i++)
            {
                var currentAttackStep = attackData[i]; 
                currentAttackStep.Damage.Add(damageValue);
            }
        }


        public bool CheckWeaponHasDamageType(Item item, DamageType.Types weaponDamageType)
        {
            var damageType = item.GetComponent<WeaponStats>().BaseDamage.List.Find(x => x.Type == weaponDamageType);
            return damageType != null ? true : false;
        }

        public void CheckVariablesToOverride()
        {

            foreach (var itemOverride in outwardOverrider.variablesToOverride)
            {
                var itemID = itemOverride.itemID;
                var item = CheckItemExists(itemID);
                if (item != null)
                {
                    if (!itemsToOverride.ContainsKey(itemOverride))
                    {
                        itemsToOverride.Add(itemOverride, item);
                    }              
                }
                else
                {
                    Debug.Log("Could not find Item by ID " + itemID + " in Resources Prefab Manager");
                }
            }
        }


        public void ReflectionSetValue<T>(T value, Type type, object obj, string field)
        {
            FieldInfo fieldInfo = type.GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(obj, value);
        }

        public object ReflectionGetValue(Type type, object obj, string value)
        {
            FieldInfo fieldInfo = type.GetField(value, BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo.GetValue(obj);
        }

        public Item CheckItemExists(int itemID)
        {
            ResourcesPrefabManager itemManager = ResourcesPrefabManager.Instance;
            var itemToCheck = itemManager.GetItemPrefab(itemID);
            if (itemToCheck != null)
            {
                Debug.Log("Item " + itemID + " Exists");
                return itemToCheck; 
            }
            else
            {
                Debug.Log("Item " + itemID + " Does Not Exist");
                return null;
            }

        }

    }
}

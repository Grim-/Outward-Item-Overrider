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

                    var value = ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus");

                    Debug.Log("Max HP bonus " + value);

                    weaponStatComponent.AttackSpeed = itemOverride.value;
                    break;
                case WeaponStatType.POUCH_BONUS:
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


            switch (damageType)
            {
                case WeaponDamageType.Physical:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Physical))
                    {
                        Debug.Log("Weapon Has Physical Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Physical, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Physical Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Physical, itemOverride.value);
                    }

                    break;
                case WeaponDamageType.Ethereal:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Ethereal))
                    {
                        Debug.Log("Weapon Has Etheral Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Ethereal, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Etheral Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Ethereal, itemOverride.value);
                    }

                    break;
                case WeaponDamageType.Decay:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Decay))
                    {
                        Debug.Log("Weapon Has Decay Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Decay, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Decay Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Decay, itemOverride.value);
                    }
                    break;
                case WeaponDamageType.Electric:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Electric))
                    {
                        Debug.Log("Weapon Has Electric Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Electric, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Electric Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Electric, itemOverride.value);
                    }
                    break;
                case WeaponDamageType.Frost:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Frost))
                    {
                        Debug.Log("Weapon Has Frost Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Frost, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Frost Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Frost, itemOverride.value);
                    }
                    break;
                case WeaponDamageType.Fire:
                    if (CheckWeaponHasDamageType(itemToChange, DamageType.Types.Fire))
                    {
                        Debug.Log("Weapon Has Fire Damage Type updating value");
                        ModifyWeaponDamage(itemToChange, DamageType.Types.Fire, itemOverride.value);
                    }
                    else
                    {
                        Debug.Log("Weapon Has No Fire Damage Type adding value");
                        AddDamageTypeToWeapon(itemToChange, DamageType.Types.Fire, itemOverride.value);
                    }
                    break;
            }

        }

        //To add a new Damage Type to a weapon
        public void AddDamageTypeToWeapon(Item item, DamageType.Types weaponDamageType, float damageAmount)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
            AddBaseDamageType(item, weaponDamageType);
            SetAttackStepDamage(weaponStatComponent.Attacks, damageAmount);
        }


        public void AddBaseDamageType(Item item, DamageType.Types weaponDamageType)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
            weaponStatComponent.BaseDamage.Add(weaponDamageType);
            //get the new count, the new count is the postition of the damage in the current AttackStep.Damage List 
            Debug.Log(weaponStatComponent.Attacks[0]);
            Debug.Log(weaponStatComponent.Attacks[0].Damage[0]);
        }

        public void ModifyWeaponDamage(Item item, DamageType.Types weaponDamageType, float damageAmount)
        {
            WeaponStats weaponStatComponent = item.GetComponent<WeaponStats>();
            AttackData[] attackData = weaponStatComponent.Attacks;
            List<DamageType> damageList = weaponStatComponent.BaseDamage.List;

            int baseDamageTypeCount = weaponStatComponent.BaseDamage.List.Count;

            int currentDamageTypeIndex = 0;


            for (int y = 0; y < damageList.Count; y++)
            {
                if (damageList[y].Type == weaponDamageType)
                {
                    Debug.Log(damageList[y].Type + " is " + y + " index");
                    currentDamageTypeIndex = y;
                    break;
                }
            }



        }

        //damageIndex is the value returned from BaseDamage
        public void SetAttackStepDamage(AttackData[] attackData, float damageValue)
        {
            Debug.Log("Setting Attack Step Damage for each step.");
            Debug.Log("  To " + damageValue);


            //iterate each attack step in attack data
            for (int i = 0; i < attackData.Length; i++)
            {
                //set reference to current step
                var currentAttackStep = attackData[i];
                //attack step has a damage array each item in the array denotes how much damage this swing does in each type   
                currentAttackStep.Damage.Add(damageValue);
                //Debug.Log("Damage : (After) " + currentAttackStep.Damage[damageIndex]);
            }
        }


        public bool CheckWeaponHasDamageType(Item item, DamageType.Types weaponDamageType)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
             var damageType = weaponStatComponent.BaseDamage.List.Find(x => x.Type == weaponDamageType);

            if (damageType != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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

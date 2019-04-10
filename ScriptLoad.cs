using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using On;
using UnityEngine;

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

            //itemToChange.GetComponent<WeaponStats>().Impact = itemOverride.value;

            //Debug.Log("Weapon Impact updated to " + itemToChange.GetComponent<WeaponStats>().Impact);


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

        public void AddDamageTypeToWeapon(Item item, DamageType.Types weaponDamageType, float damageAmount)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
            weaponStatComponent.BaseDamage.Add(weaponDamageType);
            weaponStatComponent.BaseDamage.List.Find(x => x.Type == weaponDamageType).Damage = damageAmount;
        }

        public void ModifyWeaponDamage(Item item, DamageType.Types weaponDamageType, float damageAmount)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
            weaponStatComponent.BaseDamage.List.Find(x => x.Type == weaponDamageType).Damage = damageAmount;
        }


        public bool CheckWeaponHasDamageType(Item item, DamageType.Types weaponDamageType)
        {
            var weaponStatComponent = item.GetComponent<WeaponStats>();
             var damageType = weaponStatComponent.BaseDamage.List.Find(x => x.Type == DamageType.Types.Ethereal);

            if (damageType != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void CheckVariablesToOverride()
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

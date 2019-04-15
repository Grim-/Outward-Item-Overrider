using UnityEngine;
using System.Collections.Generic;
using ODebug;
using ConfigData;

namespace ItemOverrideMod
{
    public class APILoad : MonoBehaviour
    {
        public static ItemOverrider api;
        private List<ItemOverride> m_itemsToOverride;
        private Dictionary<ItemOverride, Item> m_items = new Dictionary<ItemOverride, Item>();

        public void Initialise(OutwardItemOverrides configurationData)
        {
            m_itemsToOverride = configurationData.ItemOverrides;
       
            foreach (var item in m_itemsToOverride)
            {
                OLogger.Log("------------------------------------");
                OLogger.Log("Item ID:" + item.ItemID);
                OLogger.Log("ItemType: " + item.ItemType);
                if (item.Data.Count > 0)
                {
                    foreach (var overrides in item.Data)
                    {
                        if (overrides is WeaponOverrideData wepOverrides) {
                            OLogger.Log("ItemStatType: " + wepOverrides.ItemStatType);
                            OLogger.Log("WeaponStatType: " + wepOverrides.WeaponStatType);                         
                            OLogger.Log("DmgType: " + wepOverrides.DmgType);
                            OLogger.Log("Value: " + wepOverrides.Value);
                        } else
                        {
                            OLogger.Log("ItemStatType: " + overrides.ItemStatType);
                            OLogger.Log("ItemStatType: " + overrides.Value);
                        }
                    }         
                }
                else
                {
                    OLogger.Log("No weapon overrides found for this item to override");
                    OLogger.Log("------------------------------------");
                }
            }

            Patch();
        }

        public void Patch()
        {
            On.ResourcesPrefabManager.Load += new On.ResourcesPrefabManager.hook_Load(PrefabsLoadedHook);
        }

        private void PrefabsLoadedHook(On.ResourcesPrefabManager.orig_Load orig, ResourcesPrefabManager self)
        {
            orig(self);

            // Remove items that dont have a valid itemID
            m_itemsToOverride.RemoveAll(item => ResourcesPrefabManager.Instance.GetItemPrefab(item.ItemID) == null);

            // Now add our items we want to modify together in a Dictionary container with the modification data from our configuration file.
            m_itemsToOverride.ForEach(itemOverrides => {
                if (!m_items.ContainsKey(itemOverrides))
                {
                    m_items.Add(itemOverrides, ResourcesPrefabManager.Instance.GetItemPrefab(itemOverrides.ItemID));
                }
            });

            // Parse our modification data and apply it to the item to modify
            if (m_items.Count > 0)
            {
                foreach (var item in m_items)
                {
                    ParseItemOverrides(item.Key, item.Value);
                }
            }
        }

        private void ParseItemOverrides(ItemOverride itemOverrides, Item item)
        {

            switch (itemOverrides.ItemType)
            {
                case ItemOverrideType.NONE:
                case ItemOverrideType.ARMOUR:
                case ItemOverrideType.SPELL:
                case ItemOverrideType.BAG :
                    break;

                case ItemOverrideType.WEAPON:
                    ParseWeaponChanges(itemOverrides, item);
                    break;
            }
        }

        private void ParseWeaponChanges(ItemOverride itemOverrides, Item item)
        {
            var weaponOverrides = itemOverrides.Data;
            var weaponStatComponent = item.GetComponent<WeaponStats>();

            if (weaponOverrides.Count == 0)
            {
                OLogger.Log("ParseWeaponChanges(): Aborting, There are no weapon overrides defined.");
                return;
            }

            foreach (WeaponOverrideData weaponOverride in weaponOverrides)
            {
                switch (weaponOverride.WeaponStatType)
                {
                    case WeaponStatType.NONE:
                        break;
                    case WeaponStatType.DAMAGE:
                        OLogger.Log("ParseWeaponChanges(): Updating Damage Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        ModifyWeaponDamage(weaponOverride, item);
                        break;
                    case WeaponStatType.IMPACT:
                        OLogger.Log("ParseWeaponChanges(): Updating Impact Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        weaponStatComponent.Impact = weaponOverride.Value;
                        break;
                    case WeaponStatType.STAMINA_COST:
                        OLogger.Log("ParseWeaponChanges(): Updating Stamina Cost Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        weaponStatComponent.StamCost = weaponOverride.Value;
                        break;
                    case WeaponStatType.REACH:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Reach Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        weaponStatComponent.Reach = weaponOverride.Value;
                        break;
                    case WeaponStatType.SPEED:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Attack Speed Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        weaponStatComponent.AttackSpeed = weaponOverride.Value;
                        break;
                    case WeaponStatType.HEALTH_BONUS:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Health Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var hpBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus");
                        OLogger.Log("Max HP bonus before " + hpBonus);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus");
                        hpBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_maxHealthBonus");
                        OLogger.Log("Max HP bonus after " + hpBonus);
                        break;
                    case WeaponStatType.POUCH_BONUS:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Pouch Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var pouchBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_pouchCapacityBonus");
                        OLogger.Log("Pouch Bonus before " + pouchBonus);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_pouchCapacityBonus");
                        pouchBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_pouchCapacityBonus");
                        OLogger.Log("Pouch Bonus after " + pouchBonus);
                        break;
                    case WeaponStatType.HEAT_PROTECTION:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Heat Protection Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var heatProtection = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_heatProtection");
                        OLogger.Log("Heat Protection before " + heatProtection);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_heatProtection");
                        heatProtection = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_heatProtection");
                        OLogger.Log("Heat Protection after " + heatProtection);
                        break;
                    case WeaponStatType.COLD_PROTECTION:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Cold Protection Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var coldBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_coldProtection");
                        OLogger.Log("Cold Protection Bonus before " + coldBonus);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_coldProtection");
                        coldBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_coldProtection");
                        OLogger.Log("ParseWeaponChanges(): Cold Protection Bonus after " + coldBonus);
                        break;
                    case WeaponStatType.IMPACT_PROTECTION:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Impact Resistance Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var impactRes = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_impactResistance");
                        OLogger.Log("Impact Resistance Bonus before " + impactRes);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_impactResistance");
                        impactRes = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_impactResistance");
                        OLogger.Log("Impact Resistance Bonus after " + impactRes);
                        break;
                    case WeaponStatType.CORRUPTION_PROTECTION:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Corruption Protection Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var corruptionProtection = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_corruptionProtection");
                        OLogger.Log("Corruption Protection Bonus before " + corruptionProtection);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_corruptionProtection");
                        corruptionProtection = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_corruptionProtection");
                        OLogger.Log("Corruption Protection Bonus after " + corruptionProtection);
                        break;
                    case WeaponStatType.WATER_PROOF:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Water Proofness(?) Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var waterproofBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_waterproof");
                        OLogger.Log("Water Proofness(?) Bonus before " + waterproofBonus);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_waterproof");
                        waterproofBonus = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_waterproof");
                        OLogger.Log("Water Proofness(?) Bonus after " + waterproofBonus);
                        break;
                    case WeaponStatType.MOVEMENT_PENALTY:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Movement Penalty Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var movementPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_movementPenalty");
                        OLogger.Log("Movement Penalty Bonus before " + movementPenalty);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_movementPenalty");
                        movementPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_movementPenalty");
                        OLogger.Log("Movement Penalty Bonus after " + movementPenalty);
                        break;
                    case WeaponStatType.STAMINA_USE_PENALTY:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Stamina Use Penalty Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var staminaPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_staminaUsePenalty");
                        OLogger.Log("Stamina Use Penalty Bonus before " + staminaPenalty);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_staminaUsePenalty");
                        staminaPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_staminaUsePenalty");
                        OLogger.Log("Stamina Use Penalty Bonus after " + staminaPenalty);
                        break;
                    case WeaponStatType.HEAT_REGEN_PENALTY:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Heat Regen Penalty Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var heatRegenPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_heatRegenPenalty");
                        OLogger.Log("Heat Regen Penalty Bonus before " + heatRegenPenalty);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_heatRegenPenalty");
                        heatRegenPenalty = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_heatRegenPenalty");
                        OLogger.Log("Heat Regen Penalty Bonus after " + heatRegenPenalty);
                        break;
                    case WeaponStatType.MANA_USE_MODIFIER:
                        OLogger.Log("ParseWeaponChanges(): Updating Weapon Mana Reduction Bonus Stat for Item ID : " + itemOverrides.ItemID + " to " + weaponOverride.Value);
                        var manaReductionModified = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_manaUseModifier");
                        OLogger.Log("Mana Reduction Bonus before " + manaReductionModified);
                        ReflectionTools.ReflectionSetValue(weaponOverride.Value, typeof(EquipmentStats), weaponStatComponent, "m_manaUseModifier");
                        manaReductionModified = ReflectionTools.ReflectionGetValue(typeof(EquipmentStats), weaponStatComponent, "m_manaUseModifier");
                        OLogger.Log("Mana Reduction Bonus after " + manaReductionModified);
                        break;
                    default:
                        break;
                }

            }

            OLogger.Log("Parsing item id: " + itemOverrides.ItemID + " , Complete");
            OLogger.Log("------------------------------------");
        }

        private void ModifyWeaponDamage(WeaponOverrideData weaponOverride, Item item)
        {
            OLogger.Log("ModifyWeaponDamage(): Updating Weapon Damage");
            WeaponStats weaponStatComponent = item.GetComponent<WeaponStats>();
            float damageAmount = weaponOverride.Value;
            DamageType damageType = new DamageType(weaponOverride.DmgType, damageAmount);
            weaponStatComponent.BaseDamage.Add(damageType);
            SetAttackStepDamage(weaponStatComponent.Attacks, damageAmount);
        }

        //damageIndex is the value returned from BaseDamage
        private void SetAttackStepDamage(WeaponStats.AttackData[] attackData, float damageAmount)
        {
            OLogger.Log("SetAttackStepDamage(): Setting Attack Step Damage for each step to: " + damageAmount);
            //iterate each attack step in attack data
            for (int i = 0; i < attackData.Length; i++)
            {
                var currentAttackStep = attackData[i];
                currentAttackStep.Damage.Add(damageAmount);
            }
        }

        
    }
}

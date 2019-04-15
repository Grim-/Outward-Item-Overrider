using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigData
{

    [XmlRoot("OutwardItemOverrides",
    IsNullable = false)]
    public class OutwardItemOverrides
    {
        [XmlArray("ItemOverrides"), XmlArrayItem(typeof(ItemOverride))]
        public List<ItemOverride> ItemOverrides { get; set; }
        [XmlAttribute]
        public bool DebugMode { get; set; }

    }

    [Serializable]
    public class ItemOverride
    {
        [XmlAttribute]
        public int ItemID { get; set; }
        [XmlAttribute]
        public ItemOverrideType ItemType { get; set; }
        [XmlArray("Data"), XmlArrayItem(typeof(ItemOverrideData)), XmlArrayItem(typeof(WeaponOverrideData))]
        public List<ItemOverrideData> Data { get; set; }
    }

    [Serializable]
    public enum ItemOverrideType
    {
        [XmlEnum("NONE")]
        NONE,
        [XmlEnum("WEAPON")]
        WEAPON,
        [XmlEnum("ARMOUR")]
        ARMOUR,
        [XmlEnum("SPELL")]
        SPELL,
        [XmlEnum("BAG")]
        BAG
    }

    [XmlInclude(typeof(WeaponOverrideData))]
    [Serializable]
    public class ItemOverrideData
    {
        [XmlAttribute]
        public ItemStatType ItemStatType { get; set; }
        public virtual float Value { get; set; }
    }


    [Serializable]
    public class WeaponOverrideData : ItemOverrideData
    {
        [XmlAttribute]
        public WeaponStatType WeaponStatType { get; set; }
        [XmlAttribute]
        public DamageType.Types DmgType { get; set; }
    }

    [Serializable]
    public enum ItemStatType
    {
        NONE,
        WEIGHT,
        VALUE,
        // Add whatever you see fit here.
        COUNT,
    }

    [Serializable]
    public class DmgType
    {
        [XmlAttribute]
        public DamageType.Types Type { get; set; }

        public void Init(DamageType.Types type)
        {
            this.Type = type;
        }
    }

    [Serializable]
    public class StatType
    {
        [XmlAttribute]
        public ItemStatType Type { get; set; }

        public void Init(ItemStatType type)
        {
            this.Type = type;
        }
    }

    [Serializable]
    public enum WeaponStatType
    {
        [XmlEnum("NONE")]
        NONE,
        [XmlEnum("DAMAGE")]
        DAMAGE,
        [XmlEnum("IMPACT")]
        IMPACT,
        [XmlEnum("STAMINA_COST")]
        STAMINA_COST,
        [XmlEnum("REACH")]
        REACH,
        [XmlEnum("SPEED")]
        SPEED,
        [XmlEnum("HEALTH_BONUS")]
        HEALTH_BONUS,
        [XmlEnum("POUCH_BONUS")]
        POUCH_BONUS,
        [XmlEnum("HEAT_PROTECTION")]
        HEAT_PROTECTION,
        [XmlEnum("COLD_PROTECTION")]
        COLD_PROTECTION,
        [XmlEnum("IMPACT_PROTECTION")]
        IMPACT_PROTECTION,
        [XmlEnum("CORRUPTION_PROTECTION")]
        CORRUPTION_PROTECTION,
        [XmlEnum("WATER_PROOF")]
        WATER_PROOF,
        [XmlEnum("MOVEMENT_PENALTY")]
        MOVEMENT_PENALTY,
        [XmlEnum("STAMINA_USE_PENALTY")]
        STAMINA_USE_PENALTY,
        [XmlEnum("HEAT_REGEN_PENALTY")]
        HEAT_REGEN_PENALTY,
        [XmlEnum("MANA_USE_MODIFIER")]
        MANA_USE_MODIFIER
    }
}

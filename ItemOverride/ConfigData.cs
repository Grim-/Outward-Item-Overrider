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
        public ItemOverride ItemOverrides { get; set; }
        [XmlAttribute]
        public bool DebugMode { get; set; }

    }

    [Serializable]
    public class ItemOverride
    {
        [XmlAttribute]
        public int ItemID { get; set; }
        [XmlArray("Data"), XmlArrayItem(typeof(ItemOverrideData)), XmlArrayItem(typeof(WeaponOverrideData))]
        public List<OverrideData> Data { get; set; }
    }

    [Serializable]
    public enum OverrideType
    {
        [XmlEnum("NONE")]
        NONE,
        [XmlEnum("ITEMSTAT")]
        ITEMSTAT,
        [XmlEnum("WEAPON")]
        WEAPON,
        [XmlEnum("ARMOUR")]
        ARMOUR,
        [XmlEnum("SPELL")]
        SPELL,
        [XmlEnum("BAG")]
        BAG
    }


    [Serializable]
    public abstract class OverrideData
    {
        [XmlAttribute]
        public OverrideType OverrideType { get; set; }
        [XmlAttribute]
        public float Value { get; set; }
    }


    [Serializable]
    public class ItemOverrideData : OverrideData
    {
        [XmlAttribute]
        public ItemStatType ItemStatType { get; set; }
    }


    [Serializable]
    public class WeaponOverrideData : OverrideData
    {
        [XmlAttribute]
        public WeaponStatType WeaponStatType { get; set; }
        [XmlAttribute]
        public DamageType.Types DmgType { get; set; }
    }

    [Serializable]
    public enum ItemStatType
    {
        [XmlEnum("NONE")]
        NONE,
        [XmlEnum("RAWWEIGHT")]
        RAWWEIGHT,
        [XmlEnum("MAXDURABILITY")]
        MAXDURABILITY,
        [XmlEnum("BASEVALUE")]
        BASEVALUE,
        [XmlEnum("COUNT")]
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

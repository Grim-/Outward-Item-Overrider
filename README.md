# Outward Item Overrider 

An attempt at providing a simple method of Modifying Item Stats (Weapons, Armour, Bags) in Outward via XML configuration files.
It is in the early stages, so currently it only supports Weapon stats but there are plans to support most Items.


#### Getting Started

> Simply Include the ItemOverride.dll as you would any other Partiality mod then create a .xml formatted as below in Outward/Mods/Overrides (Create the folder if it does not exist).

> For example the below file makes three modifications two changes on one item and 1 on another. The first item has an added 200 fire damage and its pouch bonus set to 50, the second weapon has its weapons stats changed to do 10 impact.

> You can copy this into a new file and change the ID and values to get started, simply make sure the .JSON file is in the correct folder (above)

```javascript
//ItemOverrides.xml (example)
<?xml version="1.0" encoding="utf-8"?>
<OutwardItemOverrides xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" DebugMode="true">
  <ItemOverrides>
    <ItemOverride ItemID="2000090" ItemType="WEAPON">
      <Data>
        <WeaponOverrideData WeaponStatType="DAMAGE" DmgType="Fire">
          <Value>200</Value>
        </WeaponOverrideData>
		<WeaponOverrideData WeaponStatType="POUCH_BONUS">
          <Value>50</Value>
        </WeaponOverrideData>
      </Data>
    </ItemOverride>
	<ItemOverride ItemID="5110110" ItemType="WEAPON">
      <Data>
        <WeaponOverrideData WeaponStatType="IMPACT">
          <Value>10</Value>
        </WeaponOverrideData>
      </Data>
    </ItemOverride>
  </ItemOverrides>
</OutwardItemOverrides>

```
## Currently Supported XML Options
#### Serialized class ItemOverrideData 
containing XmlAttribute "ItemStatType" and XmlElement "Value"
#### Serialized class WeaponOverrideData extending ItemOverrideData 
containing XmlAttribute WeaponStatType of type WeaponStatType and XmlAttribute DmgType of type DamageType.Types listed under "DAMAGE Types"

All item overrides must be correctly formatted .xml and be placed in a folder named "Config" within the Outward folder `(Outward/Config)`.

Once the game has loaded the .dll will check for the xml file in the Config folder and apply them to the item.

## Currently Supported Types
#### Weapon - Half way
#### Armour - TODO
#### Bag - TODO
#### Skills -TODO


## Currently Supported Weapon Stats

If you are editing a Weapon's Damage Stats you must include the <WeaponOverrideData DmgType="type" (Get supported Damage Types below)

#### DAMAGE Types  
    * Physical
    * Ethereal,
    * Decay,
    * Electric,
    * Frost,
    * Fire
	* DarkOLD
	* LightOLD
	* Raw


#### Weapon Stats
	* NONE
	* DAMAGE
	* IMPACT,
	* STAMINA_COST,
	* REACH,
	* SPEED,
	* HEALTH_BONUS,
    * POUCH_BONUS,
    * HEAT_PROTECTION,
    * COLD_PROTECTION,
    * IMPACT_PROTECTION,
    * CORRUPTION_PROTECTION,
    * WATER_PROOF,
    * MOVEMENT_PENALTY,
    * STAMINA_USE_PENALTY,
    * HEAT_REGEN_PENALTY,
    * MANA_USE_MODIFIER




# Outward Item Overrider 

An attempt at providing a simple method of Modifying Item Stats (Weapons, Armour, Bags) in Outward via JSON files.

It is in the early stages, so currently it only supports Weapon stats but there are plans to support most Items.



#### Getting Started

> Simply Include the OutwardItemOverrider.dll as you would any other Partiality mod then create a .json formatted as below in Outward/Mods/Overrides (Create the folder if it does not exist).

> For example the below file makes three modifications to the weapons stats changing the physical damage to 2, ethereal damage to 200 and impact to 10.

> You can copy this into a new file and change the ID and values to get started, simply make sure the .JSON file is in the correct folder (above)

```javascript
//5110110.Json (PistolHandCannon)

{
	"itemID" : 5110110,
	"type" : "weapon",
	"overrides" : [
		{"weapon_stat_type" : "Impact", "value" : 10},
		{"weapon_stat_type" : "Damage", "weapon_damage_type" : "physical", "value" : 2},
		{"weapon_stat_type" : "Damage", "weapon_damage_type" : "ethereal", "value" : 200}
	]
}`

```


All item overrides must be correctly formatted .Json files and be placed in a folder named "Overrides" within the Mods folder `(Outward/Mods/Overrides)` and contain the ItemID and Item Type.

Once the game has loaded the .dll will check for any json files in the Overrides folder and apply them to the item.

## Currently Supported Types
#### Weapon - Half way
#### Armour - TODO
#### Bag - TODO
#### Skills -TODO


## Currently Supported Weapon Stats

If you are editing a Weapon's Damage Stats you must include the "weapon_damage_type" : "physical" (Get supported Damage Types below, type in lower case)

#### DAMAGE Types  
    * Physical
    * Ethereal,
    * Decay,
    * Electric,
    * Frost,
    * Fire


#### Weapon Stats
	* IMPACT,
	* STAMINA_COST,
	* REACH,
	* SPEED
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




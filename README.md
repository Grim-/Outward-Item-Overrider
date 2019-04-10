# Outward Item Overrider
An Attempt at providing a Simple Method of Modding Item Stats (Weapons, Armour, Bags) in Outward via .JSON file.

It is in the early stages, so currently it only supports Weapon stats.


Simply Include the OutwardItemOverrider.dll as you would any other Partiality mod then create a .json formatted as below in Outward/Mods/Overrides (Create the folder if it does not exist).


#### ItemID = ID of the item to Modify
#### Type = The Type of the item you want to modify (Only weapon supported currently)
#### Overrides = An array of Javascript Objects detailing the Stat Type, The Value to change it to and the Damage Type in the case of Weapon Damage.

All item overrides must be correctly formatted .Json files and be placed in a folder named `"Overrides"` in the Mods folder (Outward/Mods/Overrides) the itemID and type are required.

Then on game load your modifications will be applied to the item.

For example the below file makes three modifications to the weapons stats changing the physical damage to 2, ethereal damage to 200 and impact to 10.

You can copy this into a new file and change the ID and values to get started, simply make sure the .JSON file is in the correct folder (above)

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
## Currently Supported Types
#### Weapon - Half way
#### Armour - TODO
#### Bag - TODO
#### Skills -TODO


## Currently Supported Weapon Stats

#### DAMAGE Types  
    * Physical
    * Ethereal,
    * Decay,
    * Electric,
    * Frost,
    * Fire

#### IMPACT,
#### STAMINA_COST,
#### REACH,
#### SPEED


If you are editing a Weapon's Damage Stats you must include the "weapon_damage_type" : "physical" (Get supported Damage Types above type in lower case)

# Changes from Outward Item Overrider by Grim-
Added a base class called OverrideData that can be a base for any type of override data for other types such as the implemented WeaponOverrideData and ItemOverrideData, future implementations of Armor, Bags etc are on the way.
The project uses XML serialization and deserialization for its configuration file as to easily translate the configuration into C# classes and objects.
The project now also uses the OLogger class to debug information in-game

## User Installation
Download the API from the Nexus or Github Releases, unzip the file and place the .dll file into "Outward\Mods\". Mods will now be able to use functions provided.
Set the attribute DebugMode to true or false in your xml to enable debugging. You can find the attribute on the root element <OutwardItemOverrides>

## Massive thanks and pretty much all credit goes to Grim- (Emo#7953) for making the initial mod.

# Outward Item Overrider 
> Simply Include the ItemOverride.dll as you would any other Partiality mod then put xml files formatted as below in Outward/Config (Create the folder if it does not exist).
The XMLConfigHelper API will automatically load all config files in "Outward\Config\".

## Currently Supported Types
#### Weapon - Half way
#### Item - Half way
#### Armour - TODO
#### Bag - TODO
#### Skills -TODO

# Outward Item Overrider XML structure
> For example the below file makes two modifications two changes, one item and one on another. The first has an added 200 fire damage and the other has its impact set to 505.
> You can copy this into a new file and change the ID and values to get started, simply make sure the .XML file is in the correct folder (above)

```javascript
//2000090.xml (example)
<?xml version="1.0" encoding="utf-8"?>
<OutwardItemOverrides xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" DebugMode="true">
  <ItemOverrides ItemID="2000090">
    <Data>
      <WeaponOverrideData OverrideType="WEAPON" Value="200" WeaponStatType="DAMAGE" DmgType="Fire" />
	  <WeaponOverrideData OverrideType="WEAPON" Value="51" WeaponStatType="IMPACT" />
      <ItemOverrideData OverrideType="ITEMSTAT" Value="211" ItemStatType="MAXDURABILITY" />
    </Data>
  </ItemOverrides>
</OutwardItemOverrides>
```

> Another file
```javascript
//2100000.xml (example)
<?xml version="1.0" encoding="utf-8"?>
<OutwardItemOverrides xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" DebugMode="true">
  <ItemOverrides ItemID="2100000">
    <Data>
	  <WeaponOverrideData OverrideType="WEAPON" Value="500" WeaponStatType="IMPACT" />
	  <WeaponOverrideData OverrideType="WEAPON" Value="51" WeaponStatType="POUCH_BONUS" />
    </Data>
  </ItemOverrides>
</OutwardItemOverrides>
```

## Currently Supported XML Options
### itemID

### Class ItemOverrideData
* OverrideType
* ItemStatType
* Value

### Class WeaponOverrideData
* OverrideType
* WeaponStatType
* DmgType
* Value

#### OverrideType
* NONE
* ITEMSTAT
* WEAPON
* ARMOUR
* SPELL
* BAG

#### ItemStatType
* NONE
* RAWWEIGHT
* MAXDURABILITY

#### WeaponStatType
* NONE
* DAMAGE
* IMPACT
* STAMINA_COST
* REACH
* SPEED
* HEALTH_BONUS
* POUCH_BONUS
* HEAT_PROTECTION
* COLD_PROTECTION
* IMPACT_PROTECTION
* CORRUPTION_PROTECTION
* WATER_PROOF
* MOVEMENT_PENALTY
* STAMINA_USE_PENALTY
* HEAT_REGEN_PENALTY
* MANA_USE_MODIFIER

#### DmgType  
* Physical
* Ethereal
* Decay
* Electric
* Frost
* Fire
* DarkOLD
* LightOLD
* Raw

If you are editing a Weapon's Damage Stats you must include the <WeaponOverrideData DmgType="type here"


## ConfigHelper Example
```csharp
public void Initialize()
{
	// Read config file
	ConfigHelper configHelper = new ConfigHelper(ConfigHelper.ConfigModes.CreateIfMissing, "FileNameHere.xml");
	configHelper.XMLDefaultConfig = "<config><baseSneakSpeed>0.7</baseSneakSpeed><stealthTrainingBonus>1.3</stealthTrainingBonus </config>";
	Debug.Log("Trying to load " + configHelper.FullPath);
	float baseSneakSpeed = configHelper.ReadFloat("/config/baseSneakSpeed");
	float stealthTrainingBonus = configHelper.ReadFloat("/config/stealthTrainingBonus");
	configHelper.WriteValue("/config/test", "write value 1");
	for(int i = 0; i < 10; ++i)
		configHelper.WriteValue("/config/loopValues/val_" + i, i.ToString());
}
```

## ReflectionTools Example
```csharp
// These can be run once, for example in Initialize(), since their values don't change
// Variable
FieldInfo m_autoRun = ReflectionTools.GetField(typeof(LocalCharacterControl), "m_autoRun");
// Method
MethodInfo StopAutoRun = ReflectionTools.GetMethod(typeof(LocalCharacterControl), "StopAutoRun");
// Using the reflected values has to be done in a method where an instance to the class exists (in this example, self)
public void detectMovementInputs(On.LocalCharacterControl.orig_DetectMovementInputs orig, LocalCharacterControl self)
{
	// Reading variable
	if ((bool)m_autoRun.GetValue(self))
	
	// Setting variable
	m_autoRun.SetValue(self, false);
	
	// Calling a method with no parameters
	StopAutoRun.Invoke(self, null);
	
	// Calling a method with parameters
	StopAutoRun.Invoke(self, new object[] { param1, param2 ...});
}
```

## OLogger Example
```csharp
//All of these functions are called from the static class OLogger

//Here is the main Log function:
OLogger.Log(object _obj, string _color = "ffffffff", string _panel = "Default")

//_obj is the object you want to display, it will be turned into a string in the log function.
//_color is the color that the text will be, which by default will be white.
//_panel is the panel that the text will be output to.

//Other functions:
//This isn't actually needed as .Log will create a panel, however, this give you control over the writeToDisk/enabledOnCreation
OLogger.CreateLog(Rect _rect, string _panel = "Default", bool _writeToDisk = true, bool _enabledOnCreation = true);
//_writeToDisk is whether you want to output any text in this panel to a file in "mods/Debug/'PanelName'.txt"
//treat writing to disk in an update loop the same as calling Debug.Log.
OLogger.SetUIPanelEnabled(string _panel, bool _enabled); //this will set the panel "_panel" to "enabled"
OLogger.SetPanelWriteToDisk(string _panel, bool _writeToDisk) //this will set writeToDisk to "_writeToDisk"
OLogger.ClearUIPanel(string _panel); //this will clear the text in the "_panel" panel
OLogger.DestroyUIPanel(string _panel); //this will destroy the "_panel" panel;
OLogger.Warning(object _obj, string _panel = "Default"); //this will output yellow text to the "_panel" panel
OLogger.Error(object _obj, string _panel = "Default"); //this will output red text to the "_panel" panel
//Example Turn Unity Debug Into OLogger Debug:

//These changes goes into your class file that extends from PartialityMod, and thus in these functions

public override void OnEnable()
{
    ...
    ...
    ...
    GameObject obj = new GameObject();
    NameOfYourScriptClass = obj.AddComponent<NameOfYourScriptClass>();

    //Used to set the parent of all debug boxes to obj
    OLogger.SetupObject(obj);
}


public override void OnLoad()
{
	base.OnLoad();
	
	// Sets up "Default Unity Compiler" panel at location: (X:400,Y:400)
	//	size: (W:400,H:400) and have it log to file (mods/Debug/"PanelName".txt) and be enabled on start
	OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default Unity Compiler", true, true);
	
	//If you want to also debug Unity's stack trace then call this
	OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default Unity Stack Trace", true, true);
	
	// Create the default logging panel that your addon will print to
	OLogger.CreateLog(new Rect(400, 400, 400, 400), "Default", true, true);
	
	//Add ignores to OLogger ignore list (will filter out from Unity's Debug calls)
	OLogger.ignoreList.AddToIgnore("Internal", "Failed to create agent"
								  , "is registered with more than one LODGroup"
								  , "No AudioManager"); 
	//ignores can also be removed by calling OLogger.ignoreList.RemoveFromIgnore()
	//Finally hook OLogger onto logMessageReceived to receive Unity's Debug calls
	Application.logMessageReceived += OLogger.DebugMethodHook;
}
```

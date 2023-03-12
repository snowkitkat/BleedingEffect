BleedEffect is a plugin for SCP: Secret Lab. It adds a... bleed effect... to players when they take damage from configurable damage types.

You can always find the latest dll [here](https://github.com/snowkitkat/BleedEffect/releases/latest). This plugin requires EXILED, which you can find [here](https://github.com/galaxy119/EXILED).

Please report any issues, or feature requests!

## Config options

- Debug (false) - Whether or not to enable debug messages.
- BleedMessage ("You feel warm blood from your wounds.") - The message shown to players when they start bleeding.
- IncreasedBleedMessage ("You feel your wounds deepen.") - The message shown to players when they begin taking increased damage due to excessive bleeding.
- ShotsToBleed (4) - The number of shots it takes to start bleeding.
- HealthDrainPerSecond (0.1) - The health drained per second.
- HealthDrainExponential (true) - Whether or not health drain is exponential for successive bleeding.
- HealthDrainDivisor (-15) - If HealthDrainPerSecond is less than 1 and HealthDrainExponential is true, this will be the number that # of BleedEffects is divided by before raising HealthDrainPerSecond by it (HealthDrainPerSecond^(# of BleedEffects/HealthDrainDivisor) This should always be < 0. The lower this number is, the less health is drained when taking successive bleed effects.).
- HealthDrainPerSecondIncrease (0.4) - The amout of health drain increased on successive bleed effects. Disregarded if exponential.

Damage types to apply bleeds to:
- E11RifleEnabled (true)
- LogicerEnabled (true)
- MicroEnabled (true)
- Mp7Enabled (true)
- P90Enabled (true)
- UspEnabled (true)
- GrenadeEnabled (true)
- SCP049_2Enabled (true)
- SCP939Enabled (true)

Which medical items stop bleeding:
- MedKitStopsBleeding (true)
- SCP500StopsBleeding (true)
- PainkillersStopBleeding (false)
- AdrenalineStopsBleeding (false)

Which SCPs get affected, if any:
- AffectsScps (false)
- Affects173 (false)
- Affects939 (false)
- Affects049 (false)
- Affects049_2 (false)
- Affects096 (false)
- Affects106 (false)

using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BleedEffect
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Enable debugging.")]
        public bool Debug { get; set; } = false;

        [Description("The message to show users when they start bleeding.")]
        public string BleedMessage { get; set; } = "You feel warm blood from your wounds.";

        [Description("The message shown when users bleed more.")]
        public string IncreasedBleedMessage { get; set; } = "You feel your wounds deepen.";

        [Description("Sets how many shots it takes to increase the bleed effect. Must be greater than, or, 1.")]
        public int ShotsToBleed { get; set; } = 4;

        [Description("Sets how much health is drained per second.")]
        public double HealthDrainPerSecond { get; set; } = 0.1;

        [Description("Determines whether health drain is exponential or not (HealthDrainPerSecond^#BleedEffects. If HealthDrainPerSecond < 1, #BleedEffects is divided by HealthDrainDivisor).")]
        public bool HealthDrainExponential { get; set; } = true;

        [Description("If HealthDrainPerSecond < 1 and HealthDrainExponential is true, divide #BleedEffects by this number before raising HealthDrainPerSecond by it. This should always be < 0. The lower this number is, the less health is drained when taking successive bleed effects.")]
        public double HealthDrainDivisor { get; set; } = -15;

        [Description("Sets how much health is drained per second on successive bleeds, disregarded if exponential.")]
        public double HealthDrainPerSecondIncrease { get; set; } = 0.4;

        [Description("Enable for specifc guns/damage sources.")]
        public bool E11RifleEnabled { get; set; } = true;
        public bool LogicerEnabled { get; set; } = true;
        public bool MicroEnabled { get; set; } = true;
        public bool Fsp9Enabled { get; set; } = true;
        public bool CrossvecEnabled { get; set; } = true;
        public bool Com18Enabled { get; set; } = true;
        public bool Com15Enabled { get; set; } = true;
        public bool GrenadeEnabled { get; set; } = true;
        public bool SCP049_2Enabled { get; set; } = true;
        public bool SCP939Enabled { get; set; } = true;

        [Description("Determines what can stop bleeding.")]
        public bool MedKitStopsBleeding { get; set; } = true;
        public bool SCP500StopsBleeding { get; set; } = true;
        public bool PainkillersStopBleeding { get; set; } = false;
        public bool AdrenalineStopsBleeding { get; set; } = false;

        [Description("Affect SCPs.")]
        public bool AffectsScps { get; set; } = false;
        public bool Affects173 { get; set; } = false;
        public bool Affects939 { get; set; } = false;
        public bool Affects049 { get; set; } = false;
        public bool Affects049_2 { get; set; } = false;
        public bool Affects096 { get; set; } = false;
        public bool Affects106 { get; set; } = false;
    }
}

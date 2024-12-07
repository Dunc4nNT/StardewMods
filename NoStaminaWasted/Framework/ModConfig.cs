using StardewModdingAPI.Utilities;

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public StaminaConsumptionOption AxeStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption FishingRodStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption HoeStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption MilkPailStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption PickaxeStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption ShearsStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;

        public StaminaConsumptionOption WateringCanStaminaConsumption { get; set; } = StaminaConsumptionOption.NoAccidents;
    }

    internal class ModConfigKeys
    {
        public KeybindList ReloadConfig { get; set; } = new();
    }
}

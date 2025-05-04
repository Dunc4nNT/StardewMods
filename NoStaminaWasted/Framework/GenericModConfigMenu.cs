using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using System;

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework
{
    internal class GenericModConfigMenu(IModRegistry modRegistry, IManifest manifest, IMonitor monitor, Func<ModConfig> config, Action reset, Action save)
    {
        public void Register()
        {
            IGenericModConfigMenuApi configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;

            configMenu.Register(mod: manifest, reset: reset, save: save);

            configMenu.AddSectionTitle(
                mod: manifest,
                text: I18n.Config_Stamina_ConsumptionSection_Name
            );
            configMenu.AddParagraph(
                mod: manifest,
                text: I18n.Config_Stamina_ConsumptionSection_Description
            );

            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_AxeConsumption_Name,
                tooltip: I18n.Config_Stamina_AxeConsumption_Tooltip,
                getValue: () => config().AxeStaminaConsumption.ToString(),
                setValue: value => config().AxeStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_FishingRodConsumption_Name,
                tooltip: I18n.Config_Stamina_FishingRodConsumption_Tooltip,
                getValue: () => config().FishingRodStaminaConsumption.ToString(),
                setValue: value => config().FishingRodStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_HoeConsumption_Name,
                tooltip: I18n.Config_Stamina_HoeConsumption_Tooltip,
                getValue: () => config().HoeStaminaConsumption.ToString(),
                setValue: value => config().HoeStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_MilkPailConsumption_Name,
                tooltip: I18n.Config_Stamina_MilkPailConsumption_Tooltip,
                getValue: () => config().MilkPailStaminaConsumption.ToString(),
                setValue: value => config().MilkPailStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_PickaxeConsumption_Name,
                tooltip: I18n.Config_Stamina_PickaxeConsumption_Tooltip,
                getValue: () => config().PickaxeStaminaConsumption.ToString(),
                setValue: value => config().PickaxeStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_ShearsConsumption_Name,
                tooltip: I18n.Config_Stamina_ShearsConsumption_Tooltip,
                getValue: () => config().ShearsStaminaConsumption.ToString(),
                setValue: value => config().ShearsStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
            configMenu.AddTextOption(
                mod: manifest,
                name: I18n.Config_Stamina_WateringCanConsumption_Name,
                tooltip: I18n.Config_Stamina_WateringCanConsumption_Tooltip,
                getValue: () => config().WateringCanStaminaConsumption.ToString(),
                setValue: value => config().WateringCanStaminaConsumption = (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
                allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
                formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}")
            );
        }
    }
}

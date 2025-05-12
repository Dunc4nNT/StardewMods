// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework;

using System;
using NeverToxic.StardewMods.Common;
using StardewModdingAPI;

#pragma warning disable CS9113 // Parameter is unread.
internal class GenericModConfigMenu(
    IModRegistry modRegistry,
    IManifest manifest,
    IMonitor monitor,
    Func<ModConfig> config,
    Action reset,
    Action save)
#pragma warning restore CS9113 // Parameter is unread.
{
    public void Register()
    {
        IGenericModConfigMenuApi? configMenu =
            modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

        if (configMenu is null)
        {
            return;
        }

        configMenu.Register(manifest, reset, save);

        // Stamina Consumption
        configMenu.AddSectionTitle(
            manifest,
            I18n.Config_Stamina_ConsumptionSection_Name);
        configMenu.AddParagraph(
            manifest,
            I18n.Config_Stamina_ConsumptionSection_Description);

        // Axe
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_AxeConsumption_Name,
            tooltip: I18n.Config_Stamina_AxeConsumption_Tooltip,
            getValue: () => config().AxeStaminaConsumption.ToString(),
            setValue: value =>
                config().AxeStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Fishing Rod
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_FishingRodConsumption_Name,
            tooltip: I18n.Config_Stamina_FishingRodConsumption_Tooltip,
            getValue: () => config().FishingRodStaminaConsumption.ToString(),
            setValue: value =>
                config().FishingRodStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Hoe
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_HoeConsumption_Name,
            tooltip: I18n.Config_Stamina_HoeConsumption_Tooltip,
            getValue: () => config().HoeStaminaConsumption.ToString(),
            setValue: value =>
                config().HoeStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Milk Pail
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_MilkPailConsumption_Name,
            tooltip: I18n.Config_Stamina_MilkPailConsumption_Tooltip,
            getValue: () => config().MilkPailStaminaConsumption.ToString(),
            setValue: value =>
                config().MilkPailStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Pickaxe
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_PickaxeConsumption_Name,
            tooltip: I18n.Config_Stamina_PickaxeConsumption_Tooltip,
            getValue: () => config().PickaxeStaminaConsumption.ToString(),
            setValue: value =>
                config().PickaxeStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Shears
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_ShearsConsumption_Name,
            tooltip: I18n.Config_Stamina_ShearsConsumption_Tooltip,
            getValue: () => config().ShearsStaminaConsumption.ToString(),
            setValue: value =>
                config().ShearsStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));

        // Watering Can
        configMenu.AddTextOption(
            manifest,
            name: I18n.Config_Stamina_WateringCanConsumption_Name,
            tooltip: I18n.Config_Stamina_WateringCanConsumption_Tooltip,
            getValue: () => config().WateringCanStaminaConsumption.ToString(),
            setValue: value =>
                config().WateringCanStaminaConsumption =
                    (StaminaConsumptionOption)Enum.Parse(typeof(StaminaConsumptionOption), value),
            allowedValues: Enum.GetNames(typeof(StaminaConsumptionOption)),
            formatAllowedValue: value => I18n.GetByKey($"config.stamina-consumption-option.{value}"));
    }
}

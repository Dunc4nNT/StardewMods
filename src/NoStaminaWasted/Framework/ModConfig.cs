// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework;

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

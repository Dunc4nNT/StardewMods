// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.SelfServe.Framework;

using StardewModdingAPI.Utilities;

internal class ModConfig
{
    public ModConfigKeys Keys { get; set; } = new();

    public bool PierresGeneralShop { get; set; } = true;

    public bool WillysFishShop { get; set; } = true;

    public bool IceCreamShop { get; set; } = true;

    public bool BlacksmithShop { get; set; } = true;

    public bool CarpentersShop { get; set; } = true;

    public bool MarniesAnimalShop { get; set; } = true;

    public bool HospitalShop { get; set; } = true;

    public bool SaloonShop { get; set; } = true;

    public bool BooksellerShop { get; set; } = true;

    public bool TravelingMerchantShop { get; set; } = true;

    public bool ResortBarShop { get; set; } = true;

    public bool SandyOasisShop { get; set; } = true;

    public bool DesertTraderShop { get; set; } = true;

    public bool NightMarketPainterShop { get; set; } = true;

    public bool NightMarketMagicBoatShop { get; set; } = true;

    public bool NightMarketTravelingMerchantShop { get; set; } = true;

    public bool NightMarketDecorationBoatShop { get; set; } = true;

    public bool OtherShops { get; set; } = true;
}

internal class ModConfigKeys
{
    public KeybindList ReloadConfig { get; set; } = new();
}

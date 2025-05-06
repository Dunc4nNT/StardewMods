// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable InconsistentNaming
namespace NeverToxic.StardewMods.SelfServe.Framework;

using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using xTile.Dimensions;
using Rectangle = xTile.Dimensions.Rectangle;

[SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1313:Parameter names should begin with lower-case letter",
    Justification = "Harmony naming convention has double underscore.")]
internal class Patches
{
    private static ModEntry? Mod { get; set; }

    public static void Patch(ModEntry mod)
    {
        Mod = mod;

        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(GameLocation),
                nameof(GameLocation.performAction),
                [typeof(string[]), typeof(Farmer), typeof(Location)]),
            new HarmonyMethod(typeof(Patches), nameof(GameLocationPerformActionPatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(Forest),
                nameof(Forest.checkAction),
                [typeof(Location), typeof(Rectangle), typeof(Farmer)]),
            new HarmonyMethod(typeof(Patches), nameof(ForestCheckActionPatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(IslandSouth),
                nameof(IslandSouth.checkAction),
                [typeof(Location), typeof(Rectangle), typeof(Farmer)]),
            new HarmonyMethod(typeof(Patches), nameof(IslandSouthCheckActionPatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(Desert), nameof(Desert.OnDesertTrader)),
            new HarmonyMethod(typeof(Patches), nameof(DesertOnDesertTraderPatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(BeachNightMarket), nameof(BeachNightMarket.checkAction)),
            new HarmonyMethod(typeof(Patches), nameof(BeachNightMarketCheckActionPatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(Utility),
                nameof(Utility.TryOpenShopMenu),
                [typeof(string), typeof(string), typeof(bool)]),
            postfix: new HarmonyMethod(typeof(Patches), nameof(Utility_TryOpenShopMenu_PostFix)));
        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(Utility),
                nameof(Utility.TryOpenShopMenu),
                [
                    typeof(string), typeof(GameLocation), typeof(Microsoft.Xna.Framework.Rectangle), typeof(int),
                    typeof(bool), typeof(bool), typeof(Action<string>)
                ]),
            new HarmonyMethod(typeof(Patches), nameof(Utility_TryOpenShopMenu_Prefix)));
    }

    public static void Utility_TryOpenShopMenu_PostFix(string shopId, ref bool __result)
    {
        if (Mod is null || __result || !Mod.Config.OtherShops)
        {
            return;
        }

        __result = Utility.TryOpenShopMenu(shopId, Game1.player.currentLocation, forceOpen: true);
    }

    public static bool Utility_TryOpenShopMenu_Prefix(ref bool forceOpen)
    {
        if (Mod is null)
        {
            return true;
        }

        if (Mod.Config.OtherShops)
        {
            forceOpen = true;
        }

        return true;
    }

    private static bool GameLocationPerformActionPatch(ref GameLocation __instance, string[] action, ref bool __result)
    {
        try
        {
            if (Mod is null)
            {
                return __result;
            }

            if (!ArgUtility.TryGet(action, 0, out string actionType, out string error))
            {
                return true;
            }

            switch (actionType)
            {
            case "Buy":
                if (!ArgUtility.TryGet(action, 1, out string which, out string error2))
                {
                    return true;
                }

                switch (which)
                {
                case "General":
                    if (!Mod.Config.PierresGeneralShop)
                    {
                        return true;
                    }

                    Utility.TryOpenShopMenu(Game1.shop_generalStore, __instance, forceOpen: true);
                    __result = true;
                    return false;
                case "Fish":
                    if (!Mod.Config.WillysFishShop)
                    {
                        return true;
                    }

                    Utility.TryOpenShopMenu(Game1.shop_fish, __instance, forceOpen: true);
                    __result = true;
                    return false;
                case "SandyShop":
                    if (!Mod.Config.SandyOasisShop)
                    {
                        return true;
                    }

                    Utility.TryOpenShopMenu(Game1.shop_sandy, __instance, forceOpen: true);
                    __result = true;
                    return false;
                }

                return true;

            case Game1.shop_iceCreamStand:
                if (!Mod.Config.IceCreamShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(actionType, __instance, forceOpen: true);
                __result = true;
                return false;
            case Game1.shop_blacksmith:
                if (!Mod.Config.BlacksmithShop)
                {
                    return true;
                }

                ShopHelper.OpenBlacksmithShop(__instance);
                __result = true;
                return false;
            case Game1.shop_carpenter:
                if (!Mod.Config.CarpentersShop)
                {
                    return true;
                }

                ShopHelper.OpenCarpentersShop(__instance);
                __result = true;
                return false;
            case Game1.shop_animalSupplies:
                if (!Mod.Config.MarniesAnimalShop)
                {
                    return true;
                }

                ShopHelper.OpenAnimalSuppliesShop(__instance);
                __result = true;
                return false;
            case "HospitalShop":
                if (!Mod.Config.HospitalShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(Game1.shop_hospital, __instance, forceOpen: true);
                __result = true;
                return false;
            case Game1.shop_saloon:
                if (!Mod.Config.SaloonShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(actionType, __instance, forceOpen: true);
                __result = true;
                return false;
            case Game1.shop_bookseller:
                if (!Mod.Config.BooksellerShop)
                {
                    return true;
                }

                ShopHelper.OpenBookSellerShop(__instance);
                __result = true;
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(GameLocationPerformActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool ForestCheckActionPatch(ref Forest __instance, Location tileLocation, ref bool __result)
    {
        try
        {
            if (Mod is null)
            {
                return true;
            }

            if (__instance.travelingMerchantDay)
            {
                Point cartTile = __instance.GetTravelingMerchantCartTile();
                if (tileLocation.X != cartTile.X + 4 || tileLocation.Y != cartTile.Y + 1 ||
                    !Mod.Config.TravelingMerchantShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(Game1.shop_travelingCart, __instance, forceOpen: true);
                __result = true;
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(ForestCheckActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool IslandSouthCheckActionPatch(ref Forest __instance, Location tileLocation, ref bool __result)
    {
        try
        {
            if (Mod is null)
            {
                return true;
            }

            if (tileLocation is not { X: 14, Y: 22 } || !Mod.Config.ResortBarShop)
            {
                return true;
            }

            Utility.TryOpenShopMenu(Game1.shop_resortBar, __instance, forceOpen: true);
            __result = true;
            return false;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(IslandSouthCheckActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool DesertOnDesertTraderPatch(ref Desert __instance)
    {
        try
        {
            if (Mod is null)
            {
                return true;
            }

            if (!Mod.Config.DesertTraderShop)
            {
                return true;
            }

            Utility.TryOpenShopMenu(Game1.shop_desertTrader, __instance, forceOpen: true);
            return false;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(DesertOnDesertTraderPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool BeachNightMarketCheckActionPatch(
        ref BeachNightMarket __instance,
        Location tileLocation,
        ref bool __result)
    {
        try
        {
            if (Mod is null)
            {
                return true;
            }

            switch (__instance.getTileIndexAt(tileLocation, "Buildings"))
            {
            case 68:
                if (!Mod.Config.NightMarketPainterShop)
                {
                    return true;
                }

                if (Game1.player.mailReceived.Contains(
                        Mod.Helper.Reflection.GetField<string>(__instance, "paintingMailKey").GetValue()))
                {
                    Game1.drawObjectDialogue(
                        Game1.content.LoadString("Strings\\Locations:BeachNightMarket_PainterSold"));
                }
                else
                {
                    __instance.createQuestionDialogue(
                        Game1.content.LoadString("Strings\\Locations:BeachNightMarket_PainterQuestion"),
                        __instance.createYesNoResponses(),
                        "PainterQuestion");
                }

                __result = true;
                return false;
            case 70:
                if (!Mod.Config.NightMarketMagicBoatShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(
                    "Festival_NightMarket_MagicBoat_Day" + __instance.getDayOfNightMarket(),
                    __instance,
                    forceOpen: true);

                __result = true;
                return false;
            case 399:
                if (!Mod.Config.NightMarketTravelingMerchantShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu(Game1.shop_travelingCart, __instance, forceOpen: true);

                __result = true;
                return false;
            case 595:
                if (!Mod.Config.NightMarketDecorationBoatShop)
                {
                    return true;
                }

                Utility.TryOpenShopMenu("Festival_NightMarket_DecorationBoat", __instance, forceOpen: true);

                __result = true;
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(BeachNightMarketCheckActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }
}

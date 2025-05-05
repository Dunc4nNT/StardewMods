// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable InconsistentNaming
namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Patches;

using System;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.BigCraftables;
using StardewValley.Objects;
using StardewValley.Tools;
using SObject = StardewValley.Object;

[SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1313:Parameter names should begin with lower-case letter",
    Justification = "Harmony naming convention has double underscore.")]
internal class ObjectPatch
{
    private static ModEntry? Mod { get; set; }

    internal static void Patch(ModEntry mod)
    {
        Mod = mod;

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.canBePlacedHere)),
            prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(CanBePlacedHerePatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.placementAction)),
            prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PlacementActionPatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.checkForAction)),
            prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(CheckForActionPatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.minutesElapsed)),
            prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(MinutesElapsedPatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.performToolAction)),
            prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformToolActionPatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.performRemoveAction)),
            postfix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformRemoveActionPatch)));

        Mod.Harmony.Patch(
            original: AccessTools.Method(typeof(SObject), nameof(SObject.DayUpdate)),
            postfix: new HarmonyMethod(typeof(ObjectPatch), nameof(DayUpdatePatch)));
    }

    private static bool CanBePlacedHerePatch(ref SObject __instance, GameLocation l, Vector2 tile, ref bool __result)
    {
        try
        {
            if (__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}"))
            {
                // TODO: add placement validation checks here
                return true;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(CanBePlacedHerePatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool PlacementActionPatch(ref SObject __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
    {
        try
        {
            if (__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}"))
            {
                SObject littleHelperBuilding = ItemRegistry.Create<SObject>(__instance.QualifiedItemId);
                location.objects.Add(new Vector2(x / 64, y / 64), littleHelperBuilding);
                littleHelperBuilding.heldObject.Value = new Chest();
                littleHelperBuilding.readyForHarvest.Value = false;

                __result = true;
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(PlacementActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool CheckForActionPatch(ref SObject __instance, Farmer who, bool justCheckingForActivity, ref bool __result)
    {
        try
        {
            if (__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}"))
            {
                if (justCheckingForActivity)
                {
                    __result = true;
                }
                else if (__instance.heldObject.Value is Chest chest)
                {
                    chest.ShowMenu();
                    __result = true;
                }

                __result = false;

                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(CheckForActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool MinutesElapsedPatch(ref SObject __instance, ref bool __result)
    {
        try
        {
            if (__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}"))
            {
                __result = false;

                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(MinutesElapsedPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static bool PerformToolActionPatch(ref SObject __instance, Tool? t, ref bool __result)
    {
        try
        {
            if (!__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}") ||
                __instance.heldObject.Value is not Chest chest || chest.isEmpty())
            {
                return true;
            }

            chest.clearNulls();

            if (t is not null && t.isHeavyHitter() && t is not MeleeWeapon)
            {
                __instance.playNearbySoundAll("hammer");
                __instance.shakeTimer = 100;
            }

            __result = false;

            return false;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(PerformToolActionPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }

    private static void PerformRemoveActionPatch(ref SObject __instance)
    {
        try
        {
            if (__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}"))
            {
                __instance.heldObject.Value = null;
            }
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(PerformRemoveActionPatch)}:\n{e}", LogLevel.Error);
        }
    }

    private static void DayUpdatePatch(ref SObject __instance)
    {
        try
        {
            if (!__instance.QualifiedItemId.StartsWith($"(BC){Mod?.ModManifest.UniqueID}") ||
                !DataLoader.BigCraftables(Game1.content).TryGetValue(
                    __instance.ItemId,
                    out BigCraftableData? bigCraftableData) || bigCraftableData is null)
            {
                return;
            }

            // TODO: execute assigned actions
            Mod?.Monitor.Log(__instance.QualifiedItemId);
            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.HarvestCrops",
                    out string? doHarvestCrops) &&
                bool.TryParse(
                    doHarvestCrops,
                    out bool doHarvestCropsBool) &&
                doHarvestCropsBool)
            {
                Mod?.Monitor.Log("Harvested crops.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.HarvestTrees",
                    out string? doHarvestTrees) &&
                bool.TryParse(
                    doHarvestTrees,
                    out bool doHarvestTreesBool) &&
                doHarvestTreesBool)
            {
                Mod?.Monitor.Log("Harvested trees.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.WaterCrops",
                    out string? doWaterCrops) &&
                bool.TryParse(
                    doWaterCrops,
                    out bool doWaterCropsBool) &&
                doWaterCropsBool)
            {
                Mod?.Monitor.Log("Watered crops.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.ReplantCrops",
                    out string? doReplantCrops) &&
                bool.TryParse(
                    doReplantCrops,
                    out bool doReplantCropsBool) &&
                doReplantCropsBool)
            {
                Mod?.Monitor.Log("Replanted crops.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.GatherForageables",
                    out string? doGatherForageables) &&
                bool.TryParse(
                    doGatherForageables,
                    out bool doGatherForageablesBool) &&
                doGatherForageablesBool)
            {
                Mod?.Monitor.Log("Gathered forageables.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.GatherTruffles",
                    out string? doGatherTruffles) &&
                bool.TryParse(
                    doGatherTruffles,
                    out bool doGatherTrufflesBool) &&
                doGatherTrufflesBool)
            {
                Mod?.Monitor.Log("Gathered truffles.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.DigArtefacts",
                    out string? doDigArtefacts) &&
                bool.TryParse(
                    doDigArtefacts,
                    out bool doDigArtefactsBool) &&
                doDigArtefactsBool)
            {
                Mod?.Monitor.Log("Dug artefacts.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.HarvestCrabPots",
                    out string? doHarvestCrabPots) &&
                bool.TryParse(
                    doHarvestCrabPots,
                    out bool doHarvestCrabPotsBool) &&
                doHarvestCrabPotsBool)
            {
                Mod?.Monitor.Log("Harvested crab pots.");
            }

            if (bigCraftableData.CustomFields.TryGetValue(
                    $"{Mod?.ModManifest.UniqueID}.AddBaitToCrabPots",
                    out string? doAddBaitToCrabPots) &&
                bool.TryParse(
                    doAddBaitToCrabPots,
                    out bool doAddBaitToCrabPotsBool) &&
                doAddBaitToCrabPotsBool)
            {
                Mod?.Monitor.Log("Added bait to crab pots.");
            }
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(DayUpdatePatch)}:\n{e}", LogLevel.Error);
        }
    }
}

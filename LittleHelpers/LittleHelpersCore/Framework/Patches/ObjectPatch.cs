using HarmonyLib;
using Microsoft.Xna.Framework;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.BigCraftables;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using SObject = StardewValley.Object;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Patches
{
    internal class ObjectPatch
    {
        private static Harmony s_harmony;
        private static string s_assetsModId;
        private static IMonitor s_monitor;
        private static Func<ModConfig> s_config;
        private static IReflectionHelper s_reflectionHelper;

        internal static void Initialise(Harmony harmony, string assetsModId, IMonitor monitor, Func<ModConfig> config, IReflectionHelper reflectionHelper)
        {
            s_harmony = harmony;
            s_assetsModId = assetsModId;
            s_monitor = monitor;
            s_config = config;
            s_reflectionHelper = reflectionHelper;

            ApplyPatches();
        }

        private static void ApplyPatches()
        {
            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.canBePlacedHere)),
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(CanBePlacedHerePatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.placementAction)),
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PlacementActionPatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.checkForAction)),
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(CheckForActionPatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.minutesElapsed)),
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(MinutesElapsedPatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.performToolAction)),
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformToolActionPatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.performRemoveAction)),
                postfix: new HarmonyMethod(typeof(ObjectPatch), nameof(PerformRemoveActionPatch))
            );

            s_harmony.Patch(
                original: AccessTools.Method(typeof(SObject), nameof(SObject.DayUpdate)),
                postfix: new HarmonyMethod(typeof(ObjectPatch), nameof(DayUpdatePatch))
            );
        }

        private static bool CanBePlacedHerePatch(ref SObject __instance, GameLocation l, Vector2 tile, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}"))
                {
                    // TODO: add placement validation checks here
                    return true;
                }

                return true;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(CanBePlacedHerePatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static bool PlacementActionPatch(ref SObject __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}"))
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
                s_monitor.Log($"Failed in {nameof(PlacementActionPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static bool CheckForActionPatch(ref SObject __instance, Farmer who, bool justCheckingForActivity, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}"))
                {
                    if (justCheckingForActivity)
                        __result = true;
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
                s_monitor.Log($"Failed in {nameof(CheckForActionPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static bool MinutesElapsedPatch(ref SObject __instance, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}"))
                {
                    __result = false;

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(MinutesElapsedPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static bool PerformToolActionPatch(ref SObject __instance, Tool t, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}") && __instance.heldObject.Value is Chest chest && !chest.isEmpty())
                {
                    chest.clearNulls();

                    if (t is not null && t.isHeavyHitter() && t is not MeleeWeapon)
                    {
                        __instance.playNearbySoundAll("hammer");
                        __instance.shakeTimer = 100;
                    }

                    __result = false;

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(PerformToolActionPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static void PerformRemoveActionPatch(ref SObject __instance)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}"))
                {
                    __instance.heldObject.Value = null;
                }
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(PerformRemoveActionPatch)}:\n{e}", LogLevel.Error);
            }
        }

        private static void DayUpdatePatch(ref SObject __instance)
        {
            try
            {
                if (__instance.QualifiedItemId.StartsWith($"(BC){s_assetsModId}") && DataLoader.BigCraftables(Game1.content).TryGetValue(__instance.ItemId, out BigCraftableData bigCraftableData))
                {
                    // TODO: execute assigned actions
                    s_monitor.Log(__instance.QualifiedItemId);
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.HarvestCrops", out string doHarvestCrops) && bool.TryParse(doHarvestCrops, out bool doHarvestCropsBool) && doHarvestCropsBool)
                        s_monitor.Log("Harvested crops.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.HarvestTrees", out string doHarvestTrees) && bool.TryParse(doHarvestTrees, out bool doHarvestTreesBool) && doHarvestTreesBool)
                        s_monitor.Log("Harvested trees.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.WaterCrops", out string doWaterCrops) && bool.TryParse(doWaterCrops, out bool doWaterCropsBool) && doWaterCropsBool)
                        s_monitor.Log("Watered crops.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.ReplantCrops", out string doReplantCrops) && bool.TryParse(doReplantCrops, out bool doReplantCropsBool) && doReplantCropsBool)
                        s_monitor.Log("Replanted crops.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.GatherForagables", out string doGatherForagables) && bool.TryParse(doGatherForagables, out bool doGatherForagablesBool) && doGatherForagablesBool)
                        s_monitor.Log("Gathered foragables.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.GatherTruffles", out string doGatherTruffles) && bool.TryParse(doGatherTruffles, out bool doGatherTrufflesBool) && doGatherTrufflesBool)
                        s_monitor.Log("Gathered truffles.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.DigArtefacts", out string doDigArtefacts) && bool.TryParse(doDigArtefacts, out bool doDigArtefactsBool) && doDigArtefactsBool)
                        s_monitor.Log("Dug artefacts.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.HarvestCrabPots", out string doHarvestCrabPots) && bool.TryParse(doHarvestCrabPots, out bool doHarvestCrabPotsBool) && doHarvestCrabPotsBool)
                        s_monitor.Log("Harvested crab pots.");
                    if (bigCraftableData.CustomFields.TryGetValue($"{s_assetsModId}.AddBaitToCrabPots", out string doAddBaitToCrabPots) && bool.TryParse(doAddBaitToCrabPots, out bool doAddBaitToCrabPotsBool) && doAddBaitToCrabPotsBool)
                        s_monitor.Log("Added bait to crab pots.");
                }
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(DayUpdatePatch)}:\n{e}", LogLevel.Error);
            }
        }
    }
}

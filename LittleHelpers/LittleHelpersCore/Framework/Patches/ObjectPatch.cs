﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using SObject = StardewValley.Object;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Patches
{
    internal class ObjectPatch
    {
        private static Harmony s_harmony;
        private static IMonitor s_monitor;
        private static Func<ModConfig> s_config;
        private static IReflectionHelper s_reflectionHelper;

        internal static void Initialise(Harmony harmony, IMonitor monitor, Func<ModConfig> config, IReflectionHelper reflectionHelper)
        {
            s_harmony = harmony;
            s_monitor = monitor;
            s_config = config;
            s_reflectionHelper = reflectionHelper;

            ApplyPatches();
        }

        private static void ApplyPatches()
        {
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
        }


        private static bool PlacementActionPatch(ref SObject __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId == "(BC)NeverToxic.LittleHelpersAssets_JunimoHelperBuilding_0")
                {
                    SObject littleHelperBuilding = ItemRegistry.Create<SObject>("(BC)NeverToxic.LittleHelpersAssets_JunimoHelperBuilding_0");
                    location.objects.Add(new Vector2(x / 64, y / 64), littleHelperBuilding);
                    littleHelperBuilding.heldObject.Value = new Chest();
                    littleHelperBuilding.readyForHarvest.Value = false;
                    Chest chest = (Chest)littleHelperBuilding.heldObject.Value;
                    chest.addItem(ItemRegistry.Create("(O)680"));
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
                if (__instance.QualifiedItemId == "(BC)NeverToxic.LittleHelpersAssets_JunimoHelperBuilding_0")
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
                s_monitor.Log($"Failed in {nameof(PlacementActionPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }

        private static bool MinutesElapsedPatch(ref SObject __instance, bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId == "(BC)NeverToxic.LittleHelpersAssets_JunimoHelperBuilding_0")
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

        private static bool PerformToolActionPatch(ref SObject __instance, Tool t, bool __result)
        {
            try
            {
                if (__instance.QualifiedItemId == "(BC)NeverToxic.LittleHelpersAssets_JunimoHelperBuilding_0" && __instance.heldObject.Value is Chest chest && !chest.isEmpty())
                {
                    chest.clearNulls();

                    if (t is not null && t.isHeavyHitter() && !(t is MeleeWeapon))
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
    }
}
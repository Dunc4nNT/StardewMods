using HarmonyLib;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using StardewValley;
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
                prefix: new HarmonyMethod(typeof(ObjectPatch), nameof(ObjectPatch.PlacementActionPatch))
            );
        }


        private static bool PlacementActionPatch(ref GameLocation __instance, GameLocation location, int x, int y, Farmer who, ref bool __result)
        {
            try
            {
                return false;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(PlacementActionPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }
    }
}

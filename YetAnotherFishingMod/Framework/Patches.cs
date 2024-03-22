using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using System;
using SObject = StardewValley.Object;

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class Patches()
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
                original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.getFish)),
                postfix: new HarmonyMethod(typeof(Patches), nameof(GetFishPatch))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.doneFishing)),
                prefix: new HarmonyMethod(typeof(Patches), nameof(DoneFishingPatch))
            );
        }

        private static void GetFishPatch(ref GameLocation __instance, ref Item __result, Farmer who, int waterDepth, Vector2 bobberTile)
        {
            try
            {
                ModConfig config = s_config();

                if (__result.Category == SObject.FishCategory || !(config.CatchFishRetries >= 0))
                    return;

                bool isTutorialCatch = who.fishCaught.Length == 0;

                for (int i = 0; i < config.CatchFishRetries; i++)
                {
                    __result = GameLocation.GetFishFromLocationData(__instance.Name, bobberTile, waterDepth, who, isTutorialCatch, isInherited: false, __instance);

                    if (__result.Category == SObject.FishCategory)
                        return;
                }
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(GetFishPatch)}:\n{e}", LogLevel.Error);
            }
        }

        private static bool DoneFishingPatch(ref bool consumeBaitAndTackle)
        {
            try
            {
                ModConfig config = s_config();

                if (config.InfiniteBaitAndTackle)
                    consumeBaitAndTackle = false;

                return true;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(DoneFishingPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }
    }
}

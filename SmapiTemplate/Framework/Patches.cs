using HarmonyLib;
using StardewModdingAPI;
using System;

namespace NeverToxic.StardewMods.SmapiTemplate.Framework
{
    internal class Patches
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
        }
    }
}

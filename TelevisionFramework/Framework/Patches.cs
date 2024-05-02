using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NeverToxic.StardewMods.TelevisionFramework.Framework
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
            s_harmony.Patch(
                original: AccessTools.Method(typeof(TV), "getWeeklyRecipe"),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(TVgetWeeklyRecipeTranspiler))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(TV), "getRerunWeek"),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(TVgetRerunWeekTranspiler))
            );
        }

        public static IEnumerable<CodeInstruction> TVgetWeeklyRecipeTranspiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            codeMatcher.MatchStartForward(
                new(OpCodes.Call, AccessTools.Method(typeof(Game1), nameof(Game1.temporaryContent))),
                new(OpCodes.Call, AccessTools.Method(typeof(DataLoader), nameof(DataLoader.Tv_CookingChannel))),
                new(OpCodes.Stloc_1)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(TVgetWeeklyRecipeTranspiler)}.", LogLevel.Error);
                return null;
            }


            codeMatcher
                .Advance(1)
                .Insert(
                    new(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(GetQueenOfSauceWeek))),
                    new(OpCodes.Stloc_0)
                );

            return codeMatcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> TVgetRerunWeekTranspiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            codeMatcher.MatchEndForward(
                new(OpCodes.Call, AccessTools.Method(typeof(Game1), nameof(Game1.stats))),
                new(OpCodes.Callvirt, AccessTools.Method(typeof(Stats), nameof(Stats.DaysPlayed))),
                new(OpCodes.Ldc_I4_3),
                new(OpCodes.Sub),
                new(OpCodes.Ldc_I4_7),
                new(OpCodes.Div),
                new(OpCodes.Ldc_I4_S)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(TVgetRerunWeekTranspiler)}.", LogLevel.Error);
                return null;
            }

            codeMatcher
                .RemoveInstruction()
                .Insert(
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(GetQueenOfSauceRerunMaxWeek)))
                );

            return codeMatcher.InstructionEnumeration();
        }

        private static int GetQueenOfSauceWeek()
        {
            Dictionary<string, string> recipes = DataLoader.Tv_CookingChannel(Game1.temporaryContent);

            int whichWeek = (int)Game1.stats.DaysPlayed % recipes.Count;

            if (whichWeek == 0)
            {
                return recipes.Count;
            }

            return whichWeek;
        }

        private static int GetQueenOfSauceRerunMaxWeek()
        {
            return DataLoader.Tv_CookingChannel(Game1.temporaryContent).Count;
        }
    }
}

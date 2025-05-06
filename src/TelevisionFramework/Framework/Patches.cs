// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable InconsistentNaming
namespace NeverToxic.StardewMods.TelevisionFramework.Framework;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

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
            AccessTools.Method(typeof(TV), "getWeeklyRecipe"),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(TVgetWeeklyRecipeTranspiler)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(TV), "getRerunWeek"),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(TVgetRerunWeekTranspiler)));
    }

    public static IEnumerable<CodeInstruction>? TVgetWeeklyRecipeTranspiler(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Game1), nameof(Game1.temporaryContent))),
            new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(DataLoader), nameof(DataLoader.Tv_CookingChannel))),
            new CodeMatch(OpCodes.Stloc_1));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log($"Failed to patch {nameof(TVgetWeeklyRecipeTranspiler)}.", LogLevel.Error);
            return null;
        }

        codeMatcher
            .Advance(1)
            .Insert(
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patches), nameof(GetQueenOfSauceWeek))),
                new CodeInstruction(OpCodes.Stloc_0));

        return codeMatcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction>? TVgetRerunWeekTranspiler(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Game1), nameof(Game1.stats))),
            new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Stats), nameof(Stats.DaysPlayed))),
            new CodeMatch(OpCodes.Ldc_I4_3),
            new CodeMatch(OpCodes.Sub),
            new CodeMatch(OpCodes.Ldc_I4_7),
            new CodeMatch(OpCodes.Div),
            new CodeMatch(OpCodes.Ldc_I4_S));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log($"Failed to patch {nameof(TVgetRerunWeekTranspiler)}.", LogLevel.Error);
            return null;
        }

        codeMatcher
            .RemoveInstruction()
            .Insert(
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(typeof(Patches), nameof(GetQueenOfSauceRerunMaxWeek))));

        return codeMatcher.InstructionEnumeration();
    }

    private static int GetQueenOfSauceWeek()
    {
        Dictionary<string, string> recipes = DataLoader.Tv_CookingChannel(Game1.temporaryContent);

        int whichWeek = (int)Game1.stats.DaysPlayed % recipes.Count;

        return whichWeek == 0 ? recipes.Count : whichWeek;
    }

    private static int GetQueenOfSauceRerunMaxWeek()
    {
        return DataLoader.Tv_CookingChannel(Game1.temporaryContent).Count;
    }
}

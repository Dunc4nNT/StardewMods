// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework;

using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

internal class Patches()
{
    private static bool doConsumeStamina;

    private static float? staminaBeforeSuccessfulUse;

    private static float? staminaOnSuccessfulUse;

    private static ModEntry? Mod { get; set; }

    public static void Patch(ModEntry mod)
    {
        mod.Harmony.Patch(
            original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.DoFunction)),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoFunction_Transpiler)));

        mod.Harmony.Patch(
            original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.DoFunction)),
            postfix: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoFunction_Postfix)));
    }

    public static IEnumerable<CodeInstruction>? FishingRod_DoFunction_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        // First, we find where the game subtracts stamina (so a call to the Farmer.Stamina setter).
        // The fishing rod has the checkForExhaustion popup here, while other tools have it in useTool.
        // We're going to remove both the subtracting of stamina, and the check for exhaustion, since we'll do that later.
        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Callvirt, typeof(Farmer).GetProperty(nameof(Farmer.Stamina))?.SetMethod),
            new CodeMatch(OpCodes.Ldarg_S),
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.checkForExhaustion))));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoFunction_Transpiler)}. "
                + "Match for set_StaminaToConsumeOnSuccessfulUse entry point was invalid.",
                LogLevel.Error);
            return null;
        }

        // Remove the 4 lines we just found in the match.
        codeMatcher.RemoveInstructions(4);

        // Save the value that would've been subtracted from our stamina to a variable we can use later in the postfix.
        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(SetStaminaOnSuccessfulUse))));

        // We need to go through every place where there is a valid tool use.
        // For FishingRod this is only one place, right after the location.CanFishHere() && location.isTileFishable(x, y).
        // This is where we'll insert our variable to true, which we use in the postfix to check whether this tool was successfully used.
        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Callvirt, typeof(GameLocation).GetMethod(nameof(GameLocation.isTileFishable))),
            new CodeMatch(OpCodes.Brfalse),
            new CodeMatch(OpCodes.Ldarg_0));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoFunction_Transpiler)}. "
                + "Match for SetDoConsumeStaminaToTrue entry point was invalid.",
                LogLevel.Error);
            return null;
        }

        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(SetDoConsumeStaminaToTrue))));

        return codeMatcher.InstructionEnumeration();
    }

    public static void FishingRod_DoFunction_Postfix(Farmer who)
    {
        doConsumeStamina = Mod?.Config.FishingRodStaminaConsumption switch
        {
            StaminaConsumptionOption.Never => false,
            StaminaConsumptionOption.Vanilla when staminaOnSuccessfulUse < staminaBeforeSuccessfulUse => true,
            _ => doConsumeStamina,
        };

        float oldStamina = who.Stamina;
        TryConsumeStamina(who);
        who.checkForExhaustion(oldStamina);
    }

    public static void SetStaminaOnSuccessfulUse(Farmer who, float value)
    {
        staminaBeforeSuccessfulUse = who.Stamina;
        staminaOnSuccessfulUse = value;
    }

    public static void SetDoConsumeStaminaToTrue()
    {
        doConsumeStamina = true;
    }

    private static void ResetState()
    {
        doConsumeStamina = false;
        staminaBeforeSuccessfulUse = null;
        staminaOnSuccessfulUse = null;
    }

    private static void TryConsumeStamina(Farmer who)
    {
        if (doConsumeStamina && staminaOnSuccessfulUse is not null)
        {
            who.Stamina = (float)staminaOnSuccessfulUse;
        }

        ResetState();
    }
}

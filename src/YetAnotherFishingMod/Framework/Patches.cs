// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// ReSharper disable InconsistentNaming
namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Locations;
using StardewValley.Internal;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Menus;
using StardewValley.Tools;
using SObject = StardewValley.Object;

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
            AccessTools.Method(typeof(FishingRod), nameof(FishingRod.tickUpdate)),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRodTickUpdatePatch)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(BobberBar), nameof(BobberBar.update)),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(BobberBar_Update_Transpiler)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(FishingRod), "doDoneFishing"),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoDoneFishing_Transpiler)));
        Mod.Harmony.Patch(
            AccessTools.Method(
                typeof(GameLocation),
                "GetFishFromLocationData",
                [
                    typeof(string), typeof(Vector2), typeof(int), typeof(Farmer), typeof(bool), typeof(bool),
                    typeof(GameLocation), typeof(ItemQueryContext)
                ]),
            transpiler: new HarmonyMethod(typeof(Patches), nameof(GameLocation_GetFishFromLocationData_Transpiler)));
        Mod.Harmony.Patch(
            AccessTools.Method(typeof(FishingRod), nameof(FishingRod.pullFishFromWater)),
            new HarmonyMethod(typeof(Patches), nameof(PullFishFromWaterPatch)));
    }

    public static IEnumerable<CodeInstruction> FishingRodTickUpdatePatch(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        // If AutoLootFish is enabled, jump to the this.doneHoldingFish(who) call.
        // Insert our own code inside the for loop, just after the who.IsLocalPlayer call
        // Find a unique match to get the brtrue jump label.
        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Ldloc_0),
            new CodeMatch(OpCodes.Ldfld),
            new CodeMatch(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.IsLocalPlayer))),
            new CodeMatch(OpCodes.Brfalse),
            new CodeMatch(OpCodes.Ldsfld),
            new CodeMatch(OpCodes.Callvirt, typeof(InputState).GetMethod(nameof(InputState.GetMouseState))),
            new CodeMatch(OpCodes.Stloc_1),
            new CodeMatch(OpCodes.Ldloca_S),
            new CodeMatch(OpCodes.Call, typeof(MouseState).GetProperty(nameof(MouseState.LeftButton))?.GetMethod),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Beq_S),
            new CodeMatch(OpCodes.Ldc_I4_0),
            new CodeMatch(OpCodes.Call, typeof(Game1).GetMethod(nameof(Game1.didPlayerJustClickAtAll))),
            new CodeMatch(OpCodes.Brtrue_S));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log($"Failed to patch {nameof(FishingRodTickUpdatePatch)}. Match not found.", LogLevel.Error);
            return instructions;
        }

        object jumpToDoneHoldingFishLabel = codeMatcher.Instruction.Clone().operand;

        if (jumpToDoneHoldingFishLabel is not Label)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRodTickUpdatePatch)}. jumpToDoneHoldingFishLabel is not a label.",
                LogLevel.Error);
            return instructions;
        }

        // Go back to the same match, but just after the who.IsLocalPlayer call,
        // insert our jump there.
        codeMatcher.Start();
        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Ldloc_0),
            new CodeMatch(OpCodes.Ldfld),
            new CodeMatch(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.IsLocalPlayer))),
            new CodeMatch(OpCodes.Brfalse),
            new CodeMatch(OpCodes.Ldsfld),
            new CodeMatch(OpCodes.Callvirt, typeof(InputState).GetMethod(nameof(InputState.GetMouseState))),
            new CodeMatch(OpCodes.Stloc_1),
            new CodeMatch(OpCodes.Ldloca_S),
            new CodeMatch(OpCodes.Call, typeof(MouseState).GetProperty(nameof(MouseState.LeftButton))?.GetMethod),
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Beq_S),
            new CodeMatch(OpCodes.Ldc_I4_0),
            new CodeMatch(OpCodes.Call, typeof(Game1).GetMethod(nameof(Game1.didPlayerJustClickAtAll))),
            new CodeMatch(OpCodes.Brtrue_S)).Advance(4);

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log($"Failed to patch {nameof(FishingRodTickUpdatePatch)}. Match not found.", LogLevel.Error);
            return instructions;
        }

        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(DoAutoLootFish))),
            new CodeInstruction(OpCodes.Brtrue, jumpToDoneHoldingFishLabel));

        return codeMatcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction>? BobberBar_Update_Transpiler(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        Label skipVibrationsLabel = generator.DefineLabel();

        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Ldfld, typeof(BobberBar).GetField(nameof(BobberBar.distanceFromCatching))),
            new CodeMatch(OpCodes.Ldc_R4, 0.002f),
            new CodeMatch(OpCodes.Add),
            new CodeMatch(OpCodes.Stfld, typeof(BobberBar).GetField(nameof(BobberBar.distanceFromCatching))));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"distanceFromCatching\" was invalid.",
                LogLevel.Error);
            return null;
        }

        codeMatcher
            .Advance(1)
            .RemoveInstruction()
            .Insert(new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(FishInBarMultiplier))));

        codeMatcher.Start();

        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Ldfld, typeof(BobberBar).GetField(nameof(BobberBar.treasureCatchLevel))),
            new CodeMatch(OpCodes.Ldc_R4, 0.0135f),
            new CodeMatch(OpCodes.Add),
            new CodeMatch(OpCodes.Stfld, typeof(BobberBar).GetField(nameof(BobberBar.treasureCatchLevel))));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"treasureCatchLevel\" was invalid.",
                LogLevel.Error);
            return null;
        }

        codeMatcher
            .Advance(1)
            .RemoveInstruction()
            .Insert(new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(TreasureInBarMultiplier))));

        codeMatcher.Start();

        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Ldc_R4),
            new CodeMatch(OpCodes.Ldc_R4),
            new CodeMatch(
                OpCodes.Call,
                AccessTools.Method(typeof(Rumble), nameof(Rumble.rumble), [typeof(float), typeof(float)])));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"Rumble.rumble\" was invalid.",
                LogLevel.Error);
            return null;
        }

        codeMatcher.InsertAndAdvance(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(DoSkipVibration))),
            new CodeInstruction(OpCodes.Brtrue, skipVibrationsLabel));

        codeMatcher.Advance(3);

        codeMatcher.AddLabels([skipVibrationsLabel]);

        return codeMatcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction> FishingRod_DoDoneFishing_Transpiler(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        // To skip bait consumption when infinite bait is enabled, jump past the if statement wherein bait is consumed.
        // Finds a unique matching point (GetBait() call), and copies the label from the brfalse (on bait != null).
        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetBait))),
            new CodeMatch(OpCodes.Stloc_2),
            new CodeMatch(OpCodes.Ldloc_2),
            new CodeMatch(OpCodes.Brfalse_S));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for bait label entry point is invalid.",
                LogLevel.Error);
            return instructions;
        }

        object skipConsumeBaitLabel = codeMatcher.Instruction.Clone().operand;

        if (skipConsumeBaitLabel is not Label)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. skipConsumeBaitLabel is not a label.",
                LogLevel.Error);
            return instructions;
        }

        // Similarly, find a unique matching point to insert our bait consumption skip.
        // In this case it's being inserted right after the bait != null check after GetBait().
        // This skips the next part of the if statement (the ConsumeStack call), which would consume bait,
        // as well as skipping past the if statement entirely (treating the if statement as false).
        codeMatcher.Start();
        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetBait))),
            new CodeMatch(OpCodes.Stloc_2),
            new CodeMatch(OpCodes.Ldloc_2));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for infinite bait entry point is invalid.",
                LogLevel.Error);
            return instructions;
        }

        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(HasInfiniteBait))),
            new CodeInstruction(OpCodes.Brtrue, skipConsumeBaitLabel));

        // To skip tackle consumption when infinite tackles is enabled, skip the entire foreach loop its consumed inside.
        codeMatcher.Start();

        // Finds the break; inside the foreach loop, which has a label jumping us past the loop.
        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(OpCodes.Callvirt, typeof(Item).GetProperty(nameof(Item.QualifiedItemId))?.GetMethod),
            new CodeMatch(OpCodes.Ldstr),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Brfalse_S),
            new CodeMatch(OpCodes.Leave_S));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for tackle label entry point is invalid.",
                LogLevel.Error);
            return instructions;
        }

        object skipConsumeTackleLabel = codeMatcher.Instruction.Clone().operand;

        if (skipConsumeTackleLabel is not Label)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. skipConsumeTackleLabel is not a label.",
                LogLevel.Error);
            return instructions;
        }

        // Finds a unique matching point right before the foreach loop and advances to the start of it,
        // and inserts the infinite tackle jump.
        codeMatcher.Start();
        codeMatcher.MatchStartForward(
            new CodeMatch(OpCodes.Ldc_I4_1),
            new CodeMatch(OpCodes.Stloc_3),
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetTackle))),
            new CodeMatch(OpCodes.Callvirt),
            new CodeMatch(OpCodes.Stloc_S)).Advance(2);

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for infinite tackle entry point is invalid.",
                LogLevel.Error);
            return instructions;
        }

        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(HasInfiniteTackle))),
            new CodeInstruction(OpCodes.Brtrue, skipConsumeTackleLabel));

        return codeMatcher.InstructionEnumeration();
    }

    public static IEnumerable<CodeInstruction>? GameLocation_GetFishFromLocationData_Transpiler(
        ILGenerator generator,
        IEnumerable<CodeInstruction> instructions)
    {
        CodeMatcher codeMatcher = new(instructions, generator);

        codeMatcher.MatchEndForward(
            new CodeMatch(OpCodes.Ldloc_S),
            new CodeMatch(
                OpCodes.Callvirt,
                typeof(IEnumerable<SpawnFishData>).GetMethod(nameof(IEnumerable<SpawnFishData>.GetEnumerator))),
            new CodeMatch(OpCodes.Stloc_S));

        if (!codeMatcher.IsValid)
        {
            Mod?.Monitor.Log(
                $"Failed to patch {nameof(GameLocation_GetFishFromLocationData_Transpiler)}. " +
                "Match for possibleFish entry point was invalid.",
                LogLevel.Error);
            return null;
        }

        codeMatcher.Advance(-1);

        codeMatcher.RemoveInstruction();

        codeMatcher.Insert(
            new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(FilterPossibleFish))),
            new CodeInstruction(
                OpCodes.Callvirt,
                typeof(IEnumerable<SpawnFishData>).GetMethod(nameof(IEnumerable<SpawnFishData>.GetEnumerator))));

        return codeMatcher.InstructionEnumeration();
    }

    public static IEnumerable<SpawnFishData> FilterPossibleFish(IEnumerable<SpawnFishData> possibleFish)
    {
        List<SpawnFishData> filteredPossibleFish = [];
        filteredPossibleFish.AddRange(
            possibleFish.Where(fish => IsFishInPreferredCategory(ItemRegistry.GetData(fish.ItemId))));

        return filteredPossibleFish.AsEnumerable();
    }

    public static bool IsFishInPreferredCategory(ParsedItemData? fish)
    {
        if (Mod is null)
        {
            return false;
        }

        if (Mod.Config is { AllowCatchingFish: false, AllowCatchingRubbish: false, AllowCatchingOther: false })
        {
            return true;
        }

        if (fish is null)
        {
            return Mod.Config.AllowCatchingRubbish;
        }

        return (fish.Category == SObject.FishCategory && Mod.Config.AllowCatchingFish) ||
               (fish.Category == SObject.junkCategory && Mod.Config.AllowCatchingRubbish) ||
               (fish.Category != SObject.FishCategory && fish.Category != SObject.junkCategory &&
                Mod.Config.AllowCatchingOther);
    }

    public static bool DoSkipVibration()
    {
        return Mod?.Config.DisableVibrations ?? false;
    }

    public static bool HasInfiniteTackle()
    {
        return Mod?.Config.InfiniteTackle ?? false;
    }

    public static bool HasInfiniteBait()
    {
        return Mod?.Config.InfiniteBait ?? false;
    }

    public static float TreasureInBarMultiplier()
    {
        return 0.0135f * Mod?.Config.TreasureInBarMultiplier ?? 0.0135f;
    }

    public static float FishInBarMultiplier()
    {
        return 0.002f * Mod?.Config.FishInBarMultiplier ?? 0.002f;
    }

    public static bool DoAutoLootFish()
    {
        return Mod?.Config.AutoLootFish ?? false;
    }

    public static bool PullFishFromWaterPatch(ref int fishDifficulty)
    {
        try
        {
            if (Mod?.Config is { AdjustXpGainDifficulty: false, DifficultyMultiplier: > 0 })
            {
                fishDifficulty = (int)(fishDifficulty / Mod.Config.DifficultyMultiplier);
            }

            return true;
        }
        catch (Exception e)
        {
            Mod?.Monitor.Log($"Failed in {nameof(PullFishFromWaterPatch)}:\n{e}", LogLevel.Error);
            return true;
        }
    }
}

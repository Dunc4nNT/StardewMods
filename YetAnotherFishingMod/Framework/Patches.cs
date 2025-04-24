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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        public static void ApplyPatches()
        {
            s_harmony.Patch(
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.tickUpdate)),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRodTickUpdatePatch))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(BobberBar), nameof(BobberBar.update)),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(BobberBar_Update_Transpiler))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(FishingRod), "doDoneFishing"),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoDoneFishing_Transpiler))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(GameLocation), "GetFishFromLocationData", [typeof(string), typeof(Vector2), typeof(int), typeof(Farmer), typeof(bool), typeof(bool), typeof(GameLocation), typeof(ItemQueryContext)]),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(GameLocation_GetFishFromLocationData_Transpiler))
            );
            s_harmony.Patch(AccessTools.Method(typeof(FishingRod), nameof(FishingRod.pullFishFromWater)),
                prefix: new HarmonyMethod(typeof(Patches), nameof(PullFishFromWaterPatch))
            );
        }

        public static IEnumerable<CodeInstruction> FishingRodTickUpdatePatch(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            // If AutoLootFish is enabled, jump to the this.doneHoldingFish(who) call.
            // Insert our own code inside of the for loop, just after the who.IsLocalPlayer call
            // Find a unique match to get the brtrue jump label.
            codeMatcher.MatchEndForward(
                new(OpCodes.Ldloc_0),
                new(OpCodes.Ldfld),
                new(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.IsLocalPlayer))),
                new(OpCodes.Brfalse),
                new(OpCodes.Ldsfld),
                new(OpCodes.Callvirt, typeof(InputState).GetMethod(nameof(InputState.GetMouseState))),
                new(OpCodes.Stloc_1),
                new(OpCodes.Ldloca_S),
                new(OpCodes.Call, typeof(MouseState).GetProperty(nameof(MouseState.LeftButton)).GetMethod),
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Beq_S),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Call, typeof(Game1).GetMethod(nameof(Game1.didPlayerJustClickAtAll))),
                new(OpCodes.Brtrue_S)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRodTickUpdatePatch)}. Match not found.", LogLevel.Error);
                return instructions;
            }

            object jumpToDoneHoldingFishLabel = codeMatcher.Instruction.Clone().operand;

            if (jumpToDoneHoldingFishLabel is not Label)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRodTickUpdatePatch)}. jumpToDoneHoldingFishLabel is not a label.", LogLevel.Error);
                return instructions;
            }

            // Go back to the same match, but just after the who.IsLocalPlayer call,
            // insert our jump there.
            codeMatcher.Start();
            codeMatcher.MatchStartForward(
                new(OpCodes.Ldloc_0),
                new(OpCodes.Ldfld),
                new(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.IsLocalPlayer))),
                new(OpCodes.Brfalse),
                new(OpCodes.Ldsfld),
                new(OpCodes.Callvirt, typeof(InputState).GetMethod(nameof(InputState.GetMouseState))),
                new(OpCodes.Stloc_1),
                new(OpCodes.Ldloca_S),
                new(OpCodes.Call, typeof(MouseState).GetProperty(nameof(MouseState.LeftButton)).GetMethod),
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Beq_S),
                new(OpCodes.Ldc_I4_0),
                new(OpCodes.Call, typeof(Game1).GetMethod(nameof(Game1.didPlayerJustClickAtAll))),
                new(OpCodes.Brtrue_S)
            ).Advance(4);

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRodTickUpdatePatch)}. Match not found.", LogLevel.Error);
                return instructions;
            }

            codeMatcher.Insert(
                new(OpCodes.Call, typeof(Patches).GetMethod(nameof(DoAutoLootFish))),
                new(OpCodes.Brtrue, jumpToDoneHoldingFishLabel)
            );

            return codeMatcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> BobberBar_Update_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            Label skipVibrationsLabel = generator.DefineLabel();

            codeMatcher.MatchStartForward(
                new(OpCodes.Ldfld, typeof(BobberBar).GetField(nameof(BobberBar.distanceFromCatching))),
                new(OpCodes.Ldc_R4, 0.002f),
                new(OpCodes.Add),
                new(OpCodes.Stfld, typeof(BobberBar).GetField(nameof(BobberBar.distanceFromCatching)))
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"distanceFromCatching\" was invalid.", LogLevel.Error);
                return null;
            }

            codeMatcher
                .Advance(1)
                .RemoveInstruction()
                .Insert(
                    new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(FishInBarMultiplier)))
                );

            codeMatcher.Start();

            codeMatcher.MatchStartForward(
                new(OpCodes.Ldfld, typeof(BobberBar).GetField(nameof(BobberBar.treasureCatchLevel))),
                new(OpCodes.Ldc_R4, 0.0135f),
                new(OpCodes.Add),
                new(OpCodes.Stfld, typeof(BobberBar).GetField(nameof(BobberBar.treasureCatchLevel)))
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"treasureCatchLevel\" was invalid.", LogLevel.Error);
                return null;
            }

            codeMatcher
                .Advance(1)
                .RemoveInstruction()
                .Insert(
                    new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(TreasureInBarMultiplier)))
                );

            codeMatcher.Start();

            codeMatcher.MatchStartForward(
                new(OpCodes.Ldc_R4),
                new(OpCodes.Ldc_R4),
                new(OpCodes.Call, AccessTools.Method(typeof(Rumble), nameof(Rumble.rumble), [typeof(float), typeof(float)]))
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(BobberBar_Update_Transpiler)}. Match for \"Rumble.rumble\" was invalid.", LogLevel.Error);
                return null;
            }

            codeMatcher.InsertAndAdvance(
                new(OpCodes.Call, typeof(Patches).GetMethod(nameof(DoSkipVibration))),
                new(OpCodes.Brtrue, skipVibrationsLabel)
            );

            codeMatcher.Advance(3);

            codeMatcher.AddLabels(new[] { skipVibrationsLabel });

            return codeMatcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> FishingRod_DoDoneFishing_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            // To skip bait consumption when infinite bait is enabled, jump past the if statement wherein bait is consumed.
            // Finds a unique matching point (GetBait() call), and copies the label from the brfalse (on bait != null).
            codeMatcher.MatchEndForward(
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetBait))),
                new(OpCodes.Stloc_2),
                new(OpCodes.Ldloc_2),
                new(OpCodes.Brfalse_S)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for bait label entry point is invalid.", LogLevel.Error);
                return instructions;
            }

            object skipConsumeBaitLabel = codeMatcher.Instruction.Clone().operand;

            if (skipConsumeBaitLabel is not Label)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. skipConsumeBaitLabel is not a label.", LogLevel.Error);
                return instructions;
            }

            // Similarly, find a unique matching point to insert our bait consumption skip.
            // In this case it's being inserted right after the bait != null check after GetBait().
            // This skips the next part of the if statement (the ConsumeStack call), which would consume bait,
            // as well as skipping past the if statement entirely (treating the if statement as if it was false).
            codeMatcher.Start();
            codeMatcher.MatchEndForward(
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetBait))),
                new(OpCodes.Stloc_2),
                new(OpCodes.Ldloc_2)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for infinite bait entry point is invalid.", LogLevel.Error);
                return instructions;
            }

            codeMatcher.Insert(
                new(OpCodes.Call, typeof(Patches).GetMethod(nameof(HasInfiniteBait))),
                new(OpCodes.Brtrue, skipConsumeBaitLabel)
            );

            // To skip tackle consumption when infinite tackles is enabled, skip the entire foreach loop its consumed inside of.
            codeMatcher.Start();

            // Finds the break; inside the foreach loop, which has a label jumping us past the loop.
            codeMatcher.MatchEndForward(
                new(OpCodes.Ldloc_S),
                new(OpCodes.Callvirt, typeof(Item).GetProperty(nameof(Item.QualifiedItemId)).GetMethod),
                new(OpCodes.Ldstr),
                new(OpCodes.Call),
                new(OpCodes.Brfalse_S),
                new(OpCodes.Leave_S)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for tackle label entry point is invalid.", LogLevel.Error);
                return instructions;
            }

            object skipConsumeTackleLabel = codeMatcher.Instruction.Clone().operand;

            if (skipConsumeTackleLabel is not Label)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. skipConsumeTackleLabel is not a label.", LogLevel.Error);
                return instructions;
            }

            // Finds a unique matching point right before the foreach loop and advances to the start of it,
            // and inserts the infinite tackle jump.
            codeMatcher.Start();
            codeMatcher.MatchStartForward(
                new(OpCodes.Ldc_I4_1),
                new(OpCodes.Stloc_3),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, typeof(FishingRod).GetMethod(nameof(FishingRod.GetTackle))),
                new(OpCodes.Callvirt),
                new(OpCodes.Stloc_S)
            ).Advance(2);

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoDoneFishing_Transpiler)}. Match for infinite tackle entry point is invalid.", LogLevel.Error);
                return instructions;
            }

            codeMatcher.Insert(
                new(OpCodes.Call, typeof(Patches).GetMethod(nameof(HasInfiniteTackle))),
                new(OpCodes.Brtrue, skipConsumeTackleLabel)
            );

            return codeMatcher.InstructionEnumeration();
        }

        public static IEnumerable<CodeInstruction> GameLocation_GetFishFromLocationData_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            codeMatcher.MatchEndForward(
                new CodeMatch(OpCodes.Ldloc_S),
                new(OpCodes.Callvirt, typeof(IEnumerable<SpawnFishData>).GetMethod(nameof(IEnumerable<SpawnFishData>.GetEnumerator))),
                new(OpCodes.Stloc_S)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(GameLocation_GetFishFromLocationData_Transpiler)}. Match for possibleFish entry point was invalid.", LogLevel.Error);
                return null;
            }

            codeMatcher.Advance(-1);

            codeMatcher.RemoveInstruction();

            codeMatcher.Insert(
                new(OpCodes.Call, typeof(Patches).GetMethod(nameof(FilterPossibleFish))),
                new(OpCodes.Callvirt, typeof(IEnumerable<SpawnFishData>).GetMethod(nameof(IEnumerable<SpawnFishData>.GetEnumerator)))
            );

            return codeMatcher.InstructionEnumeration();
        }

        public static IEnumerable<SpawnFishData> FilterPossibleFish(IEnumerable<SpawnFishData> possibleFish)
        {
            List<SpawnFishData> filteredPossibleFish = [];

            foreach (SpawnFishData fish in possibleFish)
            {
                if (IsFishInPreferredCategory(ItemRegistry.GetData(fish.ItemId)))
                    filteredPossibleFish.Add(fish);
            }

            return filteredPossibleFish.AsEnumerable();
        }

        public static bool IsFishInPreferredCategory(ParsedItemData fish)
        {
            ModConfig config = s_config();
            if (!config.AllowCatchingFish && !config.AllowCatchingRubbish && !config.AllowCatchingOther) return true;
            if (fish == null) return config.AllowCatchingRubbish;

            if ((fish.Category == SObject.FishCategory && config.AllowCatchingFish) ||
                ((fish.Category == SObject.junkCategory) && config.AllowCatchingRubbish) ||
                (fish.Category != SObject.FishCategory && fish.Category != SObject.junkCategory && config.AllowCatchingOther))
                return true;

            return false;
        }

        public static bool DoSkipVibration()
        {
            return s_config().DisableVibrations;
        }

        public static bool HasInfiniteTackle()
        {
            return s_config().InfiniteTackle;
        }

        public static bool HasInfiniteBait()
        {
            return s_config().InfiniteBait;
        }

        public static float TreasureInBarMultiplier()
        {
            return 0.0135f * s_config().TreasureInBarMultiplier;
        }

        public static float FishInBarMultiplier()
        {
            return 0.002f * s_config().FishInBarMultiplier;
        }

        public static bool DoAutoLootFish()
        {
            return s_config().AutoLootFish;
        }

        public static bool PullFishFromWaterPatch(ref int fishDifficulty)
        {
            try
            {
                ModConfig config = s_config();

                if (!config.AdjustXpGainDifficulty && config.DifficultyMultiplier > 0)
                    fishDifficulty = (int)(fishDifficulty / config.DifficultyMultiplier);

                return true;
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(PullFishFromWaterPatch)}:\n{e}", LogLevel.Error);
                return true;
            }
        }
    }
}

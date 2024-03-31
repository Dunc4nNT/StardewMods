using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
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

        private static readonly int s_retry_catch_fish_amount = 100;

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
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.tickUpdate)),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRodTickUpdatePatch))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(BobberBar), nameof(BobberBar.update)),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(BobberBar_Update_Transpiler))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.getFish)),
                postfix: new HarmonyMethod(typeof(Patches), nameof(GetFishPatch))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.doneFishing)),
                prefix: new HarmonyMethod(typeof(Patches), nameof(DoneFishingPatch))
            );
            s_harmony.Patch(AccessTools.Method(typeof(FishingRod), nameof(FishingRod.pullFishFromWater)),
                prefix: new HarmonyMethod(typeof(Patches), nameof(PullFishFromWaterPatch))
            );
        }

        private static IEnumerable<CodeInstruction> FishingRodTickUpdatePatch(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> output = [];

            Label label = generator.DefineLabel();

            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == "coin")
                {
                    output.Insert(output.Count - 17, new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => DoAutoLootFish())));
                    output.Insert(output.Count - 17, new CodeInstruction(OpCodes.Brtrue, label));
                    output[^2].labels.Add(label);
                }
                output.Add(instruction);
            }

            return output;
        }

        private static IEnumerable<CodeInstruction> BobberBar_Update_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

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
                    new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => FishInBarMultiplier()))
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
                    new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => TreasureInBarMultiplier()))
                );

            return codeMatcher.InstructionEnumeration();
        }

        private static float TreasureInBarMultiplier()
        {
            return 0.0135f * s_config().TreasureInBarMultiplier;
        }

        private static float FishInBarMultiplier()
        {
            return 0.002f * s_config().FishInBarMultiplier;
        }

        private static bool DoAutoLootFish()
        {
            return s_config().AutoLootFish;
        }

        private static void GetFishPatch(ref GameLocation __instance, ref Item __result, Farmer who, int waterDepth, Vector2 bobberTile)
        {
            try
            {
                ModConfig config = s_config();

                if (IsFishInPreferredCategory(__result))
                    return;

                bool isTutorialCatch = who.fishCaught.Length == 0;

                for (int i = 0; i < s_retry_catch_fish_amount; i++)
                {
                    __result = GameLocation.GetFishFromLocationData(__instance.Name, bobberTile, waterDepth, who, isTutorialCatch, isInherited: false, __instance);

                    if (IsFishInPreferredCategory(__result))
                        return;
                }
            }
            catch (Exception e)
            {
                s_monitor.Log($"Failed in {nameof(GetFishPatch)}:\n{e}", LogLevel.Error);
            }
        }

        private static bool IsFishInPreferredCategory(Item item)
        {
            ModConfig config = s_config();

            if (!config.AllowCatchingFish && !config.AllowCatchingRubbish && !config.AllowCatchingOther)
                return true;

            if ((item.Category == SObject.FishCategory && config.AllowCatchingFish) ||
                ((item.Category == SObject.junkCategory || item == null) && config.AllowCatchingRubbish) ||
                (item.Category == 0 && config.AllowCatchingOther))
                return true;

            return false;
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

        private static bool PullFishFromWaterPatch(ref int fishDifficulty)
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

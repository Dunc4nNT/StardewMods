using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NeverToxic.StardewMods.NoStaminaWasted.Framework
{
    internal class Patches()
    {
        private static Harmony s_harmony;
        private static IMonitor s_monitor;
        private static Func<ModConfig> s_config;
        private static IReflectionHelper s_reflectionHelper;
        public static bool DoConsumeStamina = false;
        public static float? StaminaBeforeSuccessfulUse = null;
        public static float? StaminaOnSuccessfulUse = null;

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
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.DoFunction)),
                transpiler: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoFunction_Transpiler))
            );
            s_harmony.Patch(
                original: AccessTools.Method(typeof(FishingRod), nameof(FishingRod.DoFunction)),
                postfix: new HarmonyMethod(typeof(Patches), nameof(FishingRod_DoFunction_Postfix))
            );
        }

        public static IEnumerable<CodeInstruction> FishingRod_DoFunction_Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher codeMatcher = new(instructions, generator);

            // First, we find where the game subtracts stamina (so a call to the Farmer.Stamina setter).
            // The fishing rod has the checkForExhaustion popup here, while other tools have it in useTool.
            // We're going to remove both the subtracting of stamina, as well as the check for exhaustion, since we'll do that later.
            codeMatcher.MatchStartForward(
                new(OpCodes.Callvirt, typeof(Farmer).GetProperty(nameof(Farmer.Stamina)).SetMethod),
                new(OpCodes.Ldarg_S),
                new(OpCodes.Ldloc_S),
                new(OpCodes.Callvirt, typeof(Farmer).GetMethod(nameof(Farmer.checkForExhaustion)))
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoFunction_Transpiler)}. Match for set_StaminaToConsumeOnSuccessfulUse entry point was invalid.", LogLevel.Error);
                return null;
            }

            // Remove the 4 lines we just found in the match.
            codeMatcher.RemoveInstructions(4);

            // Save the value that would've been subtracted from our stamina to a variable we can use later in the postfix.
            codeMatcher.Insert(
                new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(SetStaminaOnSuccessfulUse)))
            );

            // We need to go through every place where there is a valid tool use.
            // For FishingRod this is only one place, right after the location.CanFishHere() && location.isTileFishable(x, y).
            // This is where we'll insert our variable to true, which we use in the postfix to check whether this tool was successfully used.
            codeMatcher.MatchEndForward(
                new(OpCodes.Callvirt, typeof(GameLocation).GetMethod(nameof(GameLocation.isTileFishable))),
                new(OpCodes.Brfalse),
                new(OpCodes.Ldarg_0)
            );

            if (!codeMatcher.IsValid)
            {
                s_monitor.Log($"Failed to patch {nameof(FishingRod_DoFunction_Transpiler)}. Match for SetDoConsumeStaminaToTrue entry point was invalid.", LogLevel.Error);
                return null;
            }

            codeMatcher.Insert(
                new CodeInstruction(OpCodes.Call, typeof(Patches).GetMethod(nameof(SetDoConsumeStaminaToTrue)))
            );

            return codeMatcher.InstructionEnumeration();
        }

        public static void FishingRod_DoFunction_Postfix(Farmer who)
        {
            ModConfig config = s_config();

            if (config.FishingRodStaminaConsumption == StaminaConsumptionOption.Never)
            {
                DoConsumeStamina = false;
            }
            else if (config.FishingRodStaminaConsumption == StaminaConsumptionOption.Vanilla && StaminaOnSuccessfulUse < StaminaBeforeSuccessfulUse)
            {
                DoConsumeStamina = true;
            }

            float oldStamina = who.Stamina;
            TryConsumeStamina(who);
            who.checkForExhaustion(oldStamina);
        }

        public static void TryConsumeStamina(Farmer who)
        {
            if (DoConsumeStamina && StaminaOnSuccessfulUse is not null)
            {
                who.Stamina = (float)StaminaOnSuccessfulUse;
            }

            ResetState();
        }

        public static void SetStaminaOnSuccessfulUse(Farmer who, float value)
        {
            StaminaBeforeSuccessfulUse = who.Stamina;
            StaminaOnSuccessfulUse = value;
        }

        public static void SetDoConsumeStaminaToTrue()
        {
            DoConsumeStamina = true;
        }

        public static void ResetState()
        {
            DoConsumeStamina = false;
            StaminaBeforeSuccessfulUse = null;
            StaminaOnSuccessfulUse = null;
        }
    }
}

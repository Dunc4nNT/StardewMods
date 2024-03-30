using System.Collections.Generic;

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public bool AutoLootFish { get; set; } = false;

        public float FishNotInBarPenaltyMultiplier { get; set; } = 1f;

        public float BarSizeMultiplier { get; set; } = 1f;

        public bool AllowCatchingFish { get; set; } = true;

        public bool AllowCatchingRubbish { get; set; } = true;

        public bool AllowCatchingOther { get; set; } = true;

        public bool SkipFishingMinigame { get; set; } = true;

        public int SkipFishingMinigameCatchesRequired { get; set; } = 0;

        public float SkipFishingMinigamePerfectChance { get; set; } = 1f;

        public float SkipFishingMinigameTreasureChance { get; set; } = 1f;

        public bool InstantCatchTreasure { get; set; } = false;

        public bool AlwaysMaxCastingPower { get; set; } = false;

        public bool AlwaysPerfect { get; set; } = false;

        public TreasureAppearanceSettings TreasureAppearence { get; set; } = TreasureAppearanceSettings.Vanilla;

        public TreasureAppearanceSettings GoldenTreasureAppearance { get; set; } = TreasureAppearanceSettings.Vanilla;

        public int NumberOfFishCaught { get; set; } = 1;

        public bool InstantBite { get; set; } = false;

        public bool AutoHook { get; set; } = false;

        public Quality FishQuality { get; set; } = Quality.Any;

        public Quality MinimumFishQuality { get; set; } = Quality.None;

        public List<string> BaitToSpawn { get; set; } = [""];

        public int AmountOfBait { get; set; } = 1;

        public List<string> TacklesToSpawn { get; set; } = ["", ""];

        public bool OverrideAttachmentLimit { get; set; } = false;

        public bool ResetAttachmentsLimitWhenNotEquipped { get; set; } = true;

        public bool InfiniteBaitAndTackle { get; set; } = false;

        public bool AutoLootTreasure { get; set; } = false;

        public float DifficultyMultiplier { get; set; } = 1.0f;

        public bool AdjustXpGainDifficulty { get; set; } = true;

        public bool DoAddEnchantments { get; set; } = false;

        public bool AddAllEnchantments { get; set; } = false;

        public bool AddAutoHookEnchantment { get; set; } = false;

        public bool AddEfficientToolEnchantment { get; set; } = false;

        public bool AddMasterEnchantment { get; set; } = false;

        public bool AddPreservingEnchantment { get; set; } = false;

        public bool ResetEnchantmentsWhenNotEquipped { get; set; } = true;
    }
}

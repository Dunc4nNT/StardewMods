namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public float FishNotInBarPenaltyMultiplier { get; set; } = 1f;

        public float BarSizeMultiplier { get; set; } = 1f;

        public bool IncreaseChanceOfFish { get; set; } = false;

        public bool SkipFishingMinigame { get; set; } = true;

        public int SkipFishingMinigameCatchesRequired { get; set; } = 0;

        public float SkipFishingMinigamePerfectChance { get; set; } = 1f;

        public float SkipFishingMinigameTreasureChance { get; set; } = 1f;

        public bool InstantCatchTreasure { get; set; } = true;

        public bool AlwaysMaxCastingPower { get; set; } = false;

        public bool AlwaysPerfect { get; set; } = false;

        public TreasureAppearanceSettings TreasureAppearence { get; set; } = TreasureAppearanceSettings.Vanilla;

        public TreasureAppearanceSettings GoldenTreasureAppearance { get; set; } = TreasureAppearanceSettings.Vanilla;

        public int NumberOfFishCaught { get; set; } = 1;

        public bool InstantBite { get; set; } = false;

        public bool AutoHook { get; set; } = false;

        public bool SpawnBaitWhenEquipped { get; set; } = false;

        public Quality FishQuality { get; set; } = Quality.Any;

        public Quality MinimumFishQuality { get; set; } = Quality.None;

        public string SpawnWhichBait { get; set; } = "(O)685";

        public bool SpawnTackleWhenEquipped { get; set; } = false;

        public string SpawnWhichTackle { get; set; } = "(O)686";

        public bool OverrideAttachmentLimit { get; set; } = false;

        public bool ResetAttachmentsWhenNotEquipped { get; set; } = true;

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

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public bool InstantCatchFish { get; set; } = true;

        public bool InstantCatchTreasure { get; set; } = false;

        public bool AlwaysMaxCastingPower { get; set; } = false;

        public bool AlwaysPerfect { get; set; } = false;

        public bool AlwaysCatchTreasure { get; set; } = false;

        public bool AlwaysCatchDouble { get; set; } = false;

        public bool InstantBite { get; set; } = false;

        public bool AutoHook { get; set; } = false;

        public bool SpawnBaitWhenEquipped { get; set; } = false;

        public Quality FishQuality { get; set; } = Quality.Any;

        public Quality MinimumFishQuality { get; set; } = Quality.None;

        public Bait SpawnWhichBait { get; set; } = Bait.Bait;

        public bool SpawnTackleWhenEquipped { get; set; } = false;

        public Tackle SpawnWhichTackle { get; set; } = Tackle.DressedSpinner;

        public bool OverrideAttachmentLimit { get; set; } = false;

        public bool ResetAttachmentsWhenNotEquipped { get; set; } = true;

        public bool InfiniteBait { get; set; } = false;

        public bool InfiniteTackle { get; set; } = false;

        public bool AutoLootTreasure { get; set; } = false;

        public float DifficultyMultiplier { get; set; } = 1.0f;

        public bool DoAddEnchantments { get; set; } = false;

        public bool AddAllEnchantments { get; set; } = false;

        public bool AddAutoHookEnchantment { get; set; } = false;

        public bool AddEfficientToolEnchantment { get; set; } = false;

        public bool AddMasterEnchantment { get; set; } = false;

        public bool AddPreservingEnchantment { get; set; } = false;

        public bool ResetEnchantmentsWhenNotEquipped { get; set; } = true;
    }
}

namespace YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public bool InstantCatchFish { get; set; } = true;

        public bool InstantCatchTreasure { get; set; } = true;

        public bool AlwaysMaxCastingPower { get; set; } = false;

        public bool AlwaysPerfect { get; set; } = false;

        public bool AlwaysCatchTreasure { get; set; } = false;

        public bool AlwaysCatchDouble { get; set; } = true;

        public bool InstantBite { get; set; } = true;

        public bool AutoHook { get; set; } = true;

        public bool SpawnBaitWhenEquipped { get; set; } = true;

        public Quality FishQuality { get; set; } = Quality.Iridium;

        public Quality MinimumFishQuality { get; set; } = Quality.Gold;

        public Bait SpawnWhichBait { get; set; } = Bait.MagicBait;

        public bool SpawnTackleWhenEquipped { get; set; } = true;

        public Tackle SpawnWhichTackle { get; set; } = Tackle.TrapBobber;

        public bool OverrideAttachmentLimit { get; set; } = true;

        public bool ResetAttachmentsWhenNotEquipped { get; set; } = true;

        public bool InfiniteBait { get; set; } = true;

        public bool InfiniteTackle { get; set; } = true;

        public bool AutoLootTreasure { get; set; } = true;

        public bool AutoEquipBait { get; set; } = true;

        public Bait AutoEquipWhichBait { get; set; } = Bait.Bait;

        public bool AutoEquipAnyBait { get; set; } = true;

        public bool AutoEquipTackle { get; set; } = true;

        public Tackle AutoEquipWhichTackle { get; set; } = Tackle.TrapBobber;

        public bool AutoEquipAnyTackle { get; set; } = true;

        public bool AlwaysRefundStamina { get; set; } = true;

        public bool RefundStaminaOnMaxCast { get; set; } = true;

        public bool RefundStaminaOnPerfect { get; set; } = true;

        public float DifficultyMultiplier { get; set; } = 1.0f;

        public bool DoAddEnchantments { get; set; } = true;

        public bool AddAllEnchantments { get; set; } = true;

        public bool AddAutoHookEnchantment { get; set; } = true;

        public bool AddEfficientToolEnchantment { get; set; } = true;

        public bool AddMasterEnchantment { get; set; } = true;

        public bool AddPreservingEnchantment { get; set; } = true;

        public bool ResetEnchantmentsWhenNotEquipped { get; set; } = true;
    }
}

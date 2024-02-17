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

        internal enum Bait { Bait = 685, WildBait = 774, MagicBait = 908 }

        public Bait SpawnWhichBait { get; set; } = Bait.Bait;

        public bool SpawnTackleWhenEquipped { get; set; } = true;

        internal enum Tackle { TreasureHunter = 166, Spinner = 686, DressedSpinner = 687, BarbedHook = 691, LeadBobber = 692, TrapBobber = 694, CorkBobber = 695, CuriosityLure = 856, QualityBobber = 877 }

        public Tackle SpawnWhichTackle { get; set; } = Tackle.TrapBobber;

        public bool OverrideAttachmentLimit { get; set; } = true;

        public bool ResetAttachmentsWhenNotEquipped { get; set; } = true;

        public bool InfiniteBait { get; set; } = true;

        public bool InfiniteTackle { get; set; } = true;

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

        public bool AddEfficientEnchantment { get; set; } = true;

        public bool AddMasterEnchantment { get; set; } = true;

        public bool AddPreservingEnchantment { get; set; } = true;

        public bool ResetEnchantmentsWhenNotEquipped { get; set; } = true;
    }
}

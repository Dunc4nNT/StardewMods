namespace YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public bool InstantCatchFish { get; set; } = true;

        public bool InstantCatchTreasure { get; set; } = true;

        public bool AlwaysMaxCastingPower { get; set; } = true;

        public bool AlwaysCatchTreasure { get; set; } = true;
    }
}

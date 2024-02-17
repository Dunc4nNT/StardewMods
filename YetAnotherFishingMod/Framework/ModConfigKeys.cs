using StardewModdingAPI.Utilities;

namespace YetAnotherFishingMod.Framework
{
    internal class ModConfigKeys
    {
        public KeybindList ReloadConfig { get; set; } = KeybindList.Parse("F5");

        public KeybindList SpawnBait { get; set; }

        public KeybindList SpawnTackle { get; set; }

        public KeybindList DecreaseDifficultyMultiplier { get; set; }

        public KeybindList IncreaseDifficultyMultiplier { get; set; }

        public KeybindList ResetDifficultyMultiplier { get; set; }

        public KeybindList ToggleEnchantments { get; set; }
    }
}

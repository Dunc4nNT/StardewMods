using StardewModdingAPI.Utilities;

namespace FishingMod.Framework
{
    internal class ModConfig
    {
        public KeybindList ReloadConfigButton { get; set; } = KeybindList.Parse("F5");

        public bool InstantCatchFish { get; set; } = false;
    }
}

using StardewModdingAPI.Utilities;

namespace YetAnotherTimeMod.Framework
{
    internal class ModConfigKeys
    {
        public KeybindList ReloadConfig { get; set; } = KeybindList.Parse("F5");

        public KeybindList IncreaseSpeed { get; set; } = KeybindList.Parse("F7");

        public KeybindList DecreaseSpeed { get; set; } = KeybindList.Parse("F6");

        public KeybindList ResetSpeed { get; set; } = KeybindList.Parse("F8");

        public KeybindList ToggleFreeze { get; set; } = KeybindList.Parse("F9");

        public KeybindList HalfSpeed { get; set; } = KeybindList.Parse("F10");

        public KeybindList DoubleSpeed { get; set; } = KeybindList.Parse("F11");
    }
}

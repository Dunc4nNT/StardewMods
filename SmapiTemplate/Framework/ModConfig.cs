﻿using StardewModdingAPI.Utilities;

namespace NeverToxic.StardewMods.SmapiTemplate.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();
    }

    internal class ModConfigKeys
    {
        public KeybindList ReloadConfig { get; set; } = new();
    }

}

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework;

using StardewModdingAPI.Utilities;

internal class ModConfigKeys
{
    public KeybindList ReloadConfig { get; set; } = new();

    public KeybindList DoAutoCast { get; set; } = new();
}

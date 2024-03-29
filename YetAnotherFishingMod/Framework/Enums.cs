using SObject = StardewValley.Object;

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal enum Quality
    {
        Any = -1,
        None = SObject.lowQuality,
        Silver = SObject.medQuality,
        Gold = SObject.highQuality,
        Iridium = SObject.bestQuality
    }

    internal enum TreasureAppearanceSettings
    {
        Vanilla,
        Never,
        Always
    }
}

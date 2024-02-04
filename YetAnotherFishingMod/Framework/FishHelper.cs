using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Tools;

namespace YetAnotherFishingMod.Framework
{
    internal class FishHelper(ModConfig config, IMonitor Monitor, IReflectionHelper reflectionHelper)
    {
        private readonly PerScreen<SBobberBar> _bobberBar = new();
        public readonly PerScreen<bool> IsInFishingMiniGame = new();

        public void ApplyFishingRodBuffs(FishingRod fishingRod_)
        {
            SFishingRod fishingRod = new(fishingRod_, reflectionHelper);

            if (config.AlwaysMaxCastingPower)
                fishingRod.CastingPower = 1.01f;
        }

        public void ApplyFishingMiniGameBuffs()
        {
            if (config.AlwaysPerfect)
                this._bobberBar.Value.Perfect = true;
        }

        public void OnFishingMiniGameStart(BobberBar bobberBar)
        {
            Monitor.Log($"instant catch set to: {config.InstantCatchFish}");
            this.IsInFishingMiniGame.Value = true;
            this._bobberBar.Value = new SBobberBar(bobberBar, reflectionHelper);

            if ((config.InstantCatchTreasure && this._bobberBar.Value.Treasure) || config.AlwaysCatchTreasure)
                this._bobberBar.Value.TreasureCaught = true;
            if (config.InstantCatchFish)
                this._bobberBar.Value.DistanceFromCatching = 1.0f;
        }

        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

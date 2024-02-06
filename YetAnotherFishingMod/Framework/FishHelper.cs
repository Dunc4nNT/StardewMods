using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Tools;
using System;

namespace YetAnotherFishingMod.Framework
{
    internal class FishHelper(Func<ModConfig> config, IMonitor monitor, IReflectionHelper reflectionHelper)
    {
        private readonly PerScreen<SBobberBar> _bobberBar = new();
        public readonly PerScreen<bool> IsInFishingMiniGame = new();

        public void ApplyFishingRodBuffs(FishingRod fishingRod_)
        {
            SFishingRod fishingRod = new(fishingRod_, reflectionHelper);

            ModConfig config_ = config();

            if (config_.AlwaysMaxCastingPower)
                fishingRod.CastingPower = 1.01f;
            if (config_.AlwaysCatchDouble)
                fishingRod.CaughtDoubleFish = true;
        }

        public void ApplyFishingMiniGameBuffs()
        {
            if (config().AlwaysPerfect)
                this._bobberBar.Value.Perfect = true;
        }

        public void OnFishingMiniGameStart(BobberBar bobberBar)
        {
            this.IsInFishingMiniGame.Value = true;
            this._bobberBar.Value = new SBobberBar(bobberBar, reflectionHelper);

            ModConfig config_ = config();

            if ((config_.InstantCatchTreasure && this._bobberBar.Value.Treasure) || config_.AlwaysCatchTreasure)
                this._bobberBar.Value.TreasureCaught = true;
            if (config_.InstantCatchFish)
                this._bobberBar.Value.DistanceFromCatching = 1.0f;
        }

        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

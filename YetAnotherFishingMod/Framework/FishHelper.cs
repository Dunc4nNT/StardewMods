using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using System;

namespace YetAnotherFishingMod.Framework
{
    internal class FishHelper(Func<ModConfig> config, IMonitor monitor, IReflectionHelper reflectionHelper)
    {
        private readonly PerScreen<SBobberBar> _bobberBar = new();
        private readonly PerScreen<SFishingRod> _fishingRod = new();
        public readonly PerScreen<bool> IsInFishingMiniGame = new();

        public void ApplyFishingRodBuffs()
        {
            ModConfig config_ = config();
            SFishingRod fishingRod = this._fishingRod.Value;

            if (config_.AlwaysMaxCastingPower)
                fishingRod.CastingPower = 1.01f;
            if (config_.AlwaysCatchDouble)
                fishingRod.CaughtDoubleFish = true;
            if (config_.InstantBite && fishingRod.TimeUntilFishingBite > 0)
                fishingRod.TimeUntilFishingBite = 0f;
            if (config_.AutoHook && fishingRod.IsNibbling && !fishingRod.Hit && !fishingRod.IsReeling && !fishingRod.PullingOutOfWater && !fishingRod.FishCaught && !fishingRod.ShowingTreasure)
            {
                fishingRod.TimePerBobberBob = 1f;
                fishingRod.TimeUntilFishingNibbleDone = FishingRod.maxTimeToNibble;
                reflectionHelper.GetMethod(fishingRod.Instance, "DoFunction").Invoke(Game1.player.currentLocation, (int)fishingRod.Instance.bobber.X, (int)fishingRod.Instance.bobber.Y, 1, Game1.player);
                Rumble.rumble(0.95f, 200f);
            }
        }

        public void ApplyFishingMiniGameBuffs()
        {
            if (config().AlwaysPerfect)
                this._bobberBar.Value.Perfect = true;
        }

        public void OnFishingRodEquipped(FishingRod fishingRod)
        {
            this._fishingRod.Value ??= new(fishingRod, reflectionHelper);

            this.ApplyFishingRodBuffs();
        }

        public void OnFishingRodNotEquipped()
        {
            this._fishingRod.Value = null;
        }

        public void OnFishingMiniGameStart(BobberBar bobberBar)
        {
            this.IsInFishingMiniGame.Value = true;
            this._bobberBar.Value = new(bobberBar, reflectionHelper);

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

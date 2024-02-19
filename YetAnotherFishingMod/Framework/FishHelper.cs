using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Tools;
using System;

namespace YetAnotherFishingMod.Framework
{
    internal class FishHelper(Func<ModConfig> config)
    {
        private readonly PerScreen<BobberBar> _bobberBar = new();
        private readonly PerScreen<SFishingRod> _fishingRod = new();
        public readonly PerScreen<bool> IsInFishingMiniGame = new();

        private void ApplyFishingRodBuffs()
        {
            ModConfig config_ = config();
            SFishingRod fishingRod = this._fishingRod.Value;

            if (config_.FishQuality != ModConfig.Quality.Any && fishingRod.Instance.fishSize > 0)
                fishingRod.Instance.fishQuality = (int)config_.FishQuality;

            if (config_.InfiniteBait)
                fishingRod.InfiniteBait();

            if (config_.InfiniteTackle)
                fishingRod.InfiniteTackle();

            if (config_.AlwaysMaxCastingPower)
                fishingRod.Instance.castingPower = 1.01f;

            if (config_.InstantBite)
                fishingRod.InstantBite();

            if (config_.AutoHook)
                fishingRod.AutoHook();

            if (config_.AlwaysCatchDouble)
                fishingRod.Instance.caughtDoubleFish = true;
        }

        public void ApplyFishingMiniGameBuffs()
        {
            BobberBar bobberBar = this._bobberBar.Value;

            if (bobberBar is null)
                return;

            if (config().AlwaysPerfect)
                bobberBar.perfect = true;
        }

        private void CreateFishingRod(FishingRod fishingRod)
        {
            ModConfig config_ = config();
            this._fishingRod.Value = new(fishingRod);

            if (config_.OverrideAttachmentLimit)
                this._fishingRod.Value.Instance.AttachmentSlotsCount = 2;

            if (config_.SpawnBaitWhenEquipped)
                this._fishingRod.Value.SpawnBait((int)config_.SpawnWhichBait);
            if (config_.SpawnTackleWhenEquipped)
                this._fishingRod.Value.SpawnTackle((int)config_.SpawnWhichTackle);
        }

        public void OnFishingRodEquipped(FishingRod fishingRod)
        {
            if (this._fishingRod.Value is null)
                this.CreateFishingRod(fishingRod);
            else if (this._fishingRod.Value.Instance != fishingRod)
            {
                ModConfig config_ = config();

                if (config_.ResetAttachmentsWhenNotEquipped)
                    this._fishingRod.Value.ResetAttachments();

                this.CreateFishingRod(fishingRod);
            }

            this.ApplyFishingRodBuffs();
        }

        public void OnFishingRodNotEquipped()
        {
            if (this._fishingRod.Value is not null)
            {
                ModConfig config_ = config();

                if (config_.ResetAttachmentsWhenNotEquipped)
                    this._fishingRod.Value.ResetAttachments();

                this._fishingRod.Value = null;
            }
        }

        public void OnFishingMiniGameStart(BobberBar bobberBar)
        {
            this.IsInFishingMiniGame.Value = true;
            this._bobberBar.Value = bobberBar;

            this.ApplyBobberBarBuffs();
        }

        private void ApplyBobberBarBuffs()
        {
            BobberBar bobberBar = this._bobberBar.Value;
            ModConfig config_ = config();

            bobberBar.difficulty *= config_.DifficultyMultiplier;

            if (bobberBar.fishQuality < (int)config_.MinimumFishQuality)
                bobberBar.fishQuality = (int)config_.MinimumFishQuality;
            if ((config_.InstantCatchTreasure && bobberBar.treasure) || config_.AlwaysCatchTreasure)
                bobberBar.treasureCaught = true;
            if (config_.InstantCatchFish)
                bobberBar.distanceFromCatching = 1.0f;
        }

        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

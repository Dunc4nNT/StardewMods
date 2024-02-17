using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using Object = StardewValley.Object;

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

            Object bait = fishingRod.Instance.GetBait();
            if (config_.InfiniteBait && bait is not null)
                bait.Stack = bait.maximumStackSize();

            Object tackle = fishingRod.Instance.GetTackle();
            if (config_.InfiniteTackle && tackle is not null)
                tackle.uses.Value = 0;

            if (config_.AlwaysMaxCastingPower)
                fishingRod.CastingPower = 1.01f;
            if (config_.InstantBite && fishingRod.TimeUntilFishingBite > 0)
                fishingRod.TimeUntilFishingBite = 0f;
            if (config_.AutoHook && fishingRod.IsNibbling && !fishingRod.Hit && !fishingRod.IsReeling && !fishingRod.PullingOutOfWater && !fishingRod.FishCaught && !fishingRod.ShowingTreasure)
            {
                fishingRod.TimePerBobberBob = 1f;
                fishingRod.TimeUntilFishingNibbleDone = FishingRod.maxTimeToNibble;
                reflectionHelper.GetMethod(fishingRod.Instance, "DoFunction").Invoke(Game1.player.currentLocation, (int)fishingRod.Instance.bobber.X, (int)fishingRod.Instance.bobber.Y, 1, Game1.player);
                Rumble.rumble(0.95f, 200f);
            }
            if (config_.AlwaysCatchDouble)
                fishingRod.CaughtDoubleFish = true;
        }

        public void ApplyFishingMiniGameBuffs()
        {
            if (config().AlwaysPerfect)
                this._bobberBar.Value.Perfect = true;
        }

        private void ResetFishingRod()
        {
            this._fishingRod.Value.Instance.AttachmentSlotsCount = this._fishingRod.Value.InitialAttachmentSlotsCount;
            if (this._fishingRod.Value.Instance.AttachmentSlotsCount >= 1)
                this._fishingRod.Value.Instance.attachments[0] = this._fishingRod.Value.InitialBait;
            if (this._fishingRod.Value.Instance.AttachmentSlotsCount >= 2)
                this._fishingRod.Value.Instance.attachments[1] = this._fishingRod.Value.InitialTackle;
        }

        private void CreateFishingRod(FishingRod fishingRod)
        {
            ModConfig config_ = config();
            this._fishingRod.Value = new(fishingRod, reflectionHelper);

            if (config_.OverrideAttachmentLimit)
                this._fishingRod.Value.Instance.AttachmentSlotsCount = 2;

            if (config_.SpawnBaitWhenEquipped)
                this.SpawnBait((int)config_.SpawnWhichBait);
            if (config_.SpawnTackleWhenEquipped)
                this.SpawnTackle((int)config_.SpawnWhichTackle);
        }

        public void SpawnBait(int baitId)
        {
            this._fishingRod.Value.Instance.attachments[0] = ItemRegistry.Create<Object>($"(O){baitId}");
        }

        public void SpawnTackle(int tackleId)
        {
            this._fishingRod.Value.Instance.attachments[1] = ItemRegistry.Create<Object>($"(O){tackleId}");
        }

        public void OnFishingRodEquipped(FishingRod fishingRod)
        {
            if (this._fishingRod.Value is null)
            {
                this.CreateFishingRod(fishingRod);
            }
            else if (this._fishingRod.Value.Instance != fishingRod)
            {
                ModConfig config_ = config();
                if (config_.ResetAttachmentsWhenNotEquipped)
                    this.ResetFishingRod();

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
                    this.ResetFishingRod();

                this._fishingRod.Value = null;
            }
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

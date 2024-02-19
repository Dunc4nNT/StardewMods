using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using SObject = StardewValley.Object;

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

            SObject bait = fishingRod.Instance.GetBait();
            if (config_.InfiniteBait && bait is not null)
                bait.Stack = bait.maximumStackSize();

            SObject tackle = fishingRod.Instance.GetTackle();
            if (config_.InfiniteTackle && tackle is not null)
                tackle.uses.Value = 0;

            if (config_.AlwaysMaxCastingPower)
                fishingRod.Instance.castingPower = 1.01f;
            if (config_.InstantBite && fishingRod.Instance.timeUntilFishingBite > 0)
                fishingRod.Instance.timeUntilFishingBite = 0f;
            if (config_.AutoHook && fishingRod.CanHook())
                fishingRod.AutoHook();
            if (config_.AlwaysCatchDouble)
                fishingRod.Instance.caughtDoubleFish = true;
        }

        public void ApplyFishingMiniGameBuffs()
        {
            if (config().AlwaysPerfect)
                this._bobberBar.Value.Perfect = true;
        }

        private void ResetFishingRod()
        {
            SFishingRod fishingRod = this._fishingRod.Value;

            if (fishingRod.Instance.AttachmentSlotsCount >= 1)
                fishingRod.Instance.attachments[0] = fishingRod.InitialBait;
            if (fishingRod.Instance.AttachmentSlotsCount >= 2)
                fishingRod.Instance.attachments[1] = fishingRod.InitialTackle;

            fishingRod.Instance.AttachmentSlotsCount = fishingRod.InitialAttachmentSlotsCount;
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

            this.ApplyBobberBarBuffs();
        }

        private void ApplyBobberBarBuffs()
        {
            SBobberBar bobberBar = this._bobberBar.Value;
            ModConfig config_ = config();

            bobberBar.Difficulty *= config_.DifficultyMultiplier;

            if ((config_.InstantCatchTreasure && bobberBar.Treasure) || config_.AlwaysCatchTreasure)
                bobberBar.TreasureCaught = true;
            if (config_.InstantCatchFish)
                bobberBar.DistanceFromCatching = 1.0f;
        }

        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

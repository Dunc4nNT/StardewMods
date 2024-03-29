using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class FishHelper(Func<ModConfig> config, IMonitor monitor)
    {
        private readonly PerScreen<BobberBar> _bobberBar = new();
        private readonly PerScreen<SFishingRod> _fishingRod = new();
        public readonly PerScreen<bool> IsInFishingMiniGame = new();

        private void ApplyFishingRodBuffs()
        {
            ModConfig config_ = config();
            SFishingRod fishingRod = this._fishingRod.Value;

            if ((int)config_.MinimumFishQuality > fishingRod.Instance.fishQuality)
                fishingRod.Instance.fishQuality = (int)config_.MinimumFishQuality;
            else if (config_.FishQuality != Quality.Any)
                fishingRod.Instance.fishQuality = (int)config_.FishQuality;

            if (config_.AlwaysMaxCastingPower)
                fishingRod.Instance.castingPower = 1.01f;

            if (config_.InstantBite)
                fishingRod.InstantBite();

            if (config_.AutoHook)
                fishingRod.AutoHook();

            if (config_.NumberOfFishCaught > fishingRod.Instance.numberOfFishCaught)
                fishingRod.Instance.numberOfFishCaught = config_.NumberOfFishCaught;
        }

        public void OnTreasureMenuOpen(ItemGrabMenu itemGrabMenu)
        {
            ModConfig config_ = config();

            if (config_.AutoLootTreasure)
            {
                IList<Item> actualInventory = itemGrabMenu.ItemsToGrabMenu.actualInventory;
                for (int i = actualInventory.Count - 1; i >= 0; i--)
                    if (Game1.player.addItemToInventoryBool(actualInventory.ElementAt(i)))
                        actualInventory.RemoveAt(i);

                if (actualInventory.Count == 0)
                    itemGrabMenu.exitThisMenu();
                else
                    Notifier.DisplayHudNotification(I18n.Message_InventoryFull(), logLevel: LogLevel.Warn);
            }
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

            this._fishingRod.Value.SpawnBait(config_.BaitToSpawn, overrideAttachmentLimit: config_.OverrideAttachmentLimit);
            this._fishingRod.Value.SpawnTackles(config_.TacklesToSpawn, overrideAttachmentLimit: config_.OverrideAttachmentLimit);

            if (config_.DoAddEnchantments)
            {
                if (!fishingRod.hasEnchantmentOfType<AutoHookEnchantment>() && (config_.AddAllEnchantments || config_.AddAutoHookEnchantment))
                    this._fishingRod.Value.AddEnchantment(new AutoHookEnchantment());
                if (!fishingRod.hasEnchantmentOfType<EfficientToolEnchantment>() && (config_.AddAllEnchantments || config_.AddEfficientToolEnchantment))
                    this._fishingRod.Value.AddEnchantment(new EfficientToolEnchantment());
                if (!fishingRod.hasEnchantmentOfType<MasterEnchantment>() && (config_.AddAllEnchantments || config_.AddMasterEnchantment))
                    this._fishingRod.Value.AddEnchantment(new MasterEnchantment());
                if (!fishingRod.hasEnchantmentOfType<PreservingEnchantment>() && (config_.AddAllEnchantments || config_.AddPreservingEnchantment))
                    this._fishingRod.Value.AddEnchantment(new PreservingEnchantment());
            }
        }

        public void OnFishingRodEquipped(FishingRod fishingRod)
        {
            if (this._fishingRod.Value is null)
                this.CreateFishingRod(fishingRod);
            else if (this._fishingRod.Value.Instance != fishingRod)
            {
                this.OnFishingRodNotEquipped();
                this.CreateFishingRod(fishingRod);
            }

            this.ApplyFishingRodBuffs();
        }

        public void OnFishingRodNotEquipped()
        {
            SFishingRod fishingRod = this._fishingRod.Value;

            if (fishingRod is not null)
            {
                ModConfig config_ = config();

                if (config_.ResetAttachmentsLimitWhenNotEquipped)
                    fishingRod.ResetAttachmentsLimit();

                if (config_.ResetEnchantmentsWhenNotEquipped)
                    fishingRod.ResetEnchantments();

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
            bobberBar.distanceFromCatchPenaltyModifier = config_.FishNotInBarPenaltyMultiplier;
            bobberBar.bobberBarHeight = (int)(bobberBar.bobberBarHeight * config_.BarSizeMultiplier);
            bobberBar.bobberBarPos = BobberBar.bobberBarTrackHeight - bobberBar.bobberBarHeight;

            if (config_.TreasureAppearence == TreasureAppearanceSettings.Never)
                bobberBar.treasure = false;
            else if (config_.TreasureAppearence == TreasureAppearanceSettings.Always)
                bobberBar.treasure = true;

            if (config_.GoldenTreasureAppearance == TreasureAppearanceSettings.Never)
                bobberBar.goldenTreasure = false;
            else if (bobberBar.treasure && config_.GoldenTreasureAppearance == TreasureAppearanceSettings.Always)
                bobberBar.goldenTreasure = true;

            if ((config_.InstantCatchTreasure && bobberBar.treasure))
                bobberBar.treasureCaught = true;

            if (this.ShouldSkipMinigame(config_.SkipFishingMinigame, config_.SkipFishingMinigameCatchesRequired))
                this.SkipMinigame(config_.SkipFishingMinigameTreasureChance, config_.InstantCatchTreasure, config_.SkipFishingMinigamePerfectChance, config_.AlwaysPerfect);
        }

        private bool ShouldSkipMinigame(bool maySkipFishingMiniGame, int minimumCatchesRequired)
        {
            if (!maySkipFishingMiniGame)
                return false;

            if (minimumCatchesRequired == 0)
                return true;
            else if (Game1.player.fishCaught.TryGetValue(ItemRegistry.GetData(this._bobberBar.Value.whichFish)?.QualifiedItemId, out int[] fishCaughtInfo) && fishCaughtInfo.Length > 0 && fishCaughtInfo[0] >= minimumCatchesRequired)
                return true;
            else
                return false;
        }

        private void SkipMinigame(float treasureChance, bool instantCatchTreasure, float perfectChance, bool alwaysPerfect)
        {
            BobberBar bobberBar = this._bobberBar.Value;

            if (bobberBar.treasure && (Game1.random.NextDouble() < treasureChance || instantCatchTreasure))
                bobberBar.treasureCaught = true;

            if (Game1.random.NextDouble() > perfectChance && !alwaysPerfect)
                bobberBar.perfect = false;

            bobberBar.distanceFromCatching = 1f;
        }

        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

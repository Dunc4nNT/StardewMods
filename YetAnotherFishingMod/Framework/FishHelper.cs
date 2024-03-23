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

            if (config_.OverrideAttachmentLimit)
                this._fishingRod.Value.Instance.numAttachmentSlots.Value = 2;

            if (config_.SpawnBaitWhenEquipped)
                this._fishingRod.Value.SpawnBait(config_.SpawnWhichBait);
            if (config_.SpawnTackleWhenEquipped)
                this._fishingRod.Value.SpawnTackle(config_.SpawnWhichTackle);

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

                if (config_.ResetAttachmentsWhenNotEquipped)
                    fishingRod.ResetAttachments(config_.SpawnBaitWhenEquipped, config_.SpawnTackleWhenEquipped);

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

            if (config_.TreasureAppearence is TreasureAppearanceSettings.Never)
            {
                bobberBar.treasure = false;
                bobberBar.treasureCaught = false;
            }
            else if ((config_.InstantCatchTreasure && bobberBar.treasure && config_.TreasureAppearence is TreasureAppearanceSettings.Vanilla) || config_.TreasureAppearence is TreasureAppearanceSettings.Always)
                bobberBar.treasureCaught = true;

            if (this.ShouldSkipMinigame(config_.SkipFishingMinigame, config_.SkipFishingMinigameCatchesRequired))
                this.SkipMinigame();
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

        private void SkipMinigame()
        {
            BobberBar bobberBar = this._bobberBar.Value;

            bobberBar.fadeOut = true;
            bobberBar.scale = 0f;
            bobberBar.distanceFromCatching = 1f;
        }


        public void OnFishingMiniGameEnd()
        {
            this.IsInFishingMiniGame.Value = false;
            this._bobberBar.Value = null;
        }
    }
}

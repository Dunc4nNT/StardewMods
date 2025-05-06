// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Enchantments;
using StardewValley.Menus;
using StardewValley.Tools;
using SObject = StardewValley.Object;

internal class FishHelper(Func<ModConfig> modConfig, IMonitor monitor, IReflectionHelper reflectionHelper)
{
    private readonly PerScreen<BobberBar?> bobberBar = new();

    private readonly PerScreen<SFishingRod?> fishingRod = new();

    internal PerScreen<bool> IsInFishingMiniGame { get; set; } = new();

    internal PerScreen<bool> DoAutoCast { get; set; } = new();

    public void OnTreasureMenuOpen(ItemGrabMenu itemGrabMenu)
    {
        ModConfig config = modConfig();

        if (!config.AutoLootTreasure)
        {
            return;
        }

        IList<Item> actualInventory = itemGrabMenu.ItemsToGrabMenu.actualInventory;
        for (int i = actualInventory.Count - 1; i >= 0; i--)
        {
            if (Game1.player.addItemToInventoryBool(actualInventory.ElementAt(i)))
            {
                actualInventory.RemoveAt(i);
            }
        }

        if (actualInventory.Count == 0)
        {
            itemGrabMenu.exitThisMenu();
        }
        else
        {
            Notifier.DisplayHudNotification(I18n.Message_InventoryFull(), logLevel: LogLevel.Warn);
        }
    }

    public void ApplyFishingMiniGameBuffs()
    {
        BobberBar? bBar = this.bobberBar.Value;

        if (bBar is null)
        {
            return;
        }

        if (modConfig().AlwaysPerfect)
        {
            bBar.perfect = true;
        }
    }

    public void OnFishingRodEquipped(FishingRod rod)
    {
        if (this.fishingRod.Value is null)
        {
            this.CreateFishingRod(rod);
        }
        else if (this.fishingRod.Value.Instance != rod)
        {
            this.OnFishingRodNotEquipped();
            this.CreateFishingRod(rod);
        }

        this.ApplyFishingRodBuffs();
    }

    public void OnFishingRodNotEquipped()
    {
        SFishingRod? rod = this.fishingRod.Value;

        if (rod is null)
        {
            return;
        }

        ModConfig config = modConfig();

        if (config.ResetAttachmentsLimitWhenNotEquipped)
        {
            rod.ResetAttachmentsLimit();
        }

        if (config.ResetEnchantmentsWhenNotEquipped)
        {
            rod.ResetEnchantments();
        }

        this.fishingRod.Value = null;
    }

    public void OnFishingMiniGameStart(BobberBar bBar)
    {
        this.IsInFishingMiniGame.Value = true;
        this.bobberBar.Value = bBar;

        this.ApplyBobberBarBuffs();
    }

    public void SpeedUpAnimations()
    {
        ModConfig config = modConfig();

        if (!config.DoSpeedUpAnimations)
        {
            return;
        }

        if (Game1.player.UsingTool && Game1.player.CurrentTool is FishingRod { isTimingCast: false, isFishing: false })
        {
            for (int i = 0; i < 10; i++)
            {
                Game1.player.Update(Game1.currentGameTime, Game1.player.currentLocation);
            }
        }

        if (!this.IsInFishingMiniGame.Value)
        {
            return;
        }

        {
            BobberBar? bBar = this.bobberBar.Value;

            if (bBar is null)
            {
                return;
            }

            bBar.everythingShakeTimer = 0f;

            SparklingText sparkleText = reflectionHelper.GetField<SparklingText>(bBar, "sparkleText").GetValue();

            for (int i = 0; i < 10; i++)
            {
                sparkleText?.update(Game1.currentGameTime);
            }
        }
    }

    public void AutoCast()
    {
        if (!this.DoAutoCast.Value)
        {
            return;
        }

        if (Game1.player.CurrentTool is FishingRod
            {
                isCasting: false, isReeling: false, fishCaught: false, showingTreasure: false, isFishing: false
            }

            && !Game1.player.UsingTool && Game1.activeClickableMenu is null && Game1.player.CanMove)
        {
            Game1.pressUseToolButton();
        }
    }

    public void OnFishingMiniGameEnd()
    {
        this.IsInFishingMiniGame.Value = false;
        this.bobberBar.Value = null;
    }

    private void ApplyFishingRodBuffs()
    {
        ModConfig config = modConfig();
        SFishingRod? rod = this.fishingRod.Value;

        if (rod is null)
        {
            return;
        }

        if (ItemRegistry.GetData(rod.Instance.whichFish?.QualifiedItemId)?.Category == SObject.FishCategory)
        {
            if ((int)config.MinimumFishQuality > rod.Instance.fishQuality)
            {
                rod.Instance.fishQuality = (int)config.MinimumFishQuality;
            }
            else if (config.FishQuality != Quality.Any)
            {
                rod.Instance.fishQuality = (int)config.FishQuality;
            }

            if (config.NumberOfFishCaught > rod.Instance.numberOfFishCaught)
            {
                rod.Instance.numberOfFishCaught = config.NumberOfFishCaught;
            }
        }

        if (config.AlwaysMaxCastingPower)
        {
            rod.Instance.castingPower = 1.01f;
        }

        if (config.InstantBite)
        {
            rod.InstantBite();
        }

        if (config.AutoHook)
        {
            rod.AutoHook(!config.DisableVibrations);
        }
    }

    private void CreateFishingRod(FishingRod rod)
    {
        ModConfig config = modConfig();
        this.fishingRod.Value = new SFishingRod(rod);

        this.fishingRod.Value.SpawnBait(config.BaitToSpawn, config.AmountOfBait, config.OverrideAttachmentLimit);
        this.fishingRod.Value.SpawnTackles(config.TacklesToSpawn, config.OverrideAttachmentLimit);

        if (!config.DoAddEnchantments)
        {
            return;
        }

        if (!rod.hasEnchantmentOfType<AutoHookEnchantment>() &&
            (config.AddAllEnchantments || config.AddAutoHookEnchantment))
        {
            this.fishingRod.Value.AddEnchantment(new AutoHookEnchantment());
        }

        if (!rod.hasEnchantmentOfType<EfficientToolEnchantment>() &&
            (config.AddAllEnchantments || config.AddEfficientToolEnchantment))
        {
            this.fishingRod.Value.AddEnchantment(new EfficientToolEnchantment());
        }

        if (!rod.hasEnchantmentOfType<MasterEnchantment>() &&
            (config.AddAllEnchantments || config.AddMasterEnchantment))
        {
            this.fishingRod.Value.AddEnchantment(new MasterEnchantment());
        }

        if (!rod.hasEnchantmentOfType<PreservingEnchantment>() &&
            (config.AddAllEnchantments || config.AddPreservingEnchantment))
        {
            this.fishingRod.Value.AddEnchantment(new PreservingEnchantment());
        }
    }

    private void ApplyBobberBarBuffs()
    {
        BobberBar? bBar = this.bobberBar.Value;
        SFishingRod? rod = this.fishingRod.Value;
        ModConfig config = modConfig();

        if (bBar is null || rod is null)
        {
            return;
        }

        bBar.difficulty *= config.DifficultyMultiplier;
        bBar.distanceFromCatchPenaltyModifier = config.FishNotInBarPenaltyMultiplier;
        bBar.bobberBarHeight = (int)(bBar.bobberBarHeight * config.BarSizeMultiplier);
        bBar.bobberBarPos = BobberBar.bobberBarTrackHeight - bBar.bobberBarHeight;

        bBar.treasure = config.TreasureAppearance switch
        {
            TreasureAppearanceSettings.Never => false,
            TreasureAppearanceSettings.Always => true,
            _ => bBar.treasure,
        };

        if (config.GoldenTreasureAppearance == TreasureAppearanceSettings.Never)
        {
            rod.Instance.goldenTreasure = false;
            bBar.goldenTreasure = false;
        }
        else if (bBar.treasure && config.GoldenTreasureAppearance == TreasureAppearanceSettings.Always)
        {
            rod.Instance.goldenTreasure = true;
            bBar.goldenTreasure = true;
        }

        if (config.InstantCatchTreasure && bBar.treasure)
        {
            bBar.treasureCaught = true;
        }

        if (this.ShouldSkipMinigame(config.SkipFishingMinigame, config.SkipFishingMinigameCatchesRequired))
        {
            this.SkipMinigame(
                config.SkipFishingMinigameTreasureChance,
                config.InstantCatchTreasure,
                config.SkipFishingMinigamePerfectChance,
                config.AlwaysPerfect,
                config.SkipMinigamePopup);
        }
    }

    private bool ShouldSkipMinigame(bool maySkipFishingMiniGame, int minimumCatchesRequired)
    {
        if (!maySkipFishingMiniGame)
        {
            return false;
        }

        if (minimumCatchesRequired == 0)
        {
            return true;
        }

        return Game1.player.fishCaught.TryGetValue(
                   ItemRegistry.GetData(this.bobberBar.Value?.whichFish)?.QualifiedItemId,
                   out int[] fishCaughtInfo) && fishCaughtInfo.Length > 0 &&
               fishCaughtInfo[0] >= minimumCatchesRequired;
    }

    private void SkipMinigame(
        float treasureChance,
        bool instantCatchTreasure,
        float perfectChance,
        bool alwaysPerfect,
        bool skipPopup)
    {
        BobberBar? bBar = this.bobberBar.Value;

        if (bBar is null)
        {
            return;
        }

        if (bBar.treasure && (Game1.random.NextDouble() < treasureChance || instantCatchTreasure))
        {
            bBar.treasureCaught = true;
        }

        if (Game1.random.NextDouble() > perfectChance && !alwaysPerfect)
        {
            bBar.perfect = false;
        }

        bBar.distanceFromCatching = 1f;

        if (!skipPopup)
        {
            return;
        }

        for (int i = 0; i < 250; i++)
        {
            bBar?.update(Game1.currentGameTime);
        }
    }
}

// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherFishingMod;

using System.Collections.Generic;
using HarmonyLib;
using NeverToxic.StardewMods.Common;
using NeverToxic.StardewMods.YetAnotherFishingMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.Menus;
using StardewValley.Tools;
using Patches = NeverToxic.StardewMods.YetAnotherFishingMod.Framework.Patches;
using SObject = StardewValley.Object;

internal sealed class ModEntry : Mod
{
    private readonly List<string> baitList = [string.Empty];

    private readonly List<string> tackleList = [string.Empty];

    internal ModConfig Config { get; set; } = null!;

    internal Harmony Harmony { get; set; } = null!;

    private FishHelper FishHelper { get; set; } = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);

        this.Config = helper.ReadConfig<ModConfig>();
        this.Harmony = new Harmony(this.ModManifest.UniqueID);
        Patches.Patch(this);
        this.FishHelper = new FishHelper(() => this.Config, this.Monitor, this.Helper.Reflection);

        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        helper.Events.Display.MenuChanged += this.OnMenuChanged;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        helper.Events.GameLoop.OneSecondUpdateTicked += this.OnSecondUpdateTicked;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        foreach (KeyValuePair<string, ObjectData> item in Game1.objectData)
        {
            switch (item.Value.Category)
            {
            case SObject.baitCategory:
                this.baitList.Add(ItemRegistry.QualifyItemId(item.Key));
                break;
            case SObject.tackleCategory:
                this.tackleList.Add(ItemRegistry.QualifyItemId(item.Key));
                break;
            }
        }

        new GenericModConfigMenu(
            this.Helper.ModRegistry,
            this.ModManifest,
            this.Monitor,
            () => this.Config,
            () => this.Config = new ModConfig(),
            () => this.Helper.WriteConfig(this.Config),
            this.baitList,
            this.tackleList).Register();
    }

    private void OnSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
    {
        this.FishHelper.AutoCast();
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady)
        {
            return;
        }

        if (Game1.player.CurrentTool is FishingRod fishingRod)
        {
            this.FishHelper.OnFishingRodEquipped(fishingRod);
        }
        else if (Game1.player.CurrentTool is not FishingRod)
        {
            this.FishHelper.OnFishingRodNotEquipped();
        }

        if (this.FishHelper.IsInFishingMiniGame.Value)
        {
            this.FishHelper.ApplyFishingMiniGameBuffs();
        }

        this.FishHelper.SpeedUpAnimations();
    }

    private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
    {
        if (e.NewMenu is BobberBar bobberBar)
        {
            this.FishHelper.OnFishingMiniGameStart(bobberBar);
        }
        else if (e.OldMenu is BobberBar)
        {
            this.FishHelper.OnFishingMiniGameEnd();
        }

        if (e.NewMenu is ItemGrabMenu { source: ItemGrabMenu.source_fishingChest } itemGrabMenu)
        {
            this.FishHelper.OnTreasureMenuOpen(itemGrabMenu);
        }
    }

    private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (!Context.IsWorldReady)
        {
            return;
        }

        if (this.Config.Keys.ReloadConfig.JustPressed())
        {
            this.ReloadConfig();
        }

        if (!this.Config.Keys.DoAutoCast.JustPressed())
        {
            return;
        }

        this.FishHelper.DoAutoCast.Value = !this.FishHelper.DoAutoCast.Value;

        if (this.FishHelper.DoAutoCast.Value)
        {
            this.Monitor.Log(I18n.Message_DoAutoCastEnabled());
            Notifier.DisplayHudNotification(I18n.Message_DoAutoCastEnabled(), 1500);
        }
        else
        {
            this.Monitor.Log(I18n.Message_DoAutoCastDisabled());
            Notifier.DisplayHudNotification(I18n.Message_DoAutoCastDisabled(), 1500);
        }
    }

    private void ReloadConfig()
    {
        this.Config = this.Helper.ReadConfig<ModConfig>();
        this.Monitor.Log(I18n.Message_ConfigReloaded());
    }
}

// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore;

using HarmonyLib;
using NeverToxic.StardewMods.LittleHelpersCore.Framework;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Patches;
using StardewModdingAPI;
using StardewModdingAPI.Events;

internal sealed class ModEntry : Mod
{
    internal ModConfig Config { get; set; } = null!;

    internal Harmony Harmony { get; set; } = null!;

    internal LittleHelpersHelper LittleHelpersHelper { get; set; } = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);

        this.Config = helper.ReadConfig<ModConfig>();
        this.LittleHelpersHelper = new LittleHelpersHelper(() => this.Config, this.Monitor);
        Harmony harmony = new(this.ModManifest.UniqueID);
        ObjectPatch.Patch(this);

        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        helper.Events.GameLoop.DayStarted += this.OnDayStarted;
    }

    private void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
        this.LittleHelpersHelper.ExecuteAllActions();
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        new GenericModConfigMenu(
            this.Helper.ModRegistry,
            this.ModManifest,
            this.Monitor,
            () => this.Config,
            () => this.Config = new ModConfig(),
            () => this.Helper.WriteConfig(this.Config)).Register();
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
    }

    private void ReloadConfig()
    {
        this.Config = this.Helper.ReadConfig<ModConfig>();
        this.Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
    }
}

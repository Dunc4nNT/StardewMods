// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherTimeMod;

using NeverToxic.StardewMods.YetAnotherTimeMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

internal class ModEntry : Mod
{
    private ModConfig config = null!;

    private TimeHelper timeHelper = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);

        this.config = helper.ReadConfig<ModConfig>();
        this.timeHelper = new TimeHelper(this.config, this.Monitor);

        helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        if (!Context.IsMainPlayer)
        {
            this.Monitor.Log(I18n.Message_NotMainPlayerWarning(), LogLevel.Warn);
        }
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady || !Context.IsMainPlayer)
        {
            return;
        }

        this.timeHelper.Update();
    }

    private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
    {
        if (!Context.IsWorldReady || !Context.IsMainPlayer)
        {
            return;
        }

        if (this.config.Keys.ReloadConfig.JustPressed())
        {
            this.ReloadConfig();
        }
        else if (this.config.Keys.IncreaseSpeed.JustPressed())
        {
            this.timeHelper.IncreaseSpeed();
        }
        else if (this.config.Keys.DecreaseSpeed.JustPressed())
        {
            this.timeHelper.DecreaseSpeed();
        }
        else if (this.config.Keys.ResetSpeed.JustPressed())
        {
            this.timeHelper.ResetSpeed();
        }
        else if (this.config.Keys.DoubleSpeed.JustPressed())
        {
            this.timeHelper.SetDoubleSpeed();
        }
        else if (this.config.Keys.HalfSpeed.JustPressed())
        {
            this.timeHelper.SetHalfSpeed();
        }
        else if (this.config.Keys.ToggleFreeze.JustPressed())
        {
            this.timeHelper.ToggleFreeze();
        }
    }

    private void ReloadConfig()
    {
        this.config = this.Helper.ReadConfig<ModConfig>();
        this.Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
    }
}

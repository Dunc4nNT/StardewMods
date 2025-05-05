// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.TelevisionFramework;

using HarmonyLib;
using NeverToxic.StardewMods.TelevisionFramework.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Patches = NeverToxic.StardewMods.TelevisionFramework.Framework.Patches;

internal sealed class ModEntry : Mod
{
    private ModConfig Config { get; set; }

    private ModConfigKeys Keys => this.Config.Keys;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);

        this.Config = helper.ReadConfig<ModConfig>();
        Harmony harmony = new(this.ModManifest.UniqueID);
        Patches.Initialise(harmony, this.Monitor, () => this.Config, this.Helper.Reflection);

        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        new GenericModConfigMenu(this.Helper.ModRegistry, this.ModManifest, this.Monitor, () => this.Config, () => this.Config = new ModConfig(), () => this.Helper.WriteConfig(this.Config)).Register();
    }

    private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
    {
        if (!Context.IsWorldReady)
        {
            return;
        }

        if (this.Keys.ReloadConfig.JustPressed())
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

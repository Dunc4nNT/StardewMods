// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.SmapiMod;

#if EnableHarmony
using HarmonyLib;
#endif
#if EnableConfig
using NeverToxic.StardewMods.SmapiMod.Framework.Config;
#endif
#if EnableHarmony
using NeverToxic.StardewMods.SmapiMod.Framework.Patches;
#endif
using StardewModdingAPI;
#if EnableConfig
using StardewModdingAPI.Events;
#endif

internal sealed class ModEntry : Mod
{
#if EnableConfig
    internal ModConfig Config { get; set; } = null!;
#endif

#if EnableHarmony
    internal Harmony Harmony { get; set; } = null!;

    internal Patcher Patcher { get; set; } = null!;
#endif

    public override void Entry(IModHelper helper)
    {
#if EnableI18n
        I18n.Init(helper.Translation);
#endif

#if EnableConfig
        this.Config = helper.ReadConfig<ModConfig>();
#endif

#if EnableHarmony
        this.Harmony = new Harmony(this.ModManifest.UniqueID);
        this.Patcher = new Patcher(this);
        this.Patcher.PatchAll();
#endif

#if EnableConfig
        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
#endif
    }

#if EnableConfig
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
#endif

#if EnableConfig
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
#endif

#if EnableConfig
    private void ReloadConfig()
    {
        this.Config = this.Helper.ReadConfig<ModConfig>();
        this.Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
    }
#endif
}

// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherTimeMod.Framework;

using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using StardewValley;

internal class TimeHelper(ModConfig config, IMonitor monitor)
{
    private int lastTimeInterval;

    private int speedPercentage = config.DefaultSpeedPercentage;

    private bool isTimeFrozen;

    private int SpeedPercentage
    {
        get => this.speedPercentage;
        set
        {
            if (value is <= 0 or > 700)
            {
                return;
            }

            this.speedPercentage = value;
            this.OnSpeedUpdate();
        }
    }

    private bool IsTimeFrozen
    {
        get => this.isTimeFrozen;
        set
        {
            this.isTimeFrozen = value;
            this.OnFreezeUpdate();
        }
    }

    public void Update()
    {
        if (this.IsTimeFrozen)
        {
            Game1.gameTimeInterval = 0;
        }

        if (Game1.gameTimeInterval < this.lastTimeInterval)
        {
            this.lastTimeInterval = 0;
        }

        Game1.gameTimeInterval = this.lastTimeInterval +
                                 ((Game1.gameTimeInterval - this.lastTimeInterval) * this.SpeedPercentage / 100);
        this.lastTimeInterval = Game1.gameTimeInterval;
    }

    public void IncreaseSpeed()
    {
        this.SpeedPercentage += 10;
    }

    public void DecreaseSpeed()
    {
        this.SpeedPercentage -= 10;
    }

    public void ResetSpeed()
    {
        this.SpeedPercentage = config.DefaultSpeedPercentage;
    }

    public void SetHalfSpeed()
    {
        this.SpeedPercentage = 50;
    }

    public void SetDoubleSpeed()
    {
        this.SpeedPercentage = 200;
    }

    public void ToggleFreeze()
    {
        this.IsTimeFrozen = !this.IsTimeFrozen;
    }

    private void OnSpeedUpdate()
    {
        string message = I18n.Message_TimeUpdate(this.SpeedPercentage);
        monitor.Log(message, LogLevel.Info);
        Notifier.DisplayHudNotification(message);
    }

    private void OnFreezeUpdate()
    {
        string message = this.IsTimeFrozen ? I18n.Message_TimeFrozen() : I18n.Message_TimeUnfrozen();
        monitor.Log(message, LogLevel.Info);
        Notifier.DisplayHudNotification(message, logLevel: LogLevel.Warn);
    }
}

﻿using Common;
using StardewModdingAPI;
using StardewValley;

namespace TimeMod.Framework
{
    internal class TimeHelper(ModConfig config, IMonitor monitor)
    {
        private int _lastTimeInterval = 0;

        private int _speedPercentage = config.DefaultSpeedPercentage;

        private bool _istimeFrozen = false;

        private int SpeedPercentage
        {
            get => this._speedPercentage;
            set
            {
                if (value is > 0 and <= 700)
                {
                    this._speedPercentage = value;
                    this.OnSpeedUpdate();
                }
            }
        }

        private bool IsTimeFrozen
        {
            get => this._istimeFrozen;
            set
            {
                this._istimeFrozen = value;
                this.OnFreezeUpdate();
            }
        }

        public void Update()
        {
            if (this._istimeFrozen)
                Game1.gameTimeInterval = 0;

            if (Game1.gameTimeInterval < this._lastTimeInterval)
            {
                this._lastTimeInterval = 0;
            }

            Game1.gameTimeInterval = this._lastTimeInterval + ((Game1.gameTimeInterval - this._lastTimeInterval) * this.SpeedPercentage / 100);
            this._lastTimeInterval = Game1.gameTimeInterval;
        }

        private void OnSpeedUpdate()
        {
            string message = I18n.Message_TimeUpdate(Percentage: this.SpeedPercentage);
            monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message);
        }

        private void OnFreezeUpdate()
        {
            string message = this.IsTimeFrozen ? I18n.Message_TimeFrozen() : I18n.Message_TimeUnfrozen();
            monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message, logLevel: LogLevel.Warn);
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
    }
}

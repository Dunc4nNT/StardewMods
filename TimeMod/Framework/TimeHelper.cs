using Common;
using StardewModdingAPI;
using StardewValley;

namespace TimeMod.Framework
{
    internal class TimeHelper
    {
        private readonly ModConfig Config;

        private readonly IMonitor Monitor;

        private int LastTimeInterval;

        private int SpeedPercentage;

        private bool TimeFrozen;

        public TimeHelper(ModConfig Config, IMonitor Monitor)
        {
            this.Config = Config;
            this.Monitor = Monitor;
            LastTimeInterval = 0;
            SpeedPercentage = Config.DefaultSpeedPercentage;
            TimeFrozen = false;
        }

        public void Update()
        {
            if (TimeFrozen)
                Game1.gameTimeInterval = 0;

            if (Game1.gameTimeInterval < LastTimeInterval)
            {
                LastTimeInterval = 0;
            }

            Game1.gameTimeInterval = LastTimeInterval + (Game1.gameTimeInterval - LastTimeInterval) * SpeedPercentage / 100;
            LastTimeInterval = Game1.gameTimeInterval;
        }

        private void OnSpeedUpdate()
        {
            string Message = I18n.Message_TimeUpdate(Percentage: SpeedPercentage);
            Monitor.Log(Message, LogLevel.Info);
            Notifier.DisplayHudNotification(Message);
        }

        public void IncreaseSpeed()
        {
            if (SpeedPercentage < 690)
                SpeedPercentage += 10;
                OnSpeedUpdate();
        }

        public void DecreaseSpeed()
        {
            if (SpeedPercentage > 10)
                SpeedPercentage -= 10;
                OnSpeedUpdate();
        }

        public void ResetSpeed()
        {
            SpeedPercentage = Config.DefaultSpeedPercentage;
            OnSpeedUpdate();
        }

        public void SetHalfSpeed()
        {
            SpeedPercentage = 50;
            OnSpeedUpdate();
        }

        public void SetDoubleSpeed()
        {
            SpeedPercentage = 200;
            OnSpeedUpdate();
        }

        public void ToggleFreeze()
        {
            TimeFrozen = !TimeFrozen;

            string Message = (TimeFrozen) ? I18n.Message_TimeFrozen() : I18n.Message_TimeUnfrozen();
            Monitor.Log(Message, LogLevel.Info);
            Notifier.DisplayHudNotification(Message, LogLevel: LogLevel.Warn);
        }
    }
}

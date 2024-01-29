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

            Game1.gameTimeInterval = (int)(LastTimeInterval + (Game1.gameTimeInterval - LastTimeInterval) * SpeedPercentage / 100);
            LastTimeInterval = Game1.gameTimeInterval;
        }

        public void IncreaseSpeed()
        {
            if (SpeedPercentage < 700)
                SpeedPercentage += 10;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        public void DecreaseSpeed()
        {
            if (SpeedPercentage > 10)
                SpeedPercentage -= 10;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        public void ResetSpeed()
        {
            SpeedPercentage = Config.DefaultSpeedPercentage;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        public void SetHalfSpeed()
        {
            SpeedPercentage = 50;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        public void SetDoubleSpeed()
        {
            SpeedPercentage = 200;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        public void ToggleFreeze()
        {
            TimeFrozen = !TimeFrozen;
            if (TimeFrozen)
                Monitor.Log(I18n.Message_TimeFrozen(), LogLevel.Info);
            else
                Monitor.Log(I18n.Message_TimeUnfrozen(), LogLevel.Info);
        }
    }
}

using Common;
using StardewModdingAPI;
using StardewValley;

namespace TimeMod.Framework
{
    internal class TimeHelper
    {
        private readonly ModConfig _config;

        private readonly IMonitor _monitor;

        private int _lastTimeInterval;

        private int _speedPercentage;

        private bool _istimeFrozen;

        private int SpeedPercentage
        {
            get => _speedPercentage;
            set
            {
                if (value > 0 && value <= 700)
                {
                    _speedPercentage = value;
                    OnSpeedUpdate();
                }
            }
        }

        private bool IsTimeFrozen
        {
            get => _istimeFrozen;
            set
            {
                _istimeFrozen = value;
                OnFreezeUpdate();
            }
        }

        public TimeHelper(ModConfig config, IMonitor monitor)
        {
            _config = config;
            _monitor = monitor;
            _lastTimeInterval = 0;
            _speedPercentage = config.DefaultSpeedPercentage;
            _istimeFrozen = false;
        }

        public void Update()
        {
            if (_istimeFrozen)
                Game1.gameTimeInterval = 0;

            if (Game1.gameTimeInterval < _lastTimeInterval)
            {
                _lastTimeInterval = 0;
            }

            Game1.gameTimeInterval = _lastTimeInterval + (Game1.gameTimeInterval - _lastTimeInterval) * SpeedPercentage / 100;
            _lastTimeInterval = Game1.gameTimeInterval;
        }

        private void OnSpeedUpdate()
        {
            string message = I18n.Message_TimeUpdate(Percentage: SpeedPercentage);
            _monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message);
        }

        private void OnFreezeUpdate()
        {
            string message = IsTimeFrozen ? I18n.Message_TimeFrozen() : I18n.Message_TimeUnfrozen();
            _monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message, logLevel: LogLevel.Warn);
        }

        public void IncreaseSpeed()
        {
            SpeedPercentage += 10;
        }

        public void DecreaseSpeed()
        {
            SpeedPercentage -= 10;
        }

        public void ResetSpeed()
        {
            SpeedPercentage = _config.DefaultSpeedPercentage;
        }

        public void SetHalfSpeed()
        {
            SpeedPercentage = 50;
        }

        public void SetDoubleSpeed()
        {
            SpeedPercentage = 200;
        }

        public void ToggleFreeze()
        {
            IsTimeFrozen = !IsTimeFrozen;
        }
    }
}

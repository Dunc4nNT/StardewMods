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

        private bool _timeFrozen;

        public TimeHelper(ModConfig config, IMonitor monitor)
        {
            _config = config;
            _monitor = monitor;
            _lastTimeInterval = 0;
            _speedPercentage = config.DefaultSpeedPercentage;
            _timeFrozen = false;
        }

        public void Update()
        {
            if (_timeFrozen)
                Game1.gameTimeInterval = 0;

            if (Game1.gameTimeInterval < _lastTimeInterval)
            {
                _lastTimeInterval = 0;
            }

            Game1.gameTimeInterval = _lastTimeInterval + (Game1.gameTimeInterval - _lastTimeInterval) * _speedPercentage / 100;
            _lastTimeInterval = Game1.gameTimeInterval;
        }

        private void OnSpeedUpdate()
        {
            string message = I18n.Message_TimeUpdate(Percentage: _speedPercentage);
            _monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message);
        }

        public void IncreaseSpeed()
        {
            if (_speedPercentage < 690)
                _speedPercentage += 10;
            OnSpeedUpdate();
        }

        public void DecreaseSpeed()
        {
            if (_speedPercentage > 10)
                _speedPercentage -= 10;
            OnSpeedUpdate();
        }

        public void ResetSpeed()
        {
            _speedPercentage = _config.DefaultSpeedPercentage;
            OnSpeedUpdate();
        }

        public void SetHalfSpeed()
        {
            _speedPercentage = 50;
            OnSpeedUpdate();
        }

        public void SetDoubleSpeed()
        {
            _speedPercentage = 200;
            OnSpeedUpdate();
        }

        public void ToggleFreeze()
        {
            _timeFrozen = !_timeFrozen;

            string message = _timeFrozen ? I18n.Message_TimeFrozen() : I18n.Message_TimeUnfrozen();
            _monitor.Log(message, LogLevel.Info);
            Notifier.DisplayHudNotification(message, logLevel: LogLevel.Warn);
        }
    }
}

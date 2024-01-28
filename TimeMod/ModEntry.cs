using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using TimeMod.Framework;

namespace TimeMod
{
    internal class ModEntry : Mod
    {
        private ModConfig Config;

        private int LastTimeInterval;

        private int SpeedPercentage;

        private bool TimeFrozen;

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            Config = helper.ReadConfig<ModConfig>();

            LastTimeInterval = 0;
            SpeedPercentage = Config.DefaultSpeedPercentage;
            TimeFrozen = false;

            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
            helper.Events.Input.ButtonsChanged += OnButtonsChanged;
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsMainPlayer)
                Monitor.Log(I18n.Message_NotMainPlayerWarning(), LogLevel.Warn);
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer)
            {
                return;
            }

            UpdateTime();
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer)
                return;

            if (Config.Keys.ReloadConfig.JustPressed())
                ReloadConfig();
            else if (Config.Keys.IncreaseSpeed.JustPressed())
                IncreaseSpeed();
            else if (Config.Keys.DecreaseSpeed.JustPressed())
                DecreaseSpeed();
            else if (Config.Keys.ResetSpeed.JustPressed())
                ResetSpeed();
            else if (Config.Keys.DoubleSpeed.JustPressed())
                SetDoubleSpeed();
            else if (Config.Keys.HalfSpeed.JustPressed())
                SetHalfSpeed();
            else if (Config.Keys.ToggleFreeze.JustPressed())
                ToggleFreeze();
        }

        private void UpdateTime()
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

        private void ReloadConfig()
        {
            Config = Helper.ReadConfig<ModConfig>();
            Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
        }

        private void IncreaseSpeed()
        {
            if (SpeedPercentage < 700)
                SpeedPercentage += 10;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }
        
        private void DecreaseSpeed()
        {
            if (SpeedPercentage > 10)
                SpeedPercentage -= 10;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        private void ResetSpeed()
        {
            SpeedPercentage = Config.DefaultSpeedPercentage;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        private void SetHalfSpeed()
        {
            SpeedPercentage = 50;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        private void SetDoubleSpeed()
        {
            SpeedPercentage = 200;
            Monitor.Log(I18n.Message_TimeUpdate(Percentage: SpeedPercentage), LogLevel.Info);
        }

        private void ToggleFreeze()
        {
            TimeFrozen = !TimeFrozen;
            if (TimeFrozen)
                Monitor.Log(I18n.Message_TimeFrozen(), LogLevel.Info);
            else
                Monitor.Log(I18n.Message_TimeUnfrozen(), LogLevel.Info);
        }
    }
}

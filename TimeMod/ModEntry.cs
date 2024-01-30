using StardewModdingAPI;
using StardewModdingAPI.Events;
using TimeMod.Framework;

namespace TimeMod
{
    internal class ModEntry : Mod
    {
        private ModConfig _config;

        private TimeHelper _timeHelper;

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            _config = helper.ReadConfig<ModConfig>();
            _timeHelper = new(_config, Monitor);

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

            _timeHelper.Update();
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer)
                return;

            if (_config.Keys.ReloadConfig.JustPressed())
                ReloadConfig();
            else if (_config.Keys.IncreaseSpeed.JustPressed())
                _timeHelper.IncreaseSpeed();
            else if (_config.Keys.DecreaseSpeed.JustPressed())
                _timeHelper.DecreaseSpeed();
            else if (_config.Keys.ResetSpeed.JustPressed())
                _timeHelper.ResetSpeed();
            else if (_config.Keys.DoubleSpeed.JustPressed())
                _timeHelper.SetDoubleSpeed();
            else if (_config.Keys.HalfSpeed.JustPressed())
                _timeHelper.SetHalfSpeed();
            else if (_config.Keys.ToggleFreeze.JustPressed())
                _timeHelper.ToggleFreeze();
        }

        private void ReloadConfig()
        {
            _config = Helper.ReadConfig<ModConfig>();
            Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
        }
    }
}

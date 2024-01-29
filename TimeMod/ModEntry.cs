using StardewModdingAPI;
using StardewModdingAPI.Events;
using TimeMod.Framework;

namespace TimeMod
{
    internal class ModEntry : Mod
    {
        private ModConfig Config;

        private TimeHelper TimeHelper;

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            Config = helper.ReadConfig<ModConfig>();
            TimeHelper = new(Config, Monitor);

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

            TimeHelper.Update();
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsMainPlayer)
                return;

            if (Config.Keys.ReloadConfig.JustPressed())
                ReloadConfig();
            else if (Config.Keys.IncreaseSpeed.JustPressed())
                TimeHelper.IncreaseSpeed();
            else if (Config.Keys.DecreaseSpeed.JustPressed())
                TimeHelper.DecreaseSpeed();
            else if (Config.Keys.ResetSpeed.JustPressed())
                TimeHelper.ResetSpeed();
            else if (Config.Keys.DoubleSpeed.JustPressed())
                TimeHelper.SetDoubleSpeed();
            else if (Config.Keys.HalfSpeed.JustPressed())
                TimeHelper.SetHalfSpeed();
            else if (Config.Keys.ToggleFreeze.JustPressed())
                TimeHelper.ToggleFreeze();
        }

        private void ReloadConfig()
        {
            Config = Helper.ReadConfig<ModConfig>();
            Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
        }
    }
}

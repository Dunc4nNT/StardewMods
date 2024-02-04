using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Tools;
using YetAnotherFishingMod.Framework;


namespace YetAnotherFishingMod
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig _config;

        private ModConfigKeys Keys => this._config.Keys;

        private FishHelper _fishHelper;

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            this._config = helper.ReadConfig<ModConfig>();
            this._fishHelper = new(this._config, this.Monitor, helper.Reflection);

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (Game1.player.CurrentTool is FishingRod fishingRod)
            {
                this._fishHelper.ApplyFishingRodBuffs(fishingRod);
            }

            if (this._fishHelper.IsInFishingMiniGame.Value)
            {
                this._fishHelper.ApplyFishingMiniGameBuffs();
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar bobberBar)
            {
                this._fishHelper.OnFishingMiniGameStart(bobberBar);
            }
            else if (e.OldMenu is BobberBar)
                this._fishHelper.OnFishingMiniGameEnd();
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (this.Keys.ReloadConfig.JustPressed())
                this.ReloadConfig();
        }

        private void ReloadConfig()
        {
            this._config = this.Helper.ReadConfig<ModConfig>();
            this.Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
            this.Monitor.Log($"instant catch changed to: {this._config.InstantCatchFish}");
        }
    }
}

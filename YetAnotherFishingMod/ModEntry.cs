using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
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

        private readonly PerScreen<SBobberBar> _bobberBar = new();

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            this._config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (Game1.player.CurrentTool is FishingRod fishingRod_)
            {
                SFishingRod fishingRod = new(fishingRod_, this.Helper.Reflection);

                if (this._config.AlwaysMaxCastingPower)
                    fishingRod.CastingPower = 1.01f;
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar bobberBar)
            {
                SBobberBar bobber = this._bobberBar.Value = new SBobberBar(bobberBar, this.Helper.Reflection);
                this.OnFishingStart(bobber);
            }
            else if (e.OldMenu is BobberBar)
                this._bobberBar.Value = null;
        }

        private void OnFishingStart(SBobberBar bobber)
        {
            if ((this._config.InstantCatchTreasure && bobber.Treasure) || this._config.AlwaysCatchTreasure)
                bobber.TreasureCaught = true;
            if (this._config.InstantCatchFish)
                bobber.DistanceFromCatching = 1.0f;
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
        }
    }
}

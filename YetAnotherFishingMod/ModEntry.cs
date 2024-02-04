using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;
using YetAnotherFishingMod.Framework;


namespace YetAnotherFishingMod
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig _config;

        private ModConfigKeys Keys => this._config.Keys;

        private readonly PerScreen<SBobberBar> _bobber = new();

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            this._config = helper.ReadConfig<ModConfig>();

            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar bar)
                this.OnFishingStart(bar);
            else if (e.OldMenu is BobberBar)
                this.OnFishingStop();
        }

        private void OnFishingStart(BobberBar bar)
        {
            SBobberBar bobber = this._bobber.Value = new SBobberBar(bar, this.Helper.Reflection);

            if (this._config.InstantCatchFish)
            {
                if (bobber.Treasure)
                    bobber.TreasureCaught = true;

                bobber.DistanceFromCatching = 1.0f;
            }
        }

        private void OnFishingStop()
        {
            this._bobber.Value = null;
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

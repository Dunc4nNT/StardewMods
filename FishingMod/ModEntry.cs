using FishingMod.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.Menus;


namespace FishingMod
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig _config;

        private readonly PerScreen<SBobberBar> _bobber = new();

        public override void Entry(IModHelper helper)
        {
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

            if (this._config.ReloadConfigButton.JustPressed())
                this._config = this.Helper.ReadConfig<ModConfig>();
        }
    }
}

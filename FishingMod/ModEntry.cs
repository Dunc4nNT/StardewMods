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
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.Input.ButtonsChanged += OnButtonsChanged;
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar bar)
                OnFishingStart(bar);
            else if (e.OldMenu is BobberBar)
                OnFishingStop();
        }

        private void OnFishingStart(BobberBar bar)
        {
            SBobberBar bobber = _bobber.Value = new SBobberBar(bar, Helper.Reflection);

            if (_config.InstantCatchFish)
            {
                if (bobber.Treasure)
                    bobber.TreasureCaught = true;

                bobber.DistanceFromCatching = 1.0f;
            }
        }

        private void OnFishingStop()
        {
            _bobber.Value = null;
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (_config.ReloadConfigButton.JustPressed())
                _config = Helper.ReadConfig<ModConfig>();
        }
    }
}

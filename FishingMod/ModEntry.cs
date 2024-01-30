using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewModdingAPI.Utilities;
using FishingMod.Framework;


namespace FishingMod
{
    internal sealed class ModEntry: Mod
    {
        private ModConfig Config;

        private readonly PerScreen<SBobberBar> Bobber = new();

        public override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<ModConfig>();

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
            SBobberBar bobber = Bobber.Value = new SBobberBar(bar, Helper.Reflection);

            if (Config.InstantCatchFish)
            {
                if (bobber.Treasure)
                    bobber.TreasureCaught = true;

                bobber.DistanceFromCatching = 1.0f;
            }
        }

        private void OnFishingStop()
        {
            Bobber.Value = null;
        }

        private void OnButtonsChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (Config.ReloadConfigButton.JustPressed())
                Config = Helper.ReadConfig<ModConfig>();
        }
    }
}

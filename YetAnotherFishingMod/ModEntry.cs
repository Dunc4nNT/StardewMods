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
        private ModConfig Config { get; set; }

        private ModConfigKeys Keys => this.Config.Keys;

        private FishHelper FishHelper { get; set; }

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            this.Config = helper.ReadConfig<ModConfig>();
            this.FishHelper = new(() => this.Config, this.Monitor, helper.Reflection);

            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (Game1.player.CurrentTool is FishingRod fishingRod)
                this.FishHelper.OnFishingRodEquipped(fishingRod);
            else if (Game1.player.CurrentTool is not FishingRod)
                this.FishHelper.OnFishingRodNotEquipped();

            if (this.FishHelper.IsInFishingMiniGame.Value)
                this.FishHelper.ApplyFishingMiniGameBuffs();
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu is BobberBar bobberBar)
                this.FishHelper.OnFishingMiniGameStart(bobberBar);
            else if (e.OldMenu is BobberBar)
                this.FishHelper.OnFishingMiniGameEnd();
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
            this.Config = this.Helper.ReadConfig<ModConfig>();
            this.Monitor.Log(I18n.Message_ConfigReloaded(), LogLevel.Info);
        }
    }
}

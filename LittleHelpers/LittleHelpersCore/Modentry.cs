using HarmonyLib;
using NeverToxic.StardewMods.LittleHelpersCore.Framework;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Patches;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace NeverToxic.StardewMods.LittleHelpersCore
{
    internal class Modentry : Mod
    {
        private ModConfig Config { get; set; }

        private ModConfigKeys Keys => this.Config.Keys;

        private LittleHelpersHelper LittleHelpersHelper { get; set; }

        public const string AssetsModId = "NeverToxic.LittleHelpersAssets";

        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);

            this.Config = helper.ReadConfig<ModConfig>();
            Harmony harmony = new(this.ModManifest.UniqueID);
            ObjectPatch.Initialise(harmony, AssetsModId, this.Monitor, () => this.Config, this.Helper.Reflection);
            this.LittleHelpersHelper = new(() => this.Config, this.Monitor);

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Input.ButtonsChanged += this.OnButtonsChanged;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        }

        public void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.LittleHelpersHelper.ExecuteAllActions();
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            new GenericModConfigMenu(this.Helper.ModRegistry, this.ModManifest, this.Monitor, () => this.Config, () => this.Config = new ModConfig(), () => this.Helper.WriteConfig(this.Config)).Register();
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

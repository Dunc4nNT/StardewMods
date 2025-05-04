using StardewModdingAPI;
using System;
using NeverToxic.StardewMods.Common;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Config
{
    internal class GenericModConfigMenu(IModRegistry modRegistry, IManifest manifest, IMonitor monitor, Func<ModConfig> config, Action reset, Action save)
    {
        public void Register()
        {
            var configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;

            configMenu.Register(mod: manifest, reset: reset, save: save);

        }
    }
}

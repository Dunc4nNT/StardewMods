using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using System;

namespace NeverToxic.StardewMods.SmapiTemplate.Framework
{
    internal class GenericModConfigMenu(IModRegistry modRegistry, IManifest manifest, IMonitor monitor, Func<ModConfig> config, Action reset, Action save)
    {
        public void Register()
        {
            IGenericModConfigMenuApi configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;

            configMenu.Register(mod: manifest, reset: reset, save: save);
        }
    }
}

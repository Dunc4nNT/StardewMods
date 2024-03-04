using NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using System;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework
{
    internal class LittleHelpersHelper(Func<ModConfig> config, IMonitor monitor)
    {
        public List<BaseBuilding> Buildings { get; set; } = [];

        public void ExecuteAllActions()
        {
            foreach (BaseBuilding building in this.Buildings)
                building.ExecuteActions();
        }
    }
}

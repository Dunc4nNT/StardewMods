using NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework
{
    internal class LittleHelpersHelper(Func<ModConfig> config, IMonitor monitor)
    {
        public void ExecuteAllActions()
        {
            List<BaseBuilding> buildings = this.GetAllBuildings();
            foreach (BaseBuilding building in buildings)
                building.ExecuteActions();
        }

        private List<BaseBuilding> GetAllBuildings()
        {
            foreach (GameLocation location in Game1.locations)
            {
            }
            return new();
        }
    }
}

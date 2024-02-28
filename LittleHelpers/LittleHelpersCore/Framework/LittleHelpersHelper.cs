using NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework
{
    internal class LittleHelpersHelper
    {
        public List<BaseBuilding> Buildings { get; set; } = [];

        public void OnDayChanged()
        {
            foreach (BaseBuilding building in this.Buildings)
                building.ExecuteCommands();
        }
    }
}

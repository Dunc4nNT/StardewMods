using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings
{
    internal interface IBuilding
    {
        List<BaseAction> Actions { get; set; }

        List<int> Tiles { get; }

        int? Radius { get; set; }

        int HelperCapacity { get; set; }

        int? Location { get; set; }

        ILocationCheck LocationCheck { get; set; }

        void ExecuteActions();
    }
}

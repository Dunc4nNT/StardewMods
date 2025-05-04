using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings
{
    internal class BaseBuilding(int helperCapacity, ILocationCheck locationCheck, int? radius = null, int? location = null) : IBuilding
    {
        public virtual List<BaseAction> Actions { get; set; } = [];

        public List<int> Tiles { get; } = [1];

        public int? Radius { get; set; } = radius;

        public int HelperCapacity { get; set; } = helperCapacity;

        public int? Location { get; set; } = location;

        public ILocationCheck LocationCheck { get; set; } = locationCheck;

        public virtual void ExecuteActions()
        {
            if (this.Actions is null)
                return;

            foreach (int tile in this.Tiles)
                foreach (BaseAction action in this.Actions)
                    action.Handle(tile);
        }
    }
}

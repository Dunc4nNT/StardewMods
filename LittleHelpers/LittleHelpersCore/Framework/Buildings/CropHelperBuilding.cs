using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings
{
    internal class CropHelperBuilding(int helperCapacity, ILocationCheck locationCheck, int radius, int? location = null, bool canScareCrows = false, bool canReplantCrops = false) : BaseBuilding(helperCapacity, locationCheck, radius: radius, location: location)
    {
        public override List<BaseAction> Actions { get; set; } = [new HarvestCropAction(), new CollectFromTreeAction()];

        public bool CanScareCrows { get; } = canScareCrows;

        public bool CanReplantCrops { get; } = canReplantCrops;

        public override void ExecuteActions()
        {
            if (this.Actions is null)
                return;

            foreach (int tile in this.Tiles)
                foreach (BaseAction action in this.Actions)
                {
                    action.Handle(tile);

                    if (action is HarvestCropAction && this.CanReplantCrops)
                    {
                        int seed = 0;
                        this.ReplantCrop(tile, seed);
                    }
                }
        }

        private void ReplantCrop(int tile, int seed)
        {
        }
    }
}

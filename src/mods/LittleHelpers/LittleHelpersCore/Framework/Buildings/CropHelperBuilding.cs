// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;

using System.Collections.Generic;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;

internal class CropHelperBuilding(
    int helperCapacity,
    ILocationCheck locationCheck,
    int radius,
    int? location = null,
    bool canScareCrows = false,
    bool canReplantCrops = false) : BaseBuilding(helperCapacity, locationCheck, radius, location)
{
    public override List<BaseAction> Actions { get; set; } = [new HarvestCropAction(), new HarvestTreeAction()];

    public bool CanScareCrows { get; } = canScareCrows;

    private bool CanReplantCrops { get; } = canReplantCrops;

    public override void ExecuteActions()
    {
        if (this.Actions.Count == 0)
        {
            return;
        }

        foreach (int tile in this.Tiles)
        {
            foreach (BaseAction action in this.Actions)
            {
                action.Handle(tile);

                if (action is not HarvestCropAction || !this.CanReplantCrops)
                {
                    continue;
                }

                int seed = 0;
                this.ReplantCrop(tile, seed);
            }
        }
    }

    private void ReplantCrop(int tile, int seed)
    {
    }
}

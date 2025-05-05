// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;

using System.Collections.Generic;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;

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
        if (this.Actions.Count == 0)
        {
            return;
        }

        foreach (int tile in this.Tiles)
        {
            foreach (BaseAction action in this.Actions)
            {
                action.Handle(tile);
            }
        }
    }
}

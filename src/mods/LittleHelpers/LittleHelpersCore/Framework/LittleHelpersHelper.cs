// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework;

using System;
using System.Collections.Generic;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Config;
using StardewModdingAPI;
using StardewValley;

internal class LittleHelpersHelper(Func<ModConfig> config, IMonitor monitor)
{
    public void ExecuteAllActions()
    {
        List<BaseBuilding> buildings = this.GetAllBuildings();
        foreach (BaseBuilding building in buildings)
        {
            building.ExecuteActions();
        }
    }

    private List<BaseBuilding> GetAllBuildings()
    {
        foreach (GameLocation location in Game1.locations)
        {
        }

        return [];
    }
}

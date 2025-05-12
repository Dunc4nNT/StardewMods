// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings;

using System.Collections.Generic;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Checks;

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

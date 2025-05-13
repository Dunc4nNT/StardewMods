// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.TestMod.Framework.Patches;

internal class Patcher(ModEntry mod)
{
    internal void PatchAll()
    {
        MyPatches.Patch(mod);
    }
}

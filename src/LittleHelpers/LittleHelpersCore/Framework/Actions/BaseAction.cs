// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions;

internal abstract class BaseAction : IAction
{
    public abstract bool CanExecute(int tile);

    public abstract void Execute(int tile);

    public void Handle(int tile)
    {
        if (this.CanExecute(tile))
        {
            this.Execute(tile);
        }
    }
}

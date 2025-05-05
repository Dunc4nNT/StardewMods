// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.TelevisionFramework.Framework;

using StardewModdingAPI.Utilities;

internal class ModConfig
{
    public ModConfigKeys Keys { get; set; } = new();
}

internal class ModConfigKeys
{
    public KeybindList ReloadConfig { get; set; } = new();
}

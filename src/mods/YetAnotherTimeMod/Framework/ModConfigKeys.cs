// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherTimeMod.Framework;

using StardewModdingAPI.Utilities;

internal class ModConfigKeys
{
    public KeybindList ReloadConfig { get; set; } = KeybindList.Parse("F5");

    public KeybindList IncreaseSpeed { get; set; } = KeybindList.Parse("F7");

    public KeybindList DecreaseSpeed { get; set; } = KeybindList.Parse("F6");

    public KeybindList ResetSpeed { get; set; } = KeybindList.Parse("F8");

    public KeybindList ToggleFreeze { get; set; } = KeybindList.Parse("F9");

    public KeybindList HalfSpeed { get; set; } = KeybindList.Parse("F10");

    public KeybindList DoubleSpeed { get; set; } = KeybindList.Parse("F11");
}

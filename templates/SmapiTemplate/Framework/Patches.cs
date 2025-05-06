// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.SmapiTemplate.Framework;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1313:Parameter names should begin with lower-case letter",
    Justification = "Harmony naming convention has double underscore.")]
internal static class Patches
{
    private static ModEntry? Mod { get; set; }

    public static void Patch(ModEntry mod)
    {
        Mod = mod;
    }
}

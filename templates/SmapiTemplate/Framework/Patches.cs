// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.SmapiTemplate.Framework;

using System;
using HarmonyLib;
using StardewModdingAPI;

internal class Patches
{
    private static Harmony s_harmony;
    private static IMonitor s_monitor;
    private static Func<ModConfig> s_config;
    private static IReflectionHelper s_reflectionHelper;

    internal static void Initialise(Harmony harmony, IMonitor monitor, Func<ModConfig> config, IReflectionHelper reflectionHelper)
    {
        s_harmony = harmony;
        s_monitor = monitor;
        s_config = config;
        s_reflectionHelper = reflectionHelper;

        ApplyPatches();
    }

    private static void ApplyPatches()
    {
    }
}

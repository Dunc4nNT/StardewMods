// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.Common;

using StardewModdingAPI;
using StardewValley;

public static class Notifier
{
    public static void DisplayHudNotification(string message, float duration = 2500, LogLevel logLevel = LogLevel.Info)
    {
        switch (logLevel)
        {
        case LogLevel.Warn:
        case LogLevel.Alert:
            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.newQuest_type) { timeLeft = duration });
            break;
        case LogLevel.Error:
            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.error_type) { timeLeft = duration });
            break;
        case LogLevel.Trace:
        case LogLevel.Debug:
        case LogLevel.Info:
        default:
            Game1.addHUDMessage(new HUDMessage(message, HUDMessage.newQuest_type) { timeLeft = duration, noIcon = true });
            break;
        }
    }
}

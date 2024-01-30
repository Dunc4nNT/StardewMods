using StardewModdingAPI;
using StardewValley;

namespace Common
{
    public class Notifier
    {
        public static void DisplayHudNotification(string message, float duration = 2500, LogLevel logLevel = LogLevel.Info)
        {
            if (logLevel is LogLevel.Warn || logLevel is LogLevel.Alert)
                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.newQuest_type) { timeLeft = duration });
            else if (logLevel is LogLevel.Error)
                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.error_type) { timeLeft = duration });
            else
                Game1.addHUDMessage(new HUDMessage(message, HUDMessage.newQuest_type) { timeLeft = duration, noIcon = true });
        }
    }
}

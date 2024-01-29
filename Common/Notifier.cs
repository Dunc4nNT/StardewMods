using StardewModdingAPI;
using StardewValley;

namespace Common
{
    public class Notifier
    {
        public static void DisplayHudNotification(string Message, float Duration = 2500, LogLevel LogLevel = LogLevel.Info)
        {
            if (LogLevel is LogLevel.Warn || LogLevel is LogLevel.Alert)
                Game1.addHUDMessage(new HUDMessage(Message, HUDMessage.newQuest_type) { timeLeft = Duration });
            else if (LogLevel is LogLevel.Error)
                Game1.addHUDMessage(new HUDMessage(Message, HUDMessage.error_type) { timeLeft = Duration });
            else
                Game1.addHUDMessage(new HUDMessage(Message, HUDMessage.newQuest_type) { timeLeft = Duration, noIcon = true });
        }
    }
}

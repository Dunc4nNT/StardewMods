using StardewValley;

namespace Common
{
    public class Notifier
    {
        public static void HudNotify(string Message, float duration = 2500)
        {
            Game1.addHUDMessage(new HUDMessage(Message, HUDMessage.newQuest_type) { timeLeft = duration });
        }
    }
}

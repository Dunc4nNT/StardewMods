namespace YetAnotherTimeMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public int DefaultSpeedPercentage { get; set; } = 100;
    }
}

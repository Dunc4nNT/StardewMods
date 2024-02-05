namespace YetAnotherFishingMod.Framework
{
    internal class ModConfig
    {
        public ModConfigKeys Keys { get; set; } = new();

        public bool InstantCatchFish { get; set; } = true;

        public bool InstantCatchTreasure { get; set; } = true;

        public bool AlwaysMaxCastingPower { get; set; } = false;

        public bool AlwaysPerfect { get; set; } = false;

        public bool AlwaysCatchTreasure { get; set; } = false;
        /*
        private static ModConfig s_instance;

        public static ModConfig GetInstance()
        {
            s_instance ??= new();
            return s_instance;
        }

        public void LoadConfig(IModHelper helper)
        {
            s_instance = helper.ReadConfig<ModConfig>();
        }
        */
    }
}

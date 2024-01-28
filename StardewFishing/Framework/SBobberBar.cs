using StardewModdingAPI;
using StardewValley.Menus;

namespace FishingMod.Framework
{
    internal class SBobberBar
    {
        private readonly IReflectedField<float> DistanceFromCatchingField;
        private readonly IReflectedField<bool> TreasureField;
        private readonly IReflectedField<bool> TreasureCaughtField;

        public BobberBar Instance { get; set; }

        public float DistanceFromCatching { 
            get => DistanceFromCatchingField.GetValue();
            set => DistanceFromCatchingField.SetValue(value);
        }
        public bool Treasure
        {
            get => TreasureField.GetValue();
            set => TreasureField.SetValue(value);
        }
        public bool TreasureCaught
        {
            get => TreasureCaughtField.GetValue();
            set => TreasureCaughtField.SetValue(value);
        }

        public SBobberBar(BobberBar instance, IReflectionHelper reflection)
        {
            Instance = instance;

            DistanceFromCatchingField = reflection.GetField<float>(instance, "distanceFromCatching");
            TreasureField = reflection.GetField<bool>(instance, "treasure");
            TreasureCaughtField = reflection.GetField<bool>(instance, "treasureCaught");
        }
    }
}

using StardewModdingAPI;
using StardewValley.Menus;

namespace FishingMod.Framework
{
    internal class SBobberBar
    {
        private readonly IReflectedField<float> _distanceFromCatchingField;
        private readonly IReflectedField<bool> _treasureField;
        private readonly IReflectedField<bool> _treasureCaughtField;

        public BobberBar Instance { get; set; }

        public float DistanceFromCatching
        {
            get => _distanceFromCatchingField.GetValue();
            set => _distanceFromCatchingField.SetValue(value);
        }
        public bool Treasure
        {
            get => _treasureField.GetValue();
            set => _treasureField.SetValue(value);
        }
        public bool TreasureCaught
        {
            get => _treasureCaughtField.GetValue();
            set => _treasureCaughtField.SetValue(value);
        }

        public SBobberBar(BobberBar instance, IReflectionHelper reflection)
        {
            Instance = instance;

            _distanceFromCatchingField = reflection.GetField<float>(instance, "distanceFromCatching");
            _treasureField = reflection.GetField<bool>(instance, "treasure");
            _treasureCaughtField = reflection.GetField<bool>(instance, "treasureCaught");
        }
    }
}

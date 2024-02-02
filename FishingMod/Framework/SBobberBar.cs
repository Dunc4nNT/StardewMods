using StardewModdingAPI;
using StardewValley.Menus;

namespace FishingMod.Framework
{
    internal class SBobberBar(BobberBar instance, IReflectionHelper reflection)
    {
        private readonly IReflectedField<float> _distanceFromCatchingField = reflection.GetField<float>(instance, "distanceFromCatching");
        private readonly IReflectedField<bool> _treasureField = reflection.GetField<bool>(instance, "treasure");
        private readonly IReflectedField<bool> _treasureCaughtField = reflection.GetField<bool>(instance, "treasureCaught");

        public BobberBar Instance { get; set; } = instance;

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
    }
}

using StardewModdingAPI;
using StardewValley.Menus;

namespace YetAnotherFishingMod.Framework
{
    internal class SBobberBar(BobberBar instance, IReflectionHelper reflection)
    {
        private readonly IReflectedField<float> _distanceFromCatchingField = reflection.GetField<float>(instance, "distanceFromCatching");
        private readonly IReflectedField<bool> _treasureField = reflection.GetField<bool>(instance, "treasure");
        private readonly IReflectedField<bool> _treasureCaughtField = reflection.GetField<bool>(instance, "treasureCaught");

        public BobberBar Instance { get; set; } = instance;

        public float DistanceFromCatching
        {
            get => this._distanceFromCatchingField.GetValue();
            set => this._distanceFromCatchingField.SetValue(value);
        }
        public bool Treasure
        {
            get => this._treasureField.GetValue();
            set => this._treasureField.SetValue(value);
        }
        public bool TreasureCaught
        {
            get => this._treasureCaughtField.GetValue();
            set => this._treasureCaughtField.SetValue(value);
        }
    }
}

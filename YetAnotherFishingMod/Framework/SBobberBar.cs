using StardewModdingAPI;
using StardewValley.Menus;

namespace YetAnotherFishingMod.Framework
{
    internal class SBobberBar(BobberBar instance, IReflectionHelper reflection)
    {
        private readonly IReflectedField<float> _distanceFromCatching = reflection.GetField<float>(instance, "distanceFromCatching");
        private readonly IReflectedField<bool> _perfect = reflection.GetField<bool>(instance, "perfect");
        private readonly IReflectedField<bool> _treasure = reflection.GetField<bool>(instance, "treasure");
        private readonly IReflectedField<bool> _treasureCaught = reflection.GetField<bool>(instance, "treasureCaught");
        private readonly IReflectedField<float> _difficulty = reflection.GetField<float>(instance, "difficulty");

        public BobberBar Instance { get; set; } = instance;

        public float DistanceFromCatching
        {
            get => this._distanceFromCatching.GetValue();
            set => this._distanceFromCatching.SetValue(value);
        }

        public bool Perfect
        {
            get => this._perfect.GetValue();
            set => this._perfect.SetValue(value);
        }

        public bool Treasure
        {
            get => this._treasure.GetValue();
            set => this._treasure.SetValue(value);
        }

        public bool TreasureCaught
        {
            get => this._treasureCaught.GetValue();
            set => this._treasureCaught.SetValue(value);
        }

        public float Difficulty
        {
            get => this._difficulty.GetValue();
            set => this._difficulty.SetValue(value);
        }
    }
}

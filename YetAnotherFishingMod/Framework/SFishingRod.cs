using StardewModdingAPI;
using StardewValley.Tools;

namespace YetAnotherFishingMod.Framework
{
    internal class SFishingRod(FishingRod instance, IReflectionHelper reflection)
    {
        private readonly IReflectedField<float> _castingPower = reflection.GetField<float>(instance, "castingPower");

        public FishingRod Instance { get; set; } = instance;

        public float CastingPower
        {
            get => this._castingPower.GetValue();
            set => this._castingPower.SetValue(value);
        }
    }
}

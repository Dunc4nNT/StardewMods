using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;

namespace YetAnotherFishingMod.Framework
{
    internal class SFishingRod(FishingRod instance, IReflectionHelper reflection)
    {
        private readonly IReflectedField<float> _castingPower = reflection.GetField<float>(instance, "castingPower");
        private readonly IReflectedField<bool> _caughtDoubleFish = reflection.GetField<bool>(instance, "caughtDoubleFish");
        private readonly IReflectedField<float> _timeUntilFishingBite = reflection.GetField<float>(instance, "timeUntilFishingBite");
        private readonly IReflectedField<bool> _isNibbling = reflection.GetField<bool>(instance, "isNibbling");
        private readonly IReflectedField<bool> _hit = reflection.GetField<bool>(instance, "hit");
        private readonly IReflectedField<bool> _isReeling = reflection.GetField<bool>(instance, "isReeling");
        private readonly IReflectedField<bool> _pullingOutOfWater = reflection.GetField<bool>(instance, "pullingOutOfWater");
        private readonly IReflectedField<bool> _fishCaught = reflection.GetField<bool>(instance, "fishCaught");
        private readonly IReflectedField<bool> _showingTreasure = reflection.GetField<bool>(instance, "showingTreasure");
        private readonly IReflectedField<float> _timePerBobberBob = reflection.GetField<float>(instance, "timePerBobberBob");
        private readonly IReflectedField<float> _timeUntilFishingNibbleDone = reflection.GetField<float>(instance, "timeUntilFishingNibbleDone");

        public FishingRod Instance { get; set; } = instance;

        private readonly int _initialAttachmentSlotsCount = instance.AttachmentSlotsCount;
        private readonly Object _initialBait = instance.GetBait();
        private readonly Object _initialTackle = instance.GetTackle();

        public int InitialAttachmentSlotsCount => this._initialAttachmentSlotsCount;
        public Object InitialBait => this._initialBait;
        public Object InitialTackle => this._initialTackle;

        public float CastingPower
        {
            get => this._castingPower.GetValue();
            set => this._castingPower.SetValue(value);
        }

        public bool CaughtDoubleFish
        {
            get => this._caughtDoubleFish.GetValue();
            set => this._caughtDoubleFish.SetValue(value);
        }

        public float TimeUntilFishingBite
        {
            get => this._timeUntilFishingBite.GetValue();
            set => this._timeUntilFishingBite.SetValue(value);
        }

        public bool IsNibbling => this._isNibbling.GetValue();

        public bool Hit => this._hit.GetValue();

        public bool IsReeling => this._isReeling.GetValue();

        public bool PullingOutOfWater => this._pullingOutOfWater.GetValue();

        public bool FishCaught => this._fishCaught.GetValue();

        public bool ShowingTreasure => this._showingTreasure.GetValue();

        public float TimePerBobberBob
        {
            get => this._timePerBobberBob.GetValue();
            set => this._timePerBobberBob.SetValue(value);
        }

        public float TimeUntilFishingNibbleDone
        {
            get => this._timeUntilFishingNibbleDone.GetValue();
            set => this._timeUntilFishingNibbleDone.SetValue(value);
        }
    }
}

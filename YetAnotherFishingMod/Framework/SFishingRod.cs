using StardewValley;
using StardewValley.Tools;
using SObject = StardewValley.Object;

namespace YetAnotherFishingMod.Framework
{
    internal class SFishingRod(FishingRod instance)
    {
        public FishingRod Instance { get; set; } = instance;

        public int InitialAttachmentSlotsCount { get; } = instance.AttachmentSlotsCount;

        public SObject InitialBait { get; } = instance.GetBait();

        public SObject InitialTackle { get; } = instance.GetTackle();

        private bool CanHook()
        {
            return
                this.Instance.isNibbling &&
                !this.Instance.hit &&
                !this.Instance.isReeling &&
                !this.Instance.pullingOutOfWater &&
                !this.Instance.fishCaught &&
                !this.Instance.showingTreasure
            ;
        }

        public void AutoHook()
        {
            if (this.CanHook())
            {
                this.Instance.timePerBobberBob = 1f;
                this.Instance.timeUntilFishingNibbleDone = FishingRod.maxTimeToNibble;
                this.Instance.DoFunction(Game1.player.currentLocation, (int)this.Instance.bobber.X, (int)this.Instance.bobber.Y, 1, Game1.player);
                Rumble.rumble(0.95f, 200f);
            }
        }

        public void SpawnBait(int baitId)
        {
            this.Instance.attachments[0] = ItemRegistry.Create<SObject>($"(O){baitId}");
        }

        public void SpawnTackle(int tackleId)
        {
            this.Instance.attachments[1] = ItemRegistry.Create<SObject>($"(O){tackleId}");
        }

        public void ResetAttachments()
        {
            if (this.Instance.AttachmentSlotsCount >= 1)
                this.Instance.attachments[0] = this.InitialBait;
            if (this.Instance.AttachmentSlotsCount >= 2)
                this.Instance.attachments[1] = this.InitialTackle;

            this.Instance.AttachmentSlotsCount = this.InitialAttachmentSlotsCount;
        }

        public void InfiniteBait()
        {
            SObject bait = this.Instance.GetBait();
            if (bait is not null)
                bait.Stack = bait.maximumStackSize();
        }

        public void InfiniteTackle()
        {
            SObject tackle = this.Instance.GetTackle();
            if (tackle is not null)
                tackle.uses.Value = 0;
        }

        public void InstantBite()
        {
            if (this.Instance.timeUntilFishingBite > 0)
                this.Instance.timeUntilFishingBite = 0f;
        }
    }
}

// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework;

using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.Tools;
using SObject = StardewValley.Object;

internal class SFishingRod(FishingRod instance)
{
    private readonly int initialAttachmentSlotsCount = instance.AttachmentSlotsCount;

    private readonly List<BaseEnchantment> addedEnchantments = [];

    public FishingRod Instance { get; set; } = instance;

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

    public void AutoHook(bool doVibrate)
    {
        if (!this.CanHook())
        {
            return;
        }

        this.Instance.timePerBobberBob = 1f;
        this.Instance.timeUntilFishingNibbleDone = FishingRod.maxTimeToNibble;
        this.Instance.DoFunction(
            Game1.player.currentLocation,
            (int)this.Instance.bobber.X,
            (int)this.Instance.bobber.Y,
            1,
            Game1.player);

        if (doVibrate)
        {
            Rumble.rumble(0.95f, 200f);
        }
    }

    public void SpawnBait(List<string?> baitIds, int amountOfBait = 1, bool overrideAttachmentLimit = false)
    {
        for (int i = FishingRod.BaitIndex; i < FishingRod.TackleIndex; i++)
        {
            string? baitId = baitIds.ElementAtOrDefault(i);

            if (string.IsNullOrEmpty(baitId) || ItemRegistry.GetDataOrErrorItem(baitId).IsErrorItem)
            {
                continue;
            }

            if (overrideAttachmentLimit && this.Instance.AttachmentSlotsCount < i + 1)
            {
                if (Game1.server != null)
                {
                    return;
                }

                this.Instance.AttachmentSlotsCount = i + 1;
            }

            if (this.Instance.AttachmentSlotsCount > i && this.Instance.attachments.ElementAt(i) == null)
            {
                this.Instance.attachments[i] = ItemRegistry.Create<SObject>(baitId, amountOfBait);
            }
        }
    }

    public void SpawnTackles(List<string?> tackleIds, bool overrideAttachmentLimit = false)
    {
        for (int i = FishingRod.TackleIndex; i < FishingRod.TackleIndex + 2; i++)
        {
            string? tackleId = tackleIds.ElementAtOrDefault(i - FishingRod.TackleIndex);

            if (string.IsNullOrEmpty(tackleId) || ItemRegistry.GetDataOrErrorItem(tackleId).IsErrorItem)
            {
                continue;
            }

            if (overrideAttachmentLimit && this.Instance.AttachmentSlotsCount < i + 1)
            {
                if (Game1.server != null)
                {
                    return;
                }

                this.Instance.AttachmentSlotsCount = i + 1;
            }

            if (this.Instance.AttachmentSlotsCount > i && this.Instance.attachments.ElementAt(i) == null)
            {
                this.Instance.attachments[i] = ItemRegistry.Create<SObject>(tackleId);
            }
        }
    }

    public void ResetAttachmentsLimit()
    {
        if (this.initialAttachmentSlotsCount == this.Instance.AttachmentSlotsCount)
        {
            return;
        }

        for (int i = this.Instance.AttachmentSlotsCount; i > this.initialAttachmentSlotsCount; i--)
        {
            this.Instance.attachments[i - 1] = null;
        }

        this.Instance.AttachmentSlotsCount = this.initialAttachmentSlotsCount;
    }

    public void AddEnchantment(BaseEnchantment enchantment)
    {
        this.Instance.enchantments.Add(enchantment);
        this.addedEnchantments.Add(enchantment);
    }

    public void ResetEnchantments()
    {
        foreach (BaseEnchantment enchantment in this.addedEnchantments)
        {
            this.Instance.enchantments.Remove(enchantment);
        }
    }

    public void InstantBite()
    {
        if (this.Instance.timeUntilFishingBite > 0)
        {
            this.Instance.timeUntilFishingBite = 0f;
        }
    }
}

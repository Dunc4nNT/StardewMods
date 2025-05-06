// Copyright (c) Dunc4nNT.
//
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework;

using System.Collections.Generic;

internal class ModConfig
{
    public ModConfigKeys Keys { get; set; } = new();

    public bool SkipMinigamePopup { get; set; } = true;

    public bool DisableVibrations { get; set; }

    public float TreasureInBarMultiplier { get; set; } = 1f;

    public float FishInBarMultiplier { get; set; } = 1f;

    public bool DoSpeedUpAnimations { get; set; }

    public bool AutoLootFish { get; set; }

    public float FishNotInBarPenaltyMultiplier { get; set; } = 1f;

    public float BarSizeMultiplier { get; set; } = 1f;

    public bool AllowCatchingFish { get; set; } = true;

    public bool AllowCatchingRubbish { get; set; } = true;

    public bool AllowCatchingOther { get; set; } = true;

    public bool SkipFishingMinigame { get; set; } = true;

    public int SkipFishingMinigameCatchesRequired { get; set; }

    public float SkipFishingMinigamePerfectChance { get; set; } = 1f;

    public float SkipFishingMinigameTreasureChance { get; set; } = 1f;

    public bool InstantCatchTreasure { get; set; }

    public bool AlwaysMaxCastingPower { get; set; }

    public bool AlwaysPerfect { get; set; }

    public TreasureAppearanceSettings TreasureAppearance { get; set; } = TreasureAppearanceSettings.Vanilla;

    public TreasureAppearanceSettings GoldenTreasureAppearance { get; set; } = TreasureAppearanceSettings.Vanilla;

    public int NumberOfFishCaught { get; set; } = 1;

    public bool InstantBite { get; set; }

    public bool AutoHook { get; set; }

    public Quality FishQuality { get; set; } = Quality.Any;

    public Quality MinimumFishQuality { get; set; } = Quality.None;

    public List<string?> BaitToSpawn { get; set; } = [string.Empty];

    public int AmountOfBait { get; set; } = 1;

    public List<string?> TacklesToSpawn { get; set; } = [string.Empty, string.Empty];

    public bool OverrideAttachmentLimit { get; set; }

    public bool ResetAttachmentsLimitWhenNotEquipped { get; set; } = true;

    public bool InfiniteBait { get; set; }

    public bool InfiniteTackle { get; set; }

    public bool AutoLootTreasure { get; set; }

    public float DifficultyMultiplier { get; set; } = 1.0f;

    public bool AdjustXpGainDifficulty { get; set; } = true;

    public bool DoAddEnchantments { get; set; }

    public bool AddAllEnchantments { get; set; }

    public bool AddAutoHookEnchantment { get; set; }

    public bool AddEfficientToolEnchantment { get; set; }

    public bool AddMasterEnchantment { get; set; }

    public bool AddPreservingEnchantment { get; set; }

    public bool ResetEnchantmentsWhenNotEquipped { get; set; } = true;
}

using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using System;

namespace NeverToxic.StardewMods.YetAnotherFishingMod.Framework
{
    internal class GenericModConfigMenu(IModRegistry modRegistry, IManifest manifest, IMonitor monitor, Func<ModConfig> config, Action reset, Action save)
    {
        public void Register()
        {
            var configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;

            configMenu.Register(mod: manifest, reset: reset, save: save);

            configMenu.AddPageLink(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.General",
                text: () => "General"
            );
            configMenu.AddPageLink(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.Attachments",
                text: () => "Attachments"
            );
            configMenu.AddPageLink(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.Enchantments",
                text: () => "Enchantments"
            );

            configMenu.AddPage(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.General",
                pageTitle: () => "General"
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "General"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Instantly Catch Fish",
                tooltip: () => "Skips the fishing minigame.",
                getValue: () => config().InstantCatchFish,
                setValue: value => config().InstantCatchFish = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Instantly Catch Treasure",
                tooltip: () => "If there are any treasures in the fishing minigame, instantly catch them even if you missed them.",
                getValue: () => config().InstantCatchTreasure,
                setValue: value => config().InstantCatchTreasure = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Always Maximum Casting Power",
                tooltip: () => "Always casts the fishing rod with maximum power, no matter how long you held the button.",
                getValue: () => config().AlwaysMaxCastingPower,
                setValue: value => config().AlwaysMaxCastingPower = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Always Perfect",
                tooltip: () => "Always get a perfect after catching a fish, even if you did not.",
                getValue: () => config().AlwaysPerfect,
                setValue: value => config().AlwaysPerfect = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Always Catch Treasure",
                tooltip: () => "Always catches a treasure chest, even if there were none.",
                getValue: () => config().AlwaysCatchTreasure,
                setValue: value => config().AlwaysCatchTreasure = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Always Catch Double",
                tooltip: () => "Always catches double fish, even if you did not have the correct bait.",
                getValue: () => config().AlwaysCatchDouble,
                setValue: value => config().AlwaysCatchDouble = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Instant Bite",
                tooltip: () => "Fish instantly bite after casting your fishing rod.",
                getValue: () => config().InstantBite,
                setValue: value => config().InstantBite = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Automatically Hook",
                tooltip: () => "Automatically starts reeling in your fish after one bites.",
                getValue: () => config().AutoHook,
                setValue: value => config().AutoHook = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Automatically Loot Treasure",
                tooltip: () => "Automatically loot and close the treasure menu popup after catching treasure.",
                getValue: () => config().AutoLootTreasure,
                setValue: value => config().AutoLootTreasure = value
            );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Difficulty Multiplier",
                tooltip: () => "Value with which the standard fishing difficulty is multiplied.",
                getValue: () => (int)config().DifficultyMultiplier,
                setValue: value => config().DifficultyMultiplier = value,
                min: (float)0.1,
                max: (float)5.0
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "Quality Options"
            );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Fish Quality",
                tooltip: () => "Always catch fish with the set quality.",
                getValue: () => (int)config().FishQuality,
                setValue: value => config().FishQuality = (Quality)value
            );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Minimum Fish Quality",
                tooltip: () => "Always gets fish of the set quality or better.",
                getValue: () => (int)config().MinimumFishQuality,
                setValue: value => config().MinimumFishQuality = (Quality)value
            );

            configMenu.AddPage(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.Attachments",
                pageTitle: () => "Attachments"
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "General"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Override Attachment Limit",
                tooltip: () => "Overrides the fishing rod's attachment limit when spawning bait and tackle.",
                getValue: () => config().OverrideAttachmentLimit,
                setValue: value => config().OverrideAttachmentLimit = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Reset Attachments",
                tooltip: () => "Resets spawned attachments and slots when fishing rod is unequipped.",
                getValue: () => config().ResetAttachmentsWhenNotEquipped,
                setValue: value => config().ResetAttachmentsWhenNotEquipped = value
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "Bait"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Spawn Bait When Equipped",
                tooltip: () => "Spawns the below selected bait item whenever you equip your fishing rod.",
                getValue: () => config().SpawnBaitWhenEquipped,
                setValue: value => config().SpawnBaitWhenEquipped = value
            );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Bait to Spawn",
                tooltip: () => "Which bait to automatically spawn when fishing rod is equipped.",
                getValue: () => (int)config().SpawnWhichBait,
                setValue: value => config().SpawnWhichBait = (Bait)value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Infinite Bait",
                tooltip: () => "Never run out of bait.",
                getValue: () => config().InfiniteBait,
                setValue: value => config().InfiniteBait = value
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "Tackles"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Spawn Tackle When Equipped",
                tooltip: () => "Spawns the below selected tackle item whenever you equip your fishing rod.",
                getValue: () => config().SpawnTackleWhenEquipped,
                setValue: value => config().SpawnTackleWhenEquipped = value
            );
            configMenu.AddNumberOption(
                mod: manifest,
                name: () => "Tackle to Spawn",
                tooltip: () => "Which tackle to automatically spawn when fishing rod is equipped.",
                getValue: () => (int)config().SpawnWhichTackle,
                setValue: value => config().SpawnWhichTackle = (Tackle)value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Infinite Tackle",
                tooltip: () => "Tackle never wears out.",
                getValue: () => config().InfiniteTackle,
                setValue: value => config().InfiniteTackle = value
            );

            configMenu.AddPage(
                mod: manifest,
                pageId: "NeverToxic.YetAnotherFishingMod.Enchantments",
                pageTitle: () => "Enchantments"
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "General"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Enable Adding Enchantments",
                tooltip: () => "Adds enchantments as set below when checked.",
                getValue: () => config().DoAddEnchantments,
                setValue: value => config().DoAddEnchantments = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Reset Enchantments",
                tooltip: () => "Resets enchantments to what they were before after unequipping the fishing rod.",
                getValue: () => config().ResetEnchantmentsWhenNotEquipped,
                setValue: value => config().ResetEnchantmentsWhenNotEquipped = value
            );
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => "Enchantments"
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Add All Enchantments",
                tooltip: () => "Adds all available enchantments when you equip a fishing rod.",
                getValue: () => config().AddAllEnchantments,
                setValue: value => config().AddAllEnchantments = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Add Autohook Enchantment",
                tooltip: () => "Adds the autohook enchantment when equipping a fishing rod.",
                getValue: () => config().AddAutoHookEnchantment,
                setValue: value => config().AddAutoHookEnchantment = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Add Efficient Tool Enchantment",
                tooltip: () => "Adds the efficient tool enchantment when equipping a fishing rod.",
                getValue: () => config().AddEfficientToolEnchantment,
                setValue: value => config().AddEfficientToolEnchantment = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Add Master Enchantment",
                tooltip: () => "Adds the master enchantment when equipping a fishing rod.",
                getValue: () => config().AddMasterEnchantment,
                setValue: value => config().AddMasterEnchantment = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => "Add Preserving Enchantment",
                tooltip: () => "Adds the preserving enchantment when equipping a fishing rod.",
                getValue: () => config().AddPreservingEnchantment,
                setValue: value => config().AddPreservingEnchantment = value
            );
        }
    }
}

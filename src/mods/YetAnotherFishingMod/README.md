<!-- omit in toc -->
# Yet Another Fishing Mod

Having difficulty fishing? Or finding it too easy? This is yet another mod that allows you to adjust various
fishing-related options.

<div align="center">

![A person cathing a fish in Stardew Valley.](./.nexusmods/header_image.jpg)

[NexusMods](https://www.nexusmods.com/stardewvalley/mods/20391) •
[CurseForge](https://www.curseforge.com/stardewvalley/mods/yet-another-fishing-mod) •
[Source](./src/mods/YetAnotherFishingMod) •
[Documentation](https://dunc4nnt.github.io/StardewMods/YetAnotherFishingMod)

</div>

<!-- omit in toc -->
## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
  - [Minigame Skip](#minigame-skip)
  - [Difficulty](#difficulty)
  - [Fishing Loot](#fishing-loot)
  - [Faster Please](#faster-please)
  - [Attachments](#attachments)
  - [Enchantments](#enchantments)
- [How to Use](#how-to-use)
- [Compatibility](#compatibility)

## Prerequisites

Mandatory and optional requirements for use of this mod.

- [SMAPI 4.0](https://www.nexusmods.com/stardewvalley/mods/2400) (required)
- [GMCM 1.12][GMCM-nexus] (optional, but highly recommended)

## Installation

1. Download and install the required prerequisites.
2. Download the latest version of this mod
   through [NexusMods](https://www.nexusmods.com/stardewvalley/mods/20391?tab=files)
   or [GitHub releases](https://github.com/Dunc4nNT/StardewMods/releases).
3. Unzip the downloaded file into the `Stardew Valley/Mods` directory.
4. Launch the game using SMAPI.

## Configuration

The mod's configurable options can be found below. These can either be adjusted
through [Generic Mod Configuration Menu (GMCM)][GMCM-nexus] or the `config.json` file. Editing the options using GMCM
is recommended. GMCM will also provide descriptive tooltips if the option itself is not clear.

### Minigame Skip

| Feature               | Description                                                                                                                                                    |
| --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Automatically Cast    | Keybind to toggle automatically casting your fishing rod. Will try casting every second if enabled.                                                            |
| Skip Minigame         | Skips the fishing minigame. Amount of catches required before skip can be changed below.                                                                       |
| Catches Required      | Amount of catches required before it skips the minigame. The "Skip Minigame" option must be enabled.                                                           |
| Chance of Perfect     | The chance that the minigame skip causes a perfect catch. "Always Perfect" trumps this option. The "Skip Minigame" option must be enabled.                     |
| Treasure Catch Chance | The chance that the skip minigame catches treasure, if there was one. "Instant Catch Treasure" trumps this option. The "Skip Minigame" option must be enabled. |

### Difficulty

| Feature                   | Description                                                                                                                            |
| ------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| Difficulty Multiplier     | Value with which the standard fishing difficulty is multiplied.                                                                        |
| Adjust XP Gain            | Adjust the XP gained based on the fish difficulty set above.                                                                           |
| Fish Catch Multiplier     | A multiplier for the increase given when the fish is inside the green bar. The higher the value, the quicker the fish is caught.       |
| Fish Escape Multiplier    | A multiplier for the penalty given when the fish is outside of the green bar. The higher the value, the quicker the fish escapes.      |
| Bar Size Multiplier       | A multiplier for the size of the bar in the fishing minigame. The higher the value, the larger the bar.                                |
| Always Max Casting Power  | Always casts the fishing rod with maximum power, no matter how long you held the button.                                               |
| Automatically Hook        | Automatically starts reeling in your fish after one bites.                                                                             |
| Always Perfect            | Always get a perfect after catching a fish, even if you did not.                                                                       |
| Instantly Catch Treasure  | If there are any treasures in the fishing minigame, instantly catch them even if you missed them.                                      |
| Treasure Catch Multiplier | A multiplier for the increase given when a treasure is inside the green bar. The higher the value, the quicker the treasure is caught. |

### Fishing Loot

| Feature                     | Description                                                                                 |
| --------------------------- | ------------------------------------------------------------------------------------------- |
| Fish                        | Allows the catching of fish when enabled.                                                   |
| Rubbish                     | Allows the catching of rubbish when enabled.                                                |
| Other                       | Allows catching of other items when enabled. This includes items such as seaweed and algae. |
| Amount of Fish              | Catch the set amount of fish. Using bait may override this amount. Whichever is highest.    |
| Minimum Fish Quality        | Always gets fish of the set quality or better. Trumps the "Fish Quality" option.            |
| Set Fish Quality            | Always catch fish with the set quality. Trumped by the "Minimum Fish Quality" option.       |
| Treasure Probability        | Adjust how often treasure chests appear. (vanilla, always, never)                           |
| Golden Treasure Probability | Adjust how often a treasure chest is a golden treasure chest. (vanilla, always, never)      |

Disabling all of Fish, Rubbish and Other will have the same effect as all of them enabled as a fallback.

### Faster Please

| Feature                     | Description                                                                                         |
| --------------------------- | --------------------------------------------------------------------------------------------------- |
| Instant Bite                | Fish instantly bite after casting your fishing rod.                                                 |
| Automatically Loot Fish     | Automatically loot and close the fish popup after catching a fish.                                  |
| Automatically Loot Treasure | Automatically loot and close the treasure menu popup after catching treasure.                       |
| Faster Animations           | Various animations are quicker. These include the casting, reeling and treasure opening animations. |

### Attachments

| Feature                    | Description                                                                                                                                                        |
| -------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Override Attachment Limit  | Overrides the fishing rod's attachment limit when spawning bait and tackle.                                                                                        |
| Reset Attachments Limit    | Resets attachment limit back to the default when unequipping the fishing rod. **WARNING: If you have any attachments in an overriden slot, they will be deleted.** |
| Infinite Bait              | Never run out of bait.                                                                                                                                             |
| Spawn Bait When Equipped   | Spawns the below selected bait item whenever you equip your fishing rod.                                                                                           |
| Amount of Bait             | How much bait to spawn.                                                                                                                                            |
| Bait to Spawn              | Which bait to automatically spawn when fishing rod is equipped.                                                                                                    |
| Infinite Tackles           | Your tackle never wears out.                                                                                                                                       |
| Spawn Tackle When Equipped | Spawns the below selected tackle item whenever you equip your fishing rod.                                                                                         |
| Tackle to Spawn            | Which tackle to automatically spawn when fishing rod is equipped.                                                                                                  |

### Enchantments

| Feature                    | Description                                                                     |
| -------------------------- | ------------------------------------------------------------------------------- |
| Enable Adding Enchantments | Adds enchantments as set below when checked.                                    |
| Reset Enchantments         | Resets enchantments to what they were before after unequipping the fishing rod. |
| Add All Enchantments       | Adds all available enchantments when you equip a fishing rod.                   |
| Add Auto-Hook Enchantment  | Adds the auto-hook enchantment when equipping a fishing rod.                    |
| Add Efficient Enchantment  | Adds the efficient tool enchantment when equipping a fishing rod.               |
| Add Master Enchantment     | Adds the master enchantment when equipping a fishing rod.                       |
| Add Preserving Enchantment | Adds the preserving enchantment when equipping a fishing rod.                   |

## How to Use

Once you've configured the mod the way you want, simply equip a fishing rod and start fishing!

## Compatibility

This mod is made for **Stardew Valley 1.6** using **SMAPI 4.0**. This mod will **not** work on versions prior to those
mentioned.

Both **singleplayer** and **multiplayer** should work. If you are having issues with multiplayer, make sure you've
downloaded at least version 1.0.1 of the mod as this included a multiplayer hotfix.

I have yet to test **split screen**, but did try to make it compatible.

This mod does not change fish spawns, so it *should* work with mods adding **custom fish**.

*Should* be compatible with mods that add **custom bait and or hooks**, I've yet to test this, but as long as the
category for the custom item was set correctly, this mod should detect them.

This mod will **not** work with **custom enchantments** at the moment. I'm looking to add support for this in the
future.

Likely to conflict with mods that try to do the same thing. Due to every option in this mod being configurable it
should not be likely to cause issues, as you can simply turn the conflicting option off.

[GMCM-nexus]: https://www.nexusmods.com/stardewvalley/mods/5098

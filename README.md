<!-- omit in toc -->
# StardewMods

This is a mono repository containing all of my Stardew Valley mods.

<!-- omit in toc -->
## Table of Contents

- [Mods](#mods)
  - [Downloading](#downloading)
  - [Documentation](#documentation)
- [Translations](#translations)
  - [Stance on Translations](#stance-on-translations)
- [Templates (for Mod Authors)](#templates-for-mod-authors)
  - [Using Templates](#using-templates)
- [Contributing (for Mod Authors and Translators)](#contributing-for-mod-authors-and-translators)
- [Support](#support)
- [Contact](#contact)
- [Links](#links)
- [Licensing](#licensing)

## Mods

Downloads, documentation, and source code for individual mods can be found on their respective page below:

- [Dutch Localisation](./src/DutchLocalisation)
- [Little Helpers](./src/LittleHelpers)
- [No Stamina Wasted](./src/NoStaminaWasted)
- [Self Serve](./src/SelfServe)
- [Television Framework](./src/TelevisionFramework)
- [Yet Another Fishing Mod](./src/YetAnotherFishingMod)
- [Yet Another Time Mod](./src/YetAnotherTimeMod)

### Downloading

All published mod releases can be found on [NexusMods][nexusmods]. A mirror of each release can be found on
[GitHub][gh-releases]. Some of my mods can also be found on [CurseFore][curseforge].\
GitHub and NexusMods releases should always be in sync, meaning you can download the latest version of whatever mod
you want from your preferred site.

I may occasionally publish a pre-release on GitHub, unavailable on other platforms.
These releases likely include some description saying "Testing bug fix"/"Debugging ...", and should not be downloaded
unless specifically instructed.

In case you are completely new to modding in Stardew Valley, please follow the instructions on the
[Stardew Valley Wiki](https://stardewvalleywiki.com/Modding:Player_Guide/Getting_Started) regarding modding.
This guides you through installing your first mod.

### Documentation

Generally speaking, documentation can be found on the above listed pages (GitHub, NexusMods, CurseForge),
as well as being including in the download as a `README.md` file for offline use.
This should often suffice, but in some cases, more extensive documentation (including examples for framework mods)
is not included as it would make the mod page unnavigable.
Complete documentation can be found on [GitHub pages][website].

## Translations

English is the only language fully supported by me, and guaranteed to be up to date.
A fair few mods, made by other mod authors, do have built-in (partial) support for multiple languages.
This is not the case for any of my mods, meaning that if you want to use the mod in another language, someone else
needs to have published them; or you have to create them yourself.

Thanks to some of the players in the community, a few of my mods have been translated.
You can find them under the "Translations" tab on NexusMods.
Do note that these may not always be up to date, or accurate, as I'm not involved in making them.

If you find there are no translations for your language for a mod you're using, or the translations you've found are
outdated, you're free to [contribute](./.github/CONTRIBUTING.md).

### Stance on Translations

As I've mentioned, if you've downloaded mods in the past you're likely used to translations being included, at least
if they exist at all. This is not the case for my mods for a few reasons:

- I do not speak every language myself, meaning I cannot create translations for every language.
- I cannot guarantee quality of community-made translations.
- I cannot guarantee translations continue every release.
- Translations should not be blockers for releasing updates. If I were to include translations in the release, I'd have
  to update these before the release.
- I do not want to clutter the repository, as well as all the mod pages with a ton of releases, that are essentially
  just updated translations. This is both a lot of work and annoyance for myself, but also every player.

I personally think deviating from the more "standard" approach of releasing mods with translations included,
and updating the mod every now and again with updated translations, is ultimately beneficial for you, the player,
translators, and me. You, the player, get to download only the translation you want, and get updates for only that
translation. The translator (perhaps also you), has a lot of freedom, and doesn't have to rely on me, by being able to
publish their own translations. I don't have to wait for translators, find translators, annoy players with update
notifications, and go through the process of creating a new release a billion times more than necessary.

## Templates (for Mod Authors)

This repository also contains various mod templates, which are used to speed up my workflow a bit by
removing the repetitive task of setting up the `.csproj`, and various standard files and folders.

- [SMAPI Template](./templates/SmapiTemplate)

These templates are personalised to fit with this repository's structure, and may not fit your structure.
Most notably, I make use of `Directory.Build.props` and `Directory.Packages.props` instead of defining the shared
properties and package versions in the `*.csproj` files.

If you only use `*.csproj` files, you'll have to put the contents of the following files inside your `*.csproj` file:

- [mod Directory.Build.props](./src/Directory.Build.props)
- [global Directory.Build.props](./Directory.Build.props)
- [Directory.Packages.props](./Directory.Packages.props)

### Using Templates

## Contributing (for Mod Authors and Translators)

If you'd like to help in the development of this mod, or want to translate a mod to your language, please check
[how to contribute](./.github/CONTRIBUTING.md).

## Support

See [SUPPORT.md](./.github/SUPPORT.md) for information on how to get help.

## Contact

See [SUPPORT.md](./.github/SUPPORT.md#contact) for my contact details.

## Links

- [NexusMods][nexusmods]
- [CurseForge][curseforge]

## Licensing

Copyright © 2024-2025 Dunc4nNT

This repository is licensed under the Mozilla Public License 2.0 (MPL 2.0). See [LICENSE](./LICENSE) for more
information.

[nexusmods]: https://next.nexusmods.com/profile/NeverToxic/mods

[curseforge]: https://www.curseforge.com/members/nevertoxic/projects

[gh-releases]: https://github.com/Dunc4nNT/StardewMods/releases

[website]: https://dunc4nnt.github.io/StardewMods

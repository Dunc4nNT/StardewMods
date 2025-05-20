<!-- omit in toc -->
# StardewMods

This is a mono repository containing everything related to Stardew Valley modding that I've created.

<div align="center">

![Stardew Valley](./src/mods/YetAnotherFishingMod/.nexusmods/header_image.jpg)

[NexusMods][nexusmods] • [CurseForge][curseforge] • [Documentation][website]

</div>

<!-- omit in toc -->
## Table of Contents

- [Downloads](#downloads)
- [Documentation](#documentation)
- [Mods](#mods)
  - [Yet Another Fishing Mod](#yet-another-fishing-mod)
- [Templates (for Mod Authors)](#templates-for-mod-authors)
  - [Using Templates](#using-templates)
- [Compiling (for Developers)](#compiling-for-developers)
- [Translations](#translations)
  - [Stance on Translations](#stance-on-translations)
- [Contributing (for Mod Authors and Translators)](#contributing-for-mod-authors-and-translators)
- [Support](#support)
- [Contact](#contact)
- [Acknowledgements](#acknowledgements)
- [Licensing](#licensing)

## Downloads

All published mod releases can be found on [NexusMods][nexusmods]. A mirror of each release can be found on
[GitHub][gh-releases]. Some of my mods can also be found on [CurseFore][curseforge]. GitHub and NexusMods releases
should always be in sync, meaning you can download the latest version of whatever mod you want from your preferred
site.

I may occasionally publish a pre-release on GitHub, unavailable on other platforms. These releases likely include
some description saying "Test build...", and should not be downloaded unless specifically instructed.

In case you are completely new to modding in Stardew Valley, please follow the instructions on the
[Stardew Valley Wiki](https://stardewvalleywiki.com/Modding:Player_Guide/Getting_Started) regarding modding.
This guides you through installing your first mod using SMAPI.

## Documentation

Complete documentation can be found on [GitHub pages][website].

Generally speaking, documentation can be found on the individual mod pages (GitHub, NexusMods, CurseForge), as well as
being including in the download as a `README.md` file for offline use. This should often suffice, but in some cases,
more extensive documentation (including examples for framework mods) is not included, as it would make the mod page
unnavigable.

## Mods

Downloads, documentation, and source code for individual mods can be found on their respective page below.

### Yet Another Fishing Mod

Having difficulty fishing and want to skip the minigame?

<div align="center">

![A person cathing a fish in Stardew Valley.](./src/mods/YetAnotherFishingMod/.nexusmods/header_image.jpg)

[NexusMods](https://www.nexusmods.com/stardewvalley/mods/20391) •
[CurseForge](https://www.curseforge.com/stardewvalley/mods/yet-another-fishing-mod) •
[Source](./src/mods/YetAnotherFishingMod) •
[Documentation](https://dunc4nnt.github.io/StardewMods/YetAnotherFishingMod)

</div>

## Templates (for Mod Authors)

This repository also contains various mod templates, which are used to speed up my workflow a bit by removing the
repetitive task of setting up the `.csproj`, and various standard files and folders.

- [SMAPI Mod Template](./templates/SmapiMod)

These templates are personalised to fit this repository's structure, and may not fit your structure. Most notably,
I make use of `Directory.Build.props` and `Directory.Packages.props` instead of defining the shared  properties and
package versions in the `*.csproj` files.

If you only use `*.csproj` files, you'll have to put the contents of the following files inside your `*.csproj` file:

- [global Directory.Build.props](./Directory.Build.props)
- [mod Directory.Build.props](./src/mods/Directory.Build.props)
- [Directory.Packages.props](./Directory.Packages.props)

### Using Templates

You can add my C# mods as
[custom dotnet templates](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates), which allows you to
create them through the CLI with `dotnet new`. It's recommended to have a look at the template arguments before
doing so, as most arguments are required. Personally, I add the template to [Rider](https://www.jetbrains.com/rider/)
by clicking `new project -> Manage Templates -> Install Template...`, but the same thing is also possible using
[Visual Studio](https://visualstudio.microsoft.com/vs/).

The SMAPI template can do the following things:

- Add a basic ModEntry file.
- Set various manifest fields in the `.csproj` file, such as `<Name>`, `<Description>`, `<UniqueId>`, `<UpdateKeys>`.
- Add various base config files and add GMCM as an optional dependency.
- Enable I18n, which adds `ModTranslationClassBuilder`, and creates a default i18n folder.
- Enable harmony and add basic harmony patches files.
- Importing [common i18n files](./src/shared/i18n)
- Importing [common code](./src/shared/Common)
- Create a `README.md` with the mod name and description.

## Compiling (for Developers)

Want to compile my mods yourself?

<!--TODO: add instructions to clone the repo, set up everything and compile my mods-->

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
- Translations should not be blockers for releasing updates. If I were to include translations in the release, I'd
have to update these before the release.
- I do not want to clutter the repository, as well as all the mod pages with a ton of releases, that are essentially
just updated translations. This is both a lot of work and annoyance for myself, but also every player.

I personally think deviating from the more "standard" approach of releasing mods with translations included,
and updating the mod every now and again with updated translations, is ultimately beneficial for you, the player,
translators, and me. You, the player, get to download only the translation you want, and get updates for only that
translation. The translator (perhaps also you), has a lot of freedom, and doesn't have to rely on me, by being able to
publish their own translations. I don't have to wait for translators, find translators, annoy players with update
notifications, and go through the process of creating a new release a billion times more than necessary.

## Contributing (for Mod Authors and Translators)

If you'd like to help in the development of this mod, or want to translate a mod to your language, please check
[how to contribute](./.github/CONTRIBUTING.md).

## Support

See [SUPPORT.md](./.github/SUPPORT.md) for information on how to get help.

## Contact

See [SUPPORT.md](./.github/SUPPORT.md#contact) for my contact details.

## Acknowledgements

<!--TODO: acknowledgements-->

## Licensing

Copyright © 2024-2025 Dunc4nNT

This repository is licensed under the Mozilla Public License 2.0 (MPL 2.0). See [LICENSE](./LICENSE) for more
information.

[nexusmods]: https://next.nexusmods.com/profile/NeverToxic/mods
[curseforge]: https://www.curseforge.com/members/nevertoxic/projects
[gh-releases]: https://github.com/Dunc4nNT/StardewMods/releases
[website]: https://dunc4nnt.github.io/StardewMods

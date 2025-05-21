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
- [Templates](#templates)
  - [Using Templates](#using-templates)
- [Tools](#tools)
- [Compiling](#compiling)
- [Translations](#translations)
  - [Stance on Translations](#stance-on-translations)
    - [Conclusion](#conclusion)
- [Contributing](#contributing)
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

![A person catching a fish in Stardew Valley.](./src/mods/YetAnotherFishingMod/.nexusmods/header_image.jpg)

[NexusMods](https://www.nexusmods.com/stardewvalley/mods/20391) •
[CurseForge](https://www.curseforge.com/stardewvalley/mods/yet-another-fishing-mod) •
[Source](./src/mods/YetAnotherFishingMod) •
[Documentation](https://dunc4nnt.github.io/StardewMods/YetAnotherFishingMod)

</div>

## Templates

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

## Tools

<!--TODO: describe tooling projects-->

## Compiling

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
outdated, you're free to [create and publish your own](./.github/CONTRIBUTING.md#contributing-to-translations).

### Stance on Translations

As I've mentioned, if you've downloaded mods in the past, you're likely used to translations being included.
This is not the case for my mods for a few reasons.

<!-- omit in toc -->
#### I do not speak every language.

I cannot create translations for every language myself. This is obvious, but it does mean I'm left with two options:
hire people to translate, or rely on the community to create translations. My mods, surprisingly, do not have the
budget to hire professional translators, or even pay people enough for their time spent translating. This leads me
to being left with only one option, and that is to rely on community-made translations.

<!-- omit in toc -->
#### I cannot guarantee high quality of community-made translations.

I dislike releasing mods in a state where I'm sufficiently dissatisfied with the quality of it. With community-made
translations in languages I don't speak, I cannot judge the quality, making it impossible to ensure quality on a
release.

<!-- omit in toc -->
#### I cannot guarantee translations continue every release.

With community-made translations, I'm trusting another person, or persons, taking the responsibility of providing
translations for their language. This includes the responsibility of updating translations whenever I update a mod.
Obviously, I cannot force another person to update their translations at my will, nor would I ever expect someone to
update their translations whenever I make changes.

Making mods for me is a hobby, I don't have a regular "update time" at which translators can check for updates. Just
like this is a hobby for me, (more or less) all the translation work done within the Stardew modding community is
also done by people offering their free time to share translations for a mod and make the mod more accessible for
those who prefer playing the game in that language. These aren't people I hire to make translations, there may not
even be a way for me to contact someone making translations.

Anyway, all this means is that those providing translations may stop at any time, or may not be available at whatever
time I want to release an update. This leads to me being forced to release a mod missing translations, which I'd say
is an incomplete release, and I prefer not publishing incomplete releases.

<!-- omit in toc -->
#### Translations should not be blockers for releasing updates.

Continuing from the previous point, say, instead of releasing an incomplete version, I do actually wait for someone
to provide translations, so I can create a release that is complete. This quickly becomes infeasible to do, at some
point translators will stop, and it becomes difficult to ensure all languages get updates. It just ends up in mod
never being updated, as they'd never be in a complete state. I don't think missing translations, or incorrect
translations should stop an update from being released.

<!-- omit in toc -->
#### I do not want to spam release.

If I don't wait for translations before updating, I'll have to periodically create new releases that update
translations. This isn't necessarily a problem for something that gets updated regularly anyway, but this isn't true
for most mods, as they rarely get updated. Even if mod updates were more frequent, it'd still be impossible to keep
up with changes to translations, and with mod updates, more translations get added, leading to even more translation
updates.

It's possible to create releases that only update translations, but this isn't something I want to do, as it
clutters the repository, and the sites I distribute my mods on with releases. Due to SMAPI limitations, I have no way
of indicating a release only has translational changes made, let alone also specify which languages got updated.
This means that all players will get update notifications on SMAPI, even if the updates are completely irrelevant for
nearly everyone. I do not want to frustrate players with constant updates, especially ones they don't benefit from - I
already feel bad enough every time I have to release a minor bug fix. Aside from player frustration, it would also
become something that takes up a lot of my time. Publishing new releases is, at the moment at least, not something
completely automated. I still have to manually compile the mod, create a release on GitHub and upload the zip, then
go to NexusMods and go through their annoying process.

#### Conclusion

I personally think deviating from the more "standard" approach of releasing mods with translations included,
and updating the mod every now and again with updated translations, is ultimately beneficial for you, the player,
translators, and me. You, the player, get to download only the translation you want, and get updates for only that
translation. The translator (perhaps also you), has a lot of freedom, and doesn't have to rely on me, by being able to
publish their own translations. I, the developer, don't have to wait for translators, find translators, annoy players
with update notifications, and go through the process of creating a new release a billion times more than necessary.

## Contributing

If you'd like to help in the development of a mod, or tool, or translate a mod, please read how you can
[contribute](./.github/CONTRIBUTING.md).

## Support

See [SUPPORT.md](./.github/SUPPORT.md) for information on how to get help.

## Contact

See [SUPPORT.md](./.github/SUPPORT.md#contact) for my contact information.

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

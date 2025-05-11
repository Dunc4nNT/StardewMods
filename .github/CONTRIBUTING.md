<!-- omit in toc -->
# Contributing

If you'd like to help fix a bug, or provide translations for your language, you're welcome to help.
Please read the contents of this document to figure out how you can help.

<!-- omit in toc -->
## Table of Contents

- [Before Contributing](#before-contributing)
- [Contributing to Mods (for Mod Authors)](#contributing-to-mods-for-mod-authors)
- [Contributing to Translations (for Translators)](#contributing-to-translations-for-translators)
  - [Publishing Translations on NexusMods](#publishing-translations-on-nexusmods)
- [Help](#help)

## Before Contributing

Before you start writing code or translating, please make sure it hasn't already been done,
isn't already being worked on, or hasn't been reject rejected.
You can check [GitHub issues][gh-issues],
and [GitHub pull requests][gh-pulls],
and the NexusMods or CurseForge mod page for the mod you're looking to contribute to.

If you're uncertain, please open an [issue][gh-issues] first.

## Contributing to Mods (for Mod Authors)

If you've already forked the repository and changed some things, you're free to open a [PR][gh-pulls].
Obviously, there is no guarantee of your PR getting accepted. I will comment on your PR what I think,
and may request changes or decline it altogether.

In a lot of cases it's preferred you open an [issue][gh-issues] first, so we can discuss, before change anything.

**The following must be true for a PR to be considered:**
- Your PR description must be detailed, I should be able to know why and what you're changing
without getting into the code.
- You've made sure the mod runs locally and all features work as intended.
- You've extensively tested the mod after your changes to ensure no new bugs are introduced.
- You've compiled a release version, and also tested this.
- No new warnings or errors are introduced.

## Contributing to Translations (for Translators)

To start, please download the mod you're looking to translate, if you haven't already.
Inside the folder you downloaded, you'll find an `i18n` folder, which contains a `default.json` file.
This is the reference file you use to translate my mod, it contains all the English phrases.

The Stardew Valley Wiki has a good [guide on translations](https://stardewvalleywiki.com/Modding:Translations).
You can follow this up to "How to provide mod translations", I diverge from this step.
Instead of providing translations directly in the original release of the mod, you can publish your own translations.
This can be done in many ways by simply sharing the file you've just created wherever you want, but the preferred way
is to publish this file as a mod translation on NexusMods.

### Publishing Translations on NexusMods

On [NexusMods](https://www.nexusmods.com/games/stardewvalley) you can click the "Upload" button in the top right, and
select "Upload mod". You'll be directed to a form, in here select "Translation" instead of the default "Mod",
and select the original mod you're translating in the field below.\
Please upload the translated file where it asks, you can either just upload the file, or create an empty
`<ModName>/i18n` folder, copy your file to it, and upload a ZIP.

<ins>**Ensure that your upload contains *only* your translation file, not the entire mod, nor the English file.**</ins>

## Help

If you've got a question, or are running into issues, feel free to [contact](./SUPPORT.md) me.

[gh-issues]: https://github.com/Dunc4nNT/StardewMods/issues
[gh-pulls]: https://github.com/Dunc4nNT/StardewMods/pulls

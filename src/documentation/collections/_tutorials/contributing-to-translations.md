# Contributing to Translations

This tutorial guides you through creating and publishing your translations on NexusMods. Although this is meant for [my mods](https://www.github.com/Dunc4nNT/StardewValleyModding), the steps are the same for most mods that support i18n, but depending on the mod author, they may prefer you send them translations instead of publish on NexusMods yourself. Before starting, it is expected you know [basic JSON]({% link _tutorials/json-syntax-with-smapi.md %}).

To get started, please download the mod you're looking to translate, if you haven't already, and remember to unzip it.
If you open this folder inside your file explorer, you'll notice it contains various files and folders. We only care
about the folder named `i18n`, i18n being short for internationalisation. This is the folder used to provide the
English text shown, as well as any translations. If a mod does not have an `i18n` folder, it means there is nothing
to translate, and the mod likely doesn't show any text on screen.

## Navigating the i18n Folder

If you open the `i18n` folder, there's two possible scenarios: either it contains a `default.json` file, or a folder
named `default`. Note that `.json` is the file extension, and may not be visible by default depending on your
operating system, it should then still have a file icon instead of a folder icon.

In case you found a single file, it means this is the file that contains all the English phrases, and is the file
you use to translate. If you found a folder instead, it means there may be multiple files to translate. Open the
`default` folder, and you likely see it contains various files with a `.json` extension. All these files are used
contain English phrases to translate.

If you're downloading a mod that already has translations included, you might find multiple files in your `i18n` folder
alongside the `default.json`, or folders named differently.

## Navigating Translation Files

Open one of the files containing English phrases (which I'll refer to as "reference file(s)" from now on) in your
favourite text editor, which can just be notepad if you don't have anything else. You should see a file formatted
like this:

```json
{
  "message.config-reloaded": "Configuration file reloaded.",
  "message.test-message": "This is a test message, this message has been shown {{count}} times."
}
```

This, as found in the [JSON tutorial]({% link _tutorials/json-syntax-with-smapi.md %}), should look fimiliar. It is important that, when translating, you don't change the [tokens]({% link _tutorials/json-syntax-with-smapi.md#tokens %}).

## Language Codes

Before we can get to actually writing translations, there is one more important concept to know. Remember how our
reference file is either named `default`, or the reference files are inside a folder named `default`. This is with a
reason, SMAPI sees this as the, well, default. If no translations are provided at all, or a translation is missing
various phrases, SMAPI uses the default. Most of the time default will be English, but this doesn't always have to be
the case.

When providing translations, we don't want SMAPI to pick the default, meaning we have to specify in which language your
translations are. The following table shows what you have to name your file, or folder, to match your language:

| Language   | Folder name | File Name  |
| ---------- | ----------- | ---------- |
| Chinese    | zh          | zh.json    |
| Dutch      | nl-NL       | nl-NL.json |
| English    | en          | en.json    |
| French     | fr          | fr.json    |
| German     | de          | de.json    |
| Hungarian  | hu          | hu.json    |
| Italian    | it          | it.json    |
| Japanese   | ja          | ja.json    |
| Korean     | ko          | ko.json    |
| Portuguese | pt          | pt.json    |
| Russian    | ru          | ru.json    |
| Spanish    | es          | es.json    |
| Turkish    | tr          | tr.json    |

Dutch is an example of a custom language, other custom languages can also be used for translations. You should use the
`LanguageCode` field of the language in that case. If you don't know what this is set to, you should ask the mod author
of the custom language.

## Creating Translations

First, we have to create a copy of the reference file(s), this is done by making a copy of either the `default.json`
file, or the `default` folder. After this is done, you can rename the copied file or folder to match the language code
matching your language in the table above. We'll call the file(s) you just created, translation file(s), and, if
applicable, the folder you just created, the translation folder.

You can now open the translation file(s) in your text editor, or if applicable and if your text editor allows it, open
the translation folder in your text editor. You can now get to writing translations, just keep all the above-mentioned
concepts in mind. Be sure to save the file whenever you make changes.

## Live Viewing Translations

Using SMAPI built-in console commands, you can reload your translations mid-game. Just type `reload_i18n` into the
SMAPI console. This makes it so you don't have to constantly restart the game whenever you make changes.

## Publishing Translations

Instead of providing translations directly in the original release of the mod, you can publish your own translations.
This can be done in many ways by simply sharing the file you've just created wherever you want, but the preferred way
is to publish this file as a mod translation on NexusMods.

That being said, the files you upload to your preferred distribution site will all be the same, so let's walk through
how to prepare the release of your translations.

### Release Preparation

Assuming you followed the guide, you should now have your translation file, or translation folder containing
translation files inside the mod's folder. We want to keep the original mod's folder structure to the translation
file(s), but we don't want to include every file that the mod provides, we only want to include the translations.

1. Open your file explorer and navigate to the folder you want to create your release.
2. Create a folder with the same name as the mod's folder, e.g. for Yet Another Fishing Mod, this is
`YetAnotherFishingMod`.
3. Navigate inside the folder you just created, and create a new folder named `i18n`.
4. Navigate inside that `i18n` folder, and copy your translation file or folder into it.
5. Navigate back up until you're back at the `YetAnotherFishingMod` folder, or whatever mod name you entered.
6. Create a `zip` of the folder, on windows this is done by right-clicking -> send to -> Compressed (zipped) folder.

### Publishing Translations on NexusMods

On [NexusMods](https://www.nexusmods.com/games/stardewvalley) you can click the "Upload" button in the top right, and
select "Upload mod". You'll be directed to a form, in here select "Translation" instead of the default "Mod",
and select the original mod you're translating in the field below. Upload your created zip file when asked.

## Example

This is a walk-through of an example mod, which you may use to double-check whether you've followed the above guide
correctly, or if you got stuck.

We just downloaded a mod named `TestMod`, the folder looks like this:

```
TestMod
│   LICENSE
│   manifest.json
│   README.txt
│   TestMod.dll
│   TestMod.xml
│
└───i18n
    └───default
            _config.json
            main.json
```

Now we can navigate to the `i18n` folder and create a copy of the `default` folder, and our file structure should
look like the following after renaming it to the correct language code:

```
TestMod
│   LICENSE
│   manifest.json
│   README.txt
│   TestMod.dll
│   TestMod.xml
│
└───i18n
    ├───default
    │       _config.json
    │       main.json
    │
    └───nl-NL
            _config.json
            main.json
```

It's time to open our `nl-NL/_config.json` file and see what's in it:

```json
{
  "message.config-reloaded": "Configuration file reloaded.",
  "message.test-message": "This is a test message, this message has been shown {{count}} times."
}
```

We can get to translating this file, resulting in a file that looks like the following:

```json
{
  "message.config-reloaded": "Configuratie bestand is herladen.",
  "message.test-message": "Dit is een test bericht, dit bericht is {{count}} keer laten zien."
}
```

It's time to prepare for a release, we're going to create it inside our `Documents` folder, which now looks like this:

```
Documents
└───TestMod
    └───i18n
        └───nl-NL
                _config.json
                main.json
```

Finally, create a zip file which you are free to distribute.

```
Documents
│   TestMod.zip
│
└───TestMod
    └───i18n
        └───nl-NL
                _config.json
                main.json
```

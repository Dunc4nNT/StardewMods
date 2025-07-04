# Templates

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

## Using Templates

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

# Compiling

Want to compile my mods yourself?

<!--TODO: add instructions to clone the repo, set up everything and compile my mods-->

## stardewvalley.targets

I use various custom properties in [mods/Directory.Build.props](./src/mods/Directory.Build.props), such as `<GamePathVersionOverride>` and `<ModsFolder>`. These properties must also be defined in a `stardewvalley.targets` file, you can find [mine](./docs/stardewvalley.targets), including extra documentation of those properties.

- `<GamePathVersionOverride>`

  This property is used to support having multiple versions of the game. I have a folder wherein I keep copies of different versions of the game. **NOTE:** "default" is a special value, which does not override GamePath.

  ```
  C:/Steam/steamapps/common
  └───Stardew Valley
  ...
  C:/GameFiles
  ├───Stardew Valley 1.6.9
  └───Stardew Valley 1.6.15
  ```
- `<ModsFolder>`

  Custom mod path instead of overriding everything in the standard `/Mods` folder.

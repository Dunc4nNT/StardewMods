namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Commands
{
    internal interface ICommand
    {
        bool CanExecute(int tile);

        void Handle(int tile);

        void Execute(int tile);
    }
}

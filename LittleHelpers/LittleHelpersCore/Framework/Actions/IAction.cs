namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions
{
    internal interface IAction
    {
        bool CanExecute(int tile);

        void Handle(int tile);

        void Execute(int tile);
    }
}

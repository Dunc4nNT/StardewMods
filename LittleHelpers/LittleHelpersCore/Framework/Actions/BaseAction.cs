namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Actions
{
    internal abstract class BaseAction : IAction
    {
        public abstract bool CanExecute(int tile);

        public abstract void Execute(int tile);

        public void Handle(int tile)
        {
            if (this.CanExecute(tile))
                this.Execute(tile);
        }
    }
}

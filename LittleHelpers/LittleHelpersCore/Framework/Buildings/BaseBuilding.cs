using NeverToxic.StardewMods.LittleHelpersCore.Framework.Commands;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Validators;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings
{
    internal class BaseBuilding(int helperCapacity, ILocationValidator locationValidator, int? radius = null, int? location = null) : IBuilding
    {
        public virtual List<BaseCommand> Commands { get; set; } = [];

        public List<int> Tiles { get; } = [1];

        public int? Radius { get; set; } = radius;

        public int HelperCapacity { get; set; } = helperCapacity;

        public int? Location { get; set; } = location;

        public ILocationValidator LocationValidator { get; set; } = locationValidator;

        public virtual void ExecuteCommands()
        {
            if (this.Commands is null)
                return;

            foreach (int tile in this.Tiles)
                foreach (BaseCommand command in this.Commands)
                    command.Handle(tile);
        }
    }
}

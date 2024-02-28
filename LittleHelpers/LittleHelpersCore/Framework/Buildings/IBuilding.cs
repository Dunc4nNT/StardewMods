using NeverToxic.StardewMods.LittleHelpersCore.Framework.Commands;
using NeverToxic.StardewMods.LittleHelpersCore.Framework.Validators;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpersCore.Framework.Buildings
{
    internal interface IBuilding
    {
        List<BaseCommand> Commands { get; set; }

        List<int> Tiles { get; }

        int? Radius { get; set; }

        int HelperCapacity { get; set; }

        int? Location { get; set; }

        ILocationValidator LocationValidator { get; set; }

        void ExecuteCommands();
    }
}

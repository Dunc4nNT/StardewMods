using NeverToxic.StardewMods.LittleHelpers.Framework.Commands;
using NeverToxic.StardewMods.LittleHelpers.Framework.Validators;
using System.Collections.Generic;

namespace NeverToxic.StardewMods.LittleHelpers.Framework.Buildings
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

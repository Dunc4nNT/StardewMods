using NeverToxic.StardewMods.Common;
using StardewModdingAPI;
using System;

namespace NeverToxic.StardewMods.SelfServe.Framework
{
    internal class GenericModConfigMenu(IModRegistry modRegistry, IManifest manifest, IMonitor monitor, Func<ModConfig> config, Action reset, Action save)
    {
        public void Register()
        {
            IGenericModConfigMenuApi configMenu = modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
                return;

            configMenu.Register(mod: manifest, reset: reset, save: save);


            configMenu.AddSectionTitle(
                mod: manifest,
                text: I18n.Config_Shops_ShopSection
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_PierresGeneralShop_Name,
                tooltip: I18n.Config_Shops_PierresGeneralShop_Tooltip,
                getValue: () => config().PierresGeneralShop,
                setValue: value => config().PierresGeneralShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_WillysFishShop_Name,
                tooltip: I18n.Config_Shops_WillysFishShop_Tooltip,
                getValue: () => config().WillysFishShop,
                setValue: value => config().WillysFishShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_IceCreamShop_Name,
                tooltip: I18n.Config_Shops_IceCreamShop_Tooltip,
                getValue: () => config().IceCreamShop,
                setValue: value => config().IceCreamShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_BlacksmithShop_Name,
                tooltip: I18n.Config_Shops_BlacksmithShop_Tooltip,
                getValue: () => config().BlacksmithShop,
                setValue: value => config().BlacksmithShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_CarpentersShop_Name,
                tooltip: I18n.Config_Shops_CarpentersShop_Tooltip,
                getValue: () => config().CarpentersShop,
                setValue: value => config().CarpentersShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_MarniesAnimalShop_Name,
                tooltip: I18n.Config_Shops_MarniesAnimalShop_Tooltip,
                getValue: () => config().MarniesAnimalShop,
                setValue: value => config().MarniesAnimalShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_HospitalShop_Name,
                tooltip: I18n.Config_Shops_HospitalShop_Tooltip,
                getValue: () => config().HospitalShop,
                setValue: value => config().HospitalShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_SaloonShop_Name,
                tooltip: I18n.Config_Shops_SaloonShop_Tooltip,
                getValue: () => config().SaloonShop,
                setValue: value => config().SaloonShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_BooksellerShop_Name,
                tooltip: I18n.Config_Shops_BooksellerShop_Tooltip,
                getValue: () => config().BooksellerShop,
                setValue: value => config().BooksellerShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_TravelingMerchantShop_Name,
                tooltip: I18n.Config_Shops_TravelingMerchantShop_Tooltip,
                getValue: () => config().TravelingMerchantShop,
                setValue: value => config().TravelingMerchantShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_ResortBarShop_Name,
                tooltip: I18n.Config_Shops_ResortBarShop_Tooltip,
                getValue: () => config().ResortBarShop,
                setValue: value => config().ResortBarShop = value
            );
            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.Config_Shops_SandyOasisShop_Name,
                tooltip: I18n.Config_Shops_SandyOasisShop_Tooltip,
                getValue: () => config().SandyOasisShop,
                setValue: value => config().SandyOasisShop = value
            );
        }
    }
}

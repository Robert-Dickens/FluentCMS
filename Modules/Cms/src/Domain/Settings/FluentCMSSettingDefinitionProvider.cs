using ByteLabs.Foundations.Settings;
using FluentCMS;

namespace FluentCMS.Domain.Settings;

public static class FluentCMSSettings
{
    public const string GroupName = FluentCMSConsts.ModuleName;


    /* Add constants for setting names. Example:
     * public const string MySettingName = GroupName + ".MySettingName";
     */
}

public class FluentCMSSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        /* Define module settings here.
         * Use names from ProductServiceSettings class.
         */
    }
}

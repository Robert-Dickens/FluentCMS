using FluentCMS;

namespace FluentCMS.Domain;

public static class FluentCMSDbProperties
{
    public static string DbTablePrefix { get; set; } = "CmsSite";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = FluentCMSConsts.ModuleName;

}

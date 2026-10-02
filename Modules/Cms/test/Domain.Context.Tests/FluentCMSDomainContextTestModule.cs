using ByteLabs.Foundations.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore.Sqlite;
using ByteLabs.Foundations.Modularity;
using ByteLabs.Foundations.Uow;
using FluentCMS.Domain;
using FluentCMS.Domain.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace FluentCMS;

[DependsOn(
    typeof(FluentCMSTestBaseModule),
    typeof(FluentCMSDomainContextModule),
    typeof(EntityFrameworkCoreSqliteModule)
)]
public class FluentCMSDomainContextTestModule : PlatformModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        var sqliteConnection = CreateDatabaseAndGetConnection();

        Configure<PlatformDbContextOptions>(options =>
        {
            options.Configure(dbContextConfigurationContext =>
            {
                dbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
            });
        });
    }

    private static SqliteConnection CreateDatabaseAndGetConnection()
    {
        var connection = new UnitTestSqliteConnection("Data Source=:memory:");
        connection.Open();

        new FluentCMSDbContext(
            new DbContextOptionsBuilder<FluentCMSDbContext>().UseSqlite(connection).Options
        ).GetService<IRelationalDatabaseCreator>().CreateTables();

        return connection;
    }
}

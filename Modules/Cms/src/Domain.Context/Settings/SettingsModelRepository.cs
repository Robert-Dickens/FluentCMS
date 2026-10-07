using ByteLabs.Foundations.Domain.Repositories.EntityFrameworkCore;
using ByteLabs.Foundations.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Domain.Settings;

public class SettingsModelRepository : EfCoreRepository<IFluentCMSDbContext, SettingsModel, Guid>, ISettingsModelRepository
{
    public SettingsModelRepository(IDbContextProvider<IFluentCMSDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<ICollection<SettingsModel>> GetByIds(IEnumerable<Guid> entityIds, CancellationToken cancellationToken = default)
    {
        var q = await GetQueryableAsync();

        q = q.Where(x => entityIds.Contains(x.Id));

        return await AsyncExecuter.ToListAsync(q, cancellationToken);

    }
    public async Task<SettingsModel> Update(Guid entityId, Dictionary<string, string> values, CancellationToken cancellationToken = default)
    {
        var existingDbSettings = await FindAsync(entityId, true, cancellationToken);

        if (existingDbSettings == null)
        {
            // Create a new Settings entity if it doesn't exist
            var newDbSettings = new SettingsModel(entityId)
            {
                Values = values.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => new SettingValuesModel
                {
                    Key = x.Key,
                    Value = x.Value,
                    SettingId = entityId
                }).ToList()
            };

            return await InsertAsync(newDbSettings, true, cancellationToken);

        }
        else
        {
            // Add updated values to SettingValues table
            foreach (var kvp in values.Where(x => !string.IsNullOrEmpty(x.Value)))
            {
                var settingValue = existingDbSettings.Values.FirstOrDefault(x => x.Key == kvp.Key);
                if (settingValue == null)
                {
                    // Create a new SettingValues entity if it doesn't exist
                    settingValue = new SettingValuesModel
                    {
                        SettingId = existingDbSettings.Id,
                        Key = kvp.Key,
                        Value = kvp.Value
                    };
                    existingDbSettings.Values.Add(settingValue);
                }
                else
                {
                    // Update the existing SettingValues entity
                    settingValue.Value = kvp.Value;
                }
                await UpdateAsync(existingDbSettings, true, cancellationToken);
            }


            return existingDbSettings;
        }

    }


}

using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Settings;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Settings;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Settings;

public class GlobalSettingsService(IGlobalSettingsModelRepository repository, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IGlobalSettingsService, ITransientDependency
{
    public async Task<GlobalSettings> Update(GlobalSettings settings, CancellationToken cancellationToken = default)
    {
        settings.SuperAdmins = settings.SuperAdmins.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

        // at least one super admin should exist
        if (!settings.SuperAdmins.Any())
            throw new AppException(ExceptionCodes.GlobalSettingsSuperAdminAtLeastOne);

        var existSetting = await repository.Get(cancellationToken) ?? new();

        var callingUser = CurrentUser.UserName ?? CurrentUser.Email;

        var allExistingSuperAdmins = existSetting.SuperAdmins?.Split(',') ?? Enumerable.Empty<string>();

        // if the current user is a super admin and is trying to remove himself from the super admin list, throw an exception
        if (allExistingSuperAdmins.Any(x => string.Equals(x, callingUser, StringComparison.CurrentCultureIgnoreCase) == true && settings.SuperAdmins.Any(y => string.Equals(y, callingUser, StringComparison.CurrentCultureIgnoreCase) == false)))
            throw new AppException(ExceptionCodes.GlobalSettingsSuperAdminCanNotBeDeleted);

        existSetting.SuperAdmins = string.Join(',', settings.SuperAdmins);

        var updatedEntity = await repository.UpdateAsync(existSetting, cancellationToken: cancellationToken)
            ?? throw new AppException(ExceptionCodes.GlobalSettingsUnableToUpdate);

        var updated = ObjectMapper.Map<GlobalSettingsModel, GlobalSettings>(updatedEntity);

        await messagePublisher.Publish(new Message<GlobalSettings>(ActionNames.GlobalSettingsUpdated, updated), cancellationToken);

        return updated;
    }

    public async Task<GlobalSettings> Get(CancellationToken cancellationToken = default)
    {
        var results = await repository.Get(cancellationToken) ??
                throw new AppException(ExceptionCodes.GlobalSettingsNotFound);

        return ObjectMapper.Map<GlobalSettingsModel, GlobalSettings>(results);
    }
}

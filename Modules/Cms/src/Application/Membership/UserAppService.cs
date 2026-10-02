using ByteLabs.Foundations;
using FluentCMS.Domain.Membership;
using FluentCMS.Domain.Settings;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using FluentCMS.Providers.MessageBusProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Membership;

[Authorize(FluentCMSPermissions.MemberShips.Members.Default)]
public class UserAppService : FluentCMSCrudAppService<UserModel, User, Guid, GetUserRequest, UserCreateRequest, UserUpdateRequest>, IUserAppService
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IMessagePublisher _messagePublisher;

    public UserAppService(IUserMembershipRepository repository, UserManager<UserModel> userManager, IMessagePublisher messagePublisher) : base(repository)
    {
        _userManager = userManager;
        _messagePublisher = messagePublisher;

        CreatePolicyName = FluentCMSPermissions.MemberShips.Members.Create;
        UpdatePolicyName = FluentCMSPermissions.MemberShips.Members.Edit;
        DeletePolicyName = FluentCMSPermissions.MemberShips.Members.Delete;
    }

    [Authorize]
    public async Task<bool> ChangePassword(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        request = Check.NotNull(request, nameof(request));
        Check.NotNullOrWhiteSpace(request.NewPassword, nameof(request.NewPassword));

        if (CurrentUser.Id == id || await base.AuthorizationService.IsGrantedAsync(FluentCMSPermissions.MemberShips.Members.ResetPassword))
        {
            return await DoChangePassword(id, request, cancellationToken);
        }
        throw new AppException(ExceptionCodes.PermissionDenied);
    }

    protected virtual async Task<bool> DoChangePassword(Guid id, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var foundUser = await Repository.FindAsync(id, false, cancellationToken) ?? throw new AppException(ExceptionCodes.UserNotFound);

        var token = await _userManager.GeneratePasswordResetTokenAsync(foundUser);
        var result = await _userManager.ResetPasswordAsync(foundUser, token, request.NewPassword);
        result.ThrowIfInvalid();

        var changedUser = await GetAsync(id);

        await _messagePublisher.Publish(new Message<User>(ActionNames.UserPasswordChanged, changedUser), cancellationToken);
        return true;
    }

    public override async Task<User> OnAfterUpdateAsync(User input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<User>(ActionNames.UserUpdated, input), cancellationToken);
        return await base.OnAfterUpdateAsync(input, cancellationToken);
    }

    public override async Task<User> OnAfterCreateAsync(User input, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<User>(ActionNames.UserCreated, input), cancellationToken);
        return await base.OnAfterCreateAsync(input, cancellationToken);
    }

    public override async Task OnBeforeDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var globalSettingsRepository = ServiceProvider.GetRequiredService<IGlobalSettingsModelRepository>();
        var globalSettings = await globalSettingsRepository.Get(cancellationToken) ??
                            throw new AppException(ExceptionCodes.GlobalSettingsNotFound);

        var user = await GetEntityByIdAsync(id);

        if (globalSettings.SuperAdmins.Contains(user.UserName))
            throw new AppException(ExceptionCodes.UserSuperAdminCanNotBeDeleted);

    }

    public override async Task OnAfterDeleteAsync(User model, CancellationToken cancellationToken = default)
    {
        await _messagePublisher.Publish(new Message<User>(ActionNames.UserDeleted, model), cancellationToken);
    }
}

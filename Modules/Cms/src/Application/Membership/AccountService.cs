using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.Domain.Membership;
using FluentCMS.Membership;
using FluentCMS.Providers.EmailProviders;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.ApiTokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.Membership;

public class AccountService(UserManager<Domain.Membership.UserModel> userManager, IEmailProvider emailProvider, IConfiguration configuration, IUserTokenProvider userTokenProvider, IMessagePublisher messagePublisher) : FluentCMSServiceAppService, IAccountService, ITransientDependency
{
    public async Task<UserToken> GetToken(User user, CancellationToken cancellationToken = default)
    {
        // Generate token
        var userToken = await userTokenProvider.Generate(user);

        if (userToken is null || string.IsNullOrEmpty(userToken.AccessToken))
            throw new AppException(ExceptionCodes.UserTokenGenerationFailed);

        // Store refresh token

        var tokenUser = ObjectMapper.Map<User, UserModel>(user);

        var identityResult = await userManager.SetAuthenticationTokenAsync(tokenUser, ServiceConstants.LOCAL_LOGIN_PROVIDER, ServiceConstants.REFRESH_TOKEN_NAME, userToken.RefreshToken);

        identityResult.ThrowIfInvalid();

        return userToken;
    }

    public async Task<bool> SendPasswordResetToken(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email) ??
            throw new AppException(ExceptionCodes.UserNotFound);

        var token = await userManager.GenerateUserTokenAsync(user, ServiceConstants.PASSWORD_RESET_TOKEN_PROVIDER, ServiceConstants.PASSWORD_RESET_PURPOSE);

        await emailProvider.Send(email, "Reset Password", $"{configuration["urls"]}/auth/reset-password?token={token}&email={email}", cancellationToken);

        await messagePublisher.Publish(new Message<User>(ActionNames.AccountPasswordSent, ObjectMapper.Map<UserModel, User>(user)), cancellationToken);

        return true;
    }

    public async Task<bool> ChangePasswordByResetToken(string email, string token, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email) ??
            throw new AppException(ExceptionCodes.UserNotFound);

        var result = await userManager.VerifyUserTokenAsync(user, ServiceConstants.PASSWORD_RESET_TOKEN_PROVIDER, ServiceConstants.PASSWORD_RESET_PURPOSE, token);
        if (result)
        {
            var resetResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            resetResult.ThrowIfInvalid();

            // Update user properties related to password changing
            user.PasswordChangedAt = DateTime.Now;
            user.PasswordChangedBy = CurrentUser.Id.ToString();
            await userManager.UpdateAsync(user);

            await messagePublisher.Publish(new Message<User>(ActionNames.AccountPasswordChangeByToken, ObjectMapper.Map<UserModel, User>(user)), cancellationToken);
        }
        return result;
    }

    public async Task<User> ChangePassword(Guid id, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id.ToString()) ??
            throw new AppException(ExceptionCodes.UserNotFound);

        if (!await userManager.CheckPasswordAsync(user, oldPassword))
            throw new AppException(ExceptionCodes.UserChangePasswordFailed);

        var idResult = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        idResult.ThrowIfInvalid();

        // Update user properties related to password changing
        user.PasswordChangedAt = DateTime.Now;
        user.PasswordChangedBy = CurrentUser.Id.ToString();
        await userManager.UpdateAsync(user);

        var results = ObjectMapper.Map<UserModel, User>(user);

        await messagePublisher.Publish(new Message<User>(ActionNames.AccountChangePassword, results), cancellationToken);

        return results;
    }

    public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByNameAsync(username);

        // Validate user password
        if (user is null || !user.Enabled || !await userManager.CheckPasswordAsync(user, password))
            throw new AppException(ExceptionCodes.UserLoginFailed);

        // Update user properties related to login
        user.LoginAt = DateTime.Now;
        user.LoginCount++;
        await userManager.UpdateAsync(user);

        var results = ObjectMapper.Map<UserModel, User>(user);
        await messagePublisher.Publish(new Message<User>(ActionNames.AccountAuthenticate, results), cancellationToken);

        return results;
    }

    public async Task<User> Register(User user, string password, CancellationToken cancellationToken = default)
    {
        var userEntity = ObjectMapper.Map<User, UserModel>(user);

        var identityResult = await userManager.CreateAsync(userEntity, password);
        identityResult.ThrowIfInvalid();

        await messagePublisher.Publish(new Message<User>(ActionNames.AccountRegister, user), cancellationToken);

        return user;
    }

    public async Task<User> Get(CancellationToken cancellationToken = default)
    {
        var userId = CurrentUser.Id;

        var user = await userManager.FindByIdAsync(userId.ToString()) ??
            throw new AppException(ExceptionCodes.UserNotFound);

        return ObjectMapper.Map<UserModel, User>(user);
    }

    public async Task<User> Update(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id != CurrentUser.Id)
            throw new AppException(ExceptionCodes.UserNotFound);

        var userToUpdate = ObjectMapper.Map<User, UserModel>(user);

        var result = await userManager.UpdateAsync(userToUpdate);
        result.ThrowIfInvalid();

        await messagePublisher.Publish(new Message<User>(ActionNames.AccountUpdated, user), cancellationToken);

        return user;
    }

    public async Task<bool> ValidatePassword(string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new AppException(ExceptionCodes.UserInvalidPassword);

        var passwordValidators = userManager.PasswordValidators;
        var identityUser = new UserModel(); // Create a dummy user for validation.

        foreach (var validator in passwordValidators)
        {
            var result = await validator.ValidateAsync(userManager, identityUser, password);
            result.ThrowIfInvalid();
        }

        return true;
    }

    public Task<bool> ValidateUserName(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new AppException(ExceptionCodes.UserInvalidUsername);

        if (username.Length < 3 || username.Length > 50)
            throw new AppException(ExceptionCodes.UserInvalidUsername);

        return Task.FromResult(true);
    }

}

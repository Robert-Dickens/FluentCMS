using ByteLabs.Foundations.DependencyInjection;
using FluentCMS.ApiTokens;
using FluentCMS.Domain.ApiTokens;
using FluentCMS.Membership;
using FluentCMS.Permissions;
using FluentCMS.Providers.ApiTokenProviders;
using FluentCMS.Providers.MessageBusProviders;
using FluentCMS.Services.Models;
using FluentCMS.Services.Permissions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace FluentCMS.Services.ApiTokens;

public class ApiTokenService(IApiTokenModelRepository apiTokenRepository, IApiTokenProvider apiTokenProvider, IMessagePublisher messagePublisher, IPermissionManager permissionManager) : FluentCMSServiceAppService, IApiTokenService, ITransientDependency
{
    public async Task<IEnumerable<ApiToken>> GetAll(CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var results = await apiTokenRepository.GetListAsync(cancellationToken: cancellationToken);
        return ObjectMapper.Map<IEnumerable<ApiTokenModel>, List<ApiToken>>(results);
    }

    public async Task<ApiToken> GetById(Guid tokenId, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var results = await apiTokenRepository.FindAsync(tokenId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenNotFound);
        return ObjectMapper.Map<ApiTokenModel, ApiToken>(results);
    }

    public async Task<ApiToken> Create(ApiToken apiToken, CancellationToken cancellationToken = default)
    {
        // remove policies with empty actions
        apiToken.Policies = apiToken.Policies.Where(x => x.Actions.Count != 0).ToList();

        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var existingApiToken = await apiTokenRepository.GetByName(apiToken.Name, cancellationToken);
        if (existingApiToken != null)
            throw new AppException(ExceptionCodes.ApiTokenNameIsDuplicated);

        apiToken.Key = apiTokenProvider.GenerateKey();
        apiToken.Secret = apiTokenProvider.GenerateSecret(apiToken.Key);

        var apiTokenEntity = ObjectMapper.Map<ApiToken, ApiTokenModel>(apiToken);

        var results = await apiTokenRepository.InsertAsync(apiTokenEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenUnableToCreate);

        apiToken = ObjectMapper.Map<ApiTokenModel, ApiToken>(results);

        await messagePublisher.Publish(new Message<ApiToken>(ActionNames.ApiTokenCreated, apiToken), cancellationToken);

        return apiToken;
    }

    public async Task<ApiToken> Update(ApiToken apiToken, CancellationToken cancellationToken = default)
    {
        // remove policies with empty actions
        apiToken.Policies = apiToken.Policies.Where(x => x.Actions.Count != 0).ToList();

        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var existingApiToken = await apiTokenRepository.FindAsync(apiToken.Id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenNotFound);

        var isSameApiTokenExist = await apiTokenRepository.GetByName(apiToken.Name, cancellationToken);
        if (isSameApiTokenExist != null && apiToken.Id != isSameApiTokenExist.Id)
            throw new AppException(ExceptionCodes.ApiTokenNameIsDuplicated);

        //apiKey is not updated here as it should be generated automatically only
        apiToken.Secret = existingApiToken.Secret;
        apiToken.Key = existingApiToken.Key;

        var apiTokenEntity = ObjectMapper.Map<ApiToken, ApiTokenModel>(apiToken);

        var updated = await apiTokenRepository.UpdateAsync(apiTokenEntity, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenUnableToUpdate);

        apiToken = ObjectMapper.Map<ApiTokenModel, ApiToken>(updated);

        await messagePublisher.Publish(new Message<ApiToken>(ActionNames.ApiTokenUpdated, apiToken), cancellationToken);

        return apiToken;
    }

    public async Task<ApiToken> Delete(Guid tokenId, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var deleted = await apiTokenRepository.DeleteWithResultAsync(tokenId, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenUnableToDelete);

        var results = ObjectMapper.Map<ApiTokenModel, ApiToken>(deleted);

        await messagePublisher.Publish(new Message<ApiToken>(ActionNames.ApiTokenDeleted, results), cancellationToken);

        return results;
    }

    public async Task<ApiToken> Validate(string apiKey, string apiSecret, CancellationToken cancellationToken = default)
    {
        // there is not need to check permissions here
        // as this method is used for API authentication
        var apiToken = await apiTokenRepository.GetByKey(apiKey, cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenNotFound);

        // check if token expired or not
        if (apiToken.ExpireAt.HasValue && apiToken.ExpireAt < DateTime.UtcNow)
            throw new AppException(ExceptionCodes.ApiTokenExpired);

        // check if the token is active or not
        if (!apiToken.Enabled)
            throw new AppException(ExceptionCodes.ApiTokenInactive);

        // check if the secret is valid or not
        if (apiToken.Secret != apiSecret)
            throw new AppException(ExceptionCodes.ApiTokenInvalidSecret);

        return ObjectMapper.Map<ApiTokenModel, ApiToken>(apiToken);
    }

    public async Task<ApiToken> RegenerateSecret(Guid id, CancellationToken cancellationToken = default)
    {
        if (!await permissionManager.HasAccess(GlobalPermissionAction.SuperAdmin, cancellationToken))
            throw new AppException(ExceptionCodes.PermissionDenied);

        var apiToken = await apiTokenRepository.FindAsync(id, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenNotFound);

        apiToken.Secret = apiTokenProvider.GenerateSecret(apiToken.Key);

        var updated = await apiTokenRepository.UpdateAsync(apiToken, cancellationToken: cancellationToken) ??
            throw new AppException(ExceptionCodes.ApiTokenUnableToUpdate);

        var results = ObjectMapper.Map<ApiTokenModel, ApiToken>(updated);

        await messagePublisher.Publish(new Message<ApiToken>(ActionNames.ApiTokenSecretRegenerated, results), cancellationToken);

        return results;
    }
}

public interface IUserTokenProvider
{
    Task<UserToken> Generate(User user);
    Task<Guid> Validate(string accessToken);
    Task<Guid> ValidateExpiredToken(string accessToken);
}

public class JwtUserTokenProvider(IOptions<JwtOptions> options) : IUserTokenProvider, ITransientDependency
{
    private readonly JwtOptions _options = options.Value;

    public virtual async Task<UserToken> Generate(User user)
    {
        return await Task.Run(() =>
        {
            var token = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            return new UserToken(token.AccessToken, refreshToken, token.Expiry);
        });
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private UserToken GenerateToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = SHA512.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(_options.Secret));
        var claims = GetJwtClaims(user);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Subject = new ClaimsIdentity(claims),
            //todo : Fix this
            Expires = _options.TokenExpiry == -1 ? DateTime.MaxValue : DateTime.UtcNow.AddSeconds(_options.TokenExpiry),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow
        };

        var token = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var accessToekn = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserToken(accessToekn, null, tokenDescriptor.Expires ?? DateTime.MaxValue);
    }

    private List<Claim> GetJwtClaims(User user)
    {
        var userId = user.Id.ToString();
        var result = new List<Claim>
        {
            new(ClaimTypes.Sid, userId),
            new(ClaimTypes.NameIdentifier, user.UserName?? string.Empty),
        };
        if (!string.IsNullOrWhiteSpace(user.NormalizedEmail))
            result.Add(new Claim(ClaimTypes.Email, user.NormalizedEmail));

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            result.Add(new Claim(ClaimTypes.Email, user.PhoneNumber));

        return result;
    }

    public async Task<Guid> Validate(string accessToken)
    {
        return await Task.Run(() =>
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = SHA512.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(_options.Secret));
            var principal = jwtTokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                SaveSigninToken = true,
                ValidAudience = _options.Audience,
                ValidIssuer = _options.Issuer
            }, out _);

            var userId = GetUserId(principal);

            return userId;
        });
    }

    public async Task<Guid> ValidateExpiredToken(string accessToken)
    {
        return await Task.Run(() =>
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = SHA512.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(_options.Secret));
            var principal = jwtTokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                RequireExpirationTime = true,
                ValidateLifetime = false,
                SaveSigninToken = true,
                ValidAudience = _options.Audience,
                ValidIssuer = _options.Issuer
            }, out _);

            var userId = GetUserId(principal);

            return userId;
        });
    }

    private Guid GetUserId(ClaimsPrincipal principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId is null) throw new Exception("Token is not valid.");

        return Guid.Parse(userId);
    }
}
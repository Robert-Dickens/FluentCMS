using System;

namespace FluentCMS.Membership;

public record UserToken(string AccessToken, string? RefreshToken, DateTime Expiry);

using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace FluentCMS.Services.Membership;

public static class IdentityResultExtensions
{
    /// <summary>
    /// Throws an exception if the IdentityResult is not successful, indicating validation errors.
    /// </summary>
    /// <param name="identityResult">The IdentityResult to check.</param>
    /// <exception cref="AppException">Thrown when the IdentityResult indicates validation errors.</exception>
    public static void ThrowIfInvalid(this IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new AppException(identityResult.Errors.Select(e => $"User.{e.Code}"));
        }
    }
}
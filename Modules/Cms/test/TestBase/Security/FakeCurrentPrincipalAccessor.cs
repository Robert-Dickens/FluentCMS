using System.Security.Claims;
using ByteLabs.Foundations.DependencyInjection;
using ByteLabs.Foundations.Security.Claims;

namespace FluentCMS.Security;

[Dependency(ReplaceServices = true)]
public class FakeCurrentPrincipalAccessor : ICurrentPrincipalAccessor, ISingletonDependency
{
    public ClaimsPrincipal Principal => GetPrincipal();
    private ClaimsPrincipal _principal;

    private ClaimsPrincipal GetPrincipal()
    {
        if (_principal == null)
        {
            lock (this)
            {
                if (_principal == null)
                {
                    _principal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new List<Claim>
                            {
                                    new Claim(PlatformClaimTypes.UserId,"2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
                                    new Claim(PlatformClaimTypes.UserName,"admin"),
                                    new Claim(PlatformClaimTypes.Email,"admin@admin.com")
                            }
                        )
                    );
                }
            }
        }

        return _principal;
    }

    public IDisposable Change(ClaimsPrincipal principal)
    {
        _principal = principal;
        return null;
    }
}

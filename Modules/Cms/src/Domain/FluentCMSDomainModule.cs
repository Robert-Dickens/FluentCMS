using ByteLabs.Foundations.Auditing;
using ByteLabs.Foundations.Domain;
using ByteLabs.Foundations.Features;
using ByteLabs.Foundations.GlobalFeatures;
using ByteLabs.Foundations.Modularity;
using ByteLabs.PlatformServices.Users.Domain;

namespace FluentCMS.Domain;

[DependsOn(
    typeof(PlatformDomainModule),
    typeof(AuditingModule),
    typeof(GlobalFeaturesModule),
    typeof(FeaturesModule),
    typeof(FluentCMSDomainAbstractionsModule),
    typeof(UsersDomainModule)
)]
public class FluentCMSDomainModule : PlatformModule
{

}

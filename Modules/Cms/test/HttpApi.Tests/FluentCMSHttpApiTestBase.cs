using ByteLabs.Foundations.AspNetCore.TestFactory.Fixtures;
using Meziantou.Xunit;

namespace FluentCMS;

[Collection(FluentCMSRemoteServiceConsts.RemoteServiceName)]
[EnableParallelization]
public abstract class FluentCMSHttpApiTestBase : AspNetCoreApiIntegrationTestBase<FluentCMSHttpApiTestModule>
{
}
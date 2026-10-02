using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bunit;
using ByteLabs.Foundations.AspNetCore.Components.DependencyInjection;
using ByteLabs.Foundations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNamespace.Tests;

namespace MyNamespace.Blazor.Tests;

public abstract class MyProjectNameBlazorTestBase : MyProjectNameTestBase<MyProjectNameBlazorTestModule>
{
    protected virtual TestContext CreateTestContext()
    {
        var testContext = new TestContext();
        var blazorise = testContext.JSInterop.SetupModule("./_content/Blazorise/utilities.js?v=1.6.2.0");
        blazorise.SetupVoid("log", _ => true);
        testContext.Services.UseServiceProviderFactory(serviceCollection =>
        {
            foreach (var service in ServiceProvider.GetRequiredService<IApplicationWithExternalServiceProvider>().Services)
            {
                serviceCollection.Add(service);
            }
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);
            return new AutofacServiceProvider(containerBuilder.Build());
        });

        testContext.Services.Replace(ServiceDescriptor.Transient<IComponentActivator, ServiceProviderComponentActivator>());

        return testContext;
    }
}

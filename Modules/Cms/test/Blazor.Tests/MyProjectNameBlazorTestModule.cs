using ByteLabs.Foundations.AspNetCore.Components;
using ByteLabs.Foundations.Modularity;
using MyNamespace.Domain.Context.Tests;

namespace MyNamespace.Blazor.Tests;

[DependsOn(
    typeof(AspNetCoreComponentsModule),
    typeof(MyProjectNameDomainContextTestModule),
    typeof(MyProjectNameBlazorModule)
)]
public class MyProjectNameBlazorTestModule : PlatformModule
{

}

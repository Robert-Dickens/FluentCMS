using ByteLabs.Foundations.AspNetCore.Components;
using MyNamespace.Localization;

namespace MyNamespace.Blazor
{
    public abstract class MyProjectNameComponentBase : AspNetCoreComponentBase
    {
        protected MyProjectNameComponentBase()
        {
            LocalizationResource = typeof(MyProjectNameResource);
        }
    }
}

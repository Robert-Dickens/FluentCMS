using ByteLabs.Foundations.AspNetCore.UI.Navigation;
using ByteLabs.Foundations.AspNetCore.Web;
using ByteLabs.Foundations.Authorization.Permissions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using MyNamespace.Localization;
using MyNamespace.Permissions;

namespace MyNamespace.Blazor.Menus;

public class MyProjectNameMenuContributor : IMenuContributor
{
    public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            var moduleMenu = await ConfigureMainMenuAsync(context);
            await AddMenuItemProducts(context, moduleMenu);
        }
    }

    private Task<ApplicationMenuItem> ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var moduleMenu = new ApplicationMenuItem(
                            MyProjectNameMenus.Prefix,
                            context.GetLocalizer<MyProjectNameResource>()[$"Menu:{MyProjectNameConsts.ModuleName}"]
                        ).SetFluentIcon(new Size24.Book());

        context.Menu.Items.AddIfNotContains(moduleMenu);

        return Task.FromResult(moduleMenu);
    }

    private Task AddMenuItemProducts(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                MyProjectNameMenus.Products,
                context.GetLocalizer<MyProjectNameResource>()["Menu:Products"],
                url: "/Products"
            ).RequirePermissions(MyProjectNamePermissions.Products.Default));

        context.Menu.Items.AddIfNotContains(parentMenu);
        return Task.CompletedTask;
    }

}

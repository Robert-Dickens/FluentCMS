using Bunit;
using BlazorPages = MyNamespace.Blazor.Pages;
namespace MyNamespace.Blazor.Tests.Pages.Products;

public class Index_Tests : MyProjectNameBlazorTestBase
{
    [Fact]
    public void Index_Test()
    {
        // Arrange
        var ctx = CreateTestContext();

        // Act
        var cut = ctx.RenderComponent<BlazorPages.Products.Index>();

        // Assert
        var divNode = cut.Nodes.FirstOrDefault(x => x.NodeName == "DIV");
        divNode.ShouldNotBeNull();
        divNode.TextContent.ShouldContain("Congratulations MyProjectNameService is successfully running!");

        cut.Find("#username").InnerHtml.Contains("Welcome Back admin@admin.com").ShouldBeTrue();
    }
}

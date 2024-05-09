using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace QuickTemplate.Pages;

public class Index_Tests : QuickTemplateWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}

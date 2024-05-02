using Generic.Abp.Host.Samples;
using Xunit;

namespace Generic.Abp.Host.EntityFrameworkCore.Applications;

[Collection(HostTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<HostEntityFrameworkCoreTestModule>
{

}

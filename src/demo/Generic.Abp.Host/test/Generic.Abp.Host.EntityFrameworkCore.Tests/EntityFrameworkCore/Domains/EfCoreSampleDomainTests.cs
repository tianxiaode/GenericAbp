using Generic.Abp.Host.Samples;
using Xunit;

namespace Generic.Abp.Host.EntityFrameworkCore.Domains;

[Collection(HostTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<HostEntityFrameworkCoreTestModule>
{

}

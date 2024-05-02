using Xunit;

namespace Generic.Abp.Host.EntityFrameworkCore;

[CollectionDefinition(HostTestConsts.CollectionDefinitionName)]
public class HostEntityFrameworkCoreCollection : ICollectionFixture<HostEntityFrameworkCoreFixture>
{

}

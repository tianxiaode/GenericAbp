using Generic.Abp.ExportManager.Samples;
using Xunit;

namespace Generic.Abp.ExportManager.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<ExportManagerMongoDbTestModule>
{

}

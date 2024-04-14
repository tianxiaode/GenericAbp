using Generic.Abp.ExportManager.MongoDB;
using Generic.Abp.ExportManager.Samples;
using Xunit;

namespace Generic.Abp.ExportManager.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<ExportManagerMongoDbTestModule>
{

}

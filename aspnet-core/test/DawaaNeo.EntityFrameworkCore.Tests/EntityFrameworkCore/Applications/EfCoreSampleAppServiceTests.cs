using DawaaNeo.Samples;
using Xunit;

namespace DawaaNeo.EntityFrameworkCore.Applications;

[Collection(DawaaNeoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<DawaaNeoEntityFrameworkCoreTestModule>
{

}

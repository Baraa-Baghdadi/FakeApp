using DawaaNeo.Samples;
using Xunit;

namespace DawaaNeo.EntityFrameworkCore.Domains;

[Collection(DawaaNeoTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<DawaaNeoEntityFrameworkCoreTestModule>
{

}

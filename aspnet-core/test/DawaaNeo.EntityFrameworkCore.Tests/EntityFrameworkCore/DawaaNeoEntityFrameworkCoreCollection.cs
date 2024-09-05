using Xunit;

namespace DawaaNeo.EntityFrameworkCore;

[CollectionDefinition(DawaaNeoTestConsts.CollectionDefinitionName)]
public class DawaaNeoEntityFrameworkCoreCollection : ICollectionFixture<DawaaNeoEntityFrameworkCoreFixture>
{

}

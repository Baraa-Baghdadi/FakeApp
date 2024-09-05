using Volo.Abp.Modularity;

namespace DawaaNeo;

public abstract class DawaaNeoApplicationTestBase<TStartupModule> : DawaaNeoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

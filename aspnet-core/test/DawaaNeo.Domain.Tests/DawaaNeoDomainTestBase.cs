using Volo.Abp.Modularity;

namespace DawaaNeo;

/* Inherit from this class for your domain layer tests. */
public abstract class DawaaNeoDomainTestBase<TStartupModule> : DawaaNeoTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}

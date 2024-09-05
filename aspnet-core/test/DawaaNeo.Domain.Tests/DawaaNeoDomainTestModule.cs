using Volo.Abp.Modularity;

namespace DawaaNeo;

[DependsOn(
    typeof(DawaaNeoDomainModule),
    typeof(DawaaNeoTestBaseModule)
)]
public class DawaaNeoDomainTestModule : AbpModule
{

}

using Volo.Abp.Modularity;

namespace DawaaNeo;

[DependsOn(
    typeof(DawaaNeoApplicationModule),
    typeof(DawaaNeoDomainTestModule)
)]
public class DawaaNeoApplicationTestModule : AbpModule
{

}

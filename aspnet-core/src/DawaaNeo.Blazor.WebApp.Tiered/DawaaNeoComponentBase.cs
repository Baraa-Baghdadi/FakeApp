using DawaaNeo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace DawaaNeo.Blazor.WebApp.Tiered;

public abstract class DawaaNeoComponentBase : AbpComponentBase
{
    protected DawaaNeoComponentBase()
    {
        LocalizationResource = typeof(DawaaNeoResource);
    }
}

using DawaaNeo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace DawaaNeo.Blazor.WebApp;

public abstract class DawaaNeoComponentBase : AbpComponentBase
{
    protected DawaaNeoComponentBase()
    {
        LocalizationResource = typeof(DawaaNeoResource);
    }
}

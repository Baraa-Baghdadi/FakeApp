using DawaaNeo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace DawaaNeo.Blazor.Client;

public abstract class DawaaNeoComponentBase : AbpComponentBase
{
    protected DawaaNeoComponentBase()
    {
        LocalizationResource = typeof(DawaaNeoResource);
    }
}

using DawaaNeo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DawaaNeo.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DawaaNeoController : AbpControllerBase
{
    protected DawaaNeoController()
    {
        LocalizationResource = typeof(DawaaNeoResource);
    }
}

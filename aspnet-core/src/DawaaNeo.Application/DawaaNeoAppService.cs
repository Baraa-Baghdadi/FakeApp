using System;
using System.Collections.Generic;
using System.Text;
using DawaaNeo.Localization;
using Volo.Abp.Application.Services;

namespace DawaaNeo;

/* Inherit your application services from this class.
 */
public abstract class DawaaNeoAppService : ApplicationService
{
    protected DawaaNeoAppService()
    {
        LocalizationResource = typeof(DawaaNeoResource);
    }
}

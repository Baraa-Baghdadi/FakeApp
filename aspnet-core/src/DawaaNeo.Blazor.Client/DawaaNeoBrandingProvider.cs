using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace DawaaNeo.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class DawaaNeoBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "DawaaNeo";
}

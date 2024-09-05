using Volo.Abp.Settings;

namespace DawaaNeo.Settings;

public class DawaaNeoSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(DawaaNeoSettings.MySetting1));
    }
}

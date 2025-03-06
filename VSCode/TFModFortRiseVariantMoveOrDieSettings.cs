using FortRise;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  public class TFModFortRiseVariantMoveOrDieSettings : ModuleSettings
  {
    [SettingsName("Activated")]
    public bool activated = false;

    [SettingsName("StationaryDeathTime")]
    [SettingsNumber(1, 5)]
    public int stationaryDeathTime = 1;

    [SettingsName("MinSpeed")]
    [SettingsNumber(1, 5)]
    public int minSpeed = 1;
  }
}

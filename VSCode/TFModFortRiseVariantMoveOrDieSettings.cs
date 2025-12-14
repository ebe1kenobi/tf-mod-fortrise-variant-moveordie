using FortRise;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  public class TFModFortRiseVariantMoveOrDieSettings : ModuleSettings
  {
    public override void Create(ISettingsCreate settings)
    {
      settings.CreateOnOff("Pickup activated even \n\nwhen variant is not selected", activated, (x) => activated = x);
      settings.CreateNumber("StationaryDeathTime", stationaryDeathTime, (x) => stationaryDeathTime = x, 1,5);
      settings.CreateNumber("MinSpeed", minSpeed, (x) => minSpeed = x, 1, 5);

    }

    //[SettingsName("Variant activated even \n\nwhen variant is not selected")]
    public bool activated { get; set; } = false;

    //[SettingsName("StationaryDeathTime")]
    //[SettingsNumber(1, 5)]
    public int stationaryDeathTime { get; set; } = 1;

    //[SettingsName("MinSpeed")]
    //[SettingsNumber(1, 5)]
    public int minSpeed { get; set; } = 1;
  }
}

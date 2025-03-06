using System;
using FortRise;
using Monocle;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  [Fort("com.ebe1.kenobi.TFModFortRisePickupLaserBomb", "TFModFortRisePickupLaserBomb")]
  public class TFModFortRiseVariantMoveOrDieModule : FortModule
  {
    public static TFModFortRiseVariantMoveOrDieModule Instance;
    public Atlas Atlas;
    public override Type SettingsType => typeof(TFModFortRiseVariantMoveOrDieSettings);
    public static TFModFortRiseVariantMoveOrDieSettings Settings => (TFModFortRiseVariantMoveOrDieSettings)Instance.InternalSettings;
    public TFModFortRiseVariantMoveOrDieModule()
    {
      Instance = this;
      //Logger.Init("TFModFortRiseVariantMoveOrDieModuleLog");
    }

    public override void Load()
    {
      MyPlayer.Load();
    }

    public override void Unload()
    {
      MyPlayer.Unload();
      Instance = null;
    }

    public override void LoadContent()
    {
      Atlas = Content.LoadAtlas("Atlas/atlas.xml", "Atlas/atlas.png");
    }


    public override void OnVariantsRegister(VariantManager manager, bool noPerPlayer = false)
    {
      var icon = new CustomVariantInfo(
          "MoveOrDie", VariantManager.GetVariantIconFromName("MoveOrDie", Atlas),
          CustomVariantFlags.None
          );
      manager.AddVariant(icon);
    }
  }
}

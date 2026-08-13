using System;
using System.Diagnostics;
using FortRise;
using System;
using System.Diagnostics;
using FortRise;
using Microsoft.Extensions.Logging;
using Monocle;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  public class TFModFortRiseVariantMoveOrDieModule : Mod
  {
    public static TFModFortRiseVariantMoveOrDieModule Instance;

    private static Type[] Registerables = [
        typeof(TextureRegistry),
        typeof(Variants)

    ];
    internal Type[] Hookables = [
        typeof(MyPlayer),
        typeof(MyVariantToggle),
    ];

    public static TFModFortRiseVariantMoveOrDieSettings Settings => Instance.GetSettings<TFModFortRiseVariantMoveOrDieSettings>()!;
    //public Atlas Atlas;
    public TFModFortRiseVariantMoveOrDieModule(IModContent content, IModuleContext context, ILogger logger) : base(content, context, logger)
    {
      if (!Debugger.IsAttached)
      {
        //Debugger.Launch(); // Proposera d’attacher Visual Studio
      }
      Instance = this;
      TFModFortRiseVariantMoveOrDie.Logger.Init(logger);


      foreach (var hookable in Hookables)
      {
        hookable.GetMethod(nameof(IHookable.Load))!.Invoke(null, [context.Harmony]);
      }

      foreach (var registerable in Registerables)
      {
        registerable.GetMethod(nameof(IRegisterable.Register))!.Invoke(null, [content, context.Registry]);
      }
    }

    /// <summary>
    /// Ecrit les reglages sur le disque tout de suite.
    ///
    /// FortRise ne les enregistre qu'en sortant de SON ecran d'options : un reglage
    /// change depuis la fenetre de la variante ne vivrait qu'en memoire et serait
    /// perdu en quittant. SaveSettings est internal cote FortRise, d'ou la reflexion.
    /// </summary>
    public static void SaveSettingsNow()
    {
      if (Instance == null)
      {
        return;
      }

      try
      {
        var method = typeof(Mod).GetMethod("SaveSettings",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(Instance, null);
      }
      catch (Exception e)
      {
        TFModFortRiseVariantMoveOrDie.Logger.Info($"[Settings] sauvegarde immediate impossible : {e.Message}");
      }
    }

    public override ModuleSettings CreateSettings()
    {
      return new TFModFortRiseVariantMoveOrDieSettings();
    }

    //public override void Load()
    //{
    //  MyPlayer.Load();
    //}

    //public override void Unload()
    //{
    //  MyPlayer.Unload();
    //  Instance = null;
    //}

    //public override void LoadContent()
    //{
    //  Atlas = Content.LoadAtlas("Atlas/atlas.xml", "Atlas/atlas.png");
    //}


    //public override void OnVariantsRegister(VariantManager manager, bool noPerPlayer = false)
    //{
    //  var icon = new CustomVariantInfo(
    //      "MoveOrDie", VariantManager.GetVariantIconFromName("MoveOrDie", Atlas),
    //      CustomVariantFlags.None
    //      );
    //  manager.AddVariant(icon);
    //}
  }
}

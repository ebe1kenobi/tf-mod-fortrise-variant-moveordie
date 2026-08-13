using System;
using System.Collections.Generic;
using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  /// <summary>
  /// Ouvre les reglages du mod depuis sa case dans l'ecran des variantes.
  ///
  /// La touche est <c>Alt2</c> - la gachette gauche du haut. Le jeu y met EXPLAIN,
  /// mais seulement pour une variante qui porte une description ; la notre n'en a pas,
  /// la touche est donc libre, et le guide du bas l'annonce.
  /// </summary>
  public class MyVariantToggle : IHookable
  {
    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(VariantToggle), nameof(VariantToggle.Update)),
          postfix: new HarmonyMethod(Update_patch)
      );

      // OnSelect est protegee : nom en dur, pas de nameof possible.
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(VariantToggle), "OnSelect"),
          postfix: new HarmonyMethod(OnSelect_patch)
      );
    }

    /// <summary>
    /// Est-ce NOTRE case ?
    ///
    /// La comparaison etait une egalite EXACTE avec le libelle enregistre, et elle
    /// echouait sans rien dire : le titre affiche ne revient pas toujours tel qu'on
    /// l'a pose - casse changee, espaces - et une case dont le libelle ne tombe pas au
    /// caractere pres n'ouvrait tout simplement jamais sa fenetre. Un seul des mods y
    /// arrivait, ce qui est le pire des cas : le mecanisme avait l'air bon.
    ///
    /// On compare donc sans tenir compte de la casse ni des espaces, et contre le
    /// LIBELLE comme contre le NOM d'enregistrement - les deux ne sont pas forcement
    /// identiques (Speed s'enregistre "Speed" et s'affiche "SPEED").
    /// </summary>
    private static bool IsOurs(VariantToggle toggle)
    {
      string shown = toggle?.Variant?.Title;

      if (string.IsNullOrEmpty(shown))
      {
        return false;
      }

      return Same(shown, Variants.TITLE) || Same(shown, Variants.MoveOrDie?.Name);
    }

    /// <summary>
    /// Les libelles deja vus, pour ne tracer qu'une fois chacun. Le survol appelle
    /// OnSelect a chaque passage du curseur : sans ce garde, le journal se remplirait.
    /// </summary>
    private static readonly System.Collections.Generic.HashSet<string> traced =
        new System.Collections.Generic.HashSet<string>();

    /// <summary>
    /// Note ce que le jeu affiche reellement sur une case, et si on s'y reconnait.
    /// C'est ce qui manquait pour comprendre pourquoi la fenetre ne s'ouvrait pas :
    /// l'echec etait muet.
    /// </summary>
    private static void Trace(VariantToggle toggle)
    {
      string shown = toggle?.Variant?.Title;

      if (string.IsNullOrEmpty(shown) || !traced.Add(shown))
      {
        return;
      }

      Logger.Info($"[Variants] case '{shown}' - a nous : {IsOurs(toggle)}");
    }

    private static bool Same(string shown, string mine)
    {
      if (string.IsNullOrEmpty(mine))
      {
        return false;
      }

      return string.Equals(shown.Replace(" ", ""), mine.Replace(" ", ""),
          StringComparison.OrdinalIgnoreCase);
    }

    public static void Update_patch(VariantToggle __instance)
    {
      try
      {
        // Selected retombe des que le jeu ouvre sa propre fenetre : les deux ne
        // peuvent donc pas s'ouvrir l'une sur l'autre.
        if (!__instance.Selected || !IsOurs(__instance) || !MenuInput.Alt2)
        {
          return;
        }

        Sounds.ui_click.Play(160f, 1f);
        __instance.Selected = false;

        var popup = new VariantPopup(__instance, Variants.TITLE, Fields(),
            new Vector2(160f, 120f));

        popup.TweenIn();
        __instance.MainMenu.Add<VariantPopup>(popup);
      }
      catch (Exception e)
      {
        Logger.Info("[Variants] fenetre de reglages impossible : " + e.Message);
      }
    }

    /// <summary>
    /// Les reglages proposes, dans l'ordre d'importance : d'abord ce qui decide si la
    /// variante s'applique, ensuite ce qu'elle exige.
    /// </summary>
    private static List<VariantPopup.Field> Fields()
    {
      TFModFortRiseVariantMoveOrDieSettings s = TFModFortRiseVariantMoveOrDieModule.Settings;

      return new List<VariantPopup.Field>
      {
        VariantPopup.Bool("ALWAYS ACTIVE", () => s.activated, v => s.activated = v),
        VariantPopup.Int("DEATH AFTER (S)",
            () => s.stationaryDeathTime, v => s.stationaryDeathTime = v, 1, 5),
        VariantPopup.Int("MIN SPEED", () => s.minSpeed, v => s.minSpeed = v, 1, 5)
      };
    }

    /// <summary>
    /// Annonce la touche dans le guide du bas.
    ///
    /// En postfix : le OnSelect du jeu vient de poser ses propres touches et de VIDER
    /// la ligne D, notre variante n'ayant pas de description. On ecrit donc en D, ce
    /// qui laisse tout visible a la fois.
    /// </summary>
    public static void OnSelect_patch(VariantToggle __instance)
    {
      try
      {
        Trace(__instance);

        if (!IsOurs(__instance))
        {
          return;
        }

        __instance.MainMenu.ButtonGuideD.SetDetails(MenuButtonGuide.ButtonModes.Alt2, "SETTINGS");
      }
      catch (Exception e)
      {
        Logger.Info("[Variants] guide des boutons : " + e.Message);
      }
    }
  }
}

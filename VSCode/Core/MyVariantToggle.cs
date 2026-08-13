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

    /// <summary>Est-ce NOTRE case ? Le libelle est le seul repere fiable ici.</summary>
    private static bool IsOurs(VariantToggle toggle)
    {
      return toggle != null
          && toggle.Variant != null
          && string.Equals(toggle.Variant.Title, Variants.TITLE, StringComparison.Ordinal);
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

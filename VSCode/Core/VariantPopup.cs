using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  /// <summary>
  /// Les reglages du mod, sur la case de sa variante.
  ///
  /// Ils vivaient uniquement dans l'ecran des mods, a deux menus de la partie qu'on
  /// s'apprete a lancer : pour changer un poids il fallait quitter la selection des
  /// variantes, traverser les options, revenir. Ici ils sont sous la main, a l'endroit
  /// exact ou l'on decide d'activer le pickup.
  ///
  /// La fenetre est calquee sur <c>VariantPerPlayer</c>, celle que le jeu ouvre sur
  /// ses variantes par joueur : meme panneau, meme entree en glissant depuis la
  /// droite, meme sortie au retour, memes sons. C'est ce qui la fait passer pour une
  /// fenetre du jeu, et c'est aussi pourquoi elle herite de MenuItem - le MainMenu ne
  /// sait faire glisser, selectionner et retirer que ca.
  /// </summary>
  public class VariantPopup : MenuItem
  {
    /// <summary>Une ligne reglable : ce qu'elle montre, et ce que les fleches font.</summary>
    public sealed class Field
    {
      public string Label;
      public Func<string> Value;
      public Action<int> Change;

      /// <summary>Null = toujours visible. Une ligne cachee est sautee par le curseur.</summary>
      public Func<bool> Visible;
    }

    private const float PanelWidth = 220f;
    private const float RowStep = 14f;
    private const float TopPadding = 26f;
    private const float BottomPadding = 12f;

    private readonly VariantToggle toggle;
    private readonly List<Field> fields;
    private readonly string title;

    /// <summary>Ou l'on veut la fenetre A L'ECRAN, camera deduite.</summary>
    private readonly Vector2 screenAnchor;

    /// <summary>Decalage horizontal restant de l'animation d'entree, en pixels.</summary>
    private float slide;

    private readonly Wiggler wiggler;

    private int selected;

    public VariantPopup(VariantToggle toggle, string title, List<Field> fields, Vector2 position)
        : base(position)
    {
      // Devant les cases de variantes, qui vivent a une profondeur ordinaire.
      Depth = -100;

      this.toggle = toggle;
      this.title = title;
      this.fields = fields;

      // La position demandee est celle qu'on veut A L'ECRAN, pas dans le monde.
      screenAnchor = position;

      // L'entree se fait par la droite : on anime un DECALAGE, pas une position
      // absolue, pour que l'ancrage sur la camera reste vrai pendant le glissement.
      slide = 320f;

      wiggler = Wiggler.Create(20, 5f, null, null, false, false);
      Add(wiggler);
    }

    // ------------------------------------------------------------------
    // Fabriques de lignes
    // ------------------------------------------------------------------

    public static Field Bool(string label, Func<bool> get, Action<bool> set)
    {
      return new Field
      {
        Label = label,
        Value = () => get() ? "ON" : "OFF",
        Change = _ => set(!get())
      };
    }

    public static Field Int(string label, Func<int> get, Action<int> set, int min, int max)
    {
      return new Field
      {
        Label = label,
        Value = () => get().ToString(),
        Change = delta => set(Math.Clamp(get() + delta, min, max))
      };
    }

    /// <summary>Une liste de libelles, que les fleches font tourner en bouclant.</summary>
    public static Field Choice(string label, string[] values, Func<string> get, Action<string> set)
    {
      return new Field
      {
        Label = label,
        Value = get,
        Change = delta =>
        {
          int at = Array.IndexOf(values, get());
          if (at < 0)
          {
            at = 0;
          }

          set(values[((at + delta) % values.Length + values.Length) % values.Length]);
        }
      };
    }

    // ------------------------------------------------------------------

    private float PanelHeight => TopPadding + Visible().Count * RowStep + BottomPadding;

    private List<Field> Visible()
    {
      var shown = new List<Field>();

      foreach (Field field in fields)
      {
        if (field.Visible == null || field.Visible())
        {
          shown.Add(field);
        }
      }

      return shown;
    }

    /// <summary>
    /// Recale la fenetre sur la camera.
    ///
    /// Elle vit sur la couche -1, celle qui porte une camera, et l'ecran des variantes
    /// FAIT DEFILER cette camera. Une position absolue restait donc la ou la liste se
    /// trouvait a l'ouverture : des qu'on avait descendu dans la liste, la fenetre se
    /// dessinait au-dessus du champ visible - creee, ajoutee, rendue, et invisible.
    ///
    /// On la repose donc a chaque image, en coordonnees d'ECRAN.
    /// </summary>
    /// <summary>
    /// Le texte tel qu'on l'affiche : en majuscules, comme tout le reste des menus du
    /// jeu.
    ///
    /// La mise en forme est faite ICI et jamais sur la valeur enregistree. Les
    /// reglages sont compares a des chaines exactes - `periodicity == "Normal"`, un
    /// identifiant de pouvoir - et les mettre en majuscules a la source casserait ces
    /// comparaisons sans qu'on voie pourquoi.
    /// </summary>
    private static string Caps(string text)
    {
      return string.IsNullOrEmpty(text) ? text : text.ToUpperInvariant();
    }

    private void Anchor()
    {
      float scroll = 0f;

      if (MainMenu != null && MainMenu.UILayer != null && MainMenu.UILayer.Camera != null)
      {
        scroll = MainMenu.UILayer.Camera.Y;
      }

      // Le DEFILEMENT seulement, donc uniquement en Y. La camera porte aussi le
      // recentrage horizontal de l'interface - WiderSet cale les 320 de large dans
      // 420 - et l'ajouter en X le comptait une seconde fois : la fenetre partait
      // cinquante pixels a gauche du milieu. Une abscisse absolue est deja centree,
      // c'est ainsi que tout le menu du jeu est ecrit.
      Position = new Vector2(screenAnchor.X + slide, scroll + screenAnchor.Y);
    }

    public override void Added()
    {
      base.Added();

      Anchor();

      // Le bouton retour doit refermer la FENETRE et non quitter l'ecran des
      // variantes : c'est ce que fait le jeu pour sa fenetre des joueurs.
      MainMenu.BackState = MainMenu.MenuState.Variants;
    }

    public override void Removed()
    {
      MainMenu.BackState = MainMenu.MenuState.VersusOptions;
      base.Removed();
    }

    public override void Update()
    {
      base.Update();
      Anchor();

      if (!Selected)
      {
        return;
      }

      List<Field> shown = Visible();

      if (shown.Count == 0)
      {
        return;
      }

      selected = Math.Clamp(selected, 0, shown.Count - 1);

      if (MenuInput.Up && selected > 0)
      {
        selected--;
        Sounds.ui_move1.Play(160f, 1f);
        return;
      }

      if (MenuInput.Down && selected < shown.Count - 1)
      {
        selected++;
        Sounds.ui_move1.Play(160f, 1f);
        return;
      }

      if (MenuInput.Left)
      {
        shown[selected].Change(-1);
        wiggler.Start();
        Sounds.ui_move1.Play(160f, 1f);
        return;
      }

      if (MenuInput.Right)
      {
        shown[selected].Change(1);
        wiggler.Start();
        Sounds.ui_move1.Play(160f, 1f);
        return;
      }

      // Confirmer ferme aussi : une fois les reglages poses, c'est le geste naturel,
      // et il n'y a rien d'autre a valider ici.
      if (MenuInput.Back || MenuInput.Alt2 || MenuInput.Confirm)
      {
        TweenOut();
        Sounds.ui_clickBack.Play(160f, 1f);
      }
    }

    public override void Render()
    {
      base.Render();
      Anchor();

      float height = PanelHeight;
      MenuPanel.DrawPanel(X - PanelWidth / 2f, Y - height / 2f, PanelWidth, height);

      Draw.TextCentered(TFGame.Font, Caps(title), Position + new Vector2(0f, -height / 2f + 10f), Color.White);

      List<Field> shown = Visible();
      float y = Y - height / 2f + TopPadding;

      for (int i = 0; i < shown.Count; i++)
      {
        bool active = i == selected;
        Color color = active ? VariantItem.ActiveSelection : Color.Gray;

        Draw.OutlineTextJustify(TFGame.Font, Caps(shown[i].Label),
            new Vector2(X - PanelWidth / 2f + 10f, y), color, Color.Black, new Vector2(0f, 0.5f), 1f);

        // La valeur est cadree a droite : les lignes ne font pas la meme longueur, et
        // une colonne alignee se lit d'un coup d'oeil.
        string value = Caps(shown[i].Value());
        float scale = active ? 1f + wiggler.Value * 0.15f : 1f;

        Draw.OutlineTextJustify(TFGame.Font, value,
            new Vector2(X + PanelWidth / 2f - 10f, y), color, Color.Black,
            new Vector2(1f, 0.5f), scale);

        y += RowStep;
      }
    }

    public override void TweenIn()
    {
      Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeOut, 20, true);
      tween.OnUpdate = t => slide = MathHelper.Lerp(320f, 0f, t.Eased);
      tween.OnComplete = t => Selected = true;
      Add(tween);
    }

    public override void TweenOut()
    {
      Selected = false;

      // Le reglage part sur le disque en refermant, et pas seulement en quittant le
      // jeu : les ModuleSettings ne sont ecrits qu'a la sortie de l'ecran des options.
      TFModFortRiseVariantMoveOrDieModule.SaveSettingsNow();

      Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeOut, 12, true);
      tween.OnUpdate = t => slide = MathHelper.Lerp(0f, 320f, t.Eased);
      tween.OnComplete = t =>
      {
        // Rendre la main a la case d'ou l'on vient, sinon l'ecran des variantes n'a
        // plus rien de selectionne et ne repond plus.
        toggle.Selected = true;
        RemoveSelf();
      };
      Add(tween);
    }

    protected override void OnSelect() { }

    protected override void OnDeselect() { }

    protected override void OnConfirm() { }
  }
}

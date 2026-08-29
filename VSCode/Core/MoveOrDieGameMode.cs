using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie
{
  /// <summary>
  /// Le MODE de jeu : la meme regle, mais sans avoir a cocher quoi que ce soit.
  ///
  /// **Il vit dans CE mod et non dans un mod a lui**, et c'est le point qui compte. La
  /// regle - s'arreter tue - tient en quarante lignes qui existent deja ici. Un mod
  /// separe aurait eu deux facons de la satisfaire : recopier ces quarante lignes, qui
  /// se seraient mises a diverger a la premiere retouche, ou demander a celui-ci de
  /// s'allumer par interop, ce qui est beaucoup de tuyauterie pour un booleen.
  ///
  /// Un mode et une variante ne sont pas deux fonctionnalites, ce sont deux PORTES vers
  /// la meme : la variante pour l'ajouter a une partie ordinaire, le mode pour en faire
  /// le sujet de la soiree.
  ///
  /// **Il n'a pas de logique de manche a lui.** La regle tue les archers, et c'est le
  /// dernier survivant qui gagne - exactement ce que la logique du jeu sait deja faire.
  /// En ecrire une n'aurait ajoute que des occasions de se tromper.
  /// </summary>
  public class MoveOrDieGameMode : IVersusGameMode, IRegisterable
  {
    private static ISubtextureEntry icon = null!;

    public static IVersusGameModeEntry Entry { get; private set; } = null!;

    public string Name => "Move or Die";
    public Color NameColor => Color.Salmon;
    public ISubtextureEntry Icon => icon;
    public bool IsTeamMode => false;

    public static void Register(IModContent content, IModRegistry registry)
    {
      icon = registry.Subtextures.RegisterTexture(
          content.Root.GetRelativePath("Content/Atlas/gamemode.png"));

      Entry = registry.GameModes.RegisterVersusGameMode(new MoveOrDieGameMode());
    }

    public void OnStartGame(Session session)
    {
    }

    public RoundLogic OnCreateRoundLogic(Session session)
    {
      return new LastManStandingRoundLogic(session);
    }

    /// <summary>
    /// Vrai quand la partie en cours est une partie de ce mode.
    ///
    /// Lu par MyPlayer au meme titre que la variante et que le reglage : ce sont trois
    /// facons d'allumer la meme regle, et une seule ligne les reunit.
    /// </summary>
    public static bool IsActive(Session session)
    {
      return session?.MatchSettings != null
          && Entry != null
          && session.MatchSettings.Mode == Entry.Modes;
    }
  }
}

using FortRise;

namespace TFModFortRiseVariantMoveOrDie
{
  public class Variants : IRegisterable
  {
    /// <summary>
    /// Le libelle de la case, et le seul repere pour la reconnaitre dans l'ecran des
    /// variantes : VariantToggle n'expose pas l'identifiant du mod.
    /// </summary>
    public const string TITLE = "MoveOrDie";

    public static IVariantEntry MoveOrDie = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
      MoveOrDie = registry.Variants.RegisterVariant("MoveOrDie", new()
      {
        // Header commun a tous mes mods : sans lui FortRise retombe sur le nom du
        // mod et chacun cree sa propre colonne dans l'ecran des variantes.
        Header = "EBE1 MODS",
        Title = TITLE,
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.MoveOrDie
      });
    }
  }
}

using FortRise;

namespace TFModFortRiseVariantMoveOrDie
{
  public class Variants : IRegisterable
  {
    public static IVariantEntry MoveOrDie = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
      MoveOrDie = registry.Variants.RegisterVariant("MoveOrDie", new()
      {
        // Header commun a tous mes mods : sans lui FortRise retombe sur le nom du
        // mod et chacun cree sa propre colonne dans l'ecran des variantes.
        Header = "EBE1 MODS",
        Title = "MoveOrDie",
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.MoveOrDie
      });
    }
  }
}

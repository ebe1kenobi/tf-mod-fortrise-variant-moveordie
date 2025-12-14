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
        Title = "MoveOrDie",
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.MoveOrDie
      });
    }
  }
}

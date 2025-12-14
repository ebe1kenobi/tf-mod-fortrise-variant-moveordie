using FortRise;
using Monocle;
using TowerFall;

namespace TFModFortRiseVariantMoveOrDie;

// Optional way to use textures
public class TextureRegistry : IRegisterable
{
    // Variants
    public static ISubtextureEntry MoveOrDie { get; private set; } = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
    MoveOrDie = registry.Subtextures.RegisterTexture(
                content.Root.GetRelativePath("Content/Atlas/moveordie.png")
            );
    }
}
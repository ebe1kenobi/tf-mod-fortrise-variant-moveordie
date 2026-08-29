using System;
using System.Collections.Generic;
using System.Net;
using FortRise;
using HarmonyLib;
using FortRise;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;
using static TowerFall.Player;

namespace TFModFortRiseVariantMoveOrDie
{
  public class MyPlayer : IHookable
  {
    //private const float MIN_SPEED = 0.1f; // Minimum speed to be considered moving
    private const float FRAME_PER_SECOND = 60f; 

    public static Dictionary<int, float> stationaryTimers = new Dictionary<int, float>();

    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredConstructor(typeof(Player), [
            typeof(int),
            typeof(Vector2),
            typeof(Allegiance),
            typeof(Allegiance),
            typeof(PlayerInventory),
            typeof(HatStates),
            typeof(bool),
            typeof(bool),
            typeof(bool)
          ]),
          postfix: new HarmonyMethod(ctor_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), nameof(Player.Update)),
          postfix: new HarmonyMethod(Update)
      );
    }

    public static void ctor_patch(Player __instance, int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, global::TowerFall.PlayerInventory inventory, global::TowerFall.Player.HatStates hatState, bool frozen, bool flash, bool indicator)
    {
      stationaryTimers[playerIndex] = 0f;
    }

    public static void Update(Player __instance)
    {
      // Trois portes vers la meme regle : la variante cochee, le reglage global, ou le
      // MODE de jeu. Une seule ligne les reunit, et le reste du fichier n'a pas a savoir
      // par laquelle on est entre.
      if (!Variants.MoveOrDie.IsActive()
          && !TFModFortRiseVariantMoveOrDieModule.Settings.activated
          && !MoveOrDieGameMode.IsActive(__instance.Level?.Session)) return;
      if (__instance.State == PlayerStates.Frozen) return;
      if (__instance.State == PlayerStates.Dying) return;
      if (__instance.State == PlayerStates.LedgeGrab) return;
      // Check if player is moving

      if (__instance.Speed.LengthSquared() < (TFModFortRiseVariantMoveOrDieModule.Settings.minSpeed) / (10 * TFModFortRiseVariantMoveOrDieModule.Settings.minSpeed / 10))
      {
        stationaryTimers[__instance.PlayerIndex] += Engine.TimeMult;

        // Flash the player when they're getting close to death
        if (stationaryTimers[__instance.PlayerIndex] >= TFModFortRiseVariantMoveOrDieModule.Settings.stationaryDeathTime * FRAME_PER_SECOND * 0.5f)
        {
          __instance.Flash(2, null);
        }

        // Kill player if they've been stationary too long
        if (stationaryTimers[__instance.PlayerIndex] >= TFModFortRiseVariantMoveOrDieModule.Settings.stationaryDeathTime * FRAME_PER_SECOND)
        {
          __instance.Die(DeathCause.Curse, -1, false, false);
          stationaryTimers[__instance.PlayerIndex] = 0f;
        }
      }
      else
      {
        stationaryTimers[__instance.PlayerIndex] = 0f;
      }
    }
  }
}

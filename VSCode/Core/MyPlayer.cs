using System;
using System.Collections.Generic;
using System.Net;
using Monocle;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;
using static TowerFall.Player;

namespace TFModFortRiseVariantMoveOrDie
{
  public class MyPlayer
  {
    //private const float MIN_SPEED = 0.1f; // Minimum speed to be considered moving
    private const float FRAME_PER_SECOND = 60f; 

    public static Dictionary<int, float> stationaryTimers = new Dictionary<int, float>();
    internal static void Load()
    {
      On.TowerFall.Player.ctor += ctor_patch;
      On.TowerFall.Player.Update += Update;
    }

    internal static void Unload()
    {
      On.TowerFall.Player.ctor += ctor_patch;
      On.TowerFall.Player.Update -= Update;
    }


    public static void ctor_patch(On.TowerFall.Player.orig_ctor orig, TowerFall.Player self, int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, global::TowerFall.PlayerInventory inventory, global::TowerFall.Player.HatStates hatState, bool frozen, bool flash, bool indicator)
    {
      orig(self, playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator);
      stationaryTimers[playerIndex] = 0f;
    }

    public static void Update(On.TowerFall.Player.orig_Update orig, global::TowerFall.Player self)
    {
      orig(self);
      if (!VariantManager.GetCustomVariant("MoveOrDie") && !TFModFortRiseVariantMoveOrDieModule.Settings.activated) return;
      if (self.State == PlayerStates.Frozen) return;
      if (self.State == PlayerStates.Dying) return;
      if (self.State == PlayerStates.LedgeGrab) return;
      // Check if player is moving

      if (self.Speed.LengthSquared() < (TFModFortRiseVariantMoveOrDieModule.Settings.minSpeed) / (10 * TFModFortRiseVariantMoveOrDieModule.Settings.minSpeed / 10))
      {
        stationaryTimers[self.PlayerIndex] += Engine.TimeMult;

        // Flash the player when they're getting close to death
        if (stationaryTimers[self.PlayerIndex] >= TFModFortRiseVariantMoveOrDieModule.Settings.stationaryDeathTime * FRAME_PER_SECOND * 0.5f)
        {
          self.Flash(2, null);
        }

        // Kill player if they've been stationary too long
        if (stationaryTimers[self.PlayerIndex] >= TFModFortRiseVariantMoveOrDieModule.Settings.stationaryDeathTime * FRAME_PER_SECOND)
        {
          self.Die(DeathCause.Curse, -1, false, false);
          stationaryTimers[self.PlayerIndex] = 0f;
        }
      }
      else
      {
        stationaryTimers[self.PlayerIndex] = 0f;
      }
    }
  }
}

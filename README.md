Skeleton of a project to Create a Mod AI for the mod "Loader AI" https://github.com/ebe1kenobi/tf-mod-fortrise-loader-ai

You just need to implements the method **AIExAgent.Move();**. All the Level information are in the property "TowerFall.Level level" in parent class Agent. Exemple in https://github.com/ebe1kenobi/tf-mod-fortrise-ai-simple

**requirements**: mod "Loader AI" https://github.com/ebe1kenobi/tf-mod-fortrise-loader-ai

```C#
using TowerFall;
using System;

namespace TFModFortRiseAiExample
{
  public class AIExAgent : TFModFortRiseLoaderAI.Agent
  {

    public AIExAgent(int index, String type, PlayerInput input) : base(index, type, input)
    {
    }

    protected override void Move()
    {
      this.input.inputState = new InputState();
      this.input.inputState.AimAxis.X = 0;
      this.input.inputState.MoveX = 0;
      this.input.inputState.AimAxis.Y = 0;
      this.input.inputState.MoveY = 0;

      if (shoot.Count == 0 && 0 == random.Next(0, 19))
      //if (0 == random.Next(0, 19))
      {
        this.input.inputState.JumpCheck = true;
        this.input.inputState.JumpPressed = !this.input.prevInputState.JumpCheck;
      }

      this.input.prevInputState = this.input.GetCopy(this.input.inputState);

    }
  }
}

```

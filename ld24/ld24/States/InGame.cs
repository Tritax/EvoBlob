using System;

using Microsoft.Xna.Framework.Graphics;

namespace ld24.States
{
   class InGame : StateBase
   {
      public override void Init(GraphicsDevice dev)
      {
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         return GameStates.InGame;
      }

      public override void Draw(GraphicsDevice dev)
      {
         base.Draw(dev);
      }
   }
}

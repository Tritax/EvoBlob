using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class InstructState : StateBase
   {
      private SpriteBatch _batch;
      private Texture2D _tex;

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _tex = g.Content.Load<Texture2D>("instruct");
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         if (IsButtonPress(Buttons.Back) || IsKeyPressed(Keys.Escape))
            return GameStates.Menu;

         return GameStates.Instruct;
      }

      public override void Draw(GraphicsDevice dev)
      {
         _batch.Begin();
         _batch.Draw(_tex, new Rectangle(0, 0, dev.Viewport.Width, dev.Viewport.Height), Color.White);
         _batch.End();
      }
   }
}

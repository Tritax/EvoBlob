using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class MenuState : StateBase
   {
      private SpriteBatch _batch;
      private Texture2D _titleTex;

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _titleTex = g.Content.Load<Texture2D>("evoblob_title");
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         if (IsButtonDown(Buttons.Start) || IsKeyPressed(Keys.Enter))
            return GameStates.InGame;

         return GameStates.Menu;
      }

      public override void Draw(GraphicsDevice dev)
      {
         base.Draw(dev);

         _batch.Begin();

         Rectangle rc = new Rectangle(0, 0, dev.Viewport.Width, dev.Viewport.Height);
         _batch.Draw(_titleTex, rc, Color.White);

         _batch.End();
      }
   }
}

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class AboutState : StateBase
   {
      private SpriteBatch _batch;
      private SpriteFont _miramonte;
      private Texture2D _black;

      private List<string> _credits = new List<string>();

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _miramonte = g.Content.Load<SpriteFont>("Miramonte");

         _black = new Texture2D(g.GraphicsDevice, 1, 1);
         _black.SetData<Color>(new Color[] { Color.Black });

         _credits.Add("EvoBlob was originally created as a part of LudumDare 24 by Team Bunnell.");
         _credits.Add("");
         _credits.Add("Team Bunnell is: Tritax (Dad), KBear (Mom), CDBeaners (Teen), Jimmy (Toddler)");
         _credits.Add("");
         _credits.Add("Game Design: Jimmy, KBear, CDBeaners");
         _credits.Add("Graphics: CDBeaners");
         _credits.Add("Level Design: KBear, CDBeaners");
         _credits.Add("Code: Tritax");
         _credits.Add("");
         _credits.Add("If interested, you may contact us at: BunnellGames@gmail.com  or  @JDBunnell (Twitter)");
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         if (IsButtonPress(Buttons.Back) || IsKeyPressed(Keys.Escape))
            return GameStates.Menu;

         return GameStates.About;
      }

      public override void Draw(GraphicsDevice dev)
      {
         base.Draw(dev);

         _batch.Begin();
         _batch.Draw(_black, new Rectangle(0, 0, dev.Viewport.Width, dev.Viewport.Height), Color.White);

         Vector2 pos = new Vector2();
         foreach (String str in _credits)
         {
            _batch.DrawString(_miramonte, str, pos, Color.White); pos.Y += _miramonte.LineSpacing + 2;
         }

         _batch.End();
      }
   }
}

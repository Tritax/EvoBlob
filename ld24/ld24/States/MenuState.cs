using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ld24.States
{
   class MenuState : StateBase
   {
      private SpriteBatch _batch;
      private SpriteFont _miramonte;
      private Texture2D _titleTex;
      private SoundEffect _navSound;
      private SoundEffect _goSound;

      private bool _canNav = true;
      private int _selected = 0;
      private const int MAX_SELECT = 3;

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _miramonte = g.Content.Load<SpriteFont>("Miramonte");
         _titleTex = g.Content.Load<Texture2D>("evoblob_title");
         _navSound = g.Content.Load<SoundEffect>("select");
         _goSound = g.Content.Load<SoundEffect>("go");
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         if (IsButtonDown(Buttons.A) || IsKeyPressed(Keys.Enter))
         {
            _goSound.Play();

            switch (_selected)
            { 
               default:
               case 2:
                  return GameStates.About;
               case 0:
                  return GameStates.InGame;
               case 1:
                  return GameStates.Instruct;
               case 3:
                  return GameStates.Quit;
            };
         }

         Vector2 nav = GetNavVector();
         if (_canNav)
         {
            if (nav.Y > 0)
            {
               _navSound.Play();

               _selected--;
               if (_selected < 0)
                  _selected = 0;

               _canNav = false;
            }
            else if (nav.Y < 0)
            {
               _navSound.Play();

               _selected++;
               if (_selected > MAX_SELECT)
                  _selected = MAX_SELECT;

               _canNav = false;
            }
         }
         else if (nav.Y == 0)
            _canNav = true;

         return GameStates.Menu;
      }

      public override void Draw(GraphicsDevice dev)
      {
         base.Draw(dev);

         _batch.Begin();

         Rectangle rc = new Rectangle(0, 0, dev.Viewport.Width, dev.Viewport.Height);
         _batch.Draw(_titleTex, rc, Color.White);

         Vector2 pos = new Vector2();
         _batch.DrawString(_miramonte, "Start Game", pos, _selected == 0 ? Color.Yellow : Color.White); pos.Y += _miramonte.LineSpacing + 2;
         _batch.DrawString(_miramonte, "Instructions", pos, _selected == 1 ? Color.Yellow : Color.White); pos.Y += _miramonte.LineSpacing + 2;
         _batch.DrawString(_miramonte, "Credits", pos, _selected == 2 ? Color.Yellow : Color.White); pos.Y += _miramonte.LineSpacing + 2;
         _batch.DrawString(_miramonte, "Quit", pos, _selected == 3 ? Color.Yellow : Color.White); pos.Y += _miramonte.LineSpacing + 2;

         _batch.End();
      }
   }
}

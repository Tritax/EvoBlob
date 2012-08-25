using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class InGame : StateBase
   {
      public const int TILE_SIZE = 32;
      public const int MAX_EVOLVE = 1;

      private SpriteBatch _batch;

      private Texture2D _blobRoll;
      private Texture2D _blobWalk;
      private Texture2D _blobJump;
      private Texture2D _sky;
      private Texture2D _tileSet;
      private Texture2D _goo;

      private Rectangle _skyBox;
      private int _halfWidth = 0;

      private double _accum;
      private int _frame;

      private int _evolutionTier = 0;
      private int _jump = 0;

      private List<Data.Particle> _particleList;

      public InGame()
      {
         _particleList = new List<Data.Particle>();
      }

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _skyBox = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
         _halfWidth = g.GraphicsDevice.Viewport.Width / 2;

         _tileSet = g.Content.Load<Texture2D>("base_tileset");
         _sky = g.Content.Load<Texture2D>("sky");
         _blobRoll = g.Content.Load<Texture2D>("blob");
         _blobWalk = g.Content.Load<Texture2D>("blob_walk");
         _blobJump = g.Content.Load<Texture2D>("blob_jump");
         _goo = g.Content.Load<Texture2D>("goo");
      }

      public override void Uninit()
      {
      }

      protected override GameStates OnUpdate(double dt)
      {
         _accum += dt;
         if (_accum > .25)
         {
            _accum = 0;
            _frame++;
            if (_frame > 2)
               _frame = 0;

            if (Game1.Player.Moved)
            {
               SpawnGoo(5, 4, 8, .75f);
            }
         }

         UpdateMovement(dt);
         if (Game1.Player.Scroll)
         {
            _skyBox.X += (Game1.Player.MovedLeft ? 1 : -1);
            if (Math.Abs(_skyBox.X) >= _skyBox.Width)
               _skyBox.X = 0;
         }

         if (IsButtonDown(Buttons.B))
         {
            SpawnGoo(5, 6, 16, 2);
         }

         if (IsButtonDown(Buttons.Y))
         {
            _evolutionTier++;
            if (_evolutionTier > MAX_EVOLVE)
               _evolutionTier = 0;
         }

         for (int i = _particleList.Count - 1; i >= 0; i--)
         {
            _particleList[i].Update(dt);
            if (_particleList[i].IsDead)
               _particleList.RemoveAt(i);
         }

         return GameStates.InGame;
      }

      private void UpdateMovement(double dt)
      {
         Rectangle bounds = Game1.Player.GetBounds();
         Vector2 move = base.GetMoveVector();

         if (IsButtonDown(Buttons.A) && _jump == 0)
            _jump = Data.Player.SCROLL_FRAMES;

         if (_jump != 0)
         {
            if (_jump > 0)
            {
               move.Y = -1;
               _jump--;
               if (_jump == 0)
                  _jump = -Data.Player.SCROLL_FRAMES;
            }
            else
            {
               move.Y = 1;
               _jump++;
            }
         }

         Game1.Player.ApplyMovementVector(move);
      }

      private void SpawnGoo(int num, int minSize, int maxSize, float maxAge)
      {
         Vector2 pos = Game1.Player.GetPos();
         float ang;
         Random rnd = new Random();
         for (int i = 0; i < num; i++)
         {
            ang = (float)(rnd.NextDouble() * Math.PI);
            Data.Particle p = new Data.Particle(rnd.NextDouble() * maxAge, minSize + rnd.Next(maxSize));
            p.SetPosition((int)pos.X + 16, (int)pos.Y + 16);
            p.SetDirection((int)(Math.Cos(ang) * 2), (int)(-Math.Sin(ang) * 2));
            _particleList.Add(p);
         }
      }

      public override void Draw(GraphicsDevice dev)
      {
         base.Draw(dev);

         _batch.Begin();
         DrawBackground();
         DrawLevel();
         DrawPlayer();
         _batch.End();
      }

      private void DrawBackground()
      {
         _batch.Draw(_sky, _skyBox, Color.White);
         if (_skyBox.X != 0)
         {
            Rectangle rc = _skyBox;
            rc.X += (_skyBox.X < 0 ? _skyBox.Width : -_skyBox.Width);

            _batch.Draw(_sky, rc, Color.White);
         }
      }

      private void DrawLevel()
      {
      }

      private void DrawPlayer()
      {
         float scale = 1f;
         SpriteEffects eff = Game1.Player.MovedLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
         Rectangle src = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);                  
         if (Game1.Player.Moved)
            src.X = (_frame * TILE_SIZE);

         Texture2D tex;
         switch (_evolutionTier)
         {
            default: tex = _blobRoll; break;
            case 1: tex = _blobWalk; break;
         };

         _batch.Draw(tex, Game1.Player.GetPos() + new Vector2(0, 300), src, Color.White, 0f, Vector2.Zero, scale, eff, 0f);

         src.X = 0;
         foreach (Data.Particle p in _particleList)
         {
            scale = (float)p.Size / 32f;
            _batch.Draw(_goo, p.GetPosition() + new Vector2(0, 300), src, Color.White * p.GetFade(), 0f, new Vector2(16, 16), scale, SpriteEffects.None, 0f);
         }
      }
   }
}

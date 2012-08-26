using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class InGame : StateBase
   {
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
      private Vector2 _offset;

      private double _accum;
      private int _frame;

      private int _evolutionTier = 0;
      private int _jump = 0;

      private List<Data.Particle> _particleList;

      private Data.Level _level;

      public InGame()
      {
         _particleList = new List<Data.Particle>();
      }

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _skyBox = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
         _halfWidth = g.GraphicsDevice.Viewport.Width / 2;

         _tileSet = g.Content.Load<Texture2D>("grassy_tileset");
         _sky = g.Content.Load<Texture2D>("sky");
         _blobRoll = g.Content.Load<Texture2D>("bwblob");
         _blobWalk = g.Content.Load<Texture2D>("blob_walk");
         _blobJump = g.Content.Load<Texture2D>("blob_jump");
         _goo = g.Content.Load<Texture2D>("bwgoo");

         string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dat\\example3.level");
         _level = Data.Level.FromFile(filePath);
         if (_level != null)
         {
            Game1.Player.SetPosition(_level.GetStartPosition() * Game1.TILE_SIZE);
         }
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

         Point pt = Game1.Player.GetTilePos();
         if (_level.GetAt(pt.X, pt.Y).Flags > 0)
         {
            bool death = false;
            switch (_level.GetAt(pt.X, pt.Y).Flags)
            {
               default: 
               case Data.Tile.FLAG_START_POS:
                  break;

               case Data.Tile.FLAG_WIN_POS:
                  break;

               case Data.Tile.FLAG_DEATH:
                  death = true;
                  break;

               case Data.Tile.FLAG_DROWN:
                  death = true;
                  break;

               case Data.Tile.FLAG_SPIKE:
                  death = true;
                  break;
            };

            if (death)
            {
               _offset = Vector2.Zero;
               SpawnGoo(5, 6, 16, 2);
               Game1.Player.SetPosition(_level.GetStartPosition() * Game1.TILE_SIZE);
            }
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

      private bool IsCollision(int x, int y)
      {
         int tx = (int)(x / Game1.TILE_SIZE);
         int ty = (int)(y / Game1.TILE_SIZE);

         if (!_level.CanWalkAt(tx, ty))
            return true;

         return false;
      }
      
      private void UpdateMovement(double dt)
      {
         Rectangle bounds = Game1.Player.GetBounds();
         Vector2 move = base.GetMoveVector();
         int x, y, a = Game1.TILE_SIZE - 1;
         Vector2 pos = Game1.Player.GetPos();

         if (IsButtonDown(Buttons.A) && _jump == 0 && !Game1.Player.Falling)
         {
            Game1.Player.SetFalling(true);
            _jump = Data.Player.SCROLL_FRAMES;
         }

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
                  
         if (move.X != 0)
         {
            Rectangle r = bounds;
            r.Offset((int)(move.X * Data.Player.MAX_WALK_SPEED), 0);

            if (IsCollision(r.Left, r.Top + Game1.TILE_SIZE) || IsCollision(r.Right, r.Top + Game1.TILE_SIZE) /*||
                IsCollision(test + new Vector2(0, a)) ||
                IsCollision(test + new Vector2(a, a))*/
               )
            {
               move.X = 0;
            }
         }

         if (_jump == 0)
         {
            move.Y = 1;
            Game1.Player.SetFalling(true);
         }

         if (move.Y > 0)
         {
            Rectangle r = bounds;
            r.Offset(0, (int)(move.Y * Data.Player.MAX_WALK_SPEED));

            if (IsCollision(r.Left, r.Bottom) || IsCollision(r.Right, r.Bottom))
            {
               move.Y = 0;
               Game1.Player.SetFalling(false);
            }
         }

         Game1.Player.ApplyMovementVector(move);
         if (Game1.Player.GetPos().X > _halfWidth)
         {
            int lvlw = _level.GetWidth() * Game1.TILE_SIZE;
            int rem = lvlw - (int)Game1.Player.GetPos().X; 

            int n = (int)(move.X * Data.Player.MAX_WALK_SPEED);
            if (n > 0)
            {
               // moving right // check offset against remaining level width
               if (_offset.X + n < rem)
               {
                  _offset.X -= n;
                  if (_halfWidth + rem < _skyBox.Width)
                     _offset.X += n;
               }
            }
            else if (n < 0)
            {
               // moving left // check offset against 0
               if (_offset.X < 0)
               {
                  _offset.X -= n;
                  if (_halfWidth + rem < _skyBox.Width)
                     _offset.X += n;
                  if (_offset.X > 0)
                     _offset.X = 0;
               }
            }
         }
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
         _level.Draw(_batch, _offset, _tileSet);
      }

      private void DrawPlayer()
      {
         Vector2 pos = Game1.Player.GetPos();
         
         float scale = 1f;
         SpriteEffects eff = Game1.Player.MovedLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
         Rectangle src = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);                  
         if (Game1.Player.Moved)
            src.X = (_frame * Game1.TILE_SIZE);

         Texture2D tex;
         switch (_evolutionTier)
         {
            default: tex = _blobRoll; break;
            case 1: tex = _blobWalk; break;
         };

         _batch.Draw(tex, Game1.Player.GetPos() + _offset, src, Color.Green, 0f, Vector2.Zero, scale, eff, 0f);

         src.X = 0;
         foreach (Data.Particle p in _particleList)
         {
            scale = (float)p.Size / 32f;
            _batch.Draw(_goo, p.GetPosition() + _offset, src, Color.Green * p.GetFade(), 0f, new Vector2(16, 16), scale, SpriteEffects.None, 0f);
         }
      }
   }
}

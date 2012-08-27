using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class InGame : StateBase
   {
      public const double WIN_SPAWN_TIME = 2.0;
      public const double SUICIDE_TIME = 2.5;
      public const double WIN_LAPSE = 1f;

      private SpriteBatch _batch;

      private Texture2D _debugTex;

      private Texture2D _blobIdle;
      private Texture2D _blobRoll;
      private Texture2D _blobEat;
      private Texture2D _blobWalk;
      private Texture2D _blobJump;
      private Texture2D _blobSpit;
      private Texture2D _blobGlide;
      private Texture2D _blobClimbG;
      private Texture2D _blobClimbC;
      private Texture2D _blobClimbU;
      private Texture2D _blobClimbD;
      private Texture2D _blobSwim;
      private Texture2D _sky;
      private Texture2D _tileSet;
      private Texture2D _goo;
      private Texture2D _frog;
      private Texture2D _spikey;
      private Texture2D _proj;
      private Texture2D _stalag;
      private Texture2D _fish;
      private Texture2D _spider;
      private Texture2D _bat;

      private Rectangle _skyBox;
      private int _halfWidth = 0;
      private int _halfHeight = 0;
      private Vector2 _offset;

      private double _accum;
      private int _frame;

      private double _eAccum;
      private int _eFrame;

      private double _winSpawner = 0;
      private double _winTimer = 0;
      private double _suicideTimer = 0;
      private Vector2 _vSuicide = Vector2.Zero;

      private int _evolutionTier = 0;
      private int _jump = 0;
      private bool _attacking = false;
      private bool _attached = false;

      private List<Data.Particle> _particleList;
      private List<Data.Projectile> _projList;

      private int _currentLevel = 0;
      private List<string> _levelList;
      private Data.Level _level;
      private bool _winner = false;

      public InGame()
      {
         _levelList = new List<string>();
         _particleList = new List<Data.Particle>();
         _projList = new List<Data.Projectile>();
      }

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _skyBox = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
         _halfWidth = g.GraphicsDevice.Viewport.Width / 2;
         _halfHeight = g.GraphicsDevice.Viewport.Height / 2;

#if DEBUG
         _debugTex = new Texture2D(g.GraphicsDevice, 1, 1);
         _debugTex.SetData<Color>(new Color[] { Color.White });
#endif
         _tileSet = g.Content.Load<Texture2D>("grassy_tileset");
         _sky = g.Content.Load<Texture2D>("sky");
         _blobIdle = g.Content.Load<Texture2D>("bwblob_idle");
         _blobRoll = g.Content.Load<Texture2D>("bwblob");
         _blobEat = g.Content.Load<Texture2D>("bwblob_eat");
         _blobWalk = g.Content.Load<Texture2D>("blob_walk");
         _blobJump = g.Content.Load<Texture2D>("blob_jump");
         _blobSpit = g.Content.Load<Texture2D>("bwblob_spit");
         _blobGlide = g.Content.Load<Texture2D>("bwblob_glide");
         _blobClimbG = g.Content.Load<Texture2D>("bwblob_climb-ground");
         _blobClimbC = g.Content.Load<Texture2D>("bwblob_climb-ceiling");
         _blobClimbU = g.Content.Load<Texture2D>("bwblob_climb-wallup");
         _blobClimbD = g.Content.Load<Texture2D>("bwblob_climb-walldn");
         _blobSwim = g.Content.Load<Texture2D>("bwblob_swim");
         _goo = g.Content.Load<Texture2D>("bwgoo");
         _frog = g.Content.Load<Texture2D>("frog");
         _spikey = g.Content.Load<Texture2D>("spikey");
         _proj = g.Content.Load<Texture2D>("projectile");
         _stalag = g.Content.Load<Texture2D>("stalagmite");
         _fish = g.Content.Load<Texture2D>("fish");
         _spider = g.Content.Load<Texture2D>("spider");
         _bat = g.Content.Load<Texture2D>("bat");

         ReadLevelList(); 
         LoadLevel();
      }

      public override void Uninit()
      {
      }

      private void ReadLevelList()
      {
         string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dat\\level.dat");
         if (File.Exists(filePath))
         {
            using (StreamReader sr = new StreamReader(filePath))
            {
               string line;
               while ((line = sr.ReadLine()) != null)
               {
                  line = line.Trim();
                  if (line.Length == 0 || line.StartsWith(";"))
                     continue;

                  _levelList.Add(line);
               }
            }
         }
      }

      private void ResetFlags()
      {
         _winner = false;
         _accum = 0;
         _frame = 0;
         _eAccum = 0;
         _eFrame = 0;
         _winSpawner = 0;
         _winTimer = 0;
         _evolutionTier = 0;
         _suicideTimer = 0;
         _jump = 0; 
         _attacking = false;
         _offset = Vector2.Zero;
      }

      private void LoadLevel()
      {
         ResetFlags();
         if (_currentLevel >= _levelList.Count)
            return;

         string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dat\\" +  _levelList[_currentLevel] + ".level");
         _level = Data.Level.FromFile(filePath);
         if (_level != null)
         {
            Game1.Player.SetPosition(_level.GetStartPosition() * Game1.TILE_SIZE);
            CalculateOffsetForLevelStart();
         }
      }

      private void CalculateOffsetForLevelStart()
      {
         _offset = Vector2.Zero;
         if (_level == null)
            return;

         Vector2 pos = _level.GetStartPosition() * Game1.TILE_SIZE;
         if (pos.X > _halfWidth)
         {
            int lvlw = _level.GetWidth() * Game1.TILE_SIZE;
            int rem = lvlw - (int)pos.X;

            if (_halfWidth + rem < _skyBox.Width)
               _offset.X = -(lvlw - _halfWidth);
            else 
               _offset.X = -(pos.X - _halfWidth);
         }

         if (pos.Y > _halfHeight)
         {
            int lvlh = _level.GetHeight() * Game1.TILE_SIZE;
            int rem = lvlh - (int)pos.Y;

            if (_halfHeight + rem < _skyBox.Height)
               _offset.Y = -(lvlh - _halfHeight);
            else
               _offset.Y = -(pos.Y - _halfHeight);
         }
      }

      private void UpdateTimers(double dt)
      {
         _accum += dt;
         if (_accum > .25)
         {
            _accum = 0;
            _frame++;
            if (_frame > 2)
            {
               _frame = 0;
               _attacking = false;
            }

            if (Game1.Player.Moved && !Game1.Player.Falling)
            {
               SpawnGoo(Game1.Player.GetPos(), 5, 4, 8, .75f, DeterminePlayerColor());
            }
         }

         _eAccum += dt;
         if (_eAccum > .25)
         {
            _eAccum = 0;
            _eFrame++;
            if (_eFrame > 2)
               _eFrame = 0;
         }

         _winSpawner += dt;
         if (_winSpawner > WIN_SPAWN_TIME)
         {
            _winSpawner = 0;
            SpawnGoo(_level.GetWinPos() * Game1.TILE_SIZE, 5, 6, 16, 2, Color.Yellow);
         }

         if (_winner)
         {
            _winTimer += dt;
            if (_winTimer > WIN_LAPSE)
            {
               _currentLevel++;
               if (_currentLevel < _levelList.Count)
                  LoadLevel();
            }
         }
      }

      protected override GameStates OnUpdate(double dt)
      {
         if (IsButtonPress(Buttons.Back) || IsKeyPressed(Keys.Escape))
            return GameStates.Menu;

         UpdateTimers(dt);
         if (_currentLevel >= _levelList.Count && _winTimer > WIN_LAPSE)
            return GameStates.Quit;

         UpdateProjectiles(dt);
         UpdateMovement(dt);
         if (Game1.Player.Scroll)
         {
            _skyBox.X += (Game1.Player.MovedLeft ? 1 : -1);
            if (Math.Abs(_skyBox.X) >= _skyBox.Width)
               _skyBox.X = 0;
         }

         if (_level.CheckEnemyCollide())
         {
            Die();
         }
         else
         {
            Data.Powerup up = _level.CheckPowerupCollide();
            if (up != null && _attacking)
            {
               _evolutionTier = up.GetType();
               _level.RemovePowerup(up);
               if (_evolutionTier == Data.Powerup.WIN_EVOLVE)
                  _winner = true;
            }
         }

         UpdateControls(dt);
         CheckForDeath();
         UpdateSuicide(dt);
         UpdateParticles(dt);

         return GameStates.InGame;
      }

      private void UpdateProjectiles(double dt)
      {
         double age = 0;
         for (int i = _projList.Count - 1; i >= 0; i--)
         {
            _projList[i].Update(dt);

            age = _projList[i].GetAge();
            Data.Badguy guy = _level.CheckEnemyCollideWith(_projList[i].GetBounds());
            if (guy != null)
            {
               age = Data.Projectile.MAX_AGE;
               _level.RemoveEnemy(guy);
            }
            else
            {
               Point pt = _projList[i].GetTilePos();
               Data.Tile tile = _level.GetAt(pt.X, pt.Y);
               if (tile != null && !tile.Passable)
                  age = Data.Projectile.MAX_AGE;
            }

            if (age >= Data.Projectile.MAX_AGE)
            {
               _projList.RemoveAt(i);
            }
         }
      }

      private void UpdateParticles(double dt)
      {
         for (int i = _particleList.Count - 1; i >= 0; i--)
         {
            _particleList[i].Update(dt);
            if (_particleList[i].IsDead)
               _particleList.RemoveAt(i);
         }
      }

      private void UpdateSuicide(double dt)
      {
         if (IsButtonPress(Buttons.Y) || IsKeyPressed(Keys.A))
            _suicideTimer += dt;

         if (_suicideTimer > 0 && (IsButtonDown(Buttons.Y) || IsKeyDown(Keys.A)))
         {
            _vSuicide.X = (_vSuicide.X < 0 ? 2 : -2);
            _suicideTimer += dt;
            if (_suicideTimer > SUICIDE_TIME)
               Die();
         }
         else
         {
            _suicideTimer = 0;
            _vSuicide.X = 0;
         }
      }

      private void CheckForDeath()
      {
         Point pt = Game1.Player.GetTilePos();
         if (_level.GetAt(pt.X, pt.Y).Flags > 0)
         {
            bool death = false;
            switch (_level.GetAt(pt.X, pt.Y).Flags)
            {
               default:
               case Data.Tile.FLAG_START_POS:
               case Data.Tile.FLAG_WIN_POS:
                  // Nothing
                  break;

               case Data.Tile.FLAG_DEATH:
                  death = true;
                  break;

               case Data.Tile.FLAG_DROWN:
                  if (_evolutionTier != Data.Powerup.FISH_EVOLVE)
                     death = true;
                  break;

               case Data.Tile.FLAG_SPIKE:
                  death = true;
                  break;
            };

            if (death)
            {
               Die();
            }
         }
      }

      private void Die()
      {
         LoadLevel();
         CalculateOffsetForLevelStart();
         SpawnGoo(Game1.Player.GetPos(), 5, 6, 16, 2, DeterminePlayerColor());
         Game1.Player.SetPosition(_level.GetStartPosition() * Game1.TILE_SIZE);
      }

      private bool IsCollision(int x, int y)
      {
         int tx = (int)(x / Game1.TILE_SIZE);
         int ty = (int)(y / Game1.TILE_SIZE);

         if (!_level.CanWalkAt(tx, ty))
            return true;

         return false;
      }

      private bool IsClimbable(int x, int y)
      {
         int tx = (int)(x / Game1.TILE_SIZE);
         int ty = (int)(y / Game1.TILE_SIZE);

         if (_level.GetAt(tx, ty).Flags == Data.Tile.FLAG_CLIMBABLE)
            return true;

         return false;
      }

      private void UpdateControls(double dt)
      {
         if (_winner)
            return;

         bool attackBtn = IsButtonPress(Buttons.X) || IsKeyPressed(Keys.X);
         if (attackBtn)
         {
            _attacking = true;
            _frame = 0;
            _accum = 0;

            if (_evolutionTier == Data.Powerup.ROCK_EVOLVE)
            {
               Data.Projectile p = new Data.Projectile();
               Vector2 pos = Game1.Player.GetPos();
               if (Game1.Player.MovedLeft)
                  pos.X -= Game1.TILE_SIZE;
               else 
                  pos.X += Game1.TILE_SIZE;

               p.SetPosition(pos);
               p.SetMoveLeft(Game1.Player.MovedLeft);

               _projList.Add(p);
            }
         }
      }
      
      private void UpdateMovement(double dt)
      {
         Rectangle bounds = Game1.Player.GetBounds();
         Vector2 move = base.GetMoveVector();
         if (_winner)
            move.X = 0;

         int x, y, a = Game1.TILE_SIZE - 1;
         Vector2 pos = Game1.Player.GetPos();

         bool jumpBtn = IsButtonPress(Buttons.A) || IsKeyPressed(Keys.Z);
         if (_attached && jumpBtn)
         {
            _attached = false;
            _jump = -Data.Player.SCROLL_FRAMES;
         }
         else if (jumpBtn && _jump == 0 && !Game1.Player.Falling)
         {
            Game1.Player.SetFalling(true);
            _jump = Data.Player.SCROLL_FRAMES;
         }

         if (_jump != 0)
         {
            if (_jump > 0)
            {
               move.Y = -1;
               if (_evolutionTier == Data.Powerup.FROG_EVOLVE)
                  move.Y *= Data.Powerup.FROG_JMP_MUL;

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

            if (IsCollision(r.Left, r.Top) || IsCollision(r.Right, r.Top))
            {
               move.X = 0;
            }

            if (IsCollision(r.Left, r.Bottom) || IsCollision(r.Right, r.Bottom))
            {
               move.X = 0;
            }
         }

         if (_jump == 0)
         {
            if (_attached)
            {
               Rectangle r = bounds;
               r.Offset(0, (int)(-1 * Data.Player.MAX_WALK_SPEED));

               _attached = IsClimbable(r.Left, r.Top) || IsClimbable(r.Right, r.Top);
            }

            if (!_attached)
            {
               move.Y = _evolutionTier == Data.Powerup.BAT_EVOLVE ? 0.5f : 1f;
               Game1.Player.SetFalling(true);
            }
         }

         if (move.Y != 0)
         {
            Rectangle r = bounds;
            r.Offset(0, (int)(move.Y * Data.Player.MAX_WALK_SPEED));

            if (IsCollision(r.Left, r.Top) || IsCollision(r.Right, r.Top))
            {
               move.Y = 0;
               if (IsClimbable(r.Left, r.Top) || IsClimbable(r.Right, r.Top))
               {
                  _jump = 0;
                  _attached = true;
               }
               else
               {
                  _attached = false;
                  _jump = -Data.Player.SCROLL_FRAMES;
               }
            }

            if (IsCollision(r.Left, r.Bottom) || IsCollision(r.Right, r.Bottom))
            {
               move.Y = 0;
               Game1.Player.SetFalling(false);
            }
         }

         Point pt = Game1.Player.ApplyMovementVector(move);
         CheckForScrolling(pt.X, pt.Y);
      }

      private void CheckForScrolling(int dx, int dy)
      {
         Vector2 playerPos = Game1.Player.GetPos();
         if ((_offset.X != 0 || playerPos.X + _offset.X > _halfWidth) && dx != 0)
         {
            int lvlw = _level.GetWidth() * Game1.TILE_SIZE;
            int rem = lvlw - (int)playerPos.X;

            if (dx > 0)
            {
               // moving right // check offset against remaining level width
               if (_offset.X + dx < rem)
               {
                  _offset.X -= dx;
                  if (_halfWidth + rem < _skyBox.Width)
                     _offset.X += dx;
               }
            }
            else if (dx < 0)
            {
               // moving left // check offset against 0
               if (_offset.X < 0)
               {
                  _offset.X -= dx;
                  if (_offset.X > 0)
                     _offset.X = 0;
               }
            }
         }

         if ((_offset.Y != 0 || playerPos.Y + _offset.Y > _halfHeight) && dy != 0)
         {
            int lvlh = _level.GetHeight() * Game1.TILE_SIZE;
            int edge = lvlh - _halfHeight;
            int rem = lvlh - (int)playerPos.Y;

            if (dy > 0)
            {
               // moving down // check offset against remaining level height
               if (_offset.Y + dy < edge)
               {
                  _offset.Y -= dy;
                  if (_offset.Y - _halfHeight < -lvlh)
                     _offset.Y = -(lvlh - _halfHeight);
               }
            }
            else if (dy < 0)
            {
               // moving up // check offset against 0
               if (_offset.Y < 0)
               {
                  _offset.Y -= dy;
                  if (_offset.Y > 0)
                     _offset.Y = 0;
               }
            }
         }
      }

      private void SpawnGoo(Vector2 pos, int num, int minSize, int maxSize, float maxAge, Color clr)
      {
         float ang;
         Random rnd = new Random();
         for (int i = 0; i < num; i++)
         {
            ang = (float)(rnd.NextDouble() * Math.PI);
            Data.Particle p = new Data.Particle(rnd.NextDouble() * maxAge, minSize + rnd.Next(maxSize), clr);
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
         DrawProjectiles();
         _batch.End();
      }

      private void DrawProjectiles()
      {
         Vector2 pos;
         Rectangle src = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
         SpriteEffects eff = SpriteEffects.None;
         foreach (Data.Projectile p in _projList)
         {
            eff = p.MovedLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            pos = p.GetPos();
            _batch.Draw(_proj, pos + _offset, src, Color.White, 0f, Vector2.Zero, 1f, eff, 0f);
         }
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
         Vector2 pos = Vector2.Zero;
         Rectangle src = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
         Data.Tile tile;

         for (int y = 0; y < _level.GetHeight(); y++)
         {
            pos.Y = y * Game1.TILE_SIZE;
            for (int x = 0; x < _level.GetWidth(); x++)
            {
               tile = _level.GetAt(x, y);
               if (tile != null)
               {
                  pos.X = x * Game1.TILE_SIZE;

                  src.X = (tile.Gfx % 8) * Game1.TILE_SIZE;
                  src.Y = (tile.Gfx / 8) * Game1.TILE_SIZE;

                  _batch.Draw(_tileSet, _offset + pos, src, Color.White);

                  Texture2D decor = null;
                  Color clr = Color.White;

                  switch (tile.Flags)
                  {
                     default: break;
                     case Data.Tile.FLAG_WIN_POS:
                        decor = _stalag;
                        clr = Color.Yellow;
                        break;
                     case Data.Tile.FLAG_SPIKEY:
                        decor = _spikey;
                        break;
                     case Data.Tile.FLAG_FROG:
                        decor = _frog;
                        break;
                     case Data.Tile.FLAG_STALAGMITE:
                        decor = _stalag;
                        break;
                     case Data.Tile.FLAG_FISH:
                        decor = _fish;
                        break;
                     case Data.Tile.FLAG_SPIDER:
                        decor = _spider;
                        break;
                     case Data.Tile.FLAG_BAT:
                        decor = _bat;
                        break;
                  };

                  if (decor != null)
                  {
                     src.X = (_eFrame * Game1.TILE_SIZE);
                     src.Y = 0;

                     _batch.Draw(decor, _offset + pos, src, clr);
                  }
               }
            }
         }
      }

      private Color DeterminePlayerColor()
      {
         Color clr = Color.Blue;
         switch (_evolutionTier)
         {
            default: break;
            case Data.Powerup.FROG_EVOLVE:
               clr = Color.Green; break;
            case Data.Powerup.ROCK_EVOLVE:
               clr = Color.Gray; break;
            case Data.Powerup.WIN_EVOLVE:
               clr = Color.Yellow; break;
         };

         return clr;
      }

      private void DrawPlayer()
      {
         Vector2 pos = Game1.Player.GetPos();
         
         float scale = 1f;
         SpriteEffects eff = Game1.Player.MovedLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
         Rectangle src = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);                  
         
         Texture2D tex;
         switch (_evolutionTier)
         {
            default:
               tex = _blobIdle;
               if (Game1.Player.Moved)
                  tex = _blobRoll;
               
               if (_attacking)
                  tex = _evolutionTier == Data.Powerup.ROCK_EVOLVE ? _blobSpit : _blobEat;
               
               break;
            case Data.Powerup.CHICKEN_EVOLVE: tex = _blobWalk; break;
            case Data.Powerup.FISH_EVOLVE: tex = _blobSwim; break;
            case Data.Powerup.SPIDER_EVOLVE: tex = _attached ? _blobClimbC : _blobClimbG; break;
            case Data.Powerup.BAT_EVOLVE: tex = _blobGlide; break;
         };

         src.X = (_frame * Game1.TILE_SIZE);
#if DEBUG
         Rectangle rc = Game1.Player.GetBounds();
         rc.X += (int)_offset.X;
         rc.Y += (int)_offset.Y;

         _batch.Draw(_debugTex, rc, Color.White * 0.25f);
#endif 
         _batch.Draw(tex, Game1.Player.GetPos() + _offset + _vSuicide, src, DeterminePlayerColor(), 0f, Vector2.Zero, scale, eff, 0f);

         src.X = 0;
         foreach (Data.Particle p in _particleList)
         {
            scale = (float)p.Size / 32f;
            _batch.Draw(_goo, p.GetPosition() + _offset, src, p.GetColor() * p.GetFade(), 0f, new Vector2(16, 16), scale, SpriteEffects.None, 0f);
         }
      }
   }
}

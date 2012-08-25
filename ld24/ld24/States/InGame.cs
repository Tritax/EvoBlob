using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ld24.States
{
   class InGame : StateBase
   {
      public const int TILE_SIZE = 32;

      private SpriteBatch _batch;

      private Texture2D _blobRoll;
      private Texture2D _blobWalk;
      private Texture2D _sky;
      private Texture2D _tileSet;

      private Rectangle _skyBox;
      private int _halfWidth = 0;

      private double _accum;
      private int _frame;

      private int _evolutionTier = 0;

      public override void Init(Game1 g)
      {
         _batch = new SpriteBatch(g.GraphicsDevice);
         _skyBox = new Rectangle(0, 0, g.GraphicsDevice.Viewport.Width, g.GraphicsDevice.Viewport.Height);
         _halfWidth = g.GraphicsDevice.Viewport.Width / 2;

         _tileSet = g.Content.Load<Texture2D>("base_tileset");
         _sky = g.Content.Load<Texture2D>("sky");
         _blobRoll = g.Content.Load<Texture2D>("blob");
         _blobWalk = g.Content.Load<Texture2D>("blob_walk");
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
         }

         UpdateMovement(dt);
         if (Game1.Player.Moved)
         {
            _skyBox.X += (Game1.Player.MovedLeft ? 1 : -1);
            if (Math.Abs(_skyBox.X) >= _skyBox.Width)
               _skyBox.X = 0;
         }

         if (IsButtonDown(Buttons.Y))
            _evolutionTier++;

         return GameStates.InGame;
      }

      private void UpdateMovement(double dt)
      {
         Rectangle bounds = Game1.Player.GetBounds();
         Vector2 move = base.GetMoveVector();

         Game1.Player.ApplyMovementVector(move);
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
         Vector2 scale = Vector2.One;
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
      }
   }
}

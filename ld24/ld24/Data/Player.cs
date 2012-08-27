using System;
using Microsoft.Xna.Framework;

namespace ld24.Data
{
   class Player
   {
      public const int BOUNDS_X = 4;
      public const int BOUNDS_Y = 8;
      public const int BOUNDS_W = 28;
      public const int BOUNDS_H = 24;

      public const int MAX_WALK_SPEED = 3;
      public const int SCROLL_FRAMES = 16;

      private Rectangle _bounds;
      private bool _scroll = false;
      private bool _moved = false;
      private bool _left = true;
      private bool _falling = true;

      public Player()
      {
         _bounds = new Rectangle(BOUNDS_X, BOUNDS_Y, BOUNDS_W, BOUNDS_H);
      }

      public Vector2 GetPos()
      {
         return new Vector2(_bounds.X, _bounds.Y);
      }

      public Rectangle GetBounds()
      {
         return _bounds;
      }

      public bool Falling { get { return _falling; } }
      public bool Moved { get { return _moved; } }
      public bool MovedLeft { get { return _left; } }
      public bool Scroll { get { return _scroll; } }

      public void SetPosition(Vector2 pos)
      {
         SetPosition(pos.X, pos.Y);
      }

      public void SetPosition(float x, float y)
      {
         _bounds.X = (int)x + BOUNDS_X;
         _bounds.Y = (int)y + BOUNDS_Y;
      }

      public void SetFalling(bool b)
      {
         _falling = b;
      }

      public Point GetTilePos()
      {
         Point pos = new Point();
         pos.X = (int)((_bounds.X + 16) / Game1.TILE_SIZE);
         pos.Y = (int)((_bounds.Y + 16) / Game1.TILE_SIZE);

         return pos;
      }

      public Point ApplyMovementVector(Vector2 move)
      {
         Point pt = Point.Zero;

         _scroll = false;
         _moved = false;
         if (move.X == 0 && move.Y == 0)
            return pt;

         pt.X = (int)(move.X * MAX_WALK_SPEED);
         pt.Y = (int)(move.Y * MAX_WALK_SPEED);
         if (pt.X == 0 && pt.Y == 0)
            return pt;

         _bounds.Offset(pt.X, pt.Y);
         _moved = true;
         _scroll = move.X != 0;
         _left = (move.X < 0);
         return pt;
      }
   }
}

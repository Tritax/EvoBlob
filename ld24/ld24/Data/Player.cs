using System;
using Microsoft.Xna.Framework;

namespace ld24.Data
{
   class Player
   {
      public const int BOUNDS_W = 32;
      public const int BOUNDS_H = 32;

      public const int MAX_WALK_SPEED = 2;
      public const int SCROLL_FRAMES = 20;

      private Rectangle _bounds;
      private bool _scroll = false;
      private bool _moved = false;
      private bool _left = true;

      public Player()
      {
         _bounds = new Rectangle(0, 0, BOUNDS_W, BOUNDS_H);
      }

      public Vector2 GetPos()
      {
         return new Vector2(_bounds.X, _bounds.Y);
      }

      public Rectangle GetBounds()
      {
         return _bounds;
      }

      public bool Moved { get { return _moved; } }
      public bool MovedLeft { get { return _left; } }
      public bool Scroll { get { return _scroll; } }
      
      public void ApplyMovementVector(Vector2 move)
      {
         _scroll = false;
         _moved = false;
         if (move.X == 0 && move.Y == 0)
            return;

         int x = (int)(move.X * MAX_WALK_SPEED);
         int y = (int)(move.Y * MAX_WALK_SPEED);

         _bounds.Offset(x, y);
         _moved = true;
         _scroll = move.X != 0;
         _left = (move.X < 0);
      }
   }
}

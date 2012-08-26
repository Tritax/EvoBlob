using System;
using Microsoft.Xna.Framework;

namespace ld24.Data
{
   class Powerup
   {      
      private Rectangle _bounds;

      public Powerup()
      {
         _bounds = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
      }

      public void SetPosition(Vector2 pos)
      {
         _bounds.X = (int)pos.X;
         _bounds.Y = (int)pos.Y;
      }

      public void SetPosition(float x, float y)
      {
         _bounds.X = (int)x;
         _bounds.Y = (int)y;
      }

      public Vector2 GetPos()
      {
         return new Vector2(_bounds.X, _bounds.Y);
      }

      public Rectangle GetBounds()
      {
         return _bounds;
      }

      public Point GetTilePos()
      {
         Point pos = new Point();
         pos.X = (int)((_bounds.X + 16) / Game1.TILE_SIZE);
         pos.Y = (int)((_bounds.Y + 16) / Game1.TILE_SIZE);

         return pos;
      }
   }
}

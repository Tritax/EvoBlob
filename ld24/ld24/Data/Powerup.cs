using System;
using Microsoft.Xna.Framework;

namespace ld24.Data
{
   public class Powerup
   {
      public const int MAX_EVOLVE = 3;

      public const int BLOB_EVOLVE = 0;
      public const int CHICKEN_EVOLVE = 1;
      public const int FROG_EVOLVE = 2;
      public const int ROCK_EVOLVE = 3;
      
      public const float FROG_JMP_MUL = 1.75f;

      private Rectangle _bounds;
      private int _type;

      public Powerup()
      {
         _bounds = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
         _type = BLOB_EVOLVE;
      }

      public void SetType(int type)
      {
         _type = type;
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

      public int GetType()
      {
         return _type;
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

using System;
using Microsoft.Xna.Framework;

namespace ld24.Data
{
   class Projectile
   {
      public const double MAX_AGE = 2;
      public const int MAX_SPEED = 5;

      private Rectangle _bounds;
      private bool _left = false;
      private double _age = 0;

      public Projectile()
      {
         _bounds = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);
      }

      public bool MovedLeft { get { return _left; } }

      public double GetAge()
      {
         return _age;
      }

      public Vector2 GetPos()
      {
         return new Vector2(_bounds.X, _bounds.Y);
      }

      public Point GetTilePos()
      {
         Point pos = new Point();
         pos.X = (int)((_bounds.X + 16) / Game1.TILE_SIZE);
         pos.Y = (int)((_bounds.Y + 16) / Game1.TILE_SIZE);

         return pos;
      }

      public Rectangle GetBounds()
      {
         return _bounds;
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

      public void SetMoveLeft(bool left)
      {
         _left = left;
      }

      public void Update(double dt)
      {
         _age += dt;
         _bounds.Offset(_left ? -MAX_SPEED : MAX_SPEED, 0);
      }
   }
}

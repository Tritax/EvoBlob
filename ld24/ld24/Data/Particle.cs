using System;

using Microsoft.Xna.Framework;

namespace ld24.Data
{
   class Particle
   {
      private double _age;
      private double _fadeAt;
      private double _max;

      private int _type;
      private int _size;

      private Vector2 _pos;
      private Vector2 _dir;

      public Particle(double max, int size)
      {
         _age = 0;
         _fadeAt = max * 0.75;
         _max = max;
         _type = 0;
         _size = size;
      }

      public Vector2 GetPosition()
      {
         return _pos;
      }

      public float GetFade()
      {
         if (_age < _fadeAt)
            return 1f;

         return 1f - (float)((_age - _fadeAt) / (_max - _fadeAt));
      }

      public void SetDirection(int x, int y)
      {
         _dir.X = x;
         _dir.Y = y;
      }

      public void SetPosition(int x, int y)
      {
         _pos.X = x;
         _pos.Y = y;
      }

      public void AdjustPosition(int x, int y)
      {
         _pos.X += x;
         _pos.Y += y;
      }

      public void Update(double dt)
      {
         _age += dt;
         _pos += _dir;
      }

      public bool IsDead { get { return _age >= _max; } }
      public int Type { get { return _type; } }
      public int Size { get { return _size; } }
   }
}

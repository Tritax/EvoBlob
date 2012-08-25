using System;

namespace ld24.Data
{
   class Level
   {
      private int _width;
      private int _height;

      private byte _tileset;
      private byte[] _tiles;

      public Level(int w, int h)
      {
         _width = w;
         _height = h;
         Initialize();
      }

      private void Initialize()
      {
         int len = _width * _height;

         _tiles = new byte[len];
         for (int i = 0; i < len; i++)
            _tiles[i] = 0;
      }

      public int GetWidth() { return _width; }
      public int GetHeight() { return _height; }
   }
}

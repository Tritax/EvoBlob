using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ld24.Data
{
   public class Tile
   {
      public const byte FLAG_START_POS = 1;
      public const byte FLAG_DEATH = 2;
      public const byte FLAG_WIN_POS = 3;
      public const byte FLAG_SPIKE = 4;
      public const byte FLAG_DROWN = 5;

      public byte Gfx { get; set; }
      public bool Passable { get; set; }
      public byte Flags { get; set; }
   }

   public class Level
   {
      private int _width;
      private int _height;

      private byte _tileset;
      private Tile[,] _tiles;

      private Vector2 _startPos = new Vector2(-1, -1);

      protected Level()
      {
      }

      public Level(int w, int h)
      {
         _width = w;
         _height = h;

         Initialize();
      }

      private void Initialize()
      {
         _tiles = new Tile[_width, _height];
         for (int i = 0; i < _width; i++)
         {
            for (int j = 0; j < _height; j++)
            {
               _tiles[i, j] = new Tile();
               _tiles[i, j].Passable = true;
            }
         }
      }

      public int GetWidth() { return _width; }
      public int GetHeight() { return _height; }
      public Vector2 GetStartPosition() { return _startPos; }

      public Tile GetAt(int x, int y)
      {
         if (x < 0 || x >= _width || y < 0 || y >= _height)
            return null;

         return _tiles[x, y];
      }

      public bool CanWalkAt(int x, int y)
      {
         if (x < 0 || x >= _width || y < 0 || y >= _height)
            return false;

         return _tiles[x, y].Passable;
      }

      private bool Compile()
      {
         for (int y = 0; y < _height; y++)
         {
            for (int x = 0; x < _width; x++)
            {
               if (_tiles[x, y].Flags == Tile.FLAG_START_POS)
                  _startPos = new Vector2(x, y);
            }
         }

         if (_startPos.X == -1 || _startPos.Y == -1)
            return false;

         return true;
      }

      public void Draw(SpriteBatch sb, Texture2D tileset)
      {
         Vector2 pos = Vector2.Zero;
         Rectangle src = new Rectangle(0, 0, Game1.TILE_SIZE, Game1.TILE_SIZE);

         for (int y = 0; y < _height; y++)
         {
            pos.Y = y * Game1.TILE_SIZE;
            for (int x = 0; x < _width; x++)
            {
               pos.X = x * Game1.TILE_SIZE;

               src.X = (_tiles[x, y].Gfx % 8) * Game1.TILE_SIZE;
               src.Y = (_tiles[x, y].Gfx / 8) * Game1.TILE_SIZE;

               sb.Draw(tileset, pos, src, Color.White);
            }
         }
      }

      public void Save(string filePath)
      {
         using (StreamWriter sw = new StreamWriter(filePath))
         {
            sw.WriteLine("[width]" + _width);
            sw.WriteLine("[height]" + _height);
            sw.WriteLine("[data]");
            sw.WriteLine("[tiles]");
            for (int y = 0; y < _height; y++)
            {
               for (int x = 0; x < _width; x++)
               {
                  sw.Write((char)(_tiles[x, y].Gfx + 32));
               }

               sw.Write("\r\n");
            }
            sw.WriteLine("[obstruct]");
            for (int y = 0; y < _height; y++)
            {
               for (int x = 0; x < _width; x++)
               {
                  sw.Write((char)(_tiles[x, y].Passable ? ' ' : '#'));
               }

               sw.Write("\r\n");
            }
            sw.WriteLine("[special]");
            for (int y = 0; y < _height; y++)
            {
               for (int x = 0; x < _width; x++)
               {
                  sw.Write((char)(_tiles[x, y].Flags + 32));
               }

               sw.Write("\r\n");
            }
         }
      }



      public static Level FromFile(string filePath)
      {
         Level lvl = new Level();
         if (!File.Exists(filePath))
            return null;

         using (StreamReader sr = new StreamReader(filePath))
         {
            bool dataSegment = false;
            int lineNum = 0, dataStep = 0, n = 0, y = 0;
            string line, key = "", val = "";
            while ((line = sr.ReadLine()) != null)
            {
               lineNum++;
               if (line.StartsWith(";"))
                  continue;

               if (line.StartsWith("["))
               {
                  int s = line.IndexOf("]");
                  if (s != -1)
                  {
                     key = line.Substring(1, s - 1);
                     if (line.Length > s + 1)
                        val = line.Substring(s + 1).Trim();
                     else
                        val = "";
                     
                     switch (key)
                     {
                        default:
                           Console.WriteLine("unknown flag in level data: " + key + " on line " + lineNum);
                           break;
                        case "width":
                           if (dataSegment)
                           {
                              Console.WriteLine("width property defined in data segment - line " + lineNum);
                              return null;
                           }

                           if (!int.TryParse(val, out n))
                           {
                              Console.WriteLine("invalid value for width - line " + lineNum);
                              return null;
                           }
                           
                           lvl._width = n;
                           break;

                        case "height":
                           if (dataSegment)
                           {
                              Console.WriteLine("height property defined in data segment - line " + lineNum);
                              return null;
                           }

                           if (!int.TryParse(val, out n))
                           {
                              Console.WriteLine("invalid value for height - line " + lineNum);
                              return null;
                           }
                           
                           lvl._height = n;
                           break;

                        case "data":
                           if (lvl._width <= 0 || lvl._height <= 0)
                           {
                              Console.WriteLine("invalid width/height for data init - line " + lineNum);
                              return null;
                           }

                           dataSegment = true;
                           lvl.Initialize();
                           break;

                        case "tiles":
                           dataStep = 1;
                           y = 0;
                           break;

                        case "obstruct":
                           dataStep = 2;
                           y = 0;
                           break;

                        case "special":
                           dataStep = 3;
                           y = 0;
                           break;

                        case "eof":
                           break;
                     };
                  }

                  continue;
               }

               if (!dataSegment)
               {
                  Console.WriteLine("invalid level format (attempt to process data segment without [data] declare) - line " + lineNum);
                  return null;
               }

               if (line.Length < lvl._width)
               {
                  Console.WriteLine("insufficient level data - line " + lineNum);
                  return null;
               }

               switch (dataStep)
               {
                  default:
                     Console.WriteLine("invalid data step - line " + lineNum);
                     return null;
                  case 1:  // tiles
                     for (int i = 0; i < lvl._width; i++)
                        lvl._tiles[i, y].Gfx = (byte)(line[i] - 32);
                     
                     y++;
                     break;
                  case 2:  // obstruction
                     for (int i = 0; i < lvl._width; i++)
                        lvl._tiles[i, y].Passable = line[i] != '#';

                     y++;
                     break;
                  case 3:  // special info
                     for (int i = 0; i < lvl._width; i++)
                        lvl._tiles[i, y].Flags = (byte)(line[i] - 32);

                     y++;
                     break;
               };
            }
         }

         Console.WriteLine("Successfully loaded level: " + Path.GetFileNameWithoutExtension(filePath));
         if (!lvl.Compile())
         {
            Console.WriteLine("Unable to compile level!");
            return null;
         }

         return lvl;
      }
   }
}

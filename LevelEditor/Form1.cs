using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
   public partial class Form1 : Form
   {
      public const int TILE_SIZE = 32;

      Dictionary<String, Bitmap> _tilesetMap = new Dictionary<String, Bitmap>();
      Bitmap _buffer;
      Point _offset;
      ld24.Data.Level _level;

      String _selectedTileset;
      String _selectedBg;
      Point _selectedTile;

      bool _showGrids = true;

      public Form1()
      {
         InitializeComponent();

         InitializeTilesets();

         vScrollBar.Minimum = 1; vScrollBar.Maximum = 1;
         hScrollBar.Minimum = 1; hScrollBar.Maximum = 1;
      }

      private void InitializeTilesets()
      {
         String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tilesets");
         foreach (string file in Directory.GetFiles(path, "*.png"))
         {
            Bitmap src = (Bitmap)Bitmap.FromFile(file);
            //Bitmap dst = new Bitmap(128, 512);
            //Graphics g = Graphics.FromImage(dst);
            //Rectangle rc = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);

            //int x = 0, y = 0;
            //for (int j = 0; j < 8; j++)
            //{
            //   for (int i = 0; i < 8; i++)
            //   {
            //      rc.X = i * TILE_SIZE;
            //      rc.Y = j * TILE_SIZE;

            //      g.DrawImage(src, x, y, rc, GraphicsUnit.Pixel);
            //      x += TILE_SIZE;
            //      if (x >= dst.Width)
            //      {
            //         x = 0;
            //         y += TILE_SIZE;
            //      }
            //   }

            //   break;
            //}

            _tilesetMap.Add(Path.GetFileNameWithoutExtension(file), src);
         }

         tilesetOptions.Items.Clear();
         foreach (String key in _tilesetMap.Keys)
         {
            tilesetOptions.Items.Add(key);
         }
      }

      private void DrawLevelPreview()
      {
         int imgWidth = _level.GetWidth() * TILE_SIZE;
         int imgHeight = _level.GetHeight() * TILE_SIZE;

         SolidBrush red = new SolidBrush(Color.FromArgb(64, Color.Red));
         SolidBrush grn = new SolidBrush(Color.FromArgb(64, Color.Green));

         _buffer = new Bitmap(imgWidth, imgHeight);
         Graphics g = Graphics.FromImage(_buffer);
         g.FillRectangle(Brushes.White, 0, 0, imgWidth, imgHeight);
         if (!String.IsNullOrEmpty(_selectedTileset))
         {
            //int bw = _buffer.Width / TILE_SIZE;
            //int bh = _buffer.Height / TILE_SIZE;

            int w = _level.GetWidth();
            int h = _level.GetHeight();
            
            Rectangle src = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            Rectangle dst = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE);
            ld24.Data.Tile tile;
            Image img = _tilesetMap[_selectedTileset];
            for (int y = 0; y < _level.GetHeight(); y++)
            {
               for (int x = 0; x < _level.GetWidth(); x++)
               {
                  tile = _level.GetAt(x, y);
                  if (tile != null)
                  {
                     dst.X = x * TILE_SIZE;
                     dst.Y = y * TILE_SIZE;

                     src.X = (tile.Gfx % 8) * TILE_SIZE;
                     src.Y = (tile.Gfx / 8) * TILE_SIZE;

                     g.DrawImage(img, dst, src, GraphicsUnit.Pixel);
                     g.FillRectangle(tile.Passable ? grn : red, dst);
                  }
               }
            }
         }

         if (_showGrids)
         {
            for (int x = 0; x < _buffer.Width; x += TILE_SIZE)
            {
               g.DrawLine(Pens.Black, new Point(x, 0), new Point(x, _buffer.Height));
            }

            for (int y = 0; y < _buffer.Height; y += TILE_SIZE)
            {
               g.DrawLine(Pens.Black, new Point(0, y), new Point(_buffer.Width, y));
            }
         }

         int xWidth = levelView.Width;
         int xHeight = levelView.Height;

         g = levelView.CreateGraphics();
         g.DrawImage(_buffer,
            new Rectangle(0, 0, xWidth, xHeight),
            new Rectangle(hScrollBar.Value, vScrollBar.Value, xWidth, xHeight), 
            GraphicsUnit.Pixel);
      }

      private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
      {
         _offset.Y = e.NewValue - vScrollBar.Minimum;
         DrawLevelPreview();
      }

      private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
      {
         _offset.X = e.NewValue - hScrollBar.Minimum;
         DrawLevelPreview();
      }

      private void newToolStripMenuItem_Click(object sender, EventArgs e)
      {
         NewLevelDialog dlg = new NewLevelDialog();
         if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            int w = 0, h = 0;
            if (int.TryParse(dlg.Width, out w) && int.TryParse(dlg.Height, out h))
            {
               _level = new ld24.Data.Level(w, h);

               vScrollBar.Minimum = 0;
               vScrollBar.Maximum = (_level.GetHeight() * TILE_SIZE) - levelView.Height;

               hScrollBar.Minimum = 0;
               hScrollBar.Maximum = (_level.GetWidth() * TILE_SIZE) - levelView.Width;

               DrawLevelPreview();
            }
         }
      }

      private void tilesetOptions_SelectedIndexChanged(object sender, EventArgs e)
      {
         String key = tilesetOptions.Items[tilesetOptions.SelectedIndex].ToString();
         if (_tilesetMap.ContainsKey(key))
         {
            _selectedTileset = key;
            toolBox.Image = _tilesetMap[key];
         }
      }

      private void toolBox_MouseClick(object sender, MouseEventArgs e)
      {
         int x = e.X / TILE_SIZE;
         int y = e.Y / TILE_SIZE;

         _selectedTile = new Point(x, y);
      }

      private void levelView_MouseClick(object sender, MouseEventArgs e)
      {
         int x = (hScrollBar.Value + e.X) / TILE_SIZE;
         int y = (vScrollBar.Value + e.Y) / TILE_SIZE;

         ld24.Data.Tile tile = _level.GetAt(x, y);
         if (tile != null)
         {
            switch (modeOptions.SelectedIndex)
            {
               default: break;
               case 0:  // paint tiles
                  tile.Gfx = (byte)((_selectedTile.Y * 8) + _selectedTile.X);
                  break;
               case 1:  // mark passable
                  tile.Passable = true;
                  break;
               case 2:  // mark obstruction
                  tile.Passable = false;
                  break;
            };

            DrawLevelPreview();
         }
      }

      private void saveToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (_sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            _level.Save(_sfd.FileName);
         }
      }

      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         if (_ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
         {
            _level = ld24.Data.Level.FromFile(_ofd.FileName);
            if (_level != null)
            {
               vScrollBar.Minimum = 0;
               vScrollBar.Maximum = (_level.GetHeight() * TILE_SIZE) - levelView.Height;

               hScrollBar.Minimum = 0;
               hScrollBar.Maximum = (_level.GetWidth() * TILE_SIZE) - levelView.Width;

               DrawLevelPreview();
            }
         }
      }

      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
         this.Close();
      }
   }
}

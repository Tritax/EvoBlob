namespace LevelEditor
{
   partial class Form1
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.levelView = new System.Windows.Forms.PictureBox();
         this.hScrollBar = new System.Windows.Forms.HScrollBar();
         this.vScrollBar = new System.Windows.Forms.VScrollBar();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
         this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
         this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolBox = new System.Windows.Forms.PictureBox();
         this.tilesetOptions = new System.Windows.Forms.ComboBox();
         this._sfd = new System.Windows.Forms.SaveFileDialog();
         this._ofd = new System.Windows.Forms.OpenFileDialog();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.levelView)).BeginInit();
         this.menuStrip1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.toolBox)).BeginInit();
         this.SuspendLayout();
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
         this.splitContainer1.IsSplitterFixed = true;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.levelView);
         this.splitContainer1.Panel1.Controls.Add(this.hScrollBar);
         this.splitContainer1.Panel1.Controls.Add(this.vScrollBar);
         this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.tilesetOptions);
         this.splitContainer1.Panel2.Controls.Add(this.toolBox);
         this.splitContainer1.Size = new System.Drawing.Size(921, 540);
         this.splitContainer1.SplitterDistance = 655;
         this.splitContainer1.TabIndex = 0;
         // 
         // levelView
         // 
         this.levelView.Location = new System.Drawing.Point(0, 27);
         this.levelView.Name = "levelView";
         this.levelView.Size = new System.Drawing.Size(637, 496);
         this.levelView.TabIndex = 3;
         this.levelView.TabStop = false;
         this.levelView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.levelView_MouseClick);
         // 
         // hScrollBar
         // 
         this.hScrollBar.Location = new System.Drawing.Point(0, 523);
         this.hScrollBar.Name = "hScrollBar";
         this.hScrollBar.Size = new System.Drawing.Size(641, 17);
         this.hScrollBar.TabIndex = 2;
         this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
         // 
         // vScrollBar
         // 
         this.vScrollBar.Location = new System.Drawing.Point(636, 24);
         this.vScrollBar.Name = "vScrollBar";
         this.vScrollBar.Size = new System.Drawing.Size(17, 499);
         this.vScrollBar.TabIndex = 1;
         this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(655, 24);
         this.menuStrip1.TabIndex = 0;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem2,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this.fileToolStripMenuItem.Text = "&File";
         // 
         // newToolStripMenuItem
         // 
         this.newToolStripMenuItem.Name = "newToolStripMenuItem";
         this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
         this.newToolStripMenuItem.Text = "&New";
         this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
         // 
         // toolStripMenuItem2
         // 
         this.toolStripMenuItem2.Name = "toolStripMenuItem2";
         this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
         // 
         // openToolStripMenuItem
         // 
         this.openToolStripMenuItem.Name = "openToolStripMenuItem";
         this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
         this.openToolStripMenuItem.Text = "&Open";
         this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
         // 
         // saveToolStripMenuItem
         // 
         this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
         this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
         this.saveToolStripMenuItem.Text = "&Save";
         this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
         // 
         // toolStripMenuItem1
         // 
         this.toolStripMenuItem1.Name = "toolStripMenuItem1";
         this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
         // 
         // exitToolStripMenuItem
         // 
         this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
         this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
         this.exitToolStripMenuItem.Text = "E&xit";
         this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
         // 
         // toolBox
         // 
         this.toolBox.Location = new System.Drawing.Point(0, 25);
         this.toolBox.Name = "toolBox";
         this.toolBox.Size = new System.Drawing.Size(260, 260);
         this.toolBox.TabIndex = 0;
         this.toolBox.TabStop = false;
         this.toolBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.toolBox_MouseClick);
         // 
         // tilesetOptions
         // 
         this.tilesetOptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.tilesetOptions.FormattingEnabled = true;
         this.tilesetOptions.Location = new System.Drawing.Point(4, 2);
         this.tilesetOptions.Name = "tilesetOptions";
         this.tilesetOptions.Size = new System.Drawing.Size(255, 21);
         this.tilesetOptions.TabIndex = 1;
         this.tilesetOptions.SelectedIndexChanged += new System.EventHandler(this.tilesetOptions_SelectedIndexChanged);
         // 
         // _ofd
         // 
         this._ofd.FileName = "openFileDialog1";
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(921, 540);
         this.Controls.Add(this.splitContainer1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MainMenuStrip = this.menuStrip1;
         this.MaximizeBox = false;
         this.Name = "Form1";
         this.Text = "EvoBlob Level Designer";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel1.PerformLayout();
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.levelView)).EndInit();
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.toolBox)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.PictureBox toolBox;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
      private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
      private System.Windows.Forms.HScrollBar hScrollBar;
      private System.Windows.Forms.VScrollBar vScrollBar;
      private System.Windows.Forms.PictureBox levelView;
      private System.Windows.Forms.ComboBox tilesetOptions;
      private System.Windows.Forms.SaveFileDialog _sfd;
      private System.Windows.Forms.OpenFileDialog _ofd;
   }
}


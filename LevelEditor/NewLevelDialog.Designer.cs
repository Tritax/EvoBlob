namespace LevelEditor
{
   partial class NewLevelDialog
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
         this.txtHeight = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.txtWidth = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.createBtn = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // txtHeight
         // 
         this.txtHeight.Location = new System.Drawing.Point(172, 43);
         this.txtHeight.Name = "txtHeight";
         this.txtHeight.Size = new System.Drawing.Size(100, 20);
         this.txtHeight.TabIndex = 11;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(122, 46);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(44, 13);
         this.label3.TabIndex = 10;
         this.label3.Text = "Height: ";
         // 
         // txtWidth
         // 
         this.txtWidth.Location = new System.Drawing.Point(172, 17);
         this.txtWidth.Name = "txtWidth";
         this.txtWidth.Size = new System.Drawing.Size(100, 20);
         this.txtWidth.TabIndex = 9;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(125, 20);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(41, 13);
         this.label2.TabIndex = 8;
         this.label2.Text = "Width: ";
         // 
         // createBtn
         // 
         this.createBtn.Location = new System.Drawing.Point(197, 91);
         this.createBtn.Name = "createBtn";
         this.createBtn.Size = new System.Drawing.Size(75, 23);
         this.createBtn.TabIndex = 12;
         this.createBtn.Text = "Create";
         this.createBtn.UseVisualStyleBackColor = true;
         this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
         // 
         // NewLevelDialog
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 131);
         this.Controls.Add(this.createBtn);
         this.Controls.Add(this.txtHeight);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.txtWidth);
         this.Controls.Add(this.label2);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.Name = "NewLevelDialog";
         this.Text = "Create New Level ...";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TextBox txtHeight;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox txtWidth;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button createBtn;
   }
}
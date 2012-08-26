using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
   public partial class NewLevelDialog : Form
   {
      public NewLevelDialog()
      {
         InitializeComponent();
      }

      private void createBtn_Click(object sender, EventArgs e)
      {
         if (String.IsNullOrEmpty(txtWidth.Text))
         {
            MessageBox.Show("Please enter a valid width.");
            return;
         }

         if (String.IsNullOrEmpty(txtHeight.Text))
         {
            MessageBox.Show("Please enter a valid height.");
            return;
         }

         this.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.Close();
      }

      public string Width
      {
         get { return txtWidth.Text; }
         set { txtWidth.Text = value; }
      }

      public string Height
      {
         get { return txtHeight.Text; }
         set { txtHeight.Text = value; }
      }
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Collector
{
    public partial class Collection_New : Form
    {
        public Collection_New()
        {
            InitializeComponent();
        }

        public int collectionType;

        #region button_Parcourir
        OpenFileDialog ofd = new OpenFileDialog();
        //### Parcourir... ###
        private void button3_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Image|*.png;*.jpg;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.FileName;
            }
        }
        #endregion

        #region button_Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            { MessageBox.Show("Name can't be empty"); }
            else
            {
                Function fctn = new Function();
                fctn.Collection_ProcessNew(this.textBox1.Text,this.textBox3.Text, Convert.ToInt32(collectionType), this.textBox2.Text, "");
                this.Close();
            }
        }
        #endregion

        #region button_Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

    }
}

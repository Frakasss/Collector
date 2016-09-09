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
    public partial class Element_New : Form
    {
        public Element_New()
        {
            InitializeComponent();
        }

        public int collectionType;
        public int collection;

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
                Int32 Gotit = 0;
                Int32 Ratinglvl = 3;

                if (this.checkBox1.Checked) { Gotit = 1; }
                if (this.radioButton1.Checked) { Ratinglvl = 1; }
                if (this.radioButton2.Checked) { Ratinglvl = 2; }
                if (this.radioButton3.Checked) { Ratinglvl = 3; }
                if (this.radioButton4.Checked) { Ratinglvl = 4; }
                if (this.radioButton5.Checked) { Ratinglvl = 5; }
                fctn.Element_ProcessNew(this.textBox1.Text, this.textBox3.Text, collection, this.textBox2.Text, "", Gotit, Ratinglvl);
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

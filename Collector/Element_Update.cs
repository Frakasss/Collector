using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Collector
{
    public partial class Element_Update : Form
    {
        public Element_Update()
        {
            InitializeComponent();
        }

        public Function fct = new Function();
        OpenFileDialog ofd = new OpenFileDialog();
        public int collectionType;
        public int collection;

        #region Combobox
        public void InitComboBox()
        {
            this.comboBox1.Items.Add(new ComboBoxItem("<Select Element>", "0"));
            this.comboBox1.SelectedIndex = 0;


            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = doc.Root.Element("myElements");
            var records = from myCollection in collec.Elements("myElement")
                          where (Int32)myCollection.Element("MemberOf") == collection
                          orderby (string)myCollection.Element("Name")
                          select myCollection;

            foreach (var myCollection in records)
            {
                this.comboBox1.Items.Add(new ComboBoxItem(myCollection.Element("Name").Value, myCollection.Element("id").Value));
            }
        }
        #endregion

        #region Combobox_IndexChanged
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = doc.Root.Element("myElements");
            var records = from myCollection in collecTypes.Elements("myElement")
                          where (Int32)myCollection.Element("id") == Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue)
                          select myCollection;
            textBox1.Text = comboBox1.Text;
            foreach (var myCollection in records)
            {
                textBox2.Text = fct.AppRootPath() + myCollection.Element("Image").Value;
                textBox3.Text = myCollection.Element("Description").Value;
                if (myCollection.Element("GotIt").Value == "1") { checkBox1.Checked = true; }
                if (myCollection.Element("Rating").Value == "1") { radioButton1.Checked = true; }
                if (myCollection.Element("Rating").Value == "2") { radioButton2.Checked = true; }
                if (myCollection.Element("Rating").Value == "3") { radioButton3.Checked = true; }
                if (myCollection.Element("Rating").Value == "4") { radioButton4.Checked = true; }
                if (myCollection.Element("Rating").Value == "5") { radioButton5.Checked = true; }


            }
        }
        #endregion

        #region button_Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region button_Update
        private void button1_Click(object sender, EventArgs e)
        {
            if (((ComboBoxItem)comboBox1.SelectedItem).HiddenValue == "0")
            {
                MessageBox.Show("Please select an Element.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select an Element.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                    fctn.Element_ProcessUpdate(Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue), textBox1.Text, textBox3.Text, collection, textBox2.Text,"", Gotit, Ratinglvl);
                    this.Close();
                }


            }
        }
        #endregion

        #region button_Parcourir
        private void button3_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Image|*.png;*.jpg;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = ofd.FileName;
            }
        }
        #endregion

    }
}

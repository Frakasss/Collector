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
    public partial class Collection_Update : Form
    {
        public Collection_Update()
        {
            InitializeComponent();
        }

        public Function fct = new Function();
        OpenFileDialog ofd = new OpenFileDialog();
        public int collectionType;

        #region Combobox
        public void InitComboBox()
        {
            this.comboBox1.Items.Add(new ComboBoxItem("<Select Collection>", "0"));
            this.comboBox1.SelectedIndex = 0;


            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = doc.Root.Element("myCollections");
            var records = from myCollection in collec.Elements("myCollection")
                          where (Int32)myCollection.Element("MemberOf") == collectionType
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
            XElement collecTypes = doc.Root.Element("myCollections");
            var records = from myCollection in collecTypes.Elements("myCollection")
                          where (Int32)myCollection.Element("id") == Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue)
                          select myCollection;
            textBox1.Text = comboBox1.Text;
            foreach (var myCollection in records) 
            { 
                textBox2.Text = fct.AppRootPath() + myCollection.Element("Image").Value;
                textBox3.Text = myCollection.Element("Description").Value; 
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
                MessageBox.Show("Please select a Collection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select a Collection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Function fctn = new Function();
                    fctn.Collection_ProcessUpdate(Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue), textBox1.Text, textBox3.Text, collectionType, textBox2.Text, "");
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

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
    public partial class CollectionType_Update : Form
    {
        public CollectionType_Update()
        {
            InitializeComponent();
        }

        public Function fct = new Function();
        OpenFileDialog ofd = new OpenFileDialog();

        #region Combobox
        public void InitComboBox()
        {
            this.comboBox1.Items.Add(new ComboBoxItem("<Select Collection Type>", "0"));
            this.comboBox1.SelectedIndex = 0;


            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = doc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
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
            XElement collecTypes = doc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
                          where (Int32)myCollection.Element("id") == Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue)
                          orderby (string)myCollection.Element("Name")
                          select myCollection;
            textBox1.Text = comboBox1.Text;
            foreach (var myCollection in records) { textBox2.Text = fct.AppRootPath() + myCollection.Element("Image").Value; }
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
                MessageBox.Show("Please select a Collection Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Please select a Collection Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else 
                {
                    Function fctn = new Function();
                    fctn.CollectionType_ProcessUpdate(Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue), textBox1.Text, textBox2.Text);
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

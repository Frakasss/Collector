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
    public partial class CollectionType_Delete : Form
    {
        public CollectionType_Delete()
        {
            InitializeComponent();
        }

        public Function fct = new Function();

        #region ComboBox
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

        #region button_Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region button_Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (((ComboBoxItem)comboBox1.SelectedItem).HiddenValue == "0")
            {
                MessageBox.Show("Please select a Collection Type.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Deleting a Collection Type will delete all Collection and Element related.\nPlease confirm deletion.", "Confirm...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    Function fctn = new Function();
                    
                    fctn.CollectionType_ProcessDelete(Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue));
                    this.Close();
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    this.Close();
                }
            }
        }
        #endregion
    }
}

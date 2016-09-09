﻿using System;
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
    public partial class Element_Delete : Form
    {
        public Element_Delete()
        {
            InitializeComponent();
        }

        public Function fct = new Function();
        public int collectionType;
        public int collection;

        #region ComboBox
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

        #region button_Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region button_Delete
        private void button1_Click(object sender, EventArgs e)
        {
            if (((ComboBoxItem)comboBox1.SelectedItem).HiddenValue == "0")
            {
                MessageBox.Show("Please select an Element.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Please confirm deletion.", "Confirm...", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    Function fctn = new Function();

                    fctn.Element_ProcessDelete(Convert.ToInt32(((ComboBoxItem)comboBox1.SelectedItem).HiddenValue));
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

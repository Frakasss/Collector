using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Collector
{
    public partial class CollectionType : Form
    {
        public CollectionType()
        {
            InitializeComponent();
        }

        #region globalVar
        public Function fct = new Function();
        public int collectionType;
        public int collection;
        public int element;
        public int parentId;
        public string parentTitle;
        public string parentDescription;
        public string parentImage;
        #endregion

        public void Init()
        {
            //### ToolBox ###
            #region ToolBox
            Button buttonImg;
            FileStream monImage;
            #endregion

            //### VarBox ###
            #region VarBox
            Int32 icoWidth;
            Int32 icoHeight;
            Int32 icoVertGap;
            Int32 icoHoriGap;
            Int32 nbOfIco;
            Int32 defaultLeftMargin;
            Int32 lastLineLeftMargin;

            Int32 x;
            Int32 y;

            Int32 tmp_ItemNb;
            Int32 tmp_DisplayItemNb;
            Int32 lastLineNb;
            Int32 lastDisplayLineNb;

            #endregion

            //### InitVar ###
            #region InitVar
            icoWidth = Convert.ToInt32(ConfigurationManager.AppSettings["CollecType_icoWidth"]);
            icoHeight = Convert.ToInt32(ConfigurationManager.AppSettings["CollecType_icoHeight"]);
            icoVertGap = Convert.ToInt32(ConfigurationManager.AppSettings["CollecType_icoVertGap"]);
            icoHoriGap = Convert.ToInt32(ConfigurationManager.AppSettings["CollecType_icoHoriGap"]);
            nbOfIco = Convert.ToInt32(ConfigurationManager.AppSettings["CollecType_nbOfColumn"]);


            tmp_ItemNb = 0;
            tmp_DisplayItemNb = 0;
            lastLineNb = 1;
            lastDisplayLineNb = 1;
            #endregion

            //### Calculate Dynamic var ###
            #region DynamicVar
            //### Search button to display ###
            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = doc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
                          orderby (string)myCollection.Element("Name")
                          select myCollection;

            //### Calculate dynamic dimensions ###
            defaultLeftMargin = (panel.Width - ((nbOfIco * icoWidth) + ((nbOfIco - 1) * icoHoriGap))) / 2;
            foreach (var myCollection in records) //### To calculate lastLineNb and lastLineLeftMargin
            {
                tmp_ItemNb = tmp_ItemNb + 1;
                if (tmp_ItemNb > nbOfIco)
                {
                    tmp_ItemNb = 1;
                    lastLineNb = lastLineNb + 1;
                }
            }
            lastLineLeftMargin = (panel.Width - ((tmp_ItemNb * icoWidth) + ((tmp_ItemNb - 1) * icoHoriGap))) / 2;
            panel.AutoScrollMinSize = new Size(((nbOfIco * icoWidth) + ((nbOfIco - 1) * icoHoriGap)), ((lastLineNb) * icoHeight) + ((lastLineNb - 1) * icoVertGap) +20);
            #endregion

            //### Update Form Content ###
            this.SuspendLayout(); 
            foreach (var myCollection in records)
            {
                buttonImg = new Button();

                //## CollectionType Button ## 
                buttonImg.SuspendLayout();
                buttonImg.Anchor = AnchorStyles.None;
                tmp_DisplayItemNb = tmp_DisplayItemNb + 1;

                if (tmp_DisplayItemNb > nbOfIco)
                {
                    tmp_DisplayItemNb = 1;
                    lastDisplayLineNb = lastDisplayLineNb + 1;
                }
                x = ((tmp_DisplayItemNb * icoWidth) - icoWidth) + ((tmp_DisplayItemNb - 1) * icoHoriGap) + defaultLeftMargin;
                y = ((lastDisplayLineNb * icoHeight) - icoHeight) + ((lastDisplayLineNb - 1) * icoVertGap) +10;
                buttonImg.Location = new Point(x, y);
                buttonImg.Size = new Size(icoWidth, icoHeight);
                buttonImg.Cursor = Cursors.Hand;
                buttonImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                buttonImg.UseVisualStyleBackColor = true;
                if (File.Exists(fct.AppRootPath() + myCollection.Element("Image").Value)) 
                {
                    monImage = new FileStream(fct.AppRootPath() + myCollection.Element("Image").Value, FileMode.Open);
                    buttonImg.Image = Image.FromStream(monImage);
                    //buttonImg.ImageAlign = ContentAlignment.MiddleLeft;
                    monImage.Close();
                }
                else
                {
                    monImage = new FileStream(fct.AppRootPath() + "Images/Default.png", FileMode.Open);
                    buttonImg.Image = Image.FromStream(monImage);
                    monImage.Close();
                }
                buttonImg.Tag = myCollection.Element("id").Value;
                buttonImg.Click += new System.EventHandler(CollectionType_click);
                
                //###Add controls ###
                panel.Controls.Add(buttonImg);
                buttonImg.ResumeLayout(false);
                buttonImg.PerformLayout();

            }
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private void CollectionType_click(object sender, EventArgs e)
        {
            Collector frmCollec = new Collector();
            frmCollec.collectionType = Convert.ToInt32(sender.GetType().GetProperty("Tag").GetValue(sender, null));
            frmCollec.Width = this.Width;
            frmCollec.Height = this.Height;
            this.Hide();
            frmCollec.ShowDialog();
            this.Close();
        }

        private void nouveauTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionType_New form = new CollectionType_New();
            form.ShowDialog();
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Width = this.Width;
            frmCollectionType.Height = this.Height;
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            this.Close();
        }

        private void miseaJourTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionType_Update form = new CollectionType_Update();
            form.InitComboBox();
            form.ShowDialog();
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Width = this.Width;
            frmCollectionType.Height = this.Height;
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            this.Close();
        }

        private void supprimerTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionType_Delete form = new CollectionType_Delete();
            form.InitComboBox();
            form.ShowDialog();
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Width = this.Width;
            frmCollectionType.Height = this.Height;
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            this.Close();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }

       
    }
}

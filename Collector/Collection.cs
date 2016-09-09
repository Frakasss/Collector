using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Collector
{
    public partial class Collector : Form
    {
        public Collector()
        {
            InitializeComponent();
        }

        #region globalVar
        public Function fct = new Function();
        public int collectionType;
        public int collection;
        public int element;
        #endregion

        private void Init(object sender, EventArgs e)
        {
            //### ToolBox ###
            #region ToolBox
            Button button;
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
            Int32 buttonWidth;
            Int32 buttonHeight;

            Int32 tmp_ItemNb;
            Int32 tmp_DisplayItemNb;
            Int32 lastLineNb;
            Int32 lastDisplayLineNb;
            #endregion

            //### InitVar ###
            #region InitVar
            icoWidth = Convert.ToInt32(ConfigurationManager.AppSettings["Collec_icoWidth"]);
            icoHeight = Convert.ToInt32(ConfigurationManager.AppSettings["Collec_icoHeight"]);
            icoVertGap = Convert.ToInt32(ConfigurationManager.AppSettings["Collec_icoVertGap"]);
            icoHoriGap = Convert.ToInt32(ConfigurationManager.AppSettings["Collec_icoHoriGap"]);
            nbOfIco = Convert.ToInt32(ConfigurationManager.AppSettings["Collec_nbOfColumn"]);

            tmp_ItemNb = 0;
            tmp_DisplayItemNb = 0;
            lastLineNb = 1;
            lastDisplayLineNb = 1;
            #endregion

            //### Dynamic Elements ###
            #region DynamicElements
            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = doc.Root.Element("myCollections");
            var records = from myCollec in collec.Elements("myCollection")
                          where (int)myCollec.Element("MemberOf") == collectionType
                          orderby myCollec.Element("Name").Value
                          select myCollec;

            //### Calculate dynamic dimensions ###
            defaultLeftMargin = (panel2.Width - ((nbOfIco * icoWidth) + ((nbOfIco - 1) * icoHoriGap))) / 2;
            foreach (var myCollection in records) //### To calculate lastLineNb and lastLineLeftMargin
            {
                tmp_ItemNb = tmp_ItemNb + 1;
                if (tmp_ItemNb > nbOfIco)
                {
                    tmp_ItemNb = 1;
                    lastLineNb = lastLineNb + 1;
                }
            }
            lastLineLeftMargin = (panel2.Width - ((tmp_ItemNb * icoWidth) + ((tmp_ItemNb - 1) * icoHoriGap))) / 2;
            panel2.AutoScrollMinSize = new Size(((nbOfIco * icoWidth) + ((nbOfIco - 1) * icoHoriGap)), ((lastLineNb) * icoHeight) + ((lastLineNb - 1) * icoVertGap) + 20 + 110);
            
            #endregion


            //### Update Form Content ###
            this.SuspendLayout();
            foreach (var myCollection in records)
            {
                button = new Button();

                //## CollectionType Button ## 
                button.Tag = myCollection.Element("id").Value;
                button.SuspendLayout();
                button.Anchor = AnchorStyles.Top;
                tmp_DisplayItemNb = tmp_DisplayItemNb + 1;

                if (tmp_DisplayItemNb > nbOfIco)
                {
                    tmp_DisplayItemNb = 1;
                    lastDisplayLineNb = lastDisplayLineNb + 1;
                }
                x = ((tmp_DisplayItemNb * icoWidth) - icoWidth) + ((tmp_DisplayItemNb - 1) * icoHoriGap) + defaultLeftMargin;
                y = ((lastDisplayLineNb * icoHeight) - icoHeight) + ((lastDisplayLineNb - 1) * icoVertGap) + 10;
                button.Location = new Point(x, y+110);
                button.Size = new Size(icoWidth, icoHeight);
                button.Cursor = Cursors.Hand;
                button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                button.UseVisualStyleBackColor = true;

                buttonWidth = icoWidth - 10;
                buttonHeight = icoHeight - 10;

                #region btnImg
                float imgWidth = buttonWidth;
                float imgHeight = buttonHeight;
                monImage = new FileStream(fct.AppRootPath() + myCollection.Element("Image").Value.ToString(), FileMode.Open);
                if (Image.FromStream(monImage).Width > Image.FromStream(monImage).Height)
                {
                    imgWidth = (buttonHeight) - 2;
                    imgHeight = ((buttonHeight / (float)Image.FromStream(monImage).Width) * (float)Image.FromStream(monImage).Height) - 2;
                }
                else
                {
                    imgWidth = ((buttonHeight / (float)Image.FromStream(monImage).Height) * (float)Image.FromStream(monImage).Width) - 2;
                    imgHeight = (buttonHeight) - 2;
                }

                Bitmap bmp = new Bitmap(Image.FromStream(monImage), (int)imgWidth, (int)imgHeight);
                monImage.Close();
                #endregion

                #region btnImgBackground
                Bitmap imgBkgnd = new Bitmap(buttonHeight, buttonHeight);
                Graphics objBackground = Graphics.FromImage(imgBkgnd);
                objBackground.Clear(System.Drawing.Color.Black);
                objBackground.Flush();
                #endregion

                #region btnTitle
                string sImageText = myCollection.Element("Name").Value.ToString();
                int intWidth = 0;
                int intHeight = 0;
                Bitmap objBmpImage = new Bitmap(2, 2);

                System.Drawing.Font objFont = new System.Drawing.Font("", 28, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
                //measure the text's width and height.
                Graphics objGraphics = Graphics.FromImage(objBmpImage);
                intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
                intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

                // Create the bmpImage again with the correct size for the text and font.
                objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));
                objGraphics = Graphics.FromImage(objBmpImage);
                objGraphics.Clear(System.Drawing.Color.Transparent);
                objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //  <-- This is the correct value to use. ClearTypeGridFit is better yet!
                objGraphics.DrawString(sImageText, objFont, new SolidBrush(System.Drawing.Color.Black), 0, 0, StringFormat.GenericDefault);
                objGraphics.Flush();
                #endregion

                #region btnDescription

                #endregion

                #region createBtnImg
                Bitmap finalImage = new Bitmap(buttonWidth, buttonHeight);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    g.DrawImage(imgBkgnd, new System.Drawing.Rectangle(0, 0, buttonHeight, buttonHeight));
                    g.DrawImage(bmp, new System.Drawing.Rectangle(((buttonHeight - (int)imgWidth) / 2), (buttonHeight - (int)imgHeight) / 2, (int)imgWidth, (int)imgHeight));
                    g.DrawImage(objBmpImage, new System.Drawing.Rectangle(buttonHeight, (buttonHeight - objBmpImage.Height) / 2, objBmpImage.Width, objBmpImage.Height));
                }
                #endregion

                    //monImage = new FileStream(fct.AppRootPath() + myCollection.Element("Image").Value, FileMode.Open);
                    //button.Image = Image.FromStream(monImage);
                    //button.ImageAlign = ContentAlignment.MiddleLeft;
                    //monImage.Close();
                button.Image = finalImage;

                button.Tag = myCollection.Element("id").Value;
                button.Click += new System.EventHandler(CollectionType_click);

                //###Add controls ###
                panel2.Controls.Add(button);
                button.ResumeLayout(false);
                button.PerformLayout();

            }
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private void nouvelleCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection_New form = new Collection_New();
            form.collectionType = this.collectionType;
            form.ShowDialog();
            Collector frmCollection = new Collector();
            frmCollection.Width = this.Width;
            frmCollection.Height = this.Height;
            frmCollection.Location = this.Location;
            frmCollection.collectionType = this.collectionType;
            this.Hide();
            frmCollection.ShowDialog();
            this.Close();
        }

        private void miseÀJourCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection_Update form = new Collection_Update();
            form.collectionType = this.collectionType;
            form.InitComboBox();
            form.ShowDialog();
            Collector frmCollection = new Collector();
            frmCollection.Width = this.Width;
            frmCollection.Height = this.Height;
            frmCollection.Location = this.Location;
            frmCollection.collectionType = this.collectionType;
            this.Hide();
            frmCollection.ShowDialog();
            this.Close();
        }

        private void supprimerCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Collection_Delete form = new Collection_Delete();
            form.collectionType = this.collectionType;
            form.InitComboBox();
            form.ShowDialog();
            Collector frmCollection = new Collector();
            frmCollection.Width = this.Width;
            frmCollection.Height = this.Height;
            frmCollection.Location = this.Location;
            frmCollection.collectionType = this.collectionType;
            this.Hide();
            frmCollection.ShowDialog();
            this.Close();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void accueilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Width = this.Width;
            frmCollectionType.Height = this.Height;
            frmCollectionType.Location = this.Location;
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }

        private void CollectionType_click(object sender, EventArgs e)
        {
            Element frmElement = new Element();
            frmElement.Width = this.Width;
            frmElement.Height = this.Height;
            frmElement.Location = this.Location;
            frmElement.collectionType = this.collectionType;
            frmElement.collection = Convert.ToInt32(sender.GetType().GetProperty("Tag").GetValue(sender, null));
            frmElement.element = 0;

            this.Hide();
            frmElement.ShowDialog();
            this.Close();
        }
    }
}

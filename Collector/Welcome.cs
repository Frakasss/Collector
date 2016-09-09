using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Collector
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        public Function fct = new Function();

        private void skipForm(object sender, System.EventArgs e)
        {
            
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            this.Close();
 /*            
            Int32 buttonWidth = 250-10;
            Int32 buttonHeight = 90-10;



            #region btnImg
            FileStream monImage;
            float imgWidth = buttonWidth;
            float imgHeight = buttonHeight;
            monImage = new FileStream(fct.AppRootPath() + "Images/tmp2.jpg", FileMode.Open);
            if (Image.FromStream(monImage).Width > Image.FromStream(monImage).Height)
            {
                imgWidth = (buttonHeight)-2;
                imgHeight = ((buttonHeight / (float)Image.FromStream(monImage).Width) * (float)Image.FromStream(monImage).Height)-2;
            }
            else
            {
                imgWidth = ((buttonHeight / (float)Image.FromStream(monImage).Height) * (float)Image.FromStream(monImage).Width)-2;
                imgHeight = (buttonHeight)-2;
            }
            
            Bitmap bmp = new Bitmap(Image.FromStream(monImage), (int)imgWidth , (int)imgHeight);
            monImage.Close();
            #endregion

            #region btnImgBackground
            Bitmap imgBkgnd = new Bitmap(buttonHeight, buttonHeight);
            Graphics objBackground = Graphics.FromImage(imgBkgnd);
            objBackground.Clear(System.Drawing.Color.Black);
            objBackground.Flush();
            #endregion

            #region btnTitle
            string sImageText = "Kikoo";
            int intWidth = 0;
            int intHeight = 0;
            Bitmap objBmpImage = new Bitmap(2, 2);
            
            System.Drawing.Font objFont = new System.Drawing.Font("", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
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
            button2.Image = finalImage;
*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CollectionType frmCollectionType = new CollectionType();
            frmCollectionType.Init();
            this.Hide();
            frmCollectionType.ShowDialog();
            
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ElementViewer : Form
    {
        public ElementViewer()
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
            int firstId = 0;
            int previousId = 0;
            int currentId = 0;
            int nextId = 0;
            int lastId = 0;
            int nbOfElements = 0;

            String firstImagePath = parentImage;
            String previousImagePath = parentImage;
            String currentImagePath = parentImage;
            String nextImagePath = parentImage;
            String lastImagePath = parentImage;
            String imagePath = parentImage;

            String firstTitle = parentImage;
            String previousTitle = parentImage;
            String currentTitle = parentImage;
            String nextTitle = parentImage;
            String lastTitle = parentImage;

            int imageHeight;
            int imageWidth;
            int imageMaxWidth;
            int imageMaxHeight;
            int verticalMargin;
            int horizontalMargin;
            float imageLocationX;
            float imageLocationY;
            float finalPictureWidth;
            float finalPictureHeight;
            float Scale;

            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = doc.Root.Element("myElements");
            var collecRecords = from myCollec in collec.Elements("myElement")
                                 where (int)myCollec.Element("MemberOf") == collection
                                 orderby myCollec.Element("Name").Value
                                 select myCollec;

            foreach (var elmnt in collecRecords)
            {
                nbOfElements = nbOfElements + 1;
                //First image
                if (firstId == 0)
                {
                    firstId = Convert.ToInt32(elmnt.Element("id").Value);
                    firstImagePath = elmnt.Element("Image").Value;
                    firstTitle = elmnt.Element("Name").Value;
                }

                //previous image
                if (String.Compare(elmnt.Element("Name").Value,parentTitle)<0)
                {
                    previousId = Convert.ToInt32(elmnt.Element("id").Value);
                    previousImagePath = elmnt.Element("Image").Value;
                    previousTitle = elmnt.Element("Name").Value;
                }

                //current image
                if (Convert.ToInt32(elmnt.Element("id").Value) == element)
                {
                    currentId = Convert.ToInt32(elmnt.Element("id").Value);
                    currentImagePath = elmnt.Element("Image").Value;
                    currentTitle = elmnt.Element("Name").Value;
                }

                //next image
                if (String.Compare(elmnt.Element("Name").Value, parentTitle) > 0)
                {
                    if(nextId == 0)
                    {
                        nextId = Convert.ToInt32(elmnt.Element("id").Value);
                        nextImagePath = elmnt.Element("Image").Value;
                        nextTitle = elmnt.Element("Name").Value;
                    }
                }

                //last image
                lastId = Convert.ToInt32(elmnt.Element("id").Value);
                lastImagePath = elmnt.Element("Image").Value;
                lastTitle = elmnt.Element("Name").Value;

            }

            //### CurrentElement
            imagePath = fct.AppRootPath() + currentImagePath;

            FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            imageHeight = Image.FromStream(fs).Height;
            imageWidth = Image.FromStream(fs).Width;
            imageMaxWidth = panel1.Width;
            imageMaxHeight = panel1.Height;
            finalPictureWidth = imageWidth;
            finalPictureHeight = imageHeight;
            imageLocationX = 0;
            imageLocationY = 0;
            verticalMargin = 2;
            horizontalMargin = 2;

            if (imageHeight + verticalMargin * 2 < panel1.Height && imageWidth + horizontalMargin*2 < panel1.Width)
            {
                imageLocationX = (panel1.Width - imageWidth) / 2;
                imageLocationY = (panel1.Height - imageHeight) / 2;
            }
            else
            {
                finalPictureWidth = imageMaxWidth - horizontalMargin*2;
                Scale = (float)(imageMaxWidth - horizontalMargin*2) / (float)imageWidth;
                finalPictureHeight = (imageHeight * Scale) - (verticalMargin*2);
                imageLocationX = horizontalMargin;
                imageLocationY = verticalMargin + (panel1.Height - finalPictureHeight) / 2;

                if (finalPictureHeight > imageMaxHeight)
                {
                    finalPictureHeight = imageMaxHeight - verticalMargin * 2;
                    Scale = (float)(imageMaxHeight - verticalMargin * 2) / (float)imageHeight;
                    finalPictureWidth = (imageWidth * Scale) - (horizontalMargin * 2);
                    imageLocationX = horizontalMargin + (panel1.Width - finalPictureWidth) / 2;
                    imageLocationY = verticalMargin;
                }

            }

            pictureBox1.Height = Convert.ToInt32(finalPictureHeight);
            pictureBox1.Width = Convert.ToInt32(finalPictureWidth);
            pictureBox1.Location = new Point(Convert.ToInt32(imageLocationX), Convert.ToInt32(imageLocationY));
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();

            //Case only one element in the collection
            if (nbOfElements == 1)
            {
                fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                //Previous = current
                nextImg.Image = System.Drawing.Image.FromStream(fs);
                nextBtn.Tag = currentId + "##" + currentTitle;
                //Next = current
                previousImg.Image = System.Drawing.Image.FromStream(fs);
                previousBtn.Tag = currentId + "##" + currentTitle;
                fs.Close();
            }


            //Case only 2 elements in the collection
            if (nbOfElements == 2)
            {
                if (firstId != currentId)
                {
                    fs = new FileStream(firstImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = firstId + "##" + firstTitle;
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = firstId + "##" + firstTitle;
                    fs.Close();
                }
                else
                {
                    fs = new FileStream(lastImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = lastId + "##" + lastTitle;
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = lastId + "##" + lastTitle;
                    fs.Close();
                }
            }

            //Case 3 and more images
            if (nbOfElements >= 3)
            {
                //Case Current = first
                if (currentId == firstId)
                {
                    fs = new FileStream(nextImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = nextId + "##" + nextTitle;
                    fs.Close();
                    fs = new FileStream(lastImagePath, FileMode.Open, FileAccess.Read);
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = lastId + "##" + lastTitle;
                    fs.Close();
                }
                else
                {
                    //Case Current = Last
                    if (currentId == lastId)
                    {
                        fs = new FileStream(firstImagePath, FileMode.Open, FileAccess.Read);
                        nextImg.Image = System.Drawing.Image.FromStream(fs);
                        nextBtn.Tag = firstId + "##" + firstTitle;
                        fs.Close();
                        fs = new FileStream(previousImagePath, FileMode.Open, FileAccess.Read);
                        previousImg.Image = System.Drawing.Image.FromStream(fs);
                        previousBtn.Tag = previousId + "##" + previousTitle;
                        fs.Close();
                    }
                    //Else
                    else
                    {
                        fs = new FileStream(nextImagePath, FileMode.Open, FileAccess.Read);
                        nextImg.Image = System.Drawing.Image.FromStream(fs);
                        nextBtn.Tag = nextId + "##" + nextTitle;
                        fs.Close();
                        fs = new FileStream(previousImagePath, FileMode.Open, FileAccess.Read);
                        previousImg.Image = System.Drawing.Image.FromStream(fs);
                        previousBtn.Tag = previousId + "##" + previousTitle;
                        fs.Close();
                    }
                }
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, EventArgs e)
        {
            String ElementParameters = sender.GetType().GetProperty("Tag").GetValue(sender, null).ToString();
            Int32 separatorIndex = ElementParameters.IndexOf("##");
            this.element = Convert.ToInt32(ElementParameters.Substring(0, separatorIndex));
            this.parentTitle = ElementParameters.Replace(this.element.ToString() + "##", "");

            
            int firstId = 0;
            int previousId = 0;
            int currentId = 0;
            int nextId = 0;
            int lastId = 0;
            int nbOfElements = 0;

            String firstImagePath = parentImage;
            String previousImagePath = parentImage;
            String currentImagePath = parentImage;
            String nextImagePath = parentImage;
            String lastImagePath = parentImage;
            String imagePath = parentImage;

            String firstTitle = parentImage;
            String previousTitle = parentImage;
            String currentTitle = parentImage;
            String nextTitle = parentImage;
            String lastTitle = parentImage;

            int imageHeight;
            int imageWidth;
            int imageMaxWidth;
            int imageMaxHeight;
            int verticalMargin;
            int horizontalMargin;
            float imageLocationX;
            float imageLocationY;
            float finalPictureWidth;
            float finalPictureHeight;
            float Scale;

            XDocument doc = XDocument.Load(fct.AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = doc.Root.Element("myElements");
            var collecRecords = from myCollec in collec.Elements("myElement")
                                where (int)myCollec.Element("MemberOf") == collection
                                orderby myCollec.Element("Name").Value
                                select myCollec;

            foreach (var elmnt in collecRecords)
            {
                nbOfElements = nbOfElements + 1;
                Int32 crnParsedId = Convert.ToInt32(elmnt.Element("id").Value);
                String crnParsedTitle = elmnt.Element("Name").Value;
                if (firstId == 0)
                {
                    firstId = Convert.ToInt32(elmnt.Element("id").Value);
                    firstImagePath = elmnt.Element("Image").Value;
                    firstTitle = elmnt.Element("Name").Value;
                }

                //previous image
                if (String.Compare(crnParsedTitle, parentTitle) < 0)
                {
                    previousId = Convert.ToInt32(elmnt.Element("id").Value);
                    previousImagePath = elmnt.Element("Image").Value;
                    previousTitle = elmnt.Element("Name").Value;
                }

                //current image
                if (crnParsedId == element)
                {
                    currentId = Convert.ToInt32(elmnt.Element("id").Value);
                    currentImagePath = elmnt.Element("Image").Value;
                    currentTitle = elmnt.Element("Name").Value;
                }

                //next image
                if (String.Compare(crnParsedTitle, parentTitle) > 0)
                {
                    if(nextId == 0)
                    {
                        nextId = Convert.ToInt32(elmnt.Element("id").Value);
                        nextImagePath = elmnt.Element("Image").Value;
                        nextTitle = elmnt.Element("Name").Value;
                    }
                }

                //last image
                lastId = Convert.ToInt32(elmnt.Element("id").Value);
                lastImagePath = elmnt.Element("Image").Value;
                lastTitle = elmnt.Element("Name").Value;

            }

            //### CurrentElement
            imagePath = fct.AppRootPath() + currentImagePath;

            FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            imageHeight = Image.FromStream(fs).Height;
            imageWidth = Image.FromStream(fs).Width;
            imageMaxWidth = panel1.Width;
            imageMaxHeight = panel1.Height;
            finalPictureWidth = imageWidth;
            finalPictureHeight = imageHeight;
            imageLocationX = 0;
            imageLocationY = 0;
            verticalMargin = 2;
            horizontalMargin = 2;

            if (imageHeight + verticalMargin * 2 < panel1.Height && imageWidth + horizontalMargin * 2 < panel1.Width)
            {
                imageLocationX = (panel1.Width - imageWidth) / 2;
                imageLocationY = (panel1.Height - imageHeight) / 2;
            }
            else
            {
                finalPictureWidth = imageMaxWidth - horizontalMargin * 2;
                Scale = (float)(imageMaxWidth - horizontalMargin * 2) / (float)imageWidth;
                finalPictureHeight = (imageHeight * Scale) - (verticalMargin * 2);
                imageLocationX = horizontalMargin;
                imageLocationY = verticalMargin + (panel1.Height - finalPictureHeight) / 2;

                if (finalPictureHeight > imageMaxHeight)
                {
                    finalPictureHeight = imageMaxHeight - verticalMargin * 2;
                    Scale = (float)(imageMaxHeight - verticalMargin * 2) / (float)imageHeight;
                    finalPictureWidth = (imageWidth * Scale) - (horizontalMargin * 2);
                    imageLocationX = horizontalMargin + (panel1.Width - finalPictureWidth) / 2;
                    imageLocationY = verticalMargin;
                }

            }

            pictureBox1.Height = Convert.ToInt32(finalPictureHeight);
            pictureBox1.Width = Convert.ToInt32(finalPictureWidth);
            pictureBox1.Location = new Point(Convert.ToInt32(imageLocationX), Convert.ToInt32(imageLocationY));
            pictureBox1.Image = System.Drawing.Image.FromStream(fs);
            fs.Close();

            //Case only one element in the collection
            if (nbOfElements == 1)
            {
                fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                //Previous = current
                nextImg.Image = System.Drawing.Image.FromStream(fs);
                nextBtn.Tag = currentId + "##" + currentTitle;
                //Next = current
                previousImg.Image = System.Drawing.Image.FromStream(fs);
                previousBtn.Tag = currentId + "##" + currentTitle;
                fs.Close();
            }


            //Case only 2 elements in the collection
            if (nbOfElements == 2)
            {
                if (firstId != currentId)
                {
                    fs = new FileStream(firstImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = firstId + "##" + firstTitle;
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = firstId + "##" + firstTitle;
                    fs.Close();
                }
                else
                {
                    fs = new FileStream(lastImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = lastId + "##" + lastTitle;
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = lastId + "##" + lastTitle;
                    fs.Close();
                }
            }

            //Case 3 and more images
            if (nbOfElements >= 3)
            {
                //Case Current = first
                if (currentId == firstId)
                {
                    fs = new FileStream(nextImagePath, FileMode.Open, FileAccess.Read);
                    nextImg.Image = System.Drawing.Image.FromStream(fs);
                    nextBtn.Tag = nextId + "##" + nextTitle;
                    fs.Close();
                    fs = new FileStream(lastImagePath, FileMode.Open, FileAccess.Read);
                    previousImg.Image = System.Drawing.Image.FromStream(fs);
                    previousBtn.Tag = lastId + "##" + lastTitle;
                    fs.Close();
                }
                else
                {
                    //Case Current = Last
                    if (currentId == lastId)
                    {
                        fs = new FileStream(firstImagePath, FileMode.Open, FileAccess.Read);
                        nextImg.Image = System.Drawing.Image.FromStream(fs);
                        nextBtn.Tag = firstId + "##" + firstTitle;
                        fs.Close();
                        fs = new FileStream(previousImagePath, FileMode.Open, FileAccess.Read);
                        previousImg.Image = System.Drawing.Image.FromStream(fs);
                        previousBtn.Tag = previousId + "##" + previousTitle;
                        fs.Close();
                    }
                    //Else
                    else
                    {
                        fs = new FileStream(nextImagePath, FileMode.Open, FileAccess.Read);
                        nextImg.Image = System.Drawing.Image.FromStream(fs);
                        nextBtn.Tag = nextId + "##" + nextTitle;
                        fs.Close();
                        fs = new FileStream(previousImagePath, FileMode.Open, FileAccess.Read);
                        previousImg.Image = System.Drawing.Image.FromStream(fs);
                        previousBtn.Tag = previousId + "##" + previousTitle;
                        fs.Close();
                    }
                }
            }            
        }
    }
}

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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Welcome());
        }
    }

    public class Function
    {
        public String AppRootPath()
        {
            return (AppDomain.CurrentDomain.BaseDirectory).Replace("\\", "/");
        }

        public int MiniatureSize = 200;

        #region CollectionTypes
        #region New
        public void CollectionType_ProcessNew(String Name, String Image)
        {
            Int32 id = 0;
            Int32 tmpId = 0;
            //### calculateId ###
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = xmlDoc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
                          orderby (int)myCollection.Element("id")
                          select myCollection;
            foreach (var myCollection in records)
            {
                tmpId = tmpId + 1;
                if (myCollection.Element("id").Value != tmpId.ToString())
                {
                    id = tmpId;
                    break;
                }
            }
            if (id == 0) { id = tmpId + 1; }
            //### Import Image ###
            if (File.Exists(Image))
            {
                String NewFileName = AppRootPath() + "Images/CollecType" + id + GlobalFunction.Right(Image, 4);                
                File.Copy(Image, NewFileName);
                Image = "Images/CollecType" + id + GlobalFunction.Right(Image, 4);
            }
            else
            {
                Image = "Images/CollecType000.png";
            }
            //### INSERT record ###
            xmlDoc.Element("myCollectionsDb").Element("myCollectionsTypes").Add(new XElement("myCollectionType",
                                                                                    new XElement("id", id.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Image", Image)
                                                                                    )
                                                                               );
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
        }
        #endregion

        #region Update
        public void CollectionType_ProcessUpdate(Int32 nodeToUpdate,String Name, String Image)
        {
            String ImageToLink= "";
            String ImageToDelete = "";
            String ImageToCopy = "";
            String CurrentImage = "";

            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = xmlDoc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
                          where (int)myCollection.Element("id") == nodeToUpdate
                          select myCollection;
            foreach (var myCollection in records)
            {
                CurrentImage = AppRootPath() + myCollection.Element("Image").Value;
            }


            if (Image == "") //image = vide => defaut
            {
                ImageToLink= "Image/CollecType000.png";
                ImageToDelete = "";
                ImageToCopy = "";
            }
            else 
            {
                if (Image == AppRootPath() + "Image/CollecType000.png")
                {
                    ImageToLink = "Image/CollecType000.png";
                    ImageToDelete = "";
                    ImageToCopy = "";
                }
                else
                {
                    if (Image == CurrentImage)
                    {
                        ImageToLink = CurrentImage.Replace(AppRootPath(),"");
                        ImageToDelete = "";
                        ImageToCopy = "";
                    }
                    else
                    {
                        if(File.Exists(Image))
                        {
                            ImageToLink = "Images/CollecType" + nodeToUpdate + GlobalFunction.Right(Image, 4);
                            ImageToDelete = CurrentImage;
                            ImageToCopy = Image;
                        }
                        else
                        {
                            ImageToLink = CurrentImage.Replace(AppRootPath(), "");
                            ImageToDelete = "";
                            ImageToCopy = "";
                        }
                    }
                }
            }

            //Delete
            if (ImageToDelete != ""){File.Delete(ImageToDelete);}

            //Upload
            if (ImageToCopy != "") { File.Copy(ImageToCopy, AppRootPath() + ImageToLink); }

            //Remove record
            records.Remove();

            //Insert Record
            //### INSERT record ###
            xmlDoc.Element("myCollectionsDb").Element("myCollectionsTypes").Add(new XElement("myCollectionType",
                                                                                    new XElement("id", nodeToUpdate.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Image", ImageToLink)
                                                                                    )
                                                                               );

            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
 




        }
        #endregion

        #region Delete
        public void CollectionType_ProcessDelete(Int32 nodeToDelete)
        {
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = xmlDoc.Root.Element("myCollectionsTypes");
            var records = from myCollection in collecTypes.Elements("myCollectionType")
                          where (int)myCollection.Element("id") == nodeToDelete
                          select myCollection;
            foreach (var myCollection in records)
            {

                String FileToDelete = AppRootPath() + myCollection.Element("Image").Value;
                if (myCollection.Element("Image").Value != "Images/CollecType000.png")
                {
                    if (File.Exists(FileToDelete))
                    {
                        File.Delete(FileToDelete);
                    }
                }
            }
            
            
            records.Remove();
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
        }
        #endregion
        #endregion

        #region Collections
        #region New
        public void Collection_ProcessNew(String Name, String Description, Int32 MemberOf, String Img, String DefImage)
        {
            //### declareVar ###
            #region declareVar
            Int32 id = 0;
            Int32 tmpId = 0;
            String Miniature = "";
            float miniWidth = 0;
            float miniHeight = 0;
            #endregion

            //### calculateId ###
            #region calculateId
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = xmlDoc.Root.Element("myCollections");
            var records = from myCollec in collec.Elements("myCollection")
                          orderby (int)myCollec.Element("id")
                          select myCollec;
            foreach (var myCollection in records){
                tmpId = tmpId + 1;
                if (myCollection.Element("id").Value != tmpId.ToString()){
                    id = tmpId;
                    break;
                }
            }
            if (id == 0) { id = tmpId + 1; }
            #endregion

            //### Import Image ###
            #region importImage
            if (File.Exists(Img))
            {
                String NewFileName = AppRootPath() + "/Images/Images/Collec" + id + GlobalFunction.Right(Img, 4);
                File.Copy(Img, NewFileName);
                Img = "Images/Images/Collec" + id + GlobalFunction.Right(Img, 4);
                Miniature = Img.Replace("Images/Images/", "Images/Miniatures/");
            }
            else
            {
                String NewFileName = AppRootPath() + "Images/Images/Collec" + id + GlobalFunction.Right(DefImage, 4);
                File.Copy(DefImage, NewFileName);
                Img = "Images/Images/Collec" + id + GlobalFunction.Right(DefImage, 4);
                Miniature = Img.Replace("Images/Images/", "Images/Miniatures/");
            }
            #endregion

            //### Create miniature ###
            #region createMiniature
            using (Image mini = Image.FromFile(Img))
            {
                if (mini.Width > MiniatureSize || mini.Height > MiniatureSize)
                {
                    if (mini.Width > mini.Height)
                    {
                        miniWidth = MiniatureSize;
                        miniHeight = ((MiniatureSize / (float)mini.Width) * (float)mini.Height);
                    }
                    else
                    {
                        miniHeight = MiniatureSize;
                        miniWidth = ((MiniatureSize / (float)mini.Height) * (float)mini.Width);
                    }
                }
                else
                {
                    miniWidth = mini.Width;
                    miniHeight = mini.Height;
                }
                Bitmap newImage = new Bitmap(mini, (int)miniWidth, (int)miniHeight);
                newImage.Save(Img.Replace("Images/Images/", "Images/Miniatures/"));
                mini.Dispose();
            }
            #endregion

            //### INSERT record ###
            #region insertXml
            xmlDoc.Element("myCollectionsDb").Element("myCollections").Add(new XElement("myCollection",
                                                                                    new XElement("id", id.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Description", Description),
                                                                                    new XElement("MemberOf", MemberOf),
                                                                                    new XElement("Image", Img),
                                                                                    new XElement("Miniature", Miniature)
                                                                                    )
                                                                               );
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
            #endregion
        }
        #endregion

        #region Update
        public void Collection_ProcessUpdate(Int32 nodeToUpdate, String Name, String Description, Int32 MemberOf, String Img, String DefImage)
        {
            String ImageToLink = "";    //Path to be inserted in DB
            String ImageToDelete = "";  //Old Image to be deleted 
            String ImageToCopy = "";    //New Image to be uploaded
            String MiniToLink = "";
            String MiniToDelete = "";
            String MiniToCopy = "";
            String CurrentImage = "";
            float miniWidth = 0;
            float miniHeight = 0;

            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = xmlDoc.Root.Element("myCollections");
            var records = from myCollection in collecTypes.Elements("myCollection")
                          where (int)myCollection.Element("id") == nodeToUpdate
                          select myCollection;
            foreach (var myCollection in records){ CurrentImage = AppRootPath() + myCollection.Element("Image").Value; }

            if (Img == "") { Img = DefImage; }

            if (Img == CurrentImage)
            {
                ImageToLink = CurrentImage.Replace(AppRootPath(), "");
                ImageToDelete = "";
                ImageToCopy = "";
                MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/");
                MiniToDelete = "";
                MiniToCopy = "";
            }
            else
            {
                if (File.Exists(Img))
                {
                    ImageToLink = "Images/Images/Collec" + nodeToUpdate + GlobalFunction.Right(Img, 4);
                    ImageToDelete = CurrentImage;
                    ImageToCopy = Img;
                    MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToDelete = CurrentImage.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToCopy = Img;
                }
                else
                {
                    ImageToLink = CurrentImage.Replace(AppRootPath(), "");
                    ImageToDelete = "";
                    ImageToCopy = "";
                    MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToDelete = "";
                    MiniToCopy = "";
                }
            }

            //Delete
            if (ImageToDelete != "")
            {
                File.Delete(ImageToDelete);
                File.Delete(MiniToDelete);
            }

            //Upload
            if (ImageToCopy != "")
            {
                File.Copy(ImageToCopy, AppRootPath() + ImageToLink);
                //### Create miniature ###
                using (Image mini = Image.FromFile(MiniToCopy))
                {
                    if (mini.Width > MiniatureSize || mini.Height > MiniatureSize)
                    {
                        if (mini.Width > mini.Height)
                        {
                            miniWidth = MiniatureSize;
                            miniHeight = ((MiniatureSize / (float)mini.Width) * (float)mini.Height);
                        }
                        else
                        {
                            miniHeight = MiniatureSize;
                            miniWidth = ((MiniatureSize / (float)mini.Height) * (float)mini.Width);
                        }
                    }
                    else
                    {
                        miniWidth = mini.Width;
                        miniHeight = mini.Height;
                    }
                    Bitmap newImage = new Bitmap(mini, (int)miniWidth, (int)miniHeight);
                    newImage.Save(AppRootPath() + MiniToLink);
                    mini.Dispose();
                }
            }

            //Remove record
            records.Remove();

            //Insert Record
            //### INSERT record ###
            xmlDoc.Element("myCollectionsDb").Element("myCollections").Add(new XElement("myCollection",
                                                                                    new XElement("id", nodeToUpdate.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Description", Description),
                                                                                    new XElement("MemberOf", MemberOf),
                                                                                    new XElement("Image", ImageToLink)
                                                                                    )
                                                                               );

            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");





        }
        #endregion

        #region Delete
        public void Collection_ProcessDelete(Int32 nodeToDelete)
        {
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = xmlDoc.Root.Element("myCollections");
            var records = from myCollection in collec.Elements("myCollection")
                          where (int)myCollection.Element("id") == nodeToDelete
                          select myCollection;
            foreach (var myCollection in records)
            {

                String FileToDelete = AppRootPath() + myCollection.Element("Image").Value;
                if (myCollection.Element("Image").Value != "Images/Collec000.png")
                {
                    if (File.Exists(FileToDelete))
                    {
                        File.Delete(FileToDelete);
                    }
                }
            }


            records.Remove();
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
        }
        #endregion

        #endregion

        #region Elements
        #region New
        public void Element_ProcessNew(String Name, String Description, Int32 MemberOf, String Img, String DefImage, Int32 GotIt, Int32 RatingLvl)
        {
            //### Declare Var ###
            Int32 id = 0;
            Int32 tmpId = 0;
            String Miniature = "";
            float miniWidth = 0;
            float miniHeight = 0;

            //### calculate Id ###
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = xmlDoc.Root.Element("myElements");
            var records = from myCollec in collec.Elements("myElement")
                          orderby (int)myCollec.Element("id")
                          select myCollec;
            foreach (var myCollection in records)
            {
                tmpId = tmpId + 1;
                if (myCollection.Element("id").Value != tmpId.ToString())
                {
                id = tmpId;
                break;
                }
            }
            if (id == 0) { id = tmpId + 1; }

            //### Import Image ###
            String NewFileName = "";
            if (File.Exists(Img))
            {
                NewFileName = AppRootPath() + "Images/Images/Element" + id + GlobalFunction.Right(Img, 4);
                File.Copy(Img, NewFileName);
                Img = "Images/Images/Element" + id + GlobalFunction.Right(Img, 4);
                Miniature = Img.Replace("Images/Images/","Images/Miniatures/");
            }
            else
            {
                NewFileName = AppRootPath() + "Images/Images/Element" + id + GlobalFunction.Right(DefImage, 4);
                //File.Copy(DefImage, NewFileName);
                Img = "Images/Images/Element" + id + GlobalFunction.Right(DefImage, 4);
                Miniature = Img.Replace("Images/Images/", "Images/Miniatures/");
            }

            //### Create miniature ###
            using (Image mini = Image.FromFile(Img))
            {
                if (mini.Width > MiniatureSize || mini.Height > MiniatureSize)
                {
                    if (mini.Width > mini.Height)
                    {
                        miniWidth = MiniatureSize;
                        miniHeight = ((MiniatureSize / (float)mini.Width) * (float)mini.Height);
                    }
                    else
                    {
                        miniHeight = MiniatureSize;
                        miniWidth = ((MiniatureSize / (float)mini.Height) * (float)mini.Width);
                    }
                }
                else
                {
                    miniWidth = mini.Width;
                    miniHeight = mini.Height;
                }
                Bitmap newImage = new Bitmap(mini, (int)miniWidth, (int)miniHeight);
                newImage.Save(Img.Replace("Images/Images/", "Images/Miniatures/"));
                mini.Dispose();
            }

            //### INSERT record ###
            xmlDoc.Element("myCollectionsDb").Element("myElements").Add(new XElement("myElement",
                                                                                    new XElement("id", id.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Description", Description),
                                                                                    new XElement("MemberOf", MemberOf),
                                                                                    new XElement("Image", Img),
                                                                                    new XElement("Miniature", Miniature),
                                                                                    new XElement("GotIt", GotIt),
                                                                                    new XElement("Rating", RatingLvl)
                                                                                    )
                                                                               );
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
        }
        #endregion

        #region Update
        public void Element_ProcessUpdate(Int32 nodeToUpdate, String Name, String Description, Int32 MemberOf, String Img, String DefImage, Int32 GotIt, Int32 RatingLvl)
        {
            String ImageToLink = "";    //Path to be inserted in DB
            String ImageToDelete = "";  //Old Image to be deleted 
            String ImageToCopy = "";    //New Image to be uploaded
            String MiniToLink = "";
            String MiniToDelete = "";
            String MiniToCopy = "";
            String CurrentImage = "";
            float miniWidth = 0;
            float miniHeight = 0;

            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collecTypes = xmlDoc.Root.Element("myElements");
            var records = from myCollection in collecTypes.Elements("myElement")
                          where (int)myCollection.Element("id") == nodeToUpdate
                          select myCollection;
            foreach (var myCollection in records){ CurrentImage = AppRootPath() + myCollection.Element("Image").Value; }

            if (Img == "") { Img = DefImage; }

            if (Img == CurrentImage)
            {
                ImageToLink = CurrentImage.Replace(AppRootPath(), "");
                ImageToDelete = "";
                ImageToCopy = "";
                MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/"); 
                MiniToDelete = "";
                MiniToCopy = "";
            }
            else
            {
                if (File.Exists(Img))
                {
                    ImageToLink = "Images/Images/Element" + nodeToUpdate + GlobalFunction.Right(Img, 4);
                    ImageToDelete = CurrentImage;
                    ImageToCopy = Img;
                    MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToDelete = CurrentImage.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToCopy = Img;
                }
                else
                {
                    ImageToLink = CurrentImage.Replace(AppRootPath(), "");
                    ImageToDelete = "";
                    ImageToCopy = "";
                    MiniToLink = ImageToLink.Replace("Images/Images/", "Images/Miniatures/");
                    MiniToDelete = "";
                    MiniToCopy = "";
                }
            }

            //Delete
            if (ImageToDelete != "") { 
                File.Delete(ImageToDelete);
                File.Delete(MiniToDelete);
            }

            //Upload
            if (ImageToCopy != "") { 
                File.Copy(ImageToCopy, AppRootPath() + ImageToLink);
                //### Create miniature ###
                using (Image mini = Image.FromFile(MiniToCopy))
                {
                    if (mini.Width > MiniatureSize || mini.Height > MiniatureSize)
                    {
                        if (mini.Width > mini.Height)
                        {
                            miniWidth = MiniatureSize;
                            miniHeight = ((MiniatureSize / (float)mini.Width) * (float)mini.Height);
                        }
                        else
                        {
                            miniHeight = MiniatureSize;
                            miniWidth = ((MiniatureSize / (float)mini.Height) * (float)mini.Width);
                        }
                    }
                    else
                    {
                        miniWidth = mini.Width;
                        miniHeight = mini.Height;
                    }
                    Bitmap newImage = new Bitmap(mini, (int)miniWidth, (int)miniHeight);
                    newImage.Save(AppRootPath() + MiniToLink);
                    mini.Dispose();
                }
            }

            //Remove record
            records.Remove();

            //### INSERT record ###
            xmlDoc.Element("myCollectionsDb").Element("myElements").Add(new XElement("myElement",
                                                                                    new XElement("id", nodeToUpdate.ToString()),
                                                                                    new XElement("Name", Name),
                                                                                    new XElement("Description", Description),
                                                                                    new XElement("MemberOf", MemberOf),
                                                                                    new XElement("Image", ImageToLink),
                                                                                    new XElement("Miniature", MiniToLink),
                                                                                    new XElement("GotIt", GotIt),
                                                                                    new XElement("Rating", RatingLvl)
                                                                                    )
                                                                               );

            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");
        }
        #endregion

        #region Delete
        public void Element_ProcessDelete(Int32 nodeToDelete)
        {
            XDocument xmlDoc = XDocument.Load(AppRootPath() + "MyDB/MyCollectionDB.xml");
            XElement collec = xmlDoc.Root.Element("myElements");
            var records = from myCollection in collec.Elements("myElement")
                          where (int)myCollection.Element("id") == nodeToDelete
                          select myCollection;
            foreach (var myCollection in records)
            {
                String FileToDelete = AppRootPath() + myCollection.Element("Image").Value;
                String MiniToDelete = AppRootPath() + myCollection.Element("Miniature").Value;
                if (File.Exists(FileToDelete))
                {
                    File.Delete(FileToDelete);
                    File.Delete(MiniToDelete);
                }
            }
            records.Remove();
            xmlDoc.Save(AppRootPath() + "MyDB/MyCollectionDB.xml");

        }
        #endregion

        #endregion
    }

    public static class GlobalFunction
    {
        public static string Right(this string value, int length)
        {
            if (String.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

       
    }

    public class ComboBoxItem
    {
       string displayValue;
       string hiddenValue;

       //Constructor
       public ComboBoxItem (string d, string h)
       {
            displayValue = d;
            hiddenValue = h;
       }

       //Accessor
       public string HiddenValue
       {
            get{return hiddenValue;}    
       }

       //Override ToString method
       public override string ToString()
       {
            return displayValue;
       }
    }

    public class CollectionItems
    {
        public int Id = 0;
        public string Name = "";
        public string Description = "";
        public string Picture = "";

        public CollectionItems(int id, string name, string picture, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            Picture = picture;
        }
    }
}

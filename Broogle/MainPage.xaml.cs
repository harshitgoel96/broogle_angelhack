using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.Windows.Resources;

namespace Broogle
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask photoCameraCapture;
        byte[] byteArray;
        public MainPage()
        {
            InitializeComponent();
            this.photoChooserTask = new PhotoChooserTask();
            this.photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            this.photoCameraCapture = new CameraCaptureTask();
            this.photoCameraCapture.Completed += new EventHandler<PhotoResult>(photoCameraCapture_Completed);
        }
        private void photoCameraCapture_Completed(object sender, PhotoResult e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
               // image1.Source = image;
                SaveToIsolatedStorage(e.ChosenPhoto, "temp.jpg");
                using (MemoryStream ms = new MemoryStream())
                {
                    WriteableBitmap btmMap = new WriteableBitmap(image);

                    // write an image into the stream
                    Extensions.SaveJpeg(btmMap, ms, image.PixelWidth,image.PixelHeight, 0, 100);

                    byteArray = ms.ToArray();
                    image.SetSource(ms);
                    image1.Source = image;

                }

            }
            catch (ArgumentNullException) { /* Nothing */ }
        }
        private void SaveToIsolatedStorage(Stream imageStream, string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    myIsolatedStorage.DeleteFile(fileName);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(imageStream);

                WriteableBitmap wb = new WriteableBitmap(bitmap);
                wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                fileStream.Close();
            }
        }
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                image1.Source = image;
                SaveToIsolatedStorage(e.ChosenPhoto, "temp.jpg");

                using (MemoryStream ms = new MemoryStream())
                {
                    WriteableBitmap btmMap = new WriteableBitmap(image);

                    // write an image into the stream
                    Extensions.SaveJpeg(btmMap, ms, image.PixelWidth, image.PixelHeight, 0, 100);

                    byteArray = ms.ToArray();
                    
                   
                }

            }
            catch (ArgumentNullException) { /* Nothing */ }
        }

        private void FromGallary_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        private void fromCamera_Click(object sender, RoutedEventArgs e)
        {
            photoCameraCapture.Show();
        }

        private void letsDoIt_Click(object sender, RoutedEventArgs e)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists("array.txt"))
                {
                    store.DeleteFile("array.txt");
                }
                using (var stream = new IsolatedStorageFileStream("array.txt",
                              FileMode.Create, FileAccess.Write, store))
                {
                    
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Close();
                }
                
            }
            
        }
    }
}
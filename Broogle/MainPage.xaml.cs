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
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Phone.Shell;

namespace Broogle
{
    public partial class MainPage : PhoneApplicationPage
    {
        public string url;
        public Dictionary<string, object> parameters;
        string boundary = "----------" + DateTime.Now.Ticks.ToString();

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
        
                using (MemoryStream ms = new MemoryStream())
                {
                    WriteableBitmap btmMap = new WriteableBitmap(image);

                    // write an image into the stream
                    Extensions.SaveJpeg(btmMap, ms, 450,450, 0, 100);

                    byteArray = ms.ToArray();
                    image.SetSource(ms);
                    image1.Source = image;

                }

            }
            catch (ArgumentNullException) { /* Nothing */ }
        }
        
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                image1.Source = image;
        

                using (MemoryStream ms = new MemoryStream())
                {
                    WriteableBitmap btmMap = new WriteableBitmap(image);

                    // write an image into the stream
                    Extensions.SaveJpeg(btmMap, ms, 450,450, 0, 100);

                    byteArray = ms.ToArray();
                    MessageBox.Show("lenth here=" + byteArray.Length);
                   
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

        public void Submit()
        {

            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://query-api.kooaba.com/v4/query", UriKind.Absolute));
            myRequest.Method = "POST";
            myRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            myRequest.Headers["Authorization"] = " Token ZxIdYnP7M7kruyvNdhaWRYsB67SFpT0ynhfWY4rY";
            myRequest.Accept = "application/json";


            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            writeMultipartObject(postStream, parameters);
            postStream.Close();
            //MessageBox.Show("Check output");

            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }
        string data;
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            string Id="";
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            //MessageBox.Show(streamRead.ReadToEnd());
            data = streamRead.ReadToEnd();
            System.Diagnostics.Debug.WriteLine("response code:" + response.StatusCode);
            //System.Diagnostics.Debug.WriteLine(" stream read data" + streamRead.ReadToEnd());
            // System.Diagnostics.Debug.WriteLine("  resp read data" + response.ReadToEnd());
            streamResponse.Close();
            streamRead.Close();
            // Release the HttpWebResponse
            response.Close();
            //System.Diagnostics.Debug.WriteLine(" response " + response.ToString());
            //System.Diagnostics.Debug.WriteLine(" stream read " + streamRead.ToString());
            System.Diagnostics.Debug.WriteLine(data);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                byte[] byteArrayed = Encoding.UTF8.GetBytes(data);
                MemoryStream streamed = new MemoryStream(byteArrayed);
                MessageBox.Show(data);
                streamed.Position = 0;
                var deserialized = (RootObject)serializer.ReadObject(streamed);

                foreach (var value in deserialized.results)
                {
                    Id = value.reference_id;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    PhoneApplicationService.Current.State["Id"] = Id;
                    NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
                }
                

            });

        }


        public void writeMultipartObject(Stream stream, object data)
        {
            StreamWriter writer = new StreamWriter(stream);
            if (data != null)
            {
                foreach (var entry in data as Dictionary<string, object>)
                {
                    WriteEntry(writer, entry.Key, entry.Value);
                }
            }
            writer.Write("--");
            writer.Write(boundary);
            writer.WriteLine("--");
            writer.Flush();
        }

        private void WriteEntry(StreamWriter writer, string key, object value)
        {
            System.Diagnostics.Debug.WriteLine("Writing the image1");
            if (value != null)
            {
                writer.Write("--");
                writer.WriteLine(boundary);
                if (value is byte[])
                {
                    byte[] ba = value as byte[];

                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""", key, "image.jpg");
                    writer.WriteLine(@"Content-Type: application/octet-stream");
                    //writer.WriteLine(@"Content-Type: image / jpeg");
                    writer.WriteLine(@"Content-Length: " + ba.Length);
                    writer.WriteLine();
                    writer.Flush();
                    System.Diagnostics.Debug.WriteLine("Writen the image1********");
                    Stream output = writer.BaseStream;

                    output.Write(ba, 0, ba.Length);
                    output.Flush();
                    writer.WriteLine();
                }
                else
                {
                    writer.WriteLine(@"Content-Disposition: form-data; name=""{0}""", key);
                    writer.WriteLine();
                    writer.WriteLine(value.ToString());
                }
            }
        }


        private void letsDoIt_Click(object sender, RoutedEventArgs e)
        {
            if (byteArray.Length > 0 && image1.Source != null)
            {
                Dictionary<string, object> data = new Dictionary<string, object>() { { "image", byteArray } };

                            parameters = data;

                            Submit();
                //NavigationService.Navigate(new Uri("/Page1.xaml",UriKind.Relative));
            }
            
        }
    }
}
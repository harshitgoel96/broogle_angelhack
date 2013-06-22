﻿using System;
using System.Runtime.Serialization.Json;
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

using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace Broogle
{
    public partial class Page1 : PhoneApplicationPage
    {

        public Page1()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //if ((App.Current as App).isDataFilled == true)
            //{
            //    RootObject newObj =(RootObject) (App.Current as App).dataAtApp;
            //    foreach (var metaData in newObj.results)
            //    {
            //        canvas1.ItemsSource = metaData.metadata;
            //    }
            //}
        }


        public string url;
        public Dictionary<string, object> parameters;
        string boundary = "----------" + DateTime.Now.Ticks.ToString();





        public void Submit()
        {
            
            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://query-api.kooaba.com/v4/query",UriKind.Absolute));
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
            System.Diagnostics.Debug.WriteLine("*****************************");
            System.Diagnostics.Debug.WriteLine(postStream.Length.ToString());
            System.Diagnostics.Debug.WriteLine("*****************************");
            postStream.Close();
            //MessageBox.Show("Check output");
           
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);
        }
        string data;
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {

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
                streamed.Position=0;
                var deserialized = (RootObject)serializer.ReadObject(streamed);
                canvas1.DataContext = deserialized;
                MessageBox.Show("EO CODE");
                //using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(data)))
                //{
                    //var datas = (RootObject)serializer.ReadObject(stream);
                    //using (var streamd = new MemoryStream(Encoding.Unicode.GetBytes(data)))
                    //{
                      //  (App.Current as App).dataAtApp = (RootObject)serializer.ReadObject(streamd);
                       // (App.Current as App).isDataFilled = true;
                    //}
                    //newPage.NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
                
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

        private void page1Loaded(object sender, RoutedEventArgs e)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists("array.txt"))
                {
                    using (var stream = new IsolatedStorageFileStream("array.txt",
                                  FileMode.Open, FileAccess.Read, store))
                    {
                        byte[] bytes = new byte[stream.Length];
                        //MessageBox.Show("Array length " + stream.Length);
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;
                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead. 
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached. 
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                        stream.Close();
                        MessageBox.Show(bytes.Length.ToString());
                        Dictionary<string, object> data = new Dictionary<string, object>() { { "image", bytes } };

                        parameters = data;
                        
                        Submit();
                    }
                }
            }
        }
    }
}
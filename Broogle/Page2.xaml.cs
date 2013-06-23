using System;
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
using System.Text;
using Newtonsoft.Json;
using Microsoft.Phone.Shell;

namespace Broogle
{
    public partial class Page2 : PhoneApplicationPage
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void PanoramaItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            getTopRatedProduct();
        }
        private void getTopRatedProduct()
        {
            string responsed="";
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchDataArray));
            string url = "http://indiancardists.com/topRated.php";
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.BeginGetResponse(r =>
            {
                var httpRequest = (HttpWebRequest)r.AsyncState;
                var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                
                using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string response = reader.ReadToEnd();
                    Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int stop = response.LastIndexOf("}");
                        responsed = response.Substring(0, stop +1);
                        MessageBox.Show(responsed);
                        SearchDataArray ent = JsonConvert.DeserializeObject<SearchDataArray>(responsed) as SearchDataArray;
                        foreach (var da in ent.Data)
                        {
                            TopResults.Items.Add (da);
                        }
                        
                    }));
           
                }
            }, request);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getSearchResult(SearchBox.Text.Replace(" ","%20"));

        }
        void getSearchResult(string value)
        {
            string responsed = "";
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchDataArray));
            string url = "http://indiancardists.com/nameSearch.php?name="+value;
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.BeginGetResponse(r =>
            {
                var httpRequest = (HttpWebRequest)r.AsyncState;
                var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);

                using (var reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string response = reader.ReadToEnd();
                    Deployment.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int stop = response.LastIndexOf("}");
                        responsed = response.Substring(0, stop + 1);
                        MessageBox.Show(responsed);
                        SearchDataArray ent = JsonConvert.DeserializeObject<SearchDataArray>(responsed) as SearchDataArray;
                        foreach (var da in ent.Data)
                        {
                            searchResult.Items.Add(da);
                        }

                    }));

                }
            }, request);
        }

        private void searchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchDataModel value = (SearchDataModel)this.searchResult.SelectedItem;
            PhoneApplicationService.Current.State["Id"] = value.Id ;
NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
            //MessageBox.Show(value.Id);
        }

        private void TopResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchDataModel value = (SearchDataModel)this.TopResults.SelectedItem;
            PhoneApplicationService.Current.State["Id"] = value.Id;
            
            NavigationService.Navigate(new Uri("/Page1.xaml", UriKind.Relative));
        }
    }
}
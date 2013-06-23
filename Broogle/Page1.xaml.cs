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
using System.IO.IsolatedStorage;
using System.Text;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

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
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string k = PhoneApplicationService.Current.State["Id"].ToString();
            if (!string.IsNullOrEmpty(k))
            {
                getData(k);
            }
            base.OnNavigatedTo(e);
        }


        void getData(string id) { 
        string responsed = "";
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchDataArray));
            string url = "http://indiancardists.com/getData.php?id=" + id;
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
                        MasterDataModel ent = JsonConvert.DeserializeObject<MasterDataModel>(responsed) as MasterDataModel;
                        
                        shoeRev.Visibility = Visibility.Visible;
                        shoeRev.IsEnabled = true;
                        if (ent.IsLocation == 1)
                        {
                            GetLocation.Visibility = Visibility.Visible;
                            GetLocation.IsEnabled = true;
                            
                        }
                        hiden.Text = ent.Id.ToString();
                        name.Text = ent.Name;
                        itemType.Text = ent.Type;
                        buy.Text = ent.Buy;
                        desc.Text = ent.Desc;
                        refID.Text = ent.Id.ToString();
                        Price.Text = ent.Price;
                        use.Text = ent.Uses;
                    }));

                }
            }, request);}


        
        private void page1Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void GetLocation_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["Id"] = hiden.Text;
            NavigationService.Navigate(new Uri("/Page3.xaml", UriKind.Relative));
        }

        private void shoeRev_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["Id"] = hiden.Text;
            NavigationService.Navigate(new Uri("/Page4.xaml", UriKind.Relative));
        }
    }
}
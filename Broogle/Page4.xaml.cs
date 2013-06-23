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
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System.IO;

namespace Broogle
{
    public partial class Page4 : PhoneApplicationPage
    {
        public Page4()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            MessageBox.Show("Onnevegated to called");
            string k = PhoneApplicationService.Current.State["Id"].ToString();
            if (!string.IsNullOrEmpty(k))
            {
                MessageBox.Show("got the id");
                getData(k);
            }
            base.OnNavigatedTo(e);
        }
        private void getData(string id)
        {
            string responsed = "";
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchDataArray));
            string url = "http://indiancardists.com/getReviews.php?pid=" + id;
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
                        reviewModel ent = JsonConvert.DeserializeObject<reviewModel>(responsed) as reviewModel;
                        foreach (var d in ent.Data)
                        {
                            MessageBox.Show(d.ReviewText);
                        }
                    }));

                }
            }, request);
        }    
    }
}
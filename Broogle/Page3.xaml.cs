﻿using System;
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
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Phone.Controls.Maps;

namespace Broogle
{
    public partial class Page3 : PhoneApplicationPage
    {
        public Page3()
        {
            InitializeComponent();
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
        private void getData(string id)
        {
            string responsed = "";
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchDataArray));
            string url = "http://indiancardists.com/getLocations.php?pid=" + id;
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
                        locationModel ent = JsonConvert.DeserializeObject<locationModel>(responsed) as locationModel;
                        Pushpin pin ;

                        //List<Pushpin> pinlist=new List<Pushpin>();
                        foreach (var loc in ent.Data)
                        {

                            
                            pin = new Pushpin();
                            pin.Location = new System.Device.Location.GeoCoordinate(Convert.ToDouble(loc.Lat), Convert.ToDouble(loc.Lon));
                            pin.Content = loc.Name;
                            map1.Children.Add(pin);
                            map1.SetView( LocationRect.CreateLocationRect(pin.Location));
                        }
                        
                        map1.ZoomLevel = 12;
                        
                        //name.Text = ent.Name;
                        //itemType.Text = ent.Type;
                        //buy.Text = ent.Buy;
                        //desc.Text = ent.Desc;
                        //refID.Text = ent.Id.ToString();
                        //price.Text = ent.Price;
                        //use.Text = ent.Uses;
                    }));

                }
            }, request);
        }
    }
}
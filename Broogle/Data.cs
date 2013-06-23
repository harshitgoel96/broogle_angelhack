using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.ComponentModel;

namespace Broogle
{

    public class Metadata : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _name;
        private string _Category;
        private string _itemType;
        private string _Manufacture_country;
        private string _Uses;
        private string _buy;
        private string _description;
        // private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("name");
            }
        }
        public string Category
        {
            get { return _Category; }
            set
            {
                _Category = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Category");
            }
        }
        public string itemType
        {
            get { return _itemType; }
            set
            {
                _itemType = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("itemType");
            }
        }
        public string Manufacture_country
        {
            get { return _Manufacture_country; }
            set
            {
                _Manufacture_country = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Manufacture_country");
            }
        }
        public string Uses
        {
            get { return _Uses; }
            set
            {
                _Uses = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Uses");
            }
        }
        public string buy
        {
            get { return _buy; }
            set
            {
                _buy = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("buy");
            }
        }
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("description");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class ReferenceProjection
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class BoundingBox
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Recognition
    {
        public string id { get; set; }
        public double score { get; set; }
        public List<ReferenceProjection> reference_projection { get; set; }
        public List<BoundingBox> bounding_box { get; set; }
    }

    public class Result
    {
        public double score { get; set; }
        public List<Metadata> metadata { get; set; }
        public string service_id { get; set; }
        public string item_uuid { get; set; }
        public string bucket_uuid { get; set; }
        public List<Recognition> recognitions { get; set; }
        public string title { get; set; }
        public object locale { get; set; }
        public string reference_id { get; set; }
    }

    public class RootObject
    {
        public string query_id { get; set; }
        public List<Result> results { get; set; }
    }

    public class SearchDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
    }
    public class SearchDataArray
    {
        public SearchDataModel[] Data { get; set; }
    }
    public class MasterDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Rating { get; set; }
        public string Uses { get; set; }
        public string Buy { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public int IsLocation { get; set; }
        public string menuf_country { get; set; }
    }
    public class Data_loc
    {

     
        public int Id;

     
        public string Name;

     
        public string Lat;

     
        public string Lon;

     
        public string Rat;

     
        public int Prodid;
    }

    public class locationModel
    {

     
        public Data_loc[] Data;
    }
    public class Data_rev
    {


        public int Id;


        public string ReviewText;


        public string ReviewerName;


        public int Prodid;


        public int Likes;
    }

    public class reviewModel
    {


        public Data_rev[] Data;
    }

}

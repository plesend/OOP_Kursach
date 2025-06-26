using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lab4_5
{
    public class Product : INotifyPropertyChanged
    {
        public ObservableCollection<string> Reviews { get; set; } = new ObservableCollection<string>();

        public int id { get; set; } 

        private string _name;
        public string Name
        {
            get => _name;
            set { if (_name != value) { _name = value; OnPropertyChanged(); } }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { if (_description != value) { _description = value; OnPropertyChanged(); } }
        }

        private string _brand;
        public string Brand
        {
            get => _brand;
            set { if (_brand != value) { _brand = value; OnPropertyChanged(); } }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set { if (_imagePath != value) { _imagePath = value; OnPropertyChanged(); } }
        }

        private string _brandDescription = "Lorem ipsum dolor sit amet. Aut molestiae incidunt ea eius possimus At laboriosam rerum At fugit veritatis. Hic debitis veritatis et galisum nobis vel dolorum numquam ut provident ipsum et exercitationem vero eos temporibus dolore. ";
        public string BrandDescription
        {
            get => _brandDescription;
            set { if (_brandDescription != value) { _brandDescription = value; OnPropertyChanged(); } }
        }

        private string _composition = "Lorem ipsum dolor sit amet. Aut molestiae incidunt ea eius possimus At laboriosam rerum At fugit veritatis. Hic debitis veritatis et galisum nobis vel dolorum numquam ut provident ipsum et exercitationem vero eos temporibus dolore. ";
        public string Composition
        {
            get => _composition;
            set { if (_composition != value) { _composition = value; OnPropertyChanged(); } }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set { if (_price != value) { _price = value; OnPropertyChanged(); } }
        }

        public Product(int id, string name, string description, string brand, string imagePath, double price, string composition)
        {
            this.id = id;
            Name = name;
            Description = description;
            Brand = brand;
            ImagePath = imagePath;
            Price = price;
            Composition = composition;
        }

        public Product(string name, string description, string brand, string imagePath, double price, string composition)
        {
            Name = name;
            Description = description;
            Brand = brand;
            ImagePath = imagePath;
            Price = price;
            Composition = composition;
        }

        public Product() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

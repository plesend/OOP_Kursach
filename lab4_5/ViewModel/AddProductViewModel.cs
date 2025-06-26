using lab4_5;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows;

public class AddProductViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public string Name { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Composition { get; set; }
    public string PriceText { get; set; }
    public string ImagePath { get; set; }

    public Product newProductReturned { get; set; }

    public Action CloseAction { get; set; }

    public RelayCommand AddProductCommand { get; }
    public RelayCommand BrowseImageCommand { get; }

    public AddProductViewModel()
    {
        AddProductCommand = new RelayCommand(_ => AddProduct());
        BrowseImageCommand = new RelayCommand(_ => BrowseImage());
    }

    private void AddProduct()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Type) ||
            string.IsNullOrWhiteSpace(Brand) ||
            string.IsNullOrWhiteSpace(Composition) ||
            string.IsNullOrWhiteSpace(PriceText) ||
            string.IsNullOrWhiteSpace(ImagePath))
        {
            MessageBox.Show("Пожалуйста, заполните все поля.");
            return;
        }

        string[] allowedBrands = { "Essence", "NYX", "Revolution", "Maybelline", "L'OREAL" };
        if (Array.IndexOf(allowedBrands, Brand.Trim()) == -1)
        {
            MessageBox.Show("Допустимые бренды: Essence, NYX, Revolution, Maybelline, L'OREAL.");
            return;
        }

        if (!double.TryParse(PriceText, out double price))
        {
            MessageBox.Show("Цена должна быть числом.");
            return;
        }

        Product newProduct = new Product(Name, Type, Brand, ImagePath, price, Composition);

        newProductReturned = newProduct;
        CloseAction?.Invoke();
    }

    private void BrowseImage()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Все файлы (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };

        if (dialog.ShowDialog() == true)
        {
            ImagePath = dialog.FileName.Replace("\\", "/");
            OnPropertyChanged(nameof(ImagePath));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

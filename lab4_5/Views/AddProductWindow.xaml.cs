using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System.IO;


namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();

            var viewModel = new AddProductViewModel();
            viewModel.CloseAction = () => this.Close();

            this.DataContext = viewModel;
        }
        //public MainWindow main;

        //public Window1(MainWindow main)
        //{
        //    InitializeComponent();
        //    this.main = main;
        //}

        //public void AddProductDefinition_Click(object sender, RoutedEventArgs e)
        //{
        //    string name = ProdName.Text;
        //    string type = ProdType.Text;
        //    string brand = ProdBrand.Text;
        //    string priceText = ProdPrice.Text;
        //    string imagePath = ProdPhotoPath.Text;

        //    if (string.IsNullOrWhiteSpace(name) ||
        //        string.IsNullOrWhiteSpace(type) ||
        //        string.IsNullOrWhiteSpace(brand) ||
        //        string.IsNullOrWhiteSpace(priceText) ||
        //        string.IsNullOrWhiteSpace(imagePath))
        //    {
        //        MessageBox.Show("Пожалуйста, заполните все поля.");
        //        return;
        //    }

        //    if (!int.TryParse(priceText, out int price))
        //    {
        //        MessageBox.Show("Цена должна быть числом.");
        //        return;
        //    }

        //    var newProduct = new Product
        //    {
        //        Name = name,
        //        Description = type,
        //        Brand = brand,
        //        ImagePath = imagePath,
        //        Price = price
        //    };

        //    string jsonPath = "D:\\лабораторные работы\\ооп\\lab4_5\\pics\\products.json";

        //    try
        //    {
        //        List<Product> allProducts = new List<Product>();

        //        if (File.Exists(jsonPath))
        //        {
        //            string existingJson = File.ReadAllText(jsonPath);
        //            allProducts = JsonConvert.DeserializeObject<List<Product>>(existingJson) ?? new List<Product>();
        //        }

        //        allProducts.Add(newProduct);

        //        string updatedJson = JsonConvert.SerializeObject(allProducts, Formatting.Indented);
        //        File.WriteAllText(jsonPath, updatedJson);

        //        main.products = allProducts;
        //        main.CatalogPanel.Children.Clear();
        //        main.LoadProducts(main.products, main.currentUser);


        //        this.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка при работе с JSON: {ex.Message}");
        //    }

        //}

        //private void BrowseImage_Click(object sender, RoutedEventArgs e)
        //{
        //    var dialog = new OpenFileDialog();
        //    dialog.Filter = "Изображения (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Все файлы (*.*)|*.*";
        //    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        //    if (dialog.ShowDialog() == true)
        //    {
        //        // Заменяем \ на /, чтобы путь был валиден и единообразен
        //        ProdPhotoPath.Text = dialog.FileName.Replace("\\", "/");
        //    }
        //}

    }
}

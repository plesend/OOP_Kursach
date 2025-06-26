using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace lab4_5
{
    public class EditProductViewModel : INotifyPropertyChanged
    {
        private readonly string connectionString =
            "Data Source=WIN-0RRORC9T71J\\SQLEXPRESS;Initial Catalog=CosmeticShop;Integrated Security=True;TrustServerCertificate=True;";

        private Product _originalProduct;
        public Product UpdatedProduct { get; set; }

        public ICommand BrowseImageCommand { get; }
        public ICommand SaveProductCommand { get; }

        public EditProductViewModel(Product product)
        {
            _originalProduct = product ?? throw new ArgumentNullException(nameof(product));

            UpdatedProduct = new Product
            {
                id = product.id,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Composition = product.Composition,
                ImagePath = product.ImagePath,
                Price = product.Price
            };

            BrowseImageCommand = new RelayCommand(_ => BrowseImage());
            SaveProductCommand = new RelayCommand(_ => SaveProduct());
        }

        private void BrowseImage()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                UpdatedProduct.ImagePath = dlg.FileName;
                OnPropertyChanged(nameof(UpdatedProduct));
            }
        }

        private void SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(UpdatedProduct.Name) ||
                string.IsNullOrWhiteSpace(UpdatedProduct.Brand) ||
                string.IsNullOrWhiteSpace(UpdatedProduct.ImagePath))
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: название, бренд и путь к изображению.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UpdatedProduct.Price <= 0)
            {
                MessageBox.Show("Цена должна быть положительным числом.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string[] allowedBrands = { "Essence", "NYX", "Revolution", "Maybelline", "L'OREAL" };
            if (Array.IndexOf(allowedBrands, UpdatedProduct.Brand.Trim()) == -1)
            {
                MessageBox.Show("Допустимые бренды: Essence, NYX, Revolution, Maybelline, L'OREAL.");
                return;
            }

            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using var transaction = conn.BeginTransaction();

                try
                {
                    int brandId;
                    using (var checkBrandCmd = new SqlCommand("SELECT BrandId FROM Brands WHERE BrandName = @BrandName", conn, transaction))
                    {
                        checkBrandCmd.Parameters.AddWithValue("@BrandName", UpdatedProduct.Brand);
                        var result = checkBrandCmd.ExecuteScalar();

                        if (result != null)
                        {
                            brandId = Convert.ToInt32(result);
                        }
                        else
                        {
                            using var insertBrandCmd = new SqlCommand(
                                "INSERT INTO Brands (BrandName) OUTPUT INSERTED.BrandId VALUES (@BrandName)", conn, transaction);
                            insertBrandCmd.Parameters.AddWithValue("@BrandName", UpdatedProduct.Brand);
                            brandId = (int)insertBrandCmd.ExecuteScalar();
                        }
                    }

                    const string updateQuery = @"
                        UPDATE Goods
                        SET Name = @Name,
                            Description = @Description,
                            Brand = @Brand,
                            ImagePath = @ImagePath,
                            Price = @Price,
                            Composition = @Composition,
                            BrandId = @BrandId
                        WHERE Id = @Id";

                    using var cmd = new SqlCommand(updateQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@Id", UpdatedProduct.id);
                    cmd.Parameters.AddWithValue("@Name", UpdatedProduct.Name);
                    cmd.Parameters.AddWithValue("@Description", (object?)UpdatedProduct.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Brand", UpdatedProduct.Brand);
                    cmd.Parameters.AddWithValue("@ImagePath", UpdatedProduct.ImagePath);
                    cmd.Parameters.AddWithValue("@Price", UpdatedProduct.Price);
                    cmd.Parameters.AddWithValue("@Composition", (object?)UpdatedProduct.Composition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BrandId", brandId);

                    cmd.ExecuteNonQuery();

                    transaction.Commit();

                    // Сохраняем в оригинальный объект
                    _originalProduct.Name = UpdatedProduct.Name;
                    _originalProduct.Description = UpdatedProduct.Description;
                    _originalProduct.Brand = UpdatedProduct.Brand;
                    _originalProduct.ImagePath = UpdatedProduct.ImagePath;
                    _originalProduct.Price = UpdatedProduct.Price;
                    _originalProduct.Composition = UpdatedProduct.Composition;

                    MessageBox.Show("Товар успешно обновлён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window currentWindow)
                    {
                        currentWindow.DialogResult = true;
                        currentWindow.Close();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Ошибка при сохранении товара: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

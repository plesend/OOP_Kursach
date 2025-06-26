using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static lab4_5.MainWindowViewModel;

namespace lab4_5
{
    public class CartWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CartItem> CartItems { get; set; } = new();
        public double TotalAmount => CartItems.Sum(item => item.Total);

        public string connectionString = "Data source=WIN-0RRORC9T71J\\SQLEXPRESS;Initial Catalog=CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;";
        private int userId;
        public bool CanCheckout => CartItems.Any();

        public ICommand RemoveProductFromCartCommand { get; }
        public ICommand ClearCartCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand OpenOrderWindowCommand { get; }

        public CartWindowViewModel(User user)
        {
            userId = user.Id;

            RemoveProductFromCartCommand = new RelayCommand<CartItem>(DelProductFromCart);
            ClearCartCommand = new RelayCommand(ClearCart);
            OpenOrderWindowCommand = new RelayCommand(OpenOrderWindow);
            IncreaseQuantityCommand = new RelayCommand<CartItem>(IncreaseQuantity);
            DecreaseQuantityCommand = new RelayCommand<CartItem>(DecreaseQuantity);

            LoadCartItems(user);
        }

        public void LoadCartItems(User user)
        {
            CartItems.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT g.Id AS ProductId, c.CartId, g.Name, g.Brand, g.Price, g.ImagePath, ci.Quantity
                    FROM CartItems ci
                    JOIN Carts c ON ci.CartId = c.CartId
                    JOIN Goods g ON ci.ProductId = g.Id
                    WHERE c.UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", user.Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new CartItem
                            {
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                CartId = Convert.ToInt32(reader["CartId"]),
                                ProductName = reader["Name"].ToString(),
                                Brand = reader["Brand"].ToString(),
                                Price = Convert.ToDouble(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                ImagePath = reader["ImagePath"].ToString()
                            };

                            item.PropertyChanged += CartItem_PropertyChanged;
                            CartItems.Add(item);
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(CanCheckout));
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CartItem.Quantity) ||
                e.PropertyName == nameof(CartItem.Total))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public void IncreaseQuantity(CartItem item)
        {
            if (item == null) return;

            item.Quantity++;
            UpdateQuantityInDb(item);
        }

        public void DecreaseQuantity(CartItem item)
        {
            if (item == null || item.Quantity <= 1) return;

            item.Quantity--;
            UpdateQuantityInDb(item);
        }

        private void UpdateQuantityInDb(CartItem item)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE CartItems SET Quantity = @Quantity WHERE CartId = @CartId AND ProductId = @ProductId", conn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@CartId", item.CartId);
                    cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DelProductFromCart(CartItem itemToRemove)
        {
            if (itemToRemove == null) return;

            var result = MessageBox.Show("Вы точно хотите удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM CartItems WHERE CartId = @CartId AND ProductId = @ProductId", conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@CartId", itemToRemove.CartId);
                    cmd.Parameters.AddWithValue("@ProductId", itemToRemove.ProductId);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        transaction.Commit();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Товар удален из корзины!");
                            itemToRemove.PropertyChanged -= CartItem_PropertyChanged;
                            CartItems.Remove(itemToRemove);
                            OnPropertyChanged(nameof(TotalAmount));
                            OnPropertyChanged(nameof(CanCheckout));
                        }
                        else
                        {
                            MessageBox.Show("Товар не найден в базе данных!");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
        }

        public void ClearCart()
        {
            var result = MessageBox.Show("Вы точно хотите очистить корзину?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
                    DELETE ci
                    FROM CartItems ci
                    JOIN Carts c ON ci.CartId = c.CartId
                    WHERE c.UserId = @UserId";

                using (SqlTransaction transaction = conn.BeginTransaction())
                using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        transaction.Commit();

                        foreach (var item in CartItems)
                        {
                            item.PropertyChanged -= CartItem_PropertyChanged;
                        }

                        CartItems.Clear();
                        MessageBox.Show("Корзина очищена");
                        OnPropertyChanged(nameof(TotalAmount));
                        OnPropertyChanged(nameof(CanCheckout));
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка при очистке корзины: " + ex.Message);
                    }
                }
            }
        }

        private void OpenOrderWindow()
        {
            var orderWindow = new OrderWindow(userId, CartItems);
            orderWindow.ShowDialog();
            LoadCartItems(new User { Id = userId });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

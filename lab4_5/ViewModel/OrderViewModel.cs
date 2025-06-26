using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace lab4_5
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private static readonly Regex PhoneRegex = new Regex(@"^\+\d{10,15}$");

        private string deliveryMethod = "Доставка";
        public string DeliveryMethod
        {
            get => deliveryMethod;
            set
            {
                deliveryMethod = value;
                OnPropertyChanged(nameof(DeliveryMethod));
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string street;
        public string Street
        {
            get => street;
            set
            {
                street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private string apartment;
        public string Apartment
        {
            get => apartment;
            set
            {
                apartment = value;
                OnPropertyChanged(nameof(Apartment));
            }
        }

        private string building;
        public string Building
        {
            get => building;
            set
            {
                building = value;
                OnPropertyChanged(nameof(Building));
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        private string paymentMethod = "Наличными курьеру";
        public string PaymentMethod
        {
            get => paymentMethod;
            set
            {
                paymentMethod = value;
                OnPropertyChanged(nameof(PaymentMethod));
            }
        }

        public ObservableCollection<string> PickupPoints { get; set; } = new ObservableCollection<string>
        {
            "Минск, ТЦ Галерея, 2 этаж",
            "Минск, ТЦ Галилео, 7 этаж",
            "Лида, ТЦ Лидапарк, 2 этаж",
            "Гродно, ТЦ TRINITI, 2 этаж"
        };

        private string selectedPickupPoint;
        public string SelectedPickupPoint
        {
            get => selectedPickupPoint;
            set
            {
                selectedPickupPoint = value;
                OnPropertyChanged(nameof(SelectedPickupPoint));
            }
        }

        public ObservableCollection<CartItem> CartItems { get; set; }

        public ICommand ConfirmOrderCommand { get; }

        private int userId;

        private string connectionString = "Data source=WIN-0RRORC9T71J\\SQLEXPRESS;Initial Catalog=CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;";

        public OrderViewModel(int userId, ObservableCollection<CartItem> cartItems)
        {
            this.userId = userId;
            CartItems = cartItems;
            ConfirmOrderCommand = new RelayCommand(ConfirmOrder, CanConfirmOrder);
        }

        private bool CanConfirmOrder(object parameter)
        {
            return !string.IsNullOrWhiteSpace(FullName)
                && !string.IsNullOrWhiteSpace(Phone)
                && CartItems.Count > 0;
        }

        private void ConfirmOrder(object parameter)
        {
            if (!PhoneRegex.IsMatch(Phone))
            {
                MessageBox.Show("Номер телефона должен начинаться с '+' и содержать от 10 до 15 цифр.", "Ошибка формата номера", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool hasAddress = !string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Street);
            bool hasPickupPoint = !string.IsNullOrWhiteSpace(SelectedPickupPoint);

            if (!hasAddress && !hasPickupPoint)
            {
                MessageBox.Show("Пожалуйста, укажите либо адрес доставки, либо выберите пункт самовывоза.", "Недостаточно информации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertOrderSql = @"
                            INSERT INTO Orders (
                                UserId, FullName, Phone, DeliveryMethod, PaymentMethod,
                                City, Street, Apartment, Building, PickupPoint, Comment, OrderDate)
                            VALUES (
                                @UserId, @FullName, @Phone, @DeliveryMethod, @PaymentMethod,
                                @City, @Street, @Apartment, @Building, @PickupPoint, @Comment, GETDATE());
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdOrder = new SqlCommand(insertOrderSql, conn, transaction);
                        cmdOrder.Parameters.AddWithValue("@UserId", userId);
                        cmdOrder.Parameters.AddWithValue("@FullName", FullName);
                        cmdOrder.Parameters.AddWithValue("@Phone", Phone);
                        cmdOrder.Parameters.AddWithValue("@PaymentMethod", PaymentMethod);
                        cmdOrder.Parameters.AddWithValue("@DeliveryMethod", DeliveryMethod);
                        cmdOrder.Parameters.AddWithValue("@Comment", (object)Comment ?? DBNull.Value);

                        if (DeliveryMethod == "Самовывоз")
                        {
                            cmdOrder.Parameters.AddWithValue("@City", DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Street", DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Apartment", DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Building", DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@PickupPoint", (object)SelectedPickupPoint ?? DBNull.Value);
                        }
                        else 
                        {
                            cmdOrder.Parameters.AddWithValue("@City", (object)City ?? DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Street", (object)Street ?? DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Apartment", (object)Apartment ?? DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@Building", (object)Building ?? DBNull.Value);
                            cmdOrder.Parameters.AddWithValue("@PickupPoint", DBNull.Value);
                        }

                        int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                        foreach (var item in CartItems)
                        {
                            SqlCommand cmdItem = new SqlCommand(
                                "INSERT INTO OrderItems (OrderId, ProductId, Quantity, PriceAtPurchase) VALUES (@OrderId, @ProductId, @Quantity, @Price)",
                                conn, transaction);

                            cmdItem.Parameters.AddWithValue("@OrderId", orderId);
                            cmdItem.Parameters.AddWithValue("@ProductId", item.ProductId);
                            cmdItem.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmdItem.Parameters.AddWithValue("@Price", item.Price);
                            cmdItem.ExecuteNonQuery();
                        }

                        string clearCartSql = @"
                            DELETE ci
                            FROM CartItems ci
                            JOIN Carts c ON ci.CartId = c.CartId
                            WHERE c.UserId = @UserId";

                        SqlCommand cmdClearCart = new SqlCommand(clearCartSql, conn, transaction);
                        cmdClearCart.Parameters.AddWithValue("@UserId", userId);
                        cmdClearCart.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Заказ успешно оформлен!");
                        CartItems.Clear();
                        Application.Current.Windows
                            .OfType<Window>()
                            .FirstOrDefault(w => w.IsActive)
                            ?.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка при оформлении заказа: " + ex.Message);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

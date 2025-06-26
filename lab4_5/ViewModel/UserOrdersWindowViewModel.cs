using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;

namespace lab4_5
{
    public class UserOrdersViewModel
    {
        public ObservableCollection<UserOrder> Orders { get; set; }

        public ICommand CancelOrderCommand { get; }

        private string connectionString = "Data source = WIN-0RRORC9T71J\\SQLEXPRESS; Initial Catalog = CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;";

        public UserOrdersViewModel(int userId)
        {
            Orders = new ObservableCollection<UserOrder>();
            CancelOrderCommand = new RelayCommand<UserOrder>(CancelOrder);

            LoadOrders(userId);
        }

        private void LoadOrders(int userId)
        {
            Orders.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"SELECT OrderId, FullName, Phone, DeliveryMethod, City, Street, Apartment, Building, PickupPoint, Comment, OrderDate, Status 
                       FROM Orders WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Orders.Add(new UserOrder
                    {
                        OrderId = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        Phone = reader.GetString(2),
                        DeliveryMethod = reader.GetString(3),
                        City = reader.IsDBNull(4) ? null : reader.GetString(4),
                        Street = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Apartment = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Building = reader.IsDBNull(7) ? null : reader.GetString(7),
                        PickupPoint = reader.IsDBNull(8) ? null : reader.GetString(8),
                        Comment = reader.IsDBNull(9) ? null : reader.GetString(9),
                        OrderDate = reader.GetDateTime(10),
                        Status = reader.GetString(11)
                    });
                }
            }
        }


        private void CancelOrder(UserOrder order)
        {
            if (order == null || order.Status != "Ожидает подтверждения")
                return;

            var result = MessageBox.Show(
                $"Вы действительно хотите отменить заказ №{order.OrderId}?",
                "Подтверждение отмены",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Orders SET Status = 'Отменён' WHERE OrderId = @OrderId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
                cmd.ExecuteNonQuery();
            }

            order.Status = "Отменён"; 
            MessageBox.Show("Заказ отменён.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}

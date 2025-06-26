using lab4_5;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;

public class ManageOrdersViewModel : INotifyPropertyChanged
{
    private readonly string connectionString = "Data Source=WIN-0RRORC9T71J\\SQLEXPRESS;Initial Catalog=CosmeticShop;TrustServerCertificate=Yes;Integrated Security=True;";

    public ObservableCollection<UserOrder> Orders { get; } = new ObservableCollection<UserOrder>();
    public ObservableCollection<OrderItem> OrderItems { get; } = new ObservableCollection<OrderItem>();

    private string searchOrderId;
    public string SearchOrderId
    {
        get => searchOrderId;
        set
        {
            searchOrderId = value;
            OnPropertyChanged(nameof(SearchOrderId));
        }
    }

    private UserOrder selectedOrder;
    public UserOrder SelectedOrder
    {
        get => selectedOrder;
        set
        {
            selectedOrder = value;
            OnPropertyChanged(nameof(SelectedOrder));
        }
    }

    public ICommand FilterCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand DeleteOrderCommand { get; }
    public ICommand ConfirmOrderCommand { get; }

    private List<OrderItem> allOrderItems;

    public ManageOrdersViewModel()
    {
        FilterCommand = new RelayCommand(_ => FilterOrderItems());
        ResetCommand = new RelayCommand(_ => ResetOrderItems());
        DeleteOrderCommand = new RelayCommand(_ => DeleteOrder(), _ => SelectedOrder != null);
        ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder(), _ => SelectedOrder != null);

        LoadOrders();
        LoadOrderItems();
    }

    private void LoadOrders()
    {
        Orders.Clear();

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string query = @"
        SELECT OrderId, FullName, Phone, DeliveryMethod, City, Street, Apartment, Building, PickupPoint, Comment, OrderDate, Status
        FROM Orders";

        using var cmd = new SqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

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

    private void LoadOrderItems()
    {
        OrderItems.Clear();

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string query = @"
        SELECT oi.OrderItemId, oi.OrderId, oi.ProductId, oi.Quantity, oi.PriceAtPurchase,
               g.Name, b.BrandName, g.Description
        FROM OrderItems oi
        INNER JOIN Goods g ON oi.ProductId = g.Id
        INNER JOIN Brands b ON g.BrandId = b.BrandId";

        using var cmd = new SqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        allOrderItems = new List<OrderItem>();

        while (reader.Read())
        {
            var item = new OrderItem
            {
                OrderItemId = reader.GetInt32(0),
                OrderId = reader.GetInt32(1),
                ProductId = reader.GetInt32(2),
                Quantity = reader.GetInt32(3),
                PriceAtPurchase = reader.GetDouble(4),
                ProductName = reader.GetString(5),
                BrandName = reader.GetString(6),
                ProductDescription = reader.GetString(7)
            };

            allOrderItems.Add(item);
        }

        foreach (var item in allOrderItems)
            OrderItems.Add(item);
    }

    

    private void FilterOrderItems()
    {
        if (int.TryParse(SearchOrderId, out int orderId))
        {
            var filtered = allOrderItems.Where(i => i.OrderId == orderId).ToList();
            OrderItems.Clear();
            foreach (var item in filtered)
                OrderItems.Add(item);
        }
    }

    private void ResetOrderItems()
    {
        SearchOrderId = string.Empty;
        LoadOrderItems();
    }

    private void DeleteOrder()
    {
        if (SelectedOrder == null)
            return;

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        using var transaction = conn.BeginTransaction();

        try
        {
            var deleteItemsCmd = new SqlCommand("DELETE FROM OrderItems WHERE OrderId = @OrderId", conn, transaction);
            deleteItemsCmd.Parameters.AddWithValue("@OrderId", SelectedOrder.OrderId);
            deleteItemsCmd.ExecuteNonQuery();

            var deleteOrderCmd = new SqlCommand("DELETE FROM Orders WHERE OrderId = @OrderId", conn, transaction);
            deleteOrderCmd.Parameters.AddWithValue("@OrderId", SelectedOrder.OrderId);
            deleteOrderCmd.ExecuteNonQuery();

            transaction.Commit();
            Orders.Remove(SelectedOrder);
            SelectedOrder = null;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private void ConfirmOrder()
    {
        if (SelectedOrder == null)
            return;

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        var cmd = new SqlCommand("UPDATE Orders SET Status = 'Подтвержден' WHERE OrderId = @OrderId", conn);
        cmd.Parameters.AddWithValue("@OrderId", SelectedOrder.OrderId);
        cmd.ExecuteNonQuery();

        SelectedOrder.Status = "Подтвержден";
        OnPropertyChanged(nameof(Orders));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

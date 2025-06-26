using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_5
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double PriceAtPurchase { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string ProductDescription { get; set; }
    }

}

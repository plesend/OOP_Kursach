using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_5
{
    public class UserOrder : INotifyPropertyChanged
    {
        private string status;

        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string DeliveryMethod { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Comment { get; set; }
        public string PickupPoint { get; set; }
        public string Apartment { get; set; }
        public string Building { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

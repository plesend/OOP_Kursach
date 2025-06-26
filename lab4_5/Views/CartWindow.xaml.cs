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

namespace lab4_5
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        private CartWindowViewModel viewModel;

        public CartWindow(User user)
        {
            InitializeComponent();
            viewModel = new CartWindowViewModel(user);
            DataContext = viewModel;

            // Привязываем вручную TotalAmount к TextBlock
            TotalAmountTextBlock.Text = $"{viewModel.TotalAmount:F2} BYN";
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(viewModel.TotalAmount))
                {
                    TotalAmountTextBlock.Text = $"{viewModel.TotalAmount:F2} BYN";
                }
            };
        }
    }

}

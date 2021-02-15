using Online_Grocery_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Online_Grocery_System.Views
{
    /// <summary>
    /// Interaction logic for ProductsStock.xaml
    /// </summary>
    public partial class ProductsStock : UserControl
    {
        public ProductsStock()
        {
            InitializeComponent();
            this.DataContext = new ProductStockViewModel();
        }
    }
}

using Online_Grocery_System.Commands;
using Online_Grocery_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Online_Grocery_System.ViewModels
{
    class ProductStockViewModel:BaseViewModel
    {
        private List<Product> productsList; //binded with the datagrid in ProductStock.xaml
        public List<Product> ProductsList { get => productsList; set { productsList = value; OnPropertyChanged("ProductsList"); } }
        ProductServices prdctSrvc;
        public ProductStockViewModel()  //initializes the productList so that it can update data in datagrid in xaml.
        {
            prdctSrvc = new ProductServices();
            ProductsList = prdctSrvc.returnProducts();
        }
    }
}

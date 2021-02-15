using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Online_Grocery_System.Models
{
    class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private string id;
        public string ID { get => id; set { id = value;   OnPropertyChanged("ID"); } }

        private string name;
        public string Name { get => name; set { name = value;  OnPropertyChanged("Name"); } }

        private string price;
        public string Price { get => price; set { price = value;  OnPropertyChanged("Price"); } }

        private string quantity;
        public string Quantity { get => quantity; set { quantity = value; OnPropertyChanged("Quantity"); } }

        //total amount taken as a DM of product class just in order to display it in a bill/receipt
        private string totalAmount;
        public string Totalamount { get => totalAmount; set { totalAmount = value;  OnPropertyChanged("Totalamount"); } }
    }
}

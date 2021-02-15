using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Online_Grocery_System.Models
{
    class Customer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;       //to update viewmodel against the upation in class's object
        private void OnPropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private string username;
        public string UserName { get => username; set { username = value; OnPropertyChanged("UserName"); } }

        private string password;
        public string Password { get => password; set { password = value; OnPropertyChanged("Password"); } }

        private string phoneno;
        public string PhoneNo { get => phoneno; set { phoneno = value; OnPropertyChanged("PhoneNo"); } }
    }
}

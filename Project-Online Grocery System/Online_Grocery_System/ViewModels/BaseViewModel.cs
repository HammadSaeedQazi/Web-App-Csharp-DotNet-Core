using Online_Grocery_System.Commands;
using Online_Grocery_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Online_Grocery_System.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public ICommand UserLoginNupdateSalePanel { get; set; } //as login function contain more funtionality than just changing the view so thats why i created it in base class and would inherit it in the CustomerViewModel.
        public ICommand updateViewCmnd { get; set; }    //command to change view and accessed by all the classes to change respective view.
        BaseViewModel selectedViewModel;    //stores the view model on which the update command will change the view
        public BaseViewModel SelectedViewModel { get => selectedViewModel; set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); } }

        ProductServices prdctSrvc;
        public BaseViewModel()
        {
            prdctSrvc = new ProductServices();
            updateViewCmnd = new DelegateCommand(update,canUpdate);
            UserLoginNupdateSalePanel = new DelegateCommand(UserLoginNsaleUpdate, canUserLoginNSaleUpdate);
        }
        virtual public bool canUpdate(object o)
        {
            return true;
        }   
        virtual public void update(object o)    //update view by checking the value of selectedViewModel variable.
        {
            if((o as string).Equals("StartupView"))
            {
                SelectedViewModel = new StartupViewModel();
            }
            if ((o as string).Equals("AdminView"))
            {
                SelectedViewModel = new AdminViewModel();
            }
            if ((o as string).Equals("ProductStockView"))
            {
                SelectedViewModel = new ProductStockViewModel();
            }
            if ((o as string).Equals("CustomerView"))
            {
                SelectedViewModel = new CustomerViewModel();
            }
            if ((o as string).Equals("SaleView"))
            {
                SelectedViewModel = new SaleViewModel();
            }
        }
        virtual public bool canUserLoginNSaleUpdate(object o)
        {
            return true;
        }
        virtual public void UserLoginNsaleUpdate(object o)  //just changes the view here and rest of the functionality is in 
        {                                                   //CustomerViewModel.
            SelectedViewModel = new SaleViewModel();
        }

        public bool idValidation(string strID)  //do ID validation and i place it in base class because AdminViewModel and SaleViewModel
        {                                       //both are accessing thi three functions so in order to make code non-redundant.
            if (string.IsNullOrEmpty(strID) || string.IsNullOrWhiteSpace(strID))
                return false;
            for (int i = 0; i < strID.Length; i++)  //id can contain numbers, characters and some accepted internatinally special characters.
            {
                if ((strID[i] >= 'a' && strID[i] <= 'z') || (strID[i] >= 'A' && strID[i] <= 'Z') || (strID[i] >= '0' && strID[i] <= '9') || strID[i] == '.' || strID[i] == '_' || strID[i] == '@') { }
                else
                    return false;
            }
            return true;
        }
        public bool quantityValidation(string strQnt)   //do quantity validation and allow only the numbers in quantity.
        {
            if (string.IsNullOrEmpty(strQnt) || string.IsNullOrWhiteSpace(strQnt) || strQnt == "0")
                return false;
            for (int i = 0; i < strQnt.Length; i++)
            {
                if ((strQnt[i] >= '0' && strQnt[i] <= '9')) { }
                else
                    return false;
            }
            return true;
        }
        public bool isIdExist(string strID) //checks whther the strID exist in DB or not with the help of prdctSrvc.
        {
            List<Product> data = prdctSrvc.returnProducts();
            foreach (Product p in data)
            {
                if (p.ID == strID)
                    return true;
            }
            return false;
        }

    }
}

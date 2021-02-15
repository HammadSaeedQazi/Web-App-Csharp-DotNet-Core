using Online_Grocery_System.Commands;
using Online_Grocery_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Online_Grocery_System.ViewModels
{
    class SaleViewModel : BaseViewModel
    {
        private bool doOpen;    //binded with the popup receipt to display it at the time of need.
        public bool DoPopupOpen { get => doOpen; set { doOpen = value; OnPropertyChanged("DoPopupOpen"); } }
        
        private string grandToal;   //binded textblock contains gradn total of bill.
        public string GrandTotal { get => grandToal; set { grandToal = value; OnPropertyChanged("GrandTotal"); } }
        
        private string itemid;  //binded with itemID entered for selection.
        public string ItemID { get => itemid; set { itemid = value; OnPropertyChanged("ItemID"); } }
        
        private string itemquantity;    //binded with itemQuantity to be purchased.
        public string ItemQuantity { get => itemquantity; set { itemquantity = value; OnPropertyChanged("ItemQuantity"); } }
        
        private string erritemid;   //binded with the textblock display the error msg against invalid input of itemID.
        public string ErrItemID { get => erritemid; set { erritemid = value; OnPropertyChanged("ErrItemID"); } }
        
        private string erritemquantity; //binded with the textblock display the error msg against invalid input of itemQuantity.
        public string ErrItemQuantity { get => erritemquantity; set { erritemquantity = value; OnPropertyChanged("ErrItemQuantity"); } }
        
        private List<Product> availableItems;   //binded with the datagrid which shows the available items in stock.
        public List<Product> AvailableItems { get => availableItems; set { availableItems = value; OnPropertyChanged("AvailableItems"); } }
        public ObservableCollection<Product> CartItems { get; set; }    //binded with the datagrid which shows the cart items.
        
        ProductServices prdctSrvc;      //to entertain SaleViewModel with the serives of products.

        string olditemid;
        string olditemquantity;

        bool enableAdd = true;  //all of the three contains true if there is no popup element poped on the screen else contains false.
        bool enableFinish=true;
        public DelegateCommand addCmnd { get; set; }    //binded with the Add button in xaml.
        public DelegateCommand finishCmnd { get; set; } //binded with the Finish button in xaml.
        public SaleViewModel()  //initializes commands and AvailableItems in stock/DB.
        {
            prdctSrvc = new ProductServices();
            AvailableItems = prdctSrvc.returnProducts();
            CartItems = new ObservableCollection<Product>();
            addCmnd = new DelegateCommand(add, canAdd);
            finishCmnd = new DelegateCommand(finish, canFinish);
        }
        public bool canAdd(object o)    //returns true if all the text fields contains some data and also remove the errors as user
        {                               //changes data in respective text field. also checks for any popup on the scree.
            if (this.ItemID != olditemid)
                ErrItemID = "";
            if (this.ItemQuantity != olditemquantity)
                ErrItemQuantity = "";
            if (!enableAdd || (string.IsNullOrEmpty(this.ItemID) || string.IsNullOrEmpty(this.ItemQuantity)))
                return false;
            return true;
        }
        public bool canFinish(object o) //returns true if cart contains atleast one item else return false. also check for for any
        {                               //popup on the screen.
            if (CartItems.Count == 0)
                return false;
            if (!enableFinish)
                return false;
            return true;
        }
        public void add(object o)
        {
            if (!idValidation(this.ItemID))
                ErrItemID = "* Invalid Product ID *";
            else if (!isIdExist(this.ItemID))
                ErrItemID = "* No Item Exist for this ID *";
            if (!quantityValidation(this.ItemQuantity))
                ErrItemQuantity = "* Invalid Quantity *";
            else if (isIdExist(this.ItemID) && !isEnoughQuantityAvailable())
                MessageBox.Show($"Not enough quantity available against the Item ID \"{ItemID}\"");
            bool isUpdated = false;
            if(idValidation(this.ItemID) && isIdExist(this.ItemID) && quantityValidation(this.ItemQuantity) && isEnoughQuantityAvailable())
            {
                List<Product> data = prdctSrvc.returnProducts();
                foreach(Product p in data)
                {
                    if (p.ID == ItemID)
                    {
                        Product temp = new Product { ID = p.ID, Name = p.Name, Price = p.Price, Quantity = ItemQuantity };
                        CartItems.Add(temp);
                        p.Quantity = (double.Parse(p.Quantity) - double.Parse(ItemQuantity)).ToString();
                        isUpdated = prdctSrvc.updateProduct(p);
                        AvailableItems = prdctSrvc.returnProducts();
                        ItemID = "";
                        ItemQuantity = "";
                    }
                }
                if (!isUpdated)
                    MessageBox.Show($"There is an error in adding the product with product ID \"{ItemID}\" to cart !");
            }
            olditemid = ItemID;
            olditemquantity = ItemQuantity;
        }
        public void finish(object o)    //counts for total amount of products added in cart and display popup on the screen and disable all the buttons
        {
            enableAdd = false;  //both of these will help in disabling the add, finish buttons. 
            enableFinish = false;
            double total = 0;
            DoPopupOpen = true;
            foreach (Product p in CartItems)
            {
                p.Totalamount = (double.Parse(p.Price) * double.Parse(p.Quantity)).ToString();
                total = total + double.Parse(p.Totalamount);
            }
            GrandTotal = $"Grand Total: {total}";
        }
        public override void update(object o)   //overriden function from BaseViewModel that hides the popup and changes the view accordingly.
        {
            DoPopupOpen = false;
            base.update(o);
        }
        internal bool isEnoughQuantityAvailable()   //checks whether is there enough quantity available to be added in the cart 
        {                                           //demanded by the user.
            List<Product> data = prdctSrvc.returnProducts();
            foreach (Product p in data)
            {
                if (p.ID == this.ItemID && double.Parse(p.Quantity) >= double.Parse(this.ItemQuantity))
                    return true;
            }
            return false;
        }
    }
}

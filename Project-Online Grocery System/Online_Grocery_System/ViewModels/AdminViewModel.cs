using Online_Grocery_System.Commands;
using Online_Grocery_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace Online_Grocery_System.ViewModels
{
    class AdminViewModel : BaseViewModel
    {
        string id;  //binded with the textbox which receives product's ID
        public string ID { get => id; set { id = value;  OnPropertyChanged("ID"); } }
        
        string name;    //binded with the textbox which receives product's Name
        public string Name { get => name; set { name = value; OnPropertyChanged("Name"); } }
        
        string price;   //binded with the textbox which receives product's Price
        public string Price { get => price; set { price = value; OnPropertyChanged("Price"); } }
        
        string quantity;    //binded with the textbox which receives product's Quantity
        public string Quantity { get => quantity; set { quantity = value; OnPropertyChanged("Quantity"); } }

        string dltid;   //binded with the textbox which receives product's ID to be deleted
        public string DltID { get => dltid; set { dltid = value; OnPropertyChanged("DltID"); } }
        
        string errid;   //binded with the textblock to show error in the case if user input wrong ID
        public string ErrID { get => errid; set { errid = value; OnPropertyChanged("ErrID"); } }
        
        string errdltid;    //binded with the textblock to show error in the case if user input wrong ID for deletion
        public string ErrDltID { get => errdltid; set { errdltid = value; OnPropertyChanged("ErrDltID"); } }
        
        string errname; //binded with the textblock to show error in the case if user input wrong Name
        public string ErrName { get => errname; set { errname = value; OnPropertyChanged("ErrName"); } }
        
        string errprice;    //binded with the textblock to show error in the case if user input wrong Price
        public string ErrPrice { get => errprice; set { errprice = value; OnPropertyChanged("ErrPrice"); } }
        
        string errquantity; //binded with the textblock to show error in the case if user input wrong Quantity
        public string ErrQuantity { get => errquantity; set { errquantity = value; OnPropertyChanged("ErrQuantity"); } }
        public DelegateCommand addCommand { get; set; } //binded with the Add Product button
        public DelegateCommand dltCommand { get; set; } //binded with the DELETE button
        
        private string oldid;       //all of the five are created to erase the error msg from textblock
        private string oldname;     //whenever user chnanges data in relevant textbox after seeing error.
        private string oldprice;
        private string oldquantity;
        private string olddltid;

        ProductServices prdctSrvc;  //to entertain this viewModel with the services of Product Services class
        public AdminViewModel() //initializes the commands
        {
            prdctSrvc = new ProductServices();
            addCommand = new DelegateCommand(AddPrdct, canAdd);
            dltCommand = new DelegateCommand(DeleteProduct, canDelete);
        }
        public bool canAdd(object o)    //only returns true only when all of the four textboxes contain some data of any form
        {                               //and also check for the equality of previous data and current data in each textbox to
            if (this.ID != oldid)       //help in removing error msg.
                ErrID = "";
            if (this.Name != oldname)
                ErrName = "";
            if (this.Price != oldprice)
                ErrPrice = "";
            if (this.Quantity != oldquantity)
                ErrQuantity = "";
            if (string.IsNullOrEmpty(this.ID) || string.IsNullOrEmpty(this.Name) || string.IsNullOrEmpty(this.Price) || string.IsNullOrEmpty(this.Quantity))
                return false;
            return true;
        }
        public bool canDelete(object o) //works same as the previous function
        {
            if (this.DltID != olddltid)
                ErrDltID = "";
            if (string.IsNullOrEmpty(this.DltID))
                return false;
            return true;
        }
        public void AddPrdct(object o)  //add product in database with the help of ProductServices class's functions
        {
            if (!idValidation(this.ID))     //validates ID input
                ErrID = "* Invalid Product ID *";
            if(!nameValidation())                   //validates Name input
                ErrName = "* Invalid Product Name *";
            if (!priceValidation())                     //validates Price input
                ErrPrice = "* Invalid Product Price *";
            if (!quantityValidation(this.Quantity))     //validates Quantity input
                ErrQuantity = "* Invalid Product Quantity *";
            bool isSame = false;    //false if the entered id exist in DB then and the name and price are not valid for that id else true.
            if (isIdExist(ID))  //if the id exist in our database then this function matches the entered name and price whether
            {                   //these values are equal to values present in database against this id. if doesnt match then mark
                List<Product> tempData = prdctSrvc.returnProducts();    //isSame false.
                foreach (Product p1 in tempData)
                {
                    if (p1.ID == this.ID && p1.Name != this.Name)
                    {
                        isSame = true;
                        ErrName = "* Invalid Name against this ID *";
                    }
                    if (p1.ID == this.ID && p1.Price != this.Price)
                    {
                        isSame = true;
                        ErrPrice = "* Invalid Price for this ID *";
                    }
                }
            }
            Product p = new Product { ID = this.ID, Name = this.Name, Price = this.Price, Quantity = this.Quantity };
            bool isAdded = false;
            bool isUpdated = false;
            if (idValidation(this.ID) && nameValidation() && priceValidation() && quantityValidation(this.Quantity) && !isSame)
            {
                if (!isIdExist(ID)) //if item doesnt not exist and all the entered inputs are valid then it creates a new record by
                {                   //the help of prdctsrvc.
                    isAdded = prdctSrvc.addProduct(p);
                    MessageBox.Show("Product Added !");
                }
                else    //if item exist in db then this funtion update the data of that record or replace that record with updated
                {       //product with the same enteries but change in quantity.
                    List<Product> data = prdctSrvc.returnProducts();
                    foreach(Product temp in data)
                    {
                        if(temp.ID == this.ID)
                        {   //as out DM in string so first we parse it in double and then modify it.
                            temp.Quantity = (double.Parse(temp.Quantity) + double.Parse(this.Quantity)).ToString();
                            isUpdated = prdctSrvc.updateProduct(temp);
                        }
                    }
                    if (isUpdated)
                        MessageBox.Show("Value Added !");
                }
                this.ID = "";   //to make all fields/textboxes empty after adding product.
                this.Name = "";
                this.Price = "";
                this.Quantity = "";
                if (!isUpdated && !isAdded)     //in case of any error occur by db this will show msg.
                    MessageBox.Show("There is an Error occur while adding product !");
            }
            oldid = this.ID;    //all of these are set equal to current input so that canAdd will make errors msgs empty as soon
            oldname = this.Name;    //as user changes the vlaue in respective textbox.
            oldprice = this.Price;
            oldquantity = this.Quantity;
        }
        public void DeleteProduct(object o) //deletes the product with the help of Product Service class
        {
            if (!idValidation(this.DltID))  //validates input entered in textbox for deletion
                ErrDltID = "* Invalid Product ID *";
            else if(!isIdExist(DltID))              //checks whether the entered id exist in DB
                ErrDltID = "* Product ID does not Exist *";
            bool isDeleted = false;
            if (idValidation(this.DltID) && isIdExist(DltID))   //deletes the record from DB with the id equal to dltdID.
            {
                isDeleted = prdctSrvc.deleteProduct(this.DltID);
                MessageBox.Show("Product Deleted !");
                this.DltID = "";
                if (!isDeleted)
                    MessageBox.Show("There is an Error in Adding Product !");   //shown in case of error by DB
            }
            olddltid = this.DltID;  //to help in hiding th error msg.
        }
        internal bool nameValidation()  //validates name according to tules that product have any name as in real life product can
        {                               //be of any name so not much validations for it.
            if (string.IsNullOrEmpty(this.Name) || string.IsNullOrWhiteSpace(this.Name))
                return false;
            return true;
        }
        internal bool priceValidation() //validates price with the assumption that price can be of decimal type.
        {
            int decimalCount = 0;
            if (string.IsNullOrEmpty(this.Price) || string.IsNullOrWhiteSpace(this.Price))
                return false;
            for (int i = 0; i < this.Price.Length; i++)
            {
                if (this.Price[i] == '.')
                    decimalCount++;
                if ((this.Price[i] >= '0' && this.Price[i] <= '9') || this.Price[i] == '.') { }
                else
                    return false;
            }
            if (decimalCount > 1)
                return false;
            return true;
        }
    }
}

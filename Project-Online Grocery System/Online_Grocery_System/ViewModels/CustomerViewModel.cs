using Online_Grocery_System.Commands;
using Online_Grocery_System.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Online_Grocery_System.ViewModels
{
    class CustomerViewModel : BaseViewModel
    {
        string usernamelogin;   //binded with the textbox contains the username for login purpose in xaml
        public string UserNameLogin { get => usernamelogin; set { usernamelogin = value; OnPropertyChanged("UserNameLogin"); } }
        
        string usernamesignup;  //binded with the textbox contains the username for signup purpose in xaml
        public string UserNameSignup { get => usernamesignup; set { usernamesignup = value; OnPropertyChanged("UserNameSignup"); } }
        
        string phonenosignup;   //binded with the textbox contains the phone number for signup purpose in xaml
        public string PhoneNoSignup { get => phonenosignup; set { phonenosignup = value; OnPropertyChanged("PhoneNoSignup"); } }

        string errusernamelogin;    //binded with the textblock displays the error msg in case of invalid input of id for login
        public string ErrUserNameLogin { get => errusernamelogin; set { errusernamelogin = value; OnPropertyChanged("ErrUserNameLogin"); } }

        string errpasswordlogin;    //binded with the textblock displays the error msg in case of invalid input of pasasword for login
        public string ErrPasswordLogin { get => errpasswordlogin; set { errpasswordlogin = value; OnPropertyChanged("ErrPasswordLogin"); } }

        string errusernamesignup;   //binded with the textblock displays the error msg in case of invalid input of id for signup
        public string ErrUserNameSignup { get => errusernamesignup; set { errusernamesignup = value; OnPropertyChanged("ErrUserNameSignup"); } }
        
        string errpasswordsignup;   //binded with the textblock displays the error msg in case of invalid input of password for signup
        public string ErrPasswordSignup { get => errpasswordsignup; set { errpasswordsignup = value; OnPropertyChanged("ErrPasswordSignup"); } }
        
        string errphonenosignup;    //binded with the textblock displays the error msg in case of invalid input of phone number for signup
        public string ErrPhoneNoSignup { get => errphonenosignup; set { errphonenosignup = value; OnPropertyChanged("ErrPhoneNoSignup"); } }

        public DelegateCommand signupCmnd { get; set; } //binded with the signup button. login button is binded twith the bae class
                                                        //command as it has to perform some functionality and has to change view as well.         
        string oldusernamelogin;    //same use as in admin view model. used to store previous entered data in order to erase error
        string oldpasswordlogin;    //msg in textblocks.
        string oldusernamesignup;
        string oldpasswordsignup;
        string oldphonenosignup;
        CustomerServices cstmrsrvc;
        public CustomerViewModel()
        {
            cstmrsrvc = new CustomerServices();
            signupCmnd = new DelegateCommand(signup, canSignup);
        }
        public override bool canUserLoginNSaleUpdate(object o)  //overrided funtion from the BaseViewModel returns true when all the
        {                               //text fields contain some input. it also checks for the removal of error msg by comparing
            PasswordBox LoginPassword;  //current value of properties and their previous values.
            try               
            {                   //recieves the value of password sent as command parameter from xaml using IValueConverter.
                var tempbox = o;
                LoginPassword = tempbox as PasswordBox;
            }
            catch
            {
                return true;
            }
            if (UserNameLogin != oldusernamelogin)
                ErrUserNameLogin = "";
            if (LoginPassword.Password != oldpasswordlogin)
                ErrPasswordLogin = "";
            if (!string.IsNullOrEmpty(UserNameLogin) && !string.IsNullOrEmpty(LoginPassword.Password))
                return true;
            return false;
        }
        public override void UserLoginNsaleUpdate(object o) //overrided function from BaseViewModel. checks for the right login
        {                               //credentials and also changes view whose funtionality is present in BaseViewModel.
            PasswordBox LoginPassword;
            try
            {                       //recieves the value of password sent as command parameter from xaml using IValueConverter.
                var tempbox = o;
                LoginPassword = tempbox as PasswordBox;
            }
            catch
            {
                return;
            }
            if (!usernameValidation(UserNameLogin))         //validates username for login
                ErrUserNameLogin = "* Invalid UserName *";
            else if (!isCustomerExist(UserNameLogin))       //checks whther entered username exist in DB or not.
                ErrUserNameLogin = "* UserName does not Exist *";
            if (!passwordValidation(LoginPassword.Password))    //validates password entered for login.
                ErrPasswordLogin = "* Invalid Password *";
            if (usernameValidation(UserNameLogin) && passwordValidation(LoginPassword.Password) && areLoginCredentialsTrue(UserNameLogin, LoginPassword.Password))
            {
                base.UserLoginNsaleUpdate(o);   //if ccredentials are right then changes the view and displays the SaleViewPanel
            }
            oldusernamelogin = UserNameLogin;       //to help in removing the error msgs.
            oldpasswordlogin = LoginPassword.Password;
        }
        public bool canSignup(object o) //returns true only if all the text fields contain some data and also help in removing error msgs.
        {                               //by comparing current entered values with previous entered values.
            PasswordBox SignupPassword;
            try
            {                       //recieves the value of password sent as command parameter from xaml using IValueConverter.
                var tempbox = o;
                SignupPassword = tempbox as PasswordBox;
            }
            catch
            {
                return true;
            }
            if (UserNameSignup != oldusernamesignup)
                ErrUserNameSignup = "";
            if (SignupPassword.Password != oldpasswordsignup)
                ErrPasswordSignup = "";
            if (PhoneNoSignup != oldphonenosignup)
                ErrPhoneNoSignup = "";
            if (!string.IsNullOrEmpty(UserNameSignup) && !string.IsNullOrEmpty(SignupPassword.Password) && !string.IsNullOrEmpty(PhoneNoSignup))
                return true;
            return false;
        }
        public void signup(object o)    //makes new account of user after validating the entered values. 
        {
            PasswordBox SignupPassword;
            try
            {                   //recieves the value of password sent as command parameter from xaml using IValueConverter.
                var tempbox = o;
                SignupPassword = tempbox as PasswordBox;
            }
            catch
            {
                return;
            }
            if (!usernameValidation(UserNameSignup))    //validates the entered username for signup purpose.
                ErrUserNameSignup = "* Invalid UserName *";
            else if(isCustomerExist(UserNameSignup))    //checks whether entered usrname already exist or not.
                ErrUserNameSignup = "* UserName already Exist *";
            if (!passwordValidation(SignupPassword.Password))   //validates the entered password for signup purpose.
                ErrPasswordSignup = "* Invalid Password *";
            if (!phoneValidation())                             //validates the entered phone number for signup purpose.
                ErrPhoneNoSignup = "* Invalid Phone No. *";
            Customer c = new Customer { UserName = UserNameSignup, Password = SignupPassword.Password, PhoneNo = PhoneNoSignup };
            bool isSignedUp = false;
            if(usernameValidation(UserNameSignup) && passwordValidation(SignupPassword.Password) && phoneValidation())
            {                                           //if all the conditions return true then creates a new record for the user.
                isSignedUp = cstmrsrvc.addCustomer(c);
                MessageBox.Show("Successfully Signed Up !");
                UserNameSignup = "";
                SignupPassword.Password = "";
                PhoneNoSignup = "";
                if (!isSignedUp)                    //display error msg in case of failure by DB.
                    MessageBox.Show("There is an error occur while Signing Up !");
            }
            oldusernamesignup = UserNameSignup;         //all of them are updated with current values to help us in showing/removing 
            oldpasswordsignup = SignupPassword.Password;    //error msgs.
            oldphonenosignup = PhoneNoSignup;
        }
        internal bool usernameValidation(string name)   //validates username with rules that username contains numbers, characters
        {                                               //and some internationally accepted special caracters for username.
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return false;
            for (int i = 0; i < name.Length; i++)
            {
                if ((name[i] >= 'a' && name[i] <= 'z') || (name[i] >= 'A' && name[i] <= 'Z') || (name[i] >= '0' && name[i] <= '9') || name[i] == '.' || name[i] == '_' || name[i] == '@') { }
                else
                    return false;
            }
            return true;
        }
        internal bool passwordValidation(string pass)   //checks for password validation that whether password contains ant space or not.
        {
            if (string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass))
                return false;
            for (int i = 0; i < pass.Length; i++)
            {
                if (pass[i] == ' ')
                    return false;
            }
            return true;
        }
        internal bool phoneValidation() //validates phone number entered by user by rules that phone number only contains numbers
        {                               //and plus sign.
            if (string.IsNullOrEmpty(this.PhoneNoSignup) || string.IsNullOrWhiteSpace(this.PhoneNoSignup))
                return false;
            for (int i = 0; i < this.PhoneNoSignup.Length; i++)
            {
                if ((this.PhoneNoSignup[i] >= '0' && this.PhoneNoSignup[i] <= '9') || this.PhoneNoSignup[0] == '+') { }
                else
                    return false;
            }
            return true;
        }
        internal bool isCustomerExist(string code)  //checks whether entered id is exist in db or not.
        {
            List<Customer> data = cstmrsrvc.returnCustomers();
            foreach (Customer c in data)
            {
                if (c.UserName == code)
                    return true;
            }
            return false;
        }
        internal bool areLoginCredentialsTrue(string usrnam, string pass)   //checks it entered id is exist in db then whether the
        {                               //entered password mathces the respective password in DB or not.
            usrnam = usrnam.ToLower();
            List<Customer> data = cstmrsrvc.returnCustomers();
            foreach (Customer c in data)
            {
                if (c.UserName==usrnam && c.Password==pass)
                    return true;
            }
            return false;
        }
    }
}

using ATM_BO;
using ATM_DAL;
using ATM_BLL;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;      //must read program
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ATM_VIEW
{
    public class MainView
    {
        CustomerBO cbo = new CustomerBO();
        AdminBO abo = new AdminBO();
         public MainBLL bll = new MainBLL();
        public void MainScreen()    //main screen with which the user interact very firstly. It also validate login credentials
        {                       //in case of wrong login id just displays error message and ask for input again, but in case of
            int wrongs = 0;     //wrong pin code it displays counter for 3 wrong pins validation. in case of admins after 3 wrong
            Clear();       //pins it will say admin to try again later but in case of customer it will send request too bll
            WriteLine("\t\t*** Welcome To ATM Management System ***\n\n");    //to disable that customer account.
        d2:
            Write("Enter Login: ");
            string login = ReadLine();
            bool isLoginAddressCorrect = bll.CheckLogin(login);     //login validation
            if (!isLoginAddressCorrect)
            {
                WriteLine("\n=> Login ID only contain numbers or characters\n");
                goto d2;
            }
            l1:
            Write("\nEnter Pin: ");
            string code = ReadLine();
            bool isPinValid = bll.DigitValidation(code);
            if (code=="" || code.Length != 5 || !isPinValid)
                WriteLine("\n=> Pin code only contains numbers - Pin must have 5 digits");
            AdminBO bo = new AdminBO { Login = login, pin = code };
            int who = -1;               //for differetiation between customer and admin; 0 for customer and 1 for admin
            bool isUserFileExist = true;
            bool isCorrect = bll.IsLoginMatched(login, code, ref wrongs, ref who, ref isUserFileExist);
            bool isUserExist = bll.isLoginUseridExist(login, null);
            bool isAdminExist = bll.isAdminIDExist(login);
            if (isCorrect && who==-1)       //success scenerio of admin
            {
                int AdminChoice = 0;
                AdminChoice = DisplayAdminMenu();
                CheckAdminChoice(AdminChoice);
            }
            else if (!isCorrect && !isAdminExist && who == -1)      //failure scenerio of admin
            {
                WriteLine($"\n=> No Account exist with the Login ID: {login}\n");
                wrongs--;
                goto d2;
            }
            else if(!isCorrect && who==-1 && wrongs == 3)       //failure scenerio as wrong inputs are now 3. so admin 
            {                                              //now should try next time or after a while.
                Console.WriteLine("\n=> Invalid Username or Password - Try Again Later\n");
                System.Environment.Exit(1);
            }
            else if (isCorrect && who == 0)     //success scenerio of customer
            {
                bool isDisable = bll.IsAccountDisable(login);   //check whether account is disables or not
                if (isDisable)
                {
                    WriteLine($"\n=> Account is against Login ID: {login} is unfortunately Disabled\n");
                    goto d2;
                }
                int choice = 0;
                cbo = bll.GetCurrentUser(login, null, 0);
                choice = DisplayUserMenu();
                CheckUserChoice(choice);
            }
            else if (!isUserFileExist)
            {
                WriteLine("\n=> We Have No Records !\n");
                wrongs--;
                goto d2;
            }
            else if(!isCorrect && who==0 && wrongs == 3)    //if wrong entries of pin are 3 now so make account disable
            {
                Console.WriteLine($"\n=> Multiple Wrong Pins - Account against Login ID: {login} is Disabled Now\n");
                bll.MakeAccountDisable(login);
                System.Environment.Exit(1);
            }
            else if (!isCorrect && !isUserExist && who == 0)
            {
                WriteLine($"\n=> No Account exist with the Login ID: {login}\n");
                wrongs--;
                goto d2;
            }
            else 
            {
                WriteLine($"\n=> Wrong Pin - Try Again - {3 - wrongs} tries left");
                goto l1;
            }
                
        }
        public int DisplayUserMenu()        //display menu for customer + validation
        {
            Clear();
            WriteLine("1----Withdraw Cash\n2----Cash Transfer\n3----Deposit Cash\n4----Display Balance\n5----Exit\n");
        d1:
            Write("Please select one of the above option: ");
            string tmp = ReadLine();
            if (tmp == "" || tmp.Length > 1 || tmp[0] < '1' || tmp[0] > '5')
            {
                WriteLine("\n=> Invalid Input\n");
                goto d1;
            }
            return (int)tmp[0]-48;
        }
        public int DisplayAdminMenu()       //display manu for admin + validation
        {
            Clear();
            WriteLine("1----Create New Account\n2----Delete Exsisting Account\n3----Update Account Information\n4----Search forAccount\n5----View Reports\n6----Exit\n");
        d1:
            Write("Enter your choice: ");
            string tmp = ReadLine();
            if (tmp == "" || tmp.Length > 1 || tmp[0] < '1' || tmp[0] > '6')
            {
                WriteLine("\n=> Invalid Input\n");
                goto d1;
            }
            return (int)tmp[0]-48;
        }
        public void CheckUserChoice(int choice)     //to check choice of user and after fulfiling his/her requirements
        {                                       //ask him/her for more work.
            Clear();
            switch (choice)
            {
                case 1:
                    WithdrawCash();
                    break;
                case 2:
                    CashTransfer();
                    break;
                case 3:
                    DepositCash();
                    break;
                case 4:
                    DisplayBalance();
                    break;
                case 5:
                    Exit();
                    break;
            }
            AskforAgain(cbo);
        }
        public void DisplayAdmins()     //in order to display admins for the help of administrations
        {
            List<AdminBO> list = bll.ReadAdmin();
            foreach (AdminBO e in list)
            {
                WriteLine($"Login : {e.Login} Pin: {e.pin}");
            }
        }
        public void CheckAdminChoice(int choice)        //to check admin choice and after fulfiling his/her requirements
        {                                       //ask him/her for more work.
            Clear();
            switch (choice)
            {
                case 1:
                    CreateNewAccount();
                    break;
                case 2:
                    DeleteExistingAccount();
                    break;
                case 3:
                    UpdateAccountInfo();
                    break;
                case 4:
                    SearchAccount();
                    break;
                case 5:
                    ViewReports();
                    break;
                case 6:
                    Exit();
                    break;
            }
            AskforAgain(null);
        }
        bool AccountNumberValidations(string Acc, ref int AccNo)    //validations of account number with Acc is inputed account
        {                                                       //number and AccNo is the variable in which the correct account
            bool isValid = bll.DigitValidation(Acc);            //will be save.
            if (!isValid)
            {
                WriteLine("\n=> Invalid Account Number\n");
                return false;
            }
            AccNo = int.Parse(Acc);
            bool isExist = bll.isAccountNoExist(AccNo);
            if (!isExist)
            {
                WriteLine($"\n=> No account exist against account number: {AccNo}\n");
                return false;
            }
            return true;
        }
        public void CreateNewAccount()      //to create user account with all validations and takes input in a customer object
        {                             //and then send that object to bll to create account.
            MainBLL bllTemp = new MainBLL();
            WriteLine("CREATE NEW ACCOUNT\n\n");
            bool checkValidity;
            d1:
            Write("Login: ");
            cbo.Login = ReadLine();
            checkValidity = bllTemp.CheckLogin(cbo.Login);      //login validation
            bool isAlreadyExist = bllTemp.isLoginUseridExist(cbo.Login, null);      //checks whether account exist or not on the base of login
            if (!checkValidity)
            {
                WriteLine("\n=> Login ID only contains numbers or characters\n");
                goto d1;
            }
            if (isAlreadyExist)
            {
                WriteLine("\n=> Login ID already Exists\n");
                goto d1;
            }
            d3:
            Write("User ID: ");
            cbo.UserID = ReadLine();
            checkValidity = bllTemp.DigitValidation(cbo.UserID);        //userid validation
            isAlreadyExist = bllTemp.isLoginUseridExist(null, cbo.UserID);      //check account exist or not based on user id
            if (cbo.UserID=="" || !checkValidity || cbo.UserID.Length != 5)
            {
                WriteLine("\n=> User ID only contains numbers - User ID must be 5 characters long\n");
                goto d3;
            }
            if (isAlreadyExist)
            {
                WriteLine("\n=> User ID already Exists\n");
                goto d3;
            }
            d4:
            Write("Pin Code: ");
            cbo.Pin = ReadLine();
            checkValidity = bllTemp.DigitValidation(cbo.Pin);       //pin validation
            if (cbo.Pin=="" || !checkValidity || cbo.Pin.Length != 5)
            {
                WriteLine("\n=> Pin only contains numbers - Pin must be of 5 digit long\n");
                goto d4;
            }
            i1:
            Write("Holder's Name: ");
            cbo.Name = ReadLine();
            if (cbo.Name == "")
            {
                WriteLine("\n=> Name must not be empty\n");
                goto i1;
            }    
        d5:
            Write("Type(Savings, Current): ");
            cbo.Type = ReadLine().ToLower();
            checkValidity = bllTemp.TypeValidation(cbo.Type);       //type validation
            if (cbo.Type=="" || !checkValidity)
            {
                WriteLine("\n=> There are only two types(Savings, Current)\n");
                goto d5;
            }
            d7:
            Write("Starting Balance: ");
            string tmp = ReadLine();
            bool isValid = bllTemp.AmountValidation(tmp);       //amount validation
            if (!isValid)
            {
                WriteLine("\n=> Invalid Input\n");
                goto d7;
            }
            cbo.Balance = decimal.Parse(tmp);
        d6:
            Write("Status: ");
            cbo.Status = ReadLine().ToLower();
            checkValidity = bllTemp.StatusValidation(cbo.Status);       //status validation
            if (cbo.Status == "" || !checkValidity)
            {
                WriteLine("\n=> There can be only two statuses of account(Active, Disable)\n");
                goto d6;
            }
            cbo.AccountNo = bllTemp.LastAccountNumber()+1;
            WriteLine($"\n=> Account Sucessfully Created - the account number assigned is: {cbo.AccountNo}");
            MainBLL bll = new MainBLL();
            bll.CreateCustomerAccount(cbo);
        }
        public void DeleteExistingAccount()     //to delete an account of customer based on the account number inputed by
        {                                   //admin.
            WriteLine("DELETE EXISTING ACCOUNT\n\n");
            int AccNo=0;
        d1:
            Write("Enter the account number you want to delete: ");
            string tmp = ReadLine();
            bool isTrue = AccountNumberValidations(tmp, ref AccNo);
            if (!isTrue)
                goto d1;
            CustomerBO tempDelete = new CustomerBO();
            tempDelete = bll.GetCurrentUser(null, null, AccNo);
            Write($"You wish to delete the account held by {tempDelete.Name}; If this information is correct please re - enter the account number: ");
            int cnfrmAcc = int.Parse(ReadLine());
            if (AccNo == cnfrmAcc)
                bll.DeleteRecord(tempDelete);
            else
            {
                WriteLine("=> Confirmation of Account Number Failed");
                goto d1;
            }
        }
        public void UpdateAccountInfo()     //Update an account by taking inputs in a list of strings and then pass it to
        {                               //update account from bll with all the error handling/validations same as in 
            WriteLine("UPDATE ACCOUNT INFORMATION\n\n");    //create user account but the empty input will be ignored here
            int AccNo = 0;                                  //as its a requirement of this method.
        d9:                                             
            Write("Enter Account Number: ");
            string temp = ReadLine();
            bool isTrue = AccountNumberValidations(temp, ref AccNo);
            if (!isTrue)
                goto d9;
            Clear();
            WriteLine("Update Account Information\n\n");
            CustomerBO tempUpdate = bll.GetCurrentUser(null, null, AccNo);
            List<string> newData = new List<string>();
            WriteLine($"Account # {tempUpdate.AccountNo}\nLogin: {tempUpdate.Login}\nUser ID: {tempUpdate.UserID}\nPin: {tempUpdate.Pin}\nHolder Name: {tempUpdate.Name}\nType: {tempUpdate.Type}\nBalance: {tempUpdate.Balance}\nStatus: {tempUpdate.Status}\n");
            WriteLine("Please enter in a field you wish to update (leave blank otherwise): ");
            string input;
        d1:
            Write("Login: ");
            input = ReadLine();
            bool checkValidity = bll.CheckLogin(input);
            bool isAlreadyExist = bll.isLoginUseridExist(input, null);
            if (input != "" && !checkValidity)
            {
                WriteLine("\n=> Login ID only contains numbers or characters\n");
                goto d1;
            }
            if (isAlreadyExist)
            {
                WriteLine("\n=> Login ID already Exists\n");
                goto d1;
            }
            newData.Add(input);
        d2:
            Write("User ID: ");
            input = ReadLine();
            checkValidity = bll.DigitValidation(input);
            isAlreadyExist = bll.isLoginUseridExist(null, input);
            if (input != "" && (!checkValidity || input.Length != 5))
            {
                WriteLine("\nU=> ser ID only contains numbers - User ID must be 5 characters long\n");
                goto d2;
            }
            if (isAlreadyExist)
            {
                WriteLine("\n=> User ID already Exists\n");
                goto d2;
            }
            newData.Add(input);
        d4:
            Write("Pin Code: ");
            input = ReadLine();
            checkValidity = bll.DigitValidation(input);
            if (input != "" && (checkValidity || input.Length != 5))
            {
                WriteLine("\n=> Pin only contains numbers - Pin must be 5 character long\n");
                goto d4;
            }
            newData.Add(input);
            UpdateNSearchValidations(ref newData);
            bll.UpdateRecords(newData, AccNo);
            WriteLine("=> Your account has been successfully updated.");
        }
        public void SearchAccount()     //searches account based on the inputs by admin. same valiations as update account info
        {                              //and same ignorance of empty input as its a requirement of a method.
            WriteLine("SEARCH FOR ACCOUNT\n\n");
            List<string> newData = new List<string>();
            WriteLine("SEARCH MENU:\n");
        d1:
            Write("Account ID: ");
            string input = ReadLine();
            bool isValid = bll.DigitValidation(input);
            if (input != "" && !isValid)
            {
                WriteLine("\n=> Invalid Account Number\n");
                goto d1;
            }
            newData.Add(input);
        d2:
            Write("User ID: ");
            input = ReadLine();
            bool checkValidity = bll.DigitValidation(input);
            if (input != "" && (!checkValidity || input.Length != 5))
            {
                WriteLine("\n=> User ID only contains numbers - User ID must be 5 characters long\n");
                goto d2;
            }
            newData.Add(input);
            UpdateNSearchValidations(ref newData);
            List<CustomerBO> FetchedData = new List<CustomerBO>();
            FetchedData = bll.SearchAccounts(newData);
            ShowResults(FetchedData);
        }
        public void UpdateNSearchValidations(ref List<string> list)   //this method is just make to reduce redundancy as these inputs are common
        {                                           //in both update account and in search account with the same validations.
            string input;
            bool checkValidity = false;
            Write("Holders Name: ");
            input = ReadLine();
            list.Add(input);
        d5:
            Write("Type(Savings, Current): ");
            input = ReadLine().ToLower();
            checkValidity = bll.TypeValidation(input);
            if (input != "" && !checkValidity)
            {
                WriteLine("\n=> There are only two types(Savings, Current)\n");
                goto d5;
            }
            list.Add(input);
        d6:
            Write("Balance: ");
            input = ReadLine();
            bool isValid = bll.AmountValidation(input);
            if (input != "" && !isValid)
            {
                WriteLine("\nInvalid Input\n");
                goto d6;
            }
            list.Add(input);
        d7:
            Write("Status: ");
            input = ReadLine().ToLower();
            checkValidity = bll.StatusValidation(input);
            if (input != "" && !checkValidity)
            {
                WriteLine("\n=> There can be only two statuses of account(Active, Disable)\n");
                goto d7;
            }
            list.Add(input);
        }
        void AskforAgain(CustomerBO cbo)        //to ask the user for more work.
        {
            Write("\nWish to do more work? (y/n): ");
            string choice = ReadLine();
            int chk;
            if (choice[0] == 'y' || choice[0] == 'Y')
            {
                if (cbo == null)
                {
                    chk = DisplayAdminMenu();
                    CheckAdminChoice(chk);
                }
                else
                {
                    chk = DisplayUserMenu();
                    CheckUserChoice(chk);
                }
            }
        }
        public void ViewReports()       //to view reports we use this method. Two methods of reports, one with amount range
        {                           //and the other with date range with the amount and date valiations.
            WriteLine("VIEW REPORTS\n\n");
            List<CustomerBO> FetchedData = new List<CustomerBO>();
            WriteLine("1---Accounts By Amount\n2---Accounts By Date");
        i1:
            Write("\nEnter Your Choice: ");
            string tmp = ReadLine();
            bool isTrue = bll.DigitValidation(tmp);
            if(!isTrue || tmp.Length != 1 || tmp[0] < '1' || tmp[0] > '2')
            {
                WriteLine("\n=> Invalid Input\n");
                goto i1;
            }
            int choice = int.Parse(tmp);
            Clear();
            WriteLine("VIEW REPORTS\n");
            if (choice == 1)
            {
            d1:
                Write("\nEnter the minimum amount: ");
                tmp = ReadLine();
                bool isValid = bll.AmountValidation(tmp);
                if (!isValid)
                {
                    WriteLine("\n=> Amount is out of range\n");
                    goto d1;
                }
                decimal minAmt = decimal.Parse(tmp);
            d2:
                Write("\nEnter the maximum amount: ");
                tmp = ReadLine();
                isValid = bll.AmountValidation(tmp);
                if (!isValid)
                {
                    WriteLine("\n=> Amount is out of range\n");
                    goto d2;
                }
                decimal maxAmt = decimal.Parse(tmp);
                if (maxAmt < minAmt)
                {
                    WriteLine("\n=> Max Amount must not be lesser than Min amount\n");
                    goto d2;
                }
                FetchedData = bll.ReportByAmount(minAmt, maxAmt);
                ShowResults(FetchedData);
            }
            else if (choice == 2)
            {
                DateTime dt1 = new DateTime();
                DateTime dt2 = new DateTime();
                string userID;
                l0:
                Write("\nEnter the User ID: ");
                userID = ReadLine();
                bool isValid = bll.DigitValidation(userID);
                if (userID.Length != 5 || !isValid)
                {
                    WriteLine("\n=> Invalid User ID - User ID is must be 5 digit long !\n");
                     goto l0;
                }
                bool isExist = bll.isLoginUseridExist(null, userID);
                if (!isExist)
                {
                    WriteLine("\n=> User ID does not Exist\n");
                    goto l0;
                }
            l1:
                try
                {
                    Write("Enter the starting date: ");
                    string date = ReadLine();
                    if (date == "")
                        throw new SystemException();
                    dt1 = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch(Exception)
                {
                    WriteLine("\n=> Invalid Input !\n");
                    goto l1;
                }
                l2:
                try
                {
                    Write("Enter the ending date: ");
                    string date = ReadLine();
                    if (date == "")
                        throw new SystemException();
                    dt2 = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (dt2 < dt1)
                        throw new SystemException("\n=> Ending date must be greater than or equal to starting date !\n");
                }
                catch (Exception ex)
                {
                    WriteLine($"=> {ex.Message}");
                    goto l2;
                }
                List<Transactions> FetchedTransactions = new List<Transactions>();
                FetchedTransactions = bll.ReportByDate(dt1, dt2, userID);
                ShowResultsOfDate(FetchedTransactions, userID);
            }
        }
        internal void ShowResultsOfDate(List<Transactions> Data, string userID) //As there are two methods of showing result
        {                                   //this will show result of the reports by date
            CustomerBO ub = bll.GetCurrentUser(null, userID, 0);
            WriteLine("\n=== SEARCH RESULTS ===\n");
            string date;
            WriteLine("     Transaction Type     User ID        Holder's Name     Amount     Date     \n");
            foreach (Transactions t in Data)
            {
                date = t.DT.ToString("dd/MM/yyyy");
                WriteLine($"     Cash {t.Type}          {t.UserID}         {ub.Name}     {t.Balance}     {date}\n");
            }
        }

        internal void ShowResults(List<CustomerBO> Data)    //shows result of report given by amount.
        {
            WriteLine("\n=== SEARCH RESULTS ===\n");
            WriteLine("     Account ID     User ID        Holder's Name     Type     Balance     Status\n");
            foreach (CustomerBO cbo in Data)
            {
                WriteLine($"     {cbo.AccountNo}          {cbo.UserID}         {cbo.Name}     {cbo.Type}     {cbo.Balance}     {cbo.Status}\n");
            }
        }
        internal void Exit()        //to exit. a single command was enough but making method is more readable.
        {
            System.Environment.Exit(1);
        }
        public void WithdrawCash()      //to withdraw cash with two sub method: i)fast cash ii)normal cash
        {                        //just take input and call the sub method according to input
            WriteLine("WITHDRAW CASH\n\n");
            WriteLine("a) Fast Cash\nb) Normal Cash");
        d1:
            Write("\nPlease select a mode of withdrawal: ");
            string tmp = ReadLine();
            if(tmp=="" || tmp.Length > 1 || (tmp[0] != 'a' && tmp[0] != 'A' && tmp[0] != 'b' && tmp[0] != 'B'))
            {
                WriteLine("\n=> Invalid Input\n");
                goto d1;
            }
            char inp = char.Parse(tmp);
            Clear();
            if (inp == 'a' || inp=='A')
                FastCash();
            else if (inp == 'b' || inp=='B')
                NormalCash();
        }
        public void FastCash()  //just ask admin to enter a number as each number has some amount. and i used a formula to
        {                       //determine the amount against the numbers 1-7
            WriteLine("WITHDRAW CASH - FAST CASH\n");
            WriteLine("1----500\n2----1000\n3----2000\n4----5000\n5----10000\n6----15000\n7----20000\n");
        d1:
            Write("\nSelect one of the dominations of money: ");
            string tmp = ReadLine();
            if (tmp == "" || tmp.Length > 1 || tmp[0] <'1' || tmp[0] > '7')
            {
                WriteLine("\n=> Invalid Input !\n");
                goto d1;
            }
            int choice = int.Parse(tmp);
            decimal amount = 0;
            string cnfrm;
            if (choice == 1)
                amount = 500;
            else if (choice <= 3)
                amount = (choice - 1) * 500 * 2;
            else
                amount = 5000 * (choice - 3);
            Write($"Are you sure you want to withdraw RS.{amount} (Y/N)? ");
            cnfrm = ReadLine();
            if (cnfrm.Length != 1 && cnfrm[0] != 'y' && cnfrm[0] != 'Y')
            {
                WriteLine("\n=> Confirmation Failed\n");
                goto d1;
            }
            bool isAllow = WithdrawCashValidations(amount);
            if (!isAllow)
                goto d1;
        }
        public void NormalCash()    //in this method just take input in original amount
        {
            WriteLine("WITHDRAW CASH - FAST CASH\n");
        d1:
            Write("\nEnter the withdrawal amount: ");
            string tmp = ReadLine();
            bool isValid = bll.AmountValidation(tmp);
            if (!isValid)
            {
                WriteLine("\n=> Invalid Input\n");
                goto d1;
            }
            decimal amount = decimal.Parse(tmp);
            bool isAllow = WithdrawCashValidations(amount);
            if (!isAllow)
                goto d1;
        }
        public bool WithdrawCashValidations(decimal amount) //just create to reduce redundancy as these validations are
        {                                              //common for both type of withdrawal.
            decimal usedAmt = 0;
            bool IsAble = bll.IsAbleToWithdraw(ref usedAmt, cbo, amount);
            if (cbo.Balance < amount)   //to check enoguh balance is present in customer's account or not
            {
                WriteLine($"=> Not enough balance in account - Your current balance is RS.{cbo.Balance}");
                return false;
            }
            else if (!IsAble)   //to check the withdrawal limit i.e. RS.20000
            {
                WriteLine($"=> You can't make such a big transaction - Remaining maximum withdraw amount for today is RS.{20000 - usedAmt}");
                if (20000 - usedAmt == 0)
                    return true;
                else
                    return false;
            }
            else if (IsAble)
            {
                Transactions ts = bll.WithdrawCash(cbo, amount);
                WriteLine("\n=> Cash Successfully Withdrawn !\n");
                AskForReceipt(ts, cbo);   
            }
            return true;
        }
        public void CashTransfer()      //to transfer cash into someone's account. Also checks for certain validations.
        {                           //whether the amount is enough or the account number exist or not.
            WriteLine("CASH TRANSFER\n");
        d1:
            Write("\nEnter the amount in multiple of 500: ");
            string tmp = ReadLine();
            bool isValid = bll.AmountValidation(tmp);
            if (!isValid)
            {
                WriteLine("\n=> Invalid Input\n");
                goto d1;
            }
            decimal amount = decimal.Parse(tmp);
            if(amount % 500 != 0)
            {
                WriteLine("=> We Cant Not Handle Such Data\n");
                goto d1;
            }
            if (amount > cbo.Balance)
            {
                WriteLine($"\n=> You Have Not much Amount In Your Account - You Can Only Transfer Maximum RS.{cbo.Balance}\n");
                goto d1;
            }
        d2:
            Write("Enter the account number to which you want to transfer: ");
            string tmp1 = ReadLine();
            int AccNo = 0;
            bool isTrue = AccountNumberValidations(tmp1, ref AccNo);
            if (!isTrue)
                goto d2;
            CustomerBO tempBo = bll.GetCurrentUser(null, null, AccNo);
            Write($"\n=> You wish to deposit RS.{amount} in account held by {tempBo.Name.ToUpper()}; If this information is correct please re-enter the account number: ");
            string tmp2 = ReadLine();
            if (tmp1 != tmp2)
            {
                WriteLine("\n=> Account Confirmation Failed\n");
                goto d2;
            }
            int cnfrmAcc = int.Parse(tmp1);
            if (AccNo == cnfrmAcc)
            {
                Transactions ts = bll.CashTransfer(cbo, amount, AccNo);
                WriteLine("=> Transaction Confirmed !");
                AskForReceipt(ts, cbo);
            }
        }
        public void DepositCash()       //used to deposit cash into the current user's account.
        {
            WriteLine("DEPOSIT CASH\n");
        d1:
            Write("\nEnter the cash amount you want to deposit: ");
            string tmp = ReadLine();
            bool isValid = bll.AmountValidation(tmp);
            if (!isValid)
            {
                Write("\n=> Invalid Input\n");
                goto d1;
            }
            decimal amount = decimal.Parse(tmp);
            Transactions ts = bll.DepositCash(cbo, amount);
            AskForReceipt(ts, cbo);
        }
        public void DisplayBalance()        //to display current user balance.
        {
            MainBLL bll = new MainBLL();
            CustomerBO ub = bll.GetCurrentUser(null, null, cbo.AccountNo);
            WriteLine($"\nAccount #{ub.AccountNo}\nDate: {DateTime.Now}\nBalance: {ub.Balance}");
        }
        public void AskForReceipt(Transactions ts, CustomerBO cb)       //ask to print receipt or not.
        {
            Write("\nDo you wish to print receipt (Y/N)? ");
            char ch = char.Parse(ReadLine());
            if (ch == 'y' || ch == 'Y')
                PrintReceipt(ts, cbo);
        }
        public void PrintReceipt(Transactions ts, CustomerBO ub)
        {
            MainBLL bll = new MainBLL();
            ub = bll.GetCurrentUser(null, null, ub.AccountNo);
            WriteLine($"\nAccount #{ub.AccountNo}\nDate: {ts.DT}\n\n{ts.Type.ToUpper()}: {ts.Balance}\nBalance: {ub.Balance}");
        }
    }
}
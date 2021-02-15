using ATM_BO;
using ATM_DAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;      //must read program.cs top
using System.Text;
using System.IO;
using System.Linq;
using System.Data;
using System.Threading;

namespace ATM_BLL
{
    public class MainBLL
    {
        public List<AdminBO> ReadAdmin()    //just to read admins from the mainDal readAdmin function
        {
            MainDAL dal = new MainDAL();
            return dal.ReadAdmin();
        }
        public List<CustomerBO> ReadCustomer()   //just to read customers from the mainDal readCustomer function
        {
            MainDAL dal = new MainDAL();
            return dal.ReadCustomer();
        }
        public void Technique(ref string s) //its a technique for the encryption/decryption of important data by just 
        {                                   //converting 'A' to 'Z' and 'Z' to 'A' for both cases of letter and '0' to '9'
            string ns = s;              // and '9' to '0' as described in the design.
            s = "";
            for (int i = 0; i < ns.Length; i++)
            {
                if (ns[i] >= 'A' && ns[i] <= 'Z')
                {
                    int temp = 'N' - ns[i];
                    char c = (char)('N' - 1 + temp);
                    s = s + c;
                }
                else if (ns[i] >= 'a' && ns[i] <= 'z')
                {
                    int temp = 'n' - ns[i];
                    char c = (char)('n' - 1 + temp);
                    s = s + c;
                }
                else if (ns[i] >= '0' && ns[i] <= '9')
                {
                    int temp = '5' - ns[i];
                    char c = (char)('5' -1 + temp);
                    s = s + c;
                }
            }
        }
        public bool CheckLogin(string login)    //for validation of login to check whether login contains an illegal characters.
        {
            if (login == "")
                return false;
            for (int i = 0; i < login.Length; i++)
                if ((login[i] >= 'a' && login[i] <= 'z') || (login[i] >= 'A' && login[i] <= 'Z') || (login[i] >= '0' && login[i] <= '9')) { }
                else
                    return false;
            return true;
        }
        public List<AdminBO> AdminDecryption()  //used for the decryption of whole admin file. just read admin in admin list and conver
        {                                   //it in decrypted form by just calling techninque method for both login and pin.
            List<AdminBO> adm = new List<AdminBO>();
            List<AdminBO> decrypted = new List<AdminBO>();
            MainDAL dal = new MainDAL();
            adm = dal.ReadAdmin();
            foreach(var itr in adm)
            {
                string s1 = itr.Login;
                string s2 = itr.pin;
                Technique(ref s1);
                Technique(ref s2);
                AdminBO tmp = new AdminBO { Login = s1, pin = s2 };
                decrypted.Add(tmp);
            }
            return decrypted;
        }
        public List<CustomerBO> CustomerDecryption()    //used for the decryption of complete customer file. just read customer in customer list and conver
        {                                   //it in decrypted form by just calling techninque method for both login and pin.
            List<CustomerBO> adm = new List<CustomerBO>();
            List<CustomerBO> decrypted = new List<CustomerBO>();
            MainDAL dal = new MainDAL();
            adm = dal.ReadCustomer();
            foreach (CustomerBO itr in adm)
            {
                string s1 = itr.Login;
                string s2 = itr.Pin;
                Technique(ref s1);
                Technique(ref s2);
                CustomerBO tmp = new CustomerBO();
                tmp = itr;
                tmp.Login = s1;
                tmp.Pin = s2;
                decrypted.Add(tmp);
            }
            return decrypted;
        }
        public AdminBO AdminEncryption(AdminBO abo)   //used to encrypt a single admin by just converting its login and pin
        {                                             //using technique. 
            string s1 = abo.Login;
            string s2 = abo.pin;
            Technique(ref s1);
            Technique(ref s2);
            abo.Login = s1;
            abo.pin = s2;
            return abo;
        }

        public CustomerBO CustomerEncryption(CustomerBO cbo)    //used to encrypt a single customer by just converting its login and pin.
        {                                                       //using technique.
            string s1 = cbo.Login;
            string s2 = cbo.Pin;
            Technique(ref s1);
            Technique(ref s2);
            cbo.Login = s1;
            cbo.Pin = s2;
            return cbo;
        }
        public bool IsLoginMatched(string loginCheck, string pinCheck, ref int wrongs, ref int who, ref bool exist)
        {                                  //used to validate login, pin credietials at the time of login into the system
            MainDAL dal = new MainDAL();   //for both admins and customers. if its correct then returns true else check
            bool isMatched = false;      //checks for the 3 wrong inputs and if there are three then returns a signal of 3
            string exName = loginCheck.ToLower();   //wrong inputs.
            if (exName[0] == 'a' && exName[1] == 'd' && exName[2] == 'm' && exName[3] == 'i' && exName[4] == 'n')
                who = -1;   //who==-1 means user is administrator. 
            else
                who = 0;    //who==0 means user is customer. 
            List<AdminBO> admList = new List<AdminBO>();
            List<CustomerBO> usrList = new List<CustomerBO>();
            if (who == -1)      //admin login validation.
            {
                bool isExist = dal.isFileExist("AdministratorData.csv");    //if file does not exist often on the first time access
                if (!isExist)       //to the system then it will automatically geneated the 3 dummy admins as there are no admins
                {                   //bydefault.
                    AdminBO abo1 = new AdminBO { Login = "admin1", pin = "12345" };
                    abo1 = AdminEncryption(abo1);
                    dal.SaveAdmin(abo1);
                    AdminBO abo2 = new AdminBO { Login = "admin2", pin = "12345" };
                    abo2 = AdminEncryption(abo2);
                    dal.SaveAdmin(abo2);
                    AdminBO abo3 = new AdminBO { Login = "admin3", pin = "12345" };
                    abo3 = AdminEncryption(abo3);
                    dal.SaveAdmin(abo3);
                }
                admList = AdminDecryption();
                foreach(AdminBO itr in admList)
                {
                    if (itr.Login == exName && itr.pin == pinCheck)
                    {
                        isMatched = true;
                        break;
                    }
                }
            }
            else if (who == 0)  //customer login validation
            {
                bool isExist = dal.isFileExist("UserAccounts.csv");
                if (!isExist)
                {
                    exist = false;
                    return false;
                }
                usrList = CustomerDecryption();
                foreach (CustomerBO itr in usrList)
                {
                    if (itr.Login == loginCheck && itr.Pin == pinCheck)
                    {
                        isMatched = true;
                        break;
                    }
                }
            }
            if (isMatched == false)
                wrongs++;
            return isMatched;
        }
        public bool IsAccountDisable(string login)  //checks whether the customer with the given login is disables or not.
        {
            List<CustomerBO> list = new List<CustomerBO>();
            list = CustomerDecryption();
            foreach(CustomerBO u in list)
            {
                if (u.Login == login && u.Status == "disable")
                    return true;
            }
            return false;
        }
        public void CreateCustomerAccount(CustomerBO cbo)   //used to create userr by just encrypting it and giving it to
        {                                                   //dal layer.
            CustomerEncryption(cbo);
            MainDAL dll = new MainDAL();
            dll.CreateCustomerAccount(cbo);
        }
        public int LastAccountNumber()  //To get last account number. if file does not exist then simply return 0 else
        {                               //or if file contains 'empty' string again returns 0 and else return account number
            MainDAL dal = new MainDAL();    //of last account created.
            bool isTrue = dal.isFileExist("UserAccounts.csv");
            if (!isTrue)
                return 0;
            else
            {
                List <string> listString= dal.LastAccNo();
                string last = listString.Last();
                if (last[last.Length-1] == 'y')
                     return 0;
                List<CustomerBO> tempCbo = CustomerDecryption();
                CustomerBO cbo = tempCbo.Last();
                return cbo.AccountNo;
            }
        }
        public void DeleteRecord(CustomerBO cbo)    //used to delete record. if the given record for deletion is the only
        {                                       //only recors in the file then simply deletes it and write 'empty' to the file
            string filePath = Path.Combine(Environment.CurrentDirectory, "UserAccounts.csv");   //else simply makes a new list
            List<CustomerBO> listUser = new List<CustomerBO>(); //of customers and store all old data into the new list except the
            List<CustomerBO> newList = new List<CustomerBO>();  //record which we want to delete. and update the new list to file.
            listUser = CustomerDecryption();
            int total = 0;
            MainDAL dal = new MainDAL();
            foreach (CustomerBO b in listUser)
                total++;
            if (total == 1)
            {
                cbo.Login = "empty";
                newList.Add(cbo);
                dal.RestoreCustomerAccounts(newList);
            }
            else
            {
                CustomerBO currentcbo = new CustomerBO();
                foreach (CustomerBO tmp in listUser)
                {
                    if (tmp.AccountNo != cbo.AccountNo)
                    {
                        currentcbo = CustomerEncryption(tmp);
                        newList.Add(currentcbo);
                    }
                        
                }
                dal.RestoreCustomerAccounts(newList);
            }
        }
        public CustomerBO UpdateSingleRecord(CustomerBO cbo, List<string> strList)  //To update single record i.e. if the
        {                                           //any entry in the string list is empty then save the old data in it
            CustomerBO insidecbo = new CustomerBO();    //else write the new data in the particular record.
            insidecbo = cbo;
            if (strList[0] != "")
                insidecbo.Login = strList[0];
            if (strList[1] != "")
                insidecbo.UserID = strList[1];
            if (strList[2] != "")
                insidecbo.Pin = strList[2];
            if (strList[3] != "")
                insidecbo.Name = strList[3];
            if (strList[4] != "")
                insidecbo.Type = strList[4];
            if (strList[5] != "")
                insidecbo.Balance = uint.Parse(strList[5]);
            if (strList[6] != "")
                insidecbo.Status = strList[6];
            return insidecbo;
        }
        public void UpdateRecords(List<string> list, int acc)   //used to update the record with the contents given in the list
        {                                                   //this method just get the decrypted data and transfer it to the new
            List<CustomerBO> old = new List<CustomerBO>();  //list of customers and update the customer with the account
            List<CustomerBO> NewData = new List<CustomerBO>();  //number given by sending that customer to the UpdateSingle
            old = CustomerDecryption();                     //Record.
            CustomerBO cbo = new CustomerBO();
            foreach (CustomerBO tmp in old)
            {
                cbo = tmp;
                if (tmp.AccountNo == acc)
                    cbo = UpdateSingleRecord(tmp, list);
                cbo = CustomerEncryption(cbo);
                NewData.Add(cbo);
            }
            MainDAL dal = new MainDAL();
            dal.RestoreCustomerAccounts(NewData);
        }
        public bool isLoginUseridExist(string login, string userID) //to check whether any customer with given login or user id
        {                                                   //exist in the file or not. if login is null then check it for
            MainDAL dal = new MainDAL();    //user idd and vice versa for the validtion purposes.
            bool isFileExist = dal.isFileExist("UserAccounts.csv");
            if (!isFileExist)
                return false;
            List<CustomerBO> list = new List<CustomerBO>();
            list = CustomerDecryption();
            if (login != null)
            {
                foreach(CustomerBO cbo in list)
                {
                    if (cbo.Login == login)
                        return true;
                }
            }
            else if (userID != null)
            {
                foreach (CustomerBO cbo in list)
                {
                    if (cbo.UserID == userID)
                        return true;
                }
            }
            return false;
        }
        public bool isAdminIDExist(string login)    //simply checks whether any admin exist with the given adminID in order
        {                                       //to validate admin.
            MainDAL dal = new MainDAL();
            bool isFileExist = dal.isFileExist("Administrators.csv");
            if (!isFileExist)
                return false;
            List<AdminBO> list = new List<AdminBO>();
            list = AdminDecryption();
            foreach (AdminBO cbo in list)
            {
                if (cbo.Login == login)
                    return true;
            }
            return false;
        }
        public bool DigitValidation(string pin) //checks whether the given string contains any character except digits. 
        {
            if (pin == "" || pin.Length > 9)
                return false;
            for (int i = 0; i < pin.Length; i++)
                if (pin[i] < '0' || pin[i] > '9')
                    return false;
            return true;
        }
        public bool AmountValidation(string amt)    //checks whether given string contains the negative amount or wrong decimal
        {                                       //input.
            int dot = 0;
            for (int i = 0; i < amt.Length; i++)
                if (amt[i] == '.')
                    dot++;
            if (amt == "" || amt.Length > 28 || dot > 1)
                return false;
            for (int i = 0; i < amt.Length; i++)
                if (amt[i] != '.' && (amt[i] < '0' || amt[i] > '9'))
                    return false;
            return true;
        }
        public bool TypeValidation(string type) //checks whether the given string contains any account type which is not
        {                                       //in our context.
            if (type != "savings" && type != "current")
                return false;
            return true;
        }
        public bool StatusValidation(string status) //checks whether the given string contains any account status which is not
        {                                           //in our context.
            if (status != "active" && status != "disable")
                return false;
            return true;
        }
        public bool isAccountNoExist(int accNo)   //checks whether the given account number exist in our database or not. 
        {
            List<CustomerBO> list = new List<CustomerBO>();
            list = CustomerDecryption();
            foreach (CustomerBO cbo in list)
            {
                if (cbo.AccountNo == accNo)
                    return true;
            }
            return false;
        }
        public bool IsRecordSame(CustomerBO cbo, List<string> list) //checks whther the given customer is same as the 
        {                                                        //the contents present in the string list. if any element of
            int empties = 6;             //list is empty then decrese the empties variable whose value is 6 by default. if
            foreach(string s in list)   //the element of list is equal to the respective cbo data member or the element of
            {       //string list in empty then decrease the empties. at the end if empties equal to 0 return true else false.
                if (s == "")
                    empties--;
            }
            if (list[0] != "" && cbo.AccountNo == int.Parse(list[0]))
                empties--;
            if (cbo.UserID == list[1])
                empties--;
            if (cbo.Name == list[2])
                empties--;
            if (cbo.Type == list[3])
                empties--;
            if (list[4] != "" && cbo.Balance == int.Parse(list[4]))
                empties--;
            if (cbo.Status == list[5])
                empties--;
            if (empties == 0)
                return true;
            return false;
        }
        public List<CustomerBO> SearchAccounts(List<string> strList) //simply searches account by checking the decrypted customers list
        {                                         //and using isRecordSame function for each element of list.
            List<CustomerBO> oldData = new List<CustomerBO>();
            List<CustomerBO> newData = new List<CustomerBO>();
            oldData = CustomerDecryption();
            bool isSame = false;
            foreach(CustomerBO cbo in oldData)
            {
                isSame = IsRecordSame(cbo, strList);
                if (isSame)
                    newData.Add(cbo);
            }
            return newData;
        }
        public List<CustomerBO> ReportByAmount(decimal min, decimal max) //reads the decrypted customer list and save only
        {                                       //those customers in new list whome balance lies between the amount range.
            List<CustomerBO> old = new List<CustomerBO>();
            List<CustomerBO> newData = new List<CustomerBO>();
            old = CustomerDecryption();
            foreach(CustomerBO cbo in old)
            {
                if (cbo.Balance >= min && cbo.Balance <= max)
                    newData.Add(cbo);
            }
            return newData;
        }
        public List<Transactions> ReportByDate(DateTime start, DateTime end, string userID) //reads the transaction
        {                               //list and save those in new list whome dates lies between the given range.
            MainDAL dal = new MainDAL();
            List<Transactions> old = new List<Transactions>();
            List<Transactions> newData = new List<Transactions>();
            old = dal.ReadTransactions();
            foreach(Transactions t in old)
            {
                if (t.DT >= start && t.DT <= end && t.UserID == userID)
                    newData.Add(t);
            }
            return newData;
        }
        public void MakeAccountDisable(string login)    //only make the customer account disable against the given login id. 
        {
            List<CustomerBO> old = new List<CustomerBO>();
            List<CustomerBO> NewData = new List<CustomerBO>();
            old = CustomerDecryption();
            CustomerBO cbo = new CustomerBO();
            foreach (CustomerBO tmp in old)
            {
                cbo = tmp;
                if (tmp.Login == login)
                    cbo.Status = "disable";
                cbo = CustomerEncryption(cbo);
                NewData.Add(cbo);
            }
            MainDAL dal = new MainDAL();
            dal.RestoreCustomerAccounts(NewData);
        }
        public bool IsAbleToWithdraw(ref decimal amt, CustomerBO cbo, decimal amount)   //checks whether the customer is able
        {                                           //to withdraw cash by checking the withdrawal limit of customer
            string path = Path.Combine(Environment.CurrentDirectory, "Transactions.csv"); //whether it reaches 20000 or not.
            bool isExist = File.Exists(path);      //amount is the given amount to withdraw and and ref amt is the amount ramins
            if (!isExist)                          //for the withdraw for today.
                return true;
            else
            {
                MainDAL dal = new MainDAL();
                List<Transactions> listTrns = dal.ReadTransactions();
                amt = CheckTotalWithdrawal(listTrns, cbo);
                if (amt + amount <= 20000)
                    return true;
            }
            return false;
        }
        public decimal CheckTotalWithdrawal(List<Transactions> list, CustomerBO cbo)    //check total withdrawal of a customer
        {                                                         //cbo within a same date from a transaction file.
            decimal ttl = 0;
            foreach(Transactions t in list)
            {
                if (t.UserID == cbo.UserID && t.Type=="withdrawn" && t.DT.Date==DateTime.Now.Date)
                    ttl = ttl + t.Balance;
            }
            return ttl;
        }
        public CustomerBO GetCurrentUser(string login, string userID, int accNo)    //used to get current avtivated customer
        {                                                //by its login id or user id or accNo just to save me from redundant methods 
            List<CustomerBO> list = CustomerDecryption(); //as i have to get current user by all of these parameters at diff places.
            if (login != null)
            {
                foreach (CustomerBO cbo in list)
                {
                    if (cbo.Login == login)
                        return cbo;
                }
            }
            else if(userID != null)
            {
                foreach (CustomerBO cbo in list)
                {
                    if (cbo.UserID == userID)
                        return cbo;
                }
            }
            else if (accNo != 0)
            {
                foreach (CustomerBO cbo in list)
                {
                    if (cbo.AccountNo == accNo)
                        return cbo;
                }
            }
            return null;
        }
        public Transactions WithdrawCash(CustomerBO cbo, decimal amount)    //first it creates transaction of withdraw type
        {                                                       //and then get the decrypted customers in a list and decrement
            Transactions ts = new Transactions { Type = "withdrawn", DT = DateTime.Now, UserID = cbo.UserID, Balance = amount };
            MainDAL dal = new MainDAL();                    //amount from balance of that customer who is cbo.
            dal.CreateTrasaction(ts);
            List<CustomerBO> list = new List<CustomerBO>();
            List<CustomerBO> newList = new List<CustomerBO>();
            list = CustomerDecryption();
            CustomerBO tempcbo = new CustomerBO();
            foreach(CustomerBO u in list)
            {
                if (u.UserID == cbo.UserID)
                    u.Balance = u.Balance - amount;
                tempcbo = u;
                tempcbo = CustomerEncryption(tempcbo);
                newList.Add(tempcbo);
            }
            dal.RestoreCustomerAccounts(newList);
            return ts;
        }
        public Transactions CashTransfer(CustomerBO cbo, decimal amount, int accNo) //used to transfer cash just like withdraw
        {                                      //creates a transaction of transfer type and then decrease the balance from the
            Transactions ts = new Transactions { Type = "transfered", DT = DateTime.Now, UserID = cbo.UserID, Balance = amount };
            MainDAL dal = new MainDAL();    //cbo's balance and increase in the balance of customer whose account number is
            dal.CreateTrasaction(ts);       //accNo.
            List<CustomerBO> list = new List<CustomerBO>();
            List<CustomerBO> newList = new List<CustomerBO>();
            list = CustomerDecryption();
            CustomerBO tempcbo = new CustomerBO();
            foreach (CustomerBO u in list)
            {
                if (u.AccountNo == cbo.AccountNo)
                    u.Balance = u.Balance - amount;
                if (u.AccountNo == accNo)
                    u.Balance = u.Balance + amount;
                tempcbo = u;
                tempcbo = CustomerEncryption(tempcbo);
                newList.Add(tempcbo);
            }
            dal.RestoreCustomerAccounts(newList);
            return ts;
        }
        public Transactions DepositCash(CustomerBO cbo, decimal amount) //used to deposit a cash into a customer account
        {                                //firstly creates a transaction of deposit type and then add amount in the balance
            Transactions ts = new Transactions { Type = "deposited", DT = DateTime.Now, UserID = cbo.UserID, Balance = amount };
            MainDAL dal = new MainDAL();   //of cbo who is current user.
            dal.CreateTrasaction(ts);
            List<CustomerBO> old = CustomerDecryption();
            List<CustomerBO> newList = new List<CustomerBO>();
            CustomerBO tempBo = new CustomerBO();
            foreach (CustomerBO u in old)
            {
                if (u.AccountNo == cbo.AccountNo)
                    u.Balance = u.Balance + amount;
                tempBo = u;
                tempBo = CustomerEncryption(tempBo);
                newList.Add(tempBo);
            }
            dal.RestoreCustomerAccounts(newList);
            return ts;
        }
    }
}

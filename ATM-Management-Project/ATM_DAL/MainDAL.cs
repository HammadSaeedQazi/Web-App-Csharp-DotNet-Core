using ATM_BO;
using System;
using System.Collections.Generic;   //must read program.cs top
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic;

namespace ATM_DAL
{
    public class MainDAL:BaseDAL
    {
        public List<AdminBO> ReadAdmin()    //Used to read list of string from admin file and then convert it into list
        {                                   //of AdminBO by splitting the string by commass and store them in the respective
                List<AdminBO> empList = new List<AdminBO>();   //data member of AdminBO.
                List<string> stringList = Read("AdministratorData.csv");
                foreach (string s in stringList)
                {
                    string[] data = s.Split(",");
                    AdminBO e = new AdminBO();
                    e.Login = data[0];
                    e.pin = data[1];
                    empList.Add(e);
                }
                return empList;
        }

        public List<CustomerBO> ReadCustomer()  //Used to read list of string from customer file and then convert it into list
        {                                       //of CustomerBO by splitting the string by commass and store them in the respective
            List<CustomerBO> empList = new List<CustomerBO>();  //data member of CustomerBO.
            List<string> stringList = Read("UserAccounts.csv");
            foreach (string s in stringList)
            {
                string[] data = s.Split(",");
                CustomerBO e = new CustomerBO();
                e.Login = data[0];
                e.UserID = data[1];
                e.Pin = data[2];
                e.Name = data[3];
                e.Type = data[4];
                e.Balance = decimal.Parse(data[5]);
                e.Status = data[6];
                e.AccountNo = int.Parse(data[7]);
                empList.Add(e);
                CustomerBO.LastAccountNumber = e.AccountNo;
            }
            return empList;
        }
        public List<Transactions> ReadTransactions()    //Used to read list of string from transaction file and then convert it into list
        {                                           //of Transaction by splitting the string by commass and store them in the respective
            List<Transactions> empList = new List<Transactions>();  //data member of Transaction.
            List<string> stringList = Read("Transactions.csv");
            foreach (string s in stringList)
            {
                string[] data = s.Split(",");
                Transactions e = new Transactions();
                e.Type = data[0];
                e.DT = DateTime.Parse(data[1]);
                e.UserID = data[2];
                e.Balance = decimal.Parse(data[3]);
                empList.Add(e);
            }
            return empList;
        }

        public void SaveAdmin(AdminBO bo)   //Used to save object of admin by converting it in a string separated by commas.
        {
            string text = $"{bo.Login},{bo.pin}";
            Save(text, "AdministratorData.csv");
        }
        public void CreateCustomerAccount(CustomerBO ubo) //Used to save object of customer by converting it in a string separated by commas.
        {
            string text = $"{ubo.Login},{ubo.UserID},{ubo.Pin},{ubo.Name},{ubo.Type},{ubo.Balance},{ubo.Status},{ubo.AccountNo}";
            SaveCustomerAccount(text, "UserAccounts.csv");
        }
        public void CreateTrasaction(Transactions ts)  //Used to save object of transaction by converting it in a string separated by commas.
        {
            string text = $"{ts.Type},{ts.DT},{ts.UserID},{ts.Balance}";
            Save(text, "Transactions.csv");
        }
        public void RestoreCustomerAccounts(List<CustomerBO> userList)  //it converts list of CustomerBO into the list of
        {                                                   //strings and then give it to base dal restoreUserAccount to save
            List<string> strList = new List<string>();  //them.
            if (userList[0].Login == "empty")
                strList.Add("empty");
            else
            {
                foreach(CustomerBO ubo in userList)
                {
                    strList.Add($"{ubo.Login},{ubo.UserID},{ubo.Pin},{ubo.Name},{ubo.Type},{ubo.Balance},{ubo.Status},{ubo.AccountNo}");
                }
            }
            RestoreCustomerAccounts(strList, "UserAccounts.csv");
        }
        public bool isFileExist(string fileName)  //used to check whether the file with given 'filename' exist or not.
        {
            string path = Path.Combine(Environment.CurrentDirectory, fileName);
            bool isExist = File.Exists(path);
            return isExist;
        }
        public List<string> LastAccNo()  //used to get lasst account number in the file by reading the file data in a list
        {                                   //of strings.
            List<string> stringList = Read("UserAccounts.csv");
            return stringList;
        }
    }
}

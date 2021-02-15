using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class CustomerBO   //Customer bussiness object with data type login, password, user name, account type, balance,
    {                       //account status, account number, user id and last account number
        public string Login { get; set; }
        public string Pin { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public int AccountNo { get; set; }
        public string UserID { get; set; }
        public static int LastAccountNumber { get; set; }
    }
}

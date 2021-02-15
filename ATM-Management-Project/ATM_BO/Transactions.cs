using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_BO
{
    public class Transactions  //In order to save transactions efficiently i created this class.
    {
        public string Type { get; set; }
        public DateTime DT { get; set; }
        public string UserID { get; set; }
        public decimal Balance { get; set; }
    }
}

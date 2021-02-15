using System;
using System.Collections.Generic;   //must read program.cs top
using System.Text;
using System.IO;

namespace ATM_DAL
{
    public class BaseDAL    //This layer is created separated from Main_DAL just to increase readability.
    {
        internal List<string> Read(string fileName)            //Used to read any file with name 'fileName'. Only opnes file
        {                                                      //and read it contents in list of string and returns it.
            List<string> list = new List<string>();
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamReader sr = new StreamReader(filePath);
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {

                list.Add(line);

            }
            sr.Close();
            return list;
        }

        internal void Save(string text, string fileName)     //Used to save a string text in a file with name 'fileName'
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            StreamWriter sw = new StreamWriter(filePath, append: true);
            sw.WriteLine(text);
            sw.Close();
        }
        internal void SaveCustomerAccount(string text, string fileName) //Basically used to save CustomerAccount. But with
        {                                                               //some additional functionality i.e. when file will be 
            List<string> checkEmpty = new List<string>();               //empty as it will contain 'empty' string in this case 
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName); //so the nw record will be overwritten to erase
            bool isExist = File.Exists(filePath);                                   //that pre written 'empty' string
            if (isExist)
            {
                checkEmpty = Read("UserAccounts.csv");
                if (checkEmpty[0] == "empty")
                {
                    StreamWriter swn = new StreamWriter(filePath);
                    swn.WriteLine(text);
                    swn.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(filePath, append: true);
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(filePath, append: true);
                sw.WriteLine(text);
                sw.Close();
            }
        }
        internal void RestoreCustomerAccounts(List<string> text, string fileName)  //Very important function use to overwrite
        {                                                                   //updated data from list string by erasing all
            string filePath = Path.Combine(Environment.CurrentDirectory, fileName);  //previous data.
            StreamWriter sw = new StreamWriter(filePath);
            foreach(string str in text)
                sw.WriteLine(str);
            sw.Close();
        }
    }
}

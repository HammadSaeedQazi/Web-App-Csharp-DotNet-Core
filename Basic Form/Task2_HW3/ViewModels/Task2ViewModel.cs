using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Task2_HW3.Commands;

namespace Task2_HW3.ViewModels
{
    class Task2ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        string name;
        public string Name { get=>name; set { name = value;  OnPropertyChanged("name");} }
        ComboBoxItem gender;
        public ComboBoxItem selectedGender { get=>gender; set { gender = value;   OnPropertyChanged("gender"); } }
        bool isYes;
        public bool isYesChecked { get=>isYes; set { isYes = value;  OnPropertyChanged("isYes"); } }
        bool isNo;
        public bool isNoChecked { get=>isNo; set { isNo = value;  OnPropertyChanged("isNo"); } }
        public DelegateCommand sbmtCmnd { get; set; }
        public DelegateCommand cancelCmnd { get; set; }
        public Task2ViewModel()
        {
            cancelCmnd = new DelegateCommand(Cancel, canCancel);
            sbmtCmnd = new DelegateCommand(Submit, canSbmt);
        }

        public bool canCancel(object o)
        {
            return true;
        }
        public void Cancel(object o)
        {
            Application.Current.Shutdown(-1);
        }
        public bool canSbmt(object o)
        {
            if (Name != "" && Name != null && selectedGender != null && (isYesChecked || isNoChecked))
                return true;
            else return false;
        }
        public void Submit(object o)
        {
            if (Name == "" || Name == null || !nameValidations())
            {
                MessageBox.Show("Invalid Name !");
            }
            else
            {
                string temp = "";
                if (isYesChecked)
                    temp = "Yes";
                else
                    temp = "No";
                MessageBox.Show($"Name:    {Name}\n\nGender:    {selectedGender.Content.ToString()}\n\nGraduated:    {temp}");
                Name = "";
                selectedGender.Content = "";
            }
            Popup mypopup = new Popup();
            mypopup.IsOpen = true;
        }
        internal bool nameValidations()
        {
            int len = Name.Length;
            int count = 0;
            for (int i = 0; i < len; i++)
            {
                if (Name[i] == ' ')
                    count++;
                if ((Name[i] >= 'a' && Name[i] <= 'z') || (Name[i] >= 'A' && Name[i] <= 'Z') || Name[i] == ' ') { }
                else
                    return false;
            }
            if (count == len)
                return false;
            return true;
        }
    }
}

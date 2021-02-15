using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Task1_HW3.Commands;

namespace Task1_HW3.ViewModels
{
    class PasswordViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private string id;
        public string ID { get => id; set { id = value;  OnPropertyChanged("id"); } }
        private string abc;
        public string ABC { get => abc; set { abc = value; OnPropertyChanged("abc"); } }
        public DelegateCommand cnfrmBtn { get; set; }
        public DelegateCommand cancelBtn { get; set; }
        public PasswordViewModel()
        {
            cnfrmBtn = new DelegateCommand(Confirm, canCnfrm);
            cancelBtn = new DelegateCommand(Cancel, canCancel);
        }
        public bool canCnfrm(object o)
        {
            var pswBoxes = o as object[];
            var OldPass = pswBoxes[0] as PasswordBox;
            var NewPass = pswBoxes[1] as PasswordBox;
            var CnfrmPass = pswBoxes[2] as PasswordBox;

            isPassMatched(NewPass.Password, CnfrmPass.Password);

            if (ID != null && ID != "" && OldPass.Password != null && OldPass.Password != "" && NewPass.Password != null && NewPass.Password != "" && CnfrmPass.Password != null && CnfrmPass.Password != "")
                return true;
            return false;
        }
        public void Confirm(object o)
        {
            var pswBoxes = o as object[];
            var OldPass = pswBoxes[0] as PasswordBox;
            var NewPass = pswBoxes[1] as PasswordBox;
            var CnfrmPass = pswBoxes[2] as PasswordBox;
            if (NewPass.Password != CnfrmPass.Password  ) { }
            else if (!IDValidation())
                MessageBox.Show("Invalid ID Input !");
            else if (!passwordValidation(OldPass.Password))
                MessageBox.Show("Invalid Old Password Input !");
            else if (!passwordValidation(NewPass.Password))
                MessageBox.Show("Invalid New Password !");
            else
            {
                MessageBox.Show($"Yipee ! You Have Successfully Setted Your New Password !\n\nID:   {ID}" +
                    $"\n\nOld Password:   { OldPass.Password}\n\nNew Password:   {NewPass.Password}");
                ID = "";
                OldPass.Password = "";
                NewPass.Password = "";
                CnfrmPass.Password = "";
            }
        }
        public bool canCancel(object o)
        {
            return true;
        }
        public void Cancel(object o)
        {
            Application.Current.MainWindow.Close();
        }
        internal bool IDValidation()
        {
            if (string.IsNullOrEmpty(ID) || string.IsNullOrWhiteSpace(ID))
                return false;
            for (int i = 0; i < ID.Length; i++)
                if ((ID[i] >= 'a' && ID[i] <= 'z') || (ID[i] >= 'A' && ID[i] <= 'Z') || (ID[i] >= '0' && ID[i] <= '9')) { }
                else
                    return false;
            return true;
        }
        internal bool passwordValidation(string pass)
        {
            for (int i = 0; i < pass.Length; i++)
                if (pass[i] == ' ')
                    return false;
            if (string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass))
                return false;
            return true;
        }
        internal void isPassMatched(string pass1, string pass2)
        {
            if (pass1 == pass2 && pass1 != "" && pass1 != null && pass2 != "" && pass2 != null)
                ABC = "* Password Matched *";
            else if (pass2 != "" && pass2 != null)
                ABC = "* Not Match *";
            else
                ABC = null;
        }
    }
}

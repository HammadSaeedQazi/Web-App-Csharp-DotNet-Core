using Online_Grocery_System.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Online_Grocery_System.ViewModels
{
    class StartupViewModel:BaseViewModel
    {
        public DelegateCommand exitCmnd { get; set; }   //binded with the exit button in StartupView.xaml and remaining two buttons
        public StartupViewModel()               //are binded with the BaseViewModel command as they are gonna change views.
        {
            exitCmnd = new DelegateCommand(exit, canExit);
        }
        public bool canExit(object o)
        {   
            return true;
        }
        public void exit(object o)      //just closes the main window.
        {
            Application.Current.MainWindow.Close();
        }
    }
}

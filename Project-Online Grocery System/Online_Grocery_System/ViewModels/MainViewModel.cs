using System;
using System.Collections.Generic;
using System.Text;

namespace Online_Grocery_System.ViewModels
{
    class MainViewModel:BaseViewModel
    {
        public MainViewModel()
        {
            SelectedViewModel = new StartupViewModel(); //do nothing but make the StartupViewModel as default view.
        }
    }
}

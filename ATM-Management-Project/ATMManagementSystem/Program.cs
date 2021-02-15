using ATM_VIEW;     //there are two mistakes in this assignment.
using System;       // 1) in reports by amount, i searched user with the current amount between the range... xD
                    // 2) in reports by date, for ending date the time considered is 12 AM.
namespace ATMManagementSystem   //thats why if ending date is 16 january then the trasanctions of 16th jan will not 
{                               //be displayed.
    class Program                       
    {                                  
        static void Main(string[] args)     
        {                               
            MainView v = new MainView();    
            v.MainScreen();
            //v.DisplayUser();
        }
    }
}
//Must Read !
/*i) There exist three dummy admins by default by which any admin can log-in and can do further processes.

		Login			Pin
		admin1			12345
		admin2			12345
		admin3			12345

ii) Admin name must starts with 'Admin' in lower or upper or mix case, otherwise it will be consider a customer.*/
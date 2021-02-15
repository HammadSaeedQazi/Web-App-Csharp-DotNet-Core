using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    static public class UserValidations
    {
        static public bool isUsernameValid(string username) //username validtions. only digits, characters and some special characters
        {                                                               //are allowd.
            for(int i = 0; i < username.Length; i++)
            {
                if ((username[i] >= 'a' && username[i] <= 'z') || (username[i] >= 'A' && username[i] <= 'Z') || (username[i] >= '0' && username[i] <= '9') || username[i] == '_' || username[i] == '@' || username[i] == '.')
                {

                }
                else
                    return false;
            }
            return true;
        }
        static public bool isUserExist(string username) //checks whether the given username already present in theDB.
        {
            List<RegularUser> userData = UserRepository.ReturnUsers();
            foreach(RegularUser ru in userData)
            {
                if (ru.Username == username)
                    return true;
            }
            return false;
        }
        static public bool checkUserExist(string username, string currentUser)  //checks the username already present in DB but not
        {                                                                       //match it with the currentUser.
            List<RegularUser> userData = UserRepository.ReturnUsers();
            foreach (RegularUser ru in userData)
            {
                if (ru.Username == username && ru.Username!=currentUser)
                    return true;
            }
            return false;
        }
        static public bool isAdmin(string usr)  //checks whether the usr is admin or not.
        {
            List<RegularUser> userData = UserRepository.ReturnUsers();
            if (usr.Length >= 5 && usr[0] == 'a' && usr[1] == 'd' && usr[2] == 'm' && usr[3] == 'i' && usr[4] == 'n')
                return true;
            return false;
        }
        static public bool isEmailExist(string email)   //check for email existance.
        {
            List<RegularUser> userData = UserRepository.ReturnUsers();
            foreach (RegularUser ru in userData)
            {
                if (ru.Email == email)
                    return true;
            }
            return false;
        }
        static public bool checkEmailExist(string email, string currentEmail)   //check for email existance but not match it with
        {                                                                               //currentEmail.
            List<RegularUser> userData = UserRepository.ReturnUsers();
            foreach (RegularUser ru in userData)
            {
                if (ru.Email == email && ru.Email != currentEmail)
                    return true;
            }
            return false;
        }
    }
}

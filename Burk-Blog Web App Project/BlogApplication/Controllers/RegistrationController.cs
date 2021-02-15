using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public ViewResult Login()   //simply returns the login view which is the default view of our application.
        {
            if(HttpContext.Session.Keys.Contains("CurrentUser"))    //removes the session if user want to go to login page.
                HttpContext.Session.Remove("CurrentUser");
            else if(HttpContext.Session.Keys.Contains("CurrentAdmin"))
                HttpContext.Session.Remove("CurrentAdmin");
            return View();
        }
        [HttpPost]
        public IActionResult Login(RegularUser regUsr)  //checks for the credentials and validations and allow user accordingly as
        {                                              //admin or normal user.
            List<RegularUser> userData = UserRepository.ReturnUsers();
            if (!string.IsNullOrEmpty(regUsr.Username) && !string.IsNullOrEmpty(regUsr.Password))   //self validtions instead of
            {                                                                                 //ModelState.IdValid.
                regUsr.Username = regUsr.Username.ToLower();
                bool isExist = UserValidations.isUserExist(regUsr.Username.ToLower());  //checks for user exist
                bool isValid = UserValidations.isUsernameValid(regUsr.Username.ToLower());  //username validation.
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username: Only letters, digits, @, _ and . are allowed !");
                    return View();
                }
                if (!isExist)
                {
                    ModelState.AddModelError(string.Empty, "Username does not exist !");
                    return View();
                }
                foreach (RegularUser usr in userData)
                {
                    if (usr.Username == regUsr.Username && usr.Password == regUsr.Password) //if matches with any record in DB.
                    {           //below is the check for admin.
                        if (regUsr.Username[0] == 'a' && regUsr.Username[1] == 'd' && regUsr.Username[2] == 'm' && regUsr.Username[3] == 'i' && regUsr.Username[4] == 'n')
                        {
                            HttpContext.Session.SetString("CurrentAdmin", usr.Username); //make session for admin here.
                            List<RegularUser> newData = AdminController.checkForAdmins(userData);
                            return RedirectToAction("AdminPanel", "Admin", newData);
                        }
                        else        //if entered credentials are correct and of some normal user except admin.
                        {
                            HttpContext.Session.SetString("CurrentUser", usr.Username); //makes session for user.
                            List<Post> postData = PostRepository.ReturnPosts();
                            AdminController.manageProfilePic(ref postData);
                            postData.Reverse();
                            ViewBag.Id = usr.Id;
                            return RedirectToAction("AtHome", "General", postData);
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Login credentials do not matched !");
                return View();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some data is missing !");
                return View();
            }
        }
        [HttpGet]
        public ViewResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(RegularUser usr)   //simply add a new user by taking inputs and applying validations.
        {
            if (ModelState.IsValid)
            {
                List<RegularUser> userData = UserRepository.ReturnUsers();
                bool isExist = UserValidations.isUserExist(usr.Username.ToLower()); //checks whether same username already exist?
                bool checkEmailExist = UserValidations.isEmailExist(usr.Email.ToLower());   //checks whther same email already exist?
                bool isValid = UserValidations.isUsernameValid(usr.Username.ToLower()); //username validations.
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username: Only letters, digits, @, _ and . are allowed !");
                    return View();
                }
                if (isExist)
                {
                    ModelState.AddModelError(string.Empty, "Username already exist !");
                    return View();
                }
                if (checkEmailExist)
                {
                    ModelState.AddModelError(string.Empty, "Email already exist !");
                    return View();
                }
                if (usr.Password != usr.anotherPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password confirmation failed !");
                    return View();
                }
                usr.Username = usr.Username.ToLower();
                usr.Email = usr.Email.ToLower();
                UserRepository.AddUser(usr);
                return View("Congrats", usr);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some data is missing !");
                return View();
            }
        }
        public IActionResult Congrats() //simply return view with direct address validation.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            else
                return View();
        }
        public ViewResult Logout()
        {
            if (HttpContext.Session.Keys.Contains("CurrentUser"))
                HttpContext.Session.Remove("CurrentUser");
            else if (HttpContext.Session.Keys.Contains("CurrentAdmin"))
                HttpContext.Session.Remove("CurrentAdmin");
            return View("Login");
        }
    }
}

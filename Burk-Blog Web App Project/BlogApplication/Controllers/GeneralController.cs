using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class GeneralController : Controller
    {
        [HttpGet]
        public IActionResult AtHome(int id)     //return the home view with the id of current loged in user with validations of address.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            List<Post> postData = PostRepository.ReturnPosts();
            AdminController.manageProfilePic(ref postData); //in order ot manage pics or add default pic in case of no pic
            if(id == 0)
            {
                RegularUser regUsr = userData.Find(regUsr => regUsr.Username == HttpContext.Session.GetString("CurrentUser"));
                id = regUsr.Id;
            }
            ViewBag.Id = id;    //to send id of current user to view as well to manage navbar there.
            postData.Reverse();
            return View("AtHome", postData);
        }
        [HttpGet]
        public IActionResult Profile(int id)    //return view and the user against the given id.
        {
            if(!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            if (string.IsNullOrEmpty(ru.picAddress))
                ru.picAddress = "~/images/temp.jpg";
            ViewBag.Id = id;
            return View("Profile", ru);
        }
        [HttpPost]
        public ViewResult Profile(RegularUser usr)  //manage the profile and all the fields updated by user.
        {
            string oldUsername = null;
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser rus = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            if (ModelState.IsValid)
            {                       //some validations for username and email.
                bool isExist = UserValidations.checkUserExist(usr.Username.ToLower(), rus.Username);
                bool isValid = UserValidations.isUsernameValid(usr.Username.ToLower());
                bool checkEmailExist = UserValidations.checkEmailExist(usr.Email.ToLower(), rus.Email);
                if(!isValid || isExist || checkEmailExist)  //to save default profile pic.
                {
                    ViewBag.Id = rus.Id;
                    if (string.IsNullOrEmpty(rus.picAddress))
                        rus.picAddress = "~/images/temp.jpg";
                }
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username: Only letters, digits, @, _ and . are allowed !");
                    return View("Profile", rus);
                }
                if (isExist)
                {
                    ModelState.AddModelError(string.Empty, "Username already exist !");
                    return View("Profile", rus);
                }
                if (checkEmailExist)
                {
                    ModelState.AddModelError(string.Empty, "Email already exist !");
                    return View("Profile", rus);
                }
                RegularUser ru = userData.Find(ru => ru.Id == usr.Id);      //password confirmation here.
                if(ru.Password==usr.Password)
                    oldUsername = ru.Username;
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect old password !");
                    ViewBag.Id = ru.Id;
                    if (string.IsNullOrEmpty(rus.picAddress))
                        rus.picAddress = "~/images/temp.jpg";
                    return View("Profile", rus);
                }

                if (!string.IsNullOrEmpty(ru.picAddress) && usr.profilePicture != null)    //in case of updated pic, old will be deleted.
                {
                    string[] listStr = ru.picAddress.Split("~/");   //address will be splited as we need only second part
                    var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", listStr[listStr.Length - 1]);  //combines path
                    System.IO.File.Delete(path);
                }

                if (usr.profilePicture != null) //to upload profile picture same as in admin controller, add user action method.
                {                               
                    var uploadeFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot/Images");
                    string sourcefile = HttpContext.Session.GetString("CurrentUser") + "-" + "profile_pic" + "-" + usr.profilePicture.FileName;
                    usr.picAddress = Path.Combine("~/images/", sourcefile);
                    string destinationPath = Path.Combine(uploadeFolder, sourcefile);
                    using (var filestream = new FileStream(destinationPath, FileMode.Create))
                    {
                        usr.profilePicture.CopyTo(filestream);
                    }
                }

                usr.Email = usr.Email.ToLower();
                usr.Username = usr.Username.ToLower();
                UserRepository.UpdateUser(usr);
                HttpContext.Session.SetString("CurrentUser", usr.Username);     //update session here for new username.
                List<Post> postData = PostRepository.ReturnPosts();
                foreach (Post p in postData)    //change username on posts as well.
                {
                    if (p.Usr == oldUsername)
                    {
                        p.Usr = usr.Username;
                        PostRepository.UpdatePost(p);
                    }
                }
                postData = PostRepository.ReturnPosts();
                userData = UserRepository.ReturnUsers();
                AdminController.manageProfilePic(ref postData);
                ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
                ViewBag.Id = ru.Id;
                postData.Reverse();
                return View("AtHome", postData);
            }
            else     //in case of invalid inputs. same way in the all other action methods and controllers with some changes.
            {
                ModelState.AddModelError(string.Empty, "Please enter correct data !");
                if (string.IsNullOrEmpty(rus.picAddress))
                    rus.picAddress = "~/images/temp.jpg";
                ViewBag.Id = rus.Id;
                return View("Profile", rus);
            }
        }
        public IActionResult About(int id)  //retutn the view and some details of the user with the id given.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Id == id);
            if (ru == null)     //if user want to see about info of some other user.
            {
                List<Post> postData = PostRepository.ReturnPosts();
                Post p = postData.Find(p => p.Id == id);
                ru = userData.Find(usr => usr.Username == p.Usr);
            }
            if (string.IsNullOrEmpty(ru.picAddress))
                ru.picAddress = "~/images/temp.jpg";
            RegularUser regUsr = userData.Find(regUsr => regUsr.Username == HttpContext.Session.GetString("CurrentUser"));
            ViewBag.Id = regUsr.Id;
            return View("About", ru);
        }
    }
}

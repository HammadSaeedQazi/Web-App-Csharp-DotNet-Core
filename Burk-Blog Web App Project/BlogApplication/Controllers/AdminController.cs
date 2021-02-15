using BlogApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminPanel()   ////just take list of users and show it in Admin default view
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))      //if no session present then simply take user to login page
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            List<RegularUser> newData = checkForAdmins(userData);
            return View("AdminPanel", newData);
        }
        public ViewResult RemoveUser(int id)    //remove user by given id parameter sent from admin view. it also removes the picture
        {                                       //of that user. remove user by simply calling a function from user repository.
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser regUsr = userData.Find(regUsr => regUsr.Id == id);
            if (!string.IsNullOrEmpty(regUsr.picAddress))
            {
                string[] listStr = regUsr.picAddress.Split("~/");               //split the path, as we only need path after ~/.
                var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", listStr[listStr.Length - 1]);
                System.IO.File.Delete(path);
            }
            UserRepository.RemoveUser(id);
            List<Post> postData = PostRepository.ReturnPosts();
            foreach (Post p in postData)
            {
                if (p.Usr == regUsr.Username)
                    PostRepository.RemovePost(p.Id);
            }
            userData = UserRepository.ReturnUsers();
            List<RegularUser> newData = checkForAdmins(userData); //to check that there must be no admin should present in the list to show.
            return View("AdminPanel", newData);
        }
        [HttpGet]
        public IActionResult AddUser()  //just return view plus checks for the direct address issue.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))
                return RedirectToAction("Login", "Registration");
            return View();
        }
        [HttpPost]
        public ViewResult AddUser(RegularUser usr)  //add user in DB by applying validations with the help of uservalidation class.
        {
            if (ModelState.IsValid)
            {
                bool isExist = UserValidations.isUserExist(usr.Username.ToLower()); //check for username already exist
                bool checkEmailExist = UserValidations.isEmailExist(usr.Email.ToLower());   //check for email already exist
                bool isValid = UserValidations.isUsernameValid(usr.Username.ToLower()); //check for username validation
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
                List<RegularUser> userData = UserRepository.ReturnUsers();
                if (usr.Password != usr.anotherPassword)    //password confirmation
                {
                    ModelState.AddModelError(string.Empty, "Password confirmation failed !");
                    return View();
                }

                if (usr.profilePicture != null)     //upload profile picture if user add it in view.
                {
                    var uploadeFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot/Images");   //combines the resident path.
                    string sourcefile = usr.Username + "-" + "profile_pic" + "-" + usr.profilePicture.FileName; //makes filename
                    usr.picAddress = Path.Combine("~/images/", sourcefile); //combine both addresses
                    string destinationPath = Path.Combine(uploadeFolder, sourcefile);   //combines both folder + filename
                    using (var filestream = new FileStream(destinationPath, FileMode.Create))
                    {
                        usr.profilePicture.CopyTo(filestream);  //saves picture with filestream object.
                    }
                }
                //add user credentials except password in lower format.
                usr.Email = usr.Email.ToLower();
                usr.Username = usr.Username.ToLower();
                UserRepository.AddUser(usr);
                userData = UserRepository.ReturnUsers();
                List<RegularUser> newData = checkForAdmins(userData);
                return View("AdminPanel", newData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some data is missing !");
                return View();
            }
        }
        [HttpGet]
        public IActionResult UpdateUser(int id)     //just return users against the given id with some validations of address.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser regUsr = userData.Find(regUsr => regUsr.Id == id);
            if (string.IsNullOrEmpty(regUsr.picAddress))        //in case of no pic, default pic will be added.
                regUsr.picAddress = "~/images/temp.jpg";
            return View("UpdateUser", regUsr);
        }
        [HttpPost]
        public ViewResult UpdateUser(RegularUser usr)   //update user with much validations same as above adduser.
        {
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser regUsr = userData.Find(regUsr => regUsr.Id == usr.Id);
            string oldUsername = regUsr.Username;
            string oldEmail = regUsr.Email;     //below the self validations by me as ModelValidations are not applicable here.
            if (!string.IsNullOrEmpty(usr.Username) && !string.IsNullOrEmpty(usr.Email) && !string.IsNullOrEmpty(usr.anotherPassword))
            {
                bool isExist = UserValidations.checkUserExist(usr.Username.ToLower(), oldUsername);     //same validations for adduser,
                bool isValid = UserValidations.isUsernameValid(usr.Username.ToLower());         //but old username and emails are
                bool checkEmailExist = UserValidations.checkEmailExist(usr.Email.ToLower(), oldEmail);  //sent along with new.
                if(!isValid || isExist || checkEmailExist)      //to add default picture.
                {
                    if (string.IsNullOrEmpty(regUsr.picAddress))
                        regUsr.picAddress = "~/images/temp.jpg";
                }
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username: Letters, digits, @, _ and . are allowed !");
                    return View("UpdateUser", regUsr);
                }
                if (isExist)
                {
                    ModelState.AddModelError(string.Empty, "Username already exist !");
                    return View("UpdateUser", regUsr);
                }
                if (checkEmailExist)
                {
                    ModelState.AddModelError(string.Empty, "Email already exist !");
                    return View("UpdateUser", regUsr);
                }

                if (!string.IsNullOrEmpty(regUsr.picAddress) && usr.profilePicture != null) //removes previous picture if present
                {                                                                           //in case of new pic uploaded by user.
                    string[] listStr = regUsr.picAddress.Split("~/");
                    var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", listStr[listStr.Length - 1]);
                    System.IO.File.Delete(path);
                }

                if (usr.profilePicture != null) //to upload profile picture same as in adduser.
                {       
                    var uploadeFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot/Images");
                    string sourcefile = usr.Username + "-" + "profile_pic" + "-" + usr.profilePicture.FileName;
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
                List<Post> postData = PostRepository.ReturnPosts();
                foreach (Post p in postData)        //update posts usernames.
                {
                    if (p.Usr == oldUsername)
                    {
                        p.Usr = usr.Username;
                        PostRepository.UpdatePost(p);
                    }
                }
                userData = UserRepository.ReturnUsers();
                List<RegularUser> newData = checkForAdmins(userData);
                return View("AdminPanel", newData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some data is missing !");
                if (string.IsNullOrEmpty(regUsr.picAddress))
                    regUsr.picAddress = "~/images/temp.jpg";
                return View("UpdateUser", regUsr);
            }

        }
        public IActionResult ManagePosts()      //just return posts to view.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            List<Post> postData = PostRepository.ReturnPosts();
            manageProfilePic(ref postData);
            postData.Reverse();     //to reverse list as new post should be present of the top.
            return View("ManagePosts", postData);
        }
        public IActionResult PostView(int id)   //show the post in view against the given id.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            List<Post> postData = PostRepository.ReturnPosts();
            Post p = postData.Find(p => p.Id == id);
            RegularUser usr = userData.Find(usr => usr.Username == p.Usr);
            if (string.IsNullOrEmpty(usr.picAddress))
                p.usrPP = "~/images/temp.jpg";
            else
                p.usrPP = usr.picAddress;
            return View("PostView", p);
        }
        public ViewResult DeletePost(int id)    //delete post against the given id.
        {
            PostRepository.RemovePost(id);
            List<Post> postData = PostRepository.ReturnPosts();
            List<RegularUser> userData = UserRepository.ReturnUsers();
            manageProfilePic(ref postData);
            postData.Reverse();
            return View("ManagePosts", postData);
        }
        [HttpGet]
        public IActionResult UpdatePost(int id) //return post view with the post having the id given in the parameter.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentAdmin"))
                return RedirectToAction("Login", "Registration");
            List<Post> postData = PostRepository.ReturnPosts();
            Post p = postData.Find(p => p.Id == id);
            return View("UpdatePost", p);
        }
        [HttpPost]
        public ViewResult UpdatePost(Post p)    //update post with all the new entries.
        {
            List<Post> postData = PostRepository.ReturnPosts();
            Post pst = postData.Find(pst => pst.Id == p.Id);
            p.Usr = pst.Usr;
            if (ModelState.IsValid)
            {
                PostRepository.UpdatePost(p);
                List<RegularUser> userData = UserRepository.ReturnUsers();
                postData = PostRepository.ReturnPosts();
                manageProfilePic(ref postData);
                postData.Reverse();
                return View("ManagePosts", postData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some Data is missing !");
                return View("UpdatePost", p);
            }
        }
        static public void manageProfilePic(ref List<Post> postData)   //checks or the picaddress for each user. if present then ok else
        {                                                   //save the default pic address there.
            List<RegularUser> userData = UserRepository.ReturnUsers();
            foreach (Post ps in postData)       //to manage profile picture.
            {
                RegularUser usr = userData.Find(usr => usr.Username == ps.Usr);
                if (string.IsNullOrEmpty(usr.picAddress))
                    ps.usrPP = "~/images/temp.jpg";
                else
                    ps.usrPP = usr.picAddress;
            }
        }
        static public List<RegularUser> checkForAdmins(List<RegularUser> userData)
        {
            List<RegularUser> newData = new List<RegularUser>();
            foreach (RegularUser u in userData)         //checks for admins.
            {
                bool isAdmin = UserValidations.isAdmin(u.Username);
                if (!isAdmin)
                    newData.Add(u);
            }
            return newData;
        }
    }
}

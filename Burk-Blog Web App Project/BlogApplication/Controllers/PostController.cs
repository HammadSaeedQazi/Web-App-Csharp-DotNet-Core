using BlogApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        public IActionResult CreatePost()   //return view of create post with the direct address validations.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            ViewBag.Id = ru.Id;
            return View();
        }
        [HttpPost]
        public ViewResult CreatePost(Post p)    //saves post posted by the user in DB.
        {
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            if (ModelState.IsValid)
            {
                p.Date = System.DateTime.Now.ToString("dddd, dd MMMM yyyy h:mm:tt");    //save current time for the post
                p.Usr = HttpContext.Session.GetString("CurrentUser");   //save current user for the post.
                PostRepository.AddPost(p);
                List<Post> postData = PostRepository.ReturnPosts();
                AdminController.manageProfilePic(ref postData);
                ViewBag.Id = ru.Id;
                postData.Reverse();
                return View("~/Views/General/AtHome.cshtml", postData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some data is missing !");
                return View();
            }
        }
        public IActionResult ViewPost(int id)   //selectes which view to return with the post having id given in parameter.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            ViewBag.Id = ru.Id;
            List<Post> postData = PostRepository.ReturnPosts();
            Post p = postData.Find(p => p.Id == id);
            RegularUser usr = userData.Find(usr => usr.Username == p.Usr);
            if (string.IsNullOrEmpty(usr.picAddress))
                p.usrPP = "~/images/temp.jpg";
            else
                p.usrPP = usr.picAddress;
            if (p.Usr == HttpContext.Session.GetString("CurrentUser"))  //if user clicks on its own post then this view will be showm.
            {
                return View("ViewOwnPost", p);
            }
            else                      //if user clicks on someone else post then this post will be shown.
            {
                return View("ViewOtherPost", p);
            }
        }
        public ViewResult RemovePost(int id)    //remove post with the help of postrepository class.
        {
            PostRepository.RemovePost(id);
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            ViewBag.Id = ru.Id;
            List<Post> postData = PostRepository.ReturnPosts();
            AdminController.manageProfilePic(ref postData);
            postData.Reverse();
            return View("~/Views/General/AtHome.cshtml", postData);
        }
        [HttpGet]
        public IActionResult EditPost(int id)   //return view with the post having id equals to the id given.
        {
            if (!HttpContext.Session.Keys.Contains("CurrentUser"))
                return RedirectToAction("Login", "Registration");
            List<RegularUser> userData = UserRepository.ReturnUsers();
            RegularUser ru = userData.Find(ru => ru.Username == HttpContext.Session.GetString("CurrentUser"));
            ViewBag.Id = ru.Id;
            List<Post> postData = PostRepository.ReturnPosts();
            Post p = postData.Find(p => p.Id == id);
            return View("EditPost", p);
        }
        [HttpPost]
        public ViewResult EditPost(Post p)  //edit and saves the changes in the post made by the user.
        {
            List<Post> postData = PostRepository.ReturnPosts();
            Post pst = postData.Find(pst => pst.Id == p.Id);
            p.Usr = pst.Usr;
            if (ModelState.IsValid)
            {
                PostRepository.UpdatePost(p);
                List<RegularUser> userData = UserRepository.ReturnUsers();
                RegularUser regUsr = userData.Find(regUsr => regUsr.Username == HttpContext.Session.GetString("CurrentUser"));
                ViewBag.Id = regUsr.Id;
                postData = PostRepository.ReturnPosts();
                AdminController.manageProfilePic(ref postData);
                postData.Reverse();
                return View("~/Views/General/AtHome.cshtml", postData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Some Data is missing !");
                return View("EditPost", p);
            }
        }
    }
}

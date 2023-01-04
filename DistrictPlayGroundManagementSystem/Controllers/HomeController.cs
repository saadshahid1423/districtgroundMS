using DistrictPlayGroundManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistrictPlayGroundManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            try
            {

                if (login.Type == "Admin")
                {
                    var admin = dbcontext.Admins.Where(x => x.Email == login.Email && x.Password == login.Password).SingleOrDefault();
                    if(admin != null)
                    {
                        
                        Session["AdminId"] = admin.Id;
                        Session["UserName"] = admin.UserName;
                        Session["Email"] = admin.Email;
                        string url = "/Admin/Dashboard/Index";
                        return RedirectPermanent(url);
                    }
                    else
                    {
                        ViewBag.Message = "Inccorrect UserName or Password";
                    }
                }
                else if (login.Type == "User")
                {
                    var User = dbcontext.Users.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
                    if (User != null)
                    {
                        if(User.IsActive == true)
                        {
                            Session["UserId"] = User.Id;
                            Session["UserName"] = User.Name;
                            Session["Email"] = User.Email;
                            Session["Contact"] = User.Contact;
                            string url = "/User/Dashboard/Index";
                            return RedirectPermanent(url);
                        }
                        else
                        {
                            ViewBag.Message = "Your Account has been deactivated";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Inccorrect UserName or Password";
                    }

                }
                
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error Occured Please Contact Admin";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _user = dbcontext.Users.Where(x => x.Email == user.Email).Count();
                    if (_user == 0)
                    {
                        user.IsActive = true;
                        user.IsDeleted = false;
                        dbcontext.Users.Add(user);
                        dbcontext.SaveChanges();
                        Session["UserId"] = user.Id;
                        Session["UserName"] = user.Name;
                        Session["Email"] = user.Email;
                        Session["Contact"] = user.Contact;
                        string url = "/User/Dashboard/Index";
                        return RedirectPermanent(url);
                    }
                    else
                    {
                        ViewBag.Message = "User Already Exist";
                        
                    }

                }
                else
                {
                    ViewBag.Message = "Please Fill form Correctly";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error Occured Please Contact Admin";
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SignOut()
        {
            if (Session["UserId"] != null)
            {
                Session["UserId"] = "";
                Session["UserName"] = "";
                Session["Email"] = "";
                Session["Contact"] = "";
            }
            else
            {
                Session["AdminId"] = "";
                Session["UserName"] = "";
                Session["Email"] = "";
            }
            string url = "/Home/Login";
            return RedirectPermanent(url);
        }
    }
}
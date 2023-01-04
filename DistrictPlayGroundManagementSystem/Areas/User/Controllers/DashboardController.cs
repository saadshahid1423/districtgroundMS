using DistrictPlayGroundManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistrictPlayGroundManagementSystem.Areas.User.Controllers
{
    [AuthorizeUser]
    public class DashboardController : Controller
    {
        // GET: User/Dashboard
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Profile()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            int Id = (int)Session["UserId"];
            DistrictPlayGroundManagementSystem.User users = dbcontext.Users.Where(x => x.Id == Id).SingleOrDefault();
            return View(users);
        }

        [HttpPost]
        public ActionResult Profile(DistrictPlayGroundManagementSystem.User users)
        {
            try
            {
                int Id = (int)Session["UserId"];
                var _users = dbcontext.Users.Where(x => x.Id == Id).SingleOrDefault();
                _users.Name = users.Name;
                _users.Email = users.Email;
                _users.Password = users.Password;
                _users.Contact = users.Contact;
                dbcontext.Entry(_users).State = EntityState.Modified;
                dbcontext.SaveChanges();
                Session["UserName"] = _users.Name;
                Session["Contact"] = _users.Contact;
                Session["Email"] = _users.Email;
                TempData["Message"] = "User Updated Successfully";


            }
            catch (Exception)
            {
                TempData["Error"] = "An error occured";
            }
            return RedirectToAction("Index");
        }
    }
}
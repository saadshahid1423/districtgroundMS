using DistrictPlayGroundManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistrictPlayGroundManagementSystem.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class DashboardController : Controller
    {
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Profile()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            int Id = (int)Session["AdminId"];
            DistrictPlayGroundManagementSystem.Admin admin = dbcontext.Admins.Where(x => x.Id == Id).SingleOrDefault();
            return View(admin);
        }

        [HttpPost]
        public ActionResult Profile(DistrictPlayGroundManagementSystem.Admin admin)
        {
            try
            {
                int Id = (int)Session["AdminId"];
                var _admin = dbcontext.Admins.Where(x => x.Id == Id).SingleOrDefault();
                _admin.UserName = admin.UserName;
                _admin.Email = admin.Email;
                _admin.Password = admin.Password;
                dbcontext.Entry(_admin).State = EntityState.Modified;
                dbcontext.SaveChanges();
                Session["UserName"] = admin.UserName;
                Session["Email"] = admin.Email;
                TempData["Message"] = "Admin Updated Successfully";


            }
            catch (Exception)
            {
                TempData["Error"] = "An error occured";
            }
            return View();
        }
    }
}
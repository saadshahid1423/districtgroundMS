using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistrictPlayGroundManagementSystem.Models;


namespace DistrictPlayGroundManagementSystem.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class UsersController : Controller
    {
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var list = dbcontext.Users.Where(x=> x.IsDeleted == false).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
          
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DistrictPlayGroundManagementSystem.User user)
        {
            try
            {
                var _user = dbcontext.Users.Where(x => x.Email == user.Email).Count();
                if(_user == 0)
                {
                    user.IsActive = true;
                    user.IsDeleted = false;
                    dbcontext.Users.Add(user);
                    dbcontext.SaveChanges();
                    TempData["Message"] = "User Created Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "User Already Exist";
                    return RedirectToAction("Index");
                }
               
            }
            catch (Exception)
            {
                TempData["Error"] = "An error Occured";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var model = dbcontext.Users.Where(x => x.Id == Id).FirstOrDefault();
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.User user)
        {
            try
            {
                DistrictPlayGroundManagementSystem.User _USER = dbcontext.Users.Where(X => X.Id == user.Id).FirstOrDefault();
                _USER.Name = user.Name;
                _USER.Password = user.Password;
                _USER.Email = user.Email;
                _USER.Contact = user.Contact;
                dbcontext.Entry(_USER).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "User Updated Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error Occured";
            }
          
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var User = dbcontext.Users.Where(x => x.Id == id).FirstOrDefault();
                User.IsDeleted = true;
                dbcontext.Entry(User).State = EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "User Deleted Successfully";

            }
            catch (Exception)
            {
                TempData["Error"] = "An error Occured";
            }
            return RedirectToAction("Index");
        }
        public ActionResult ActivateDeactivate(int id)
        {
            try
            {
                var User = dbcontext.Users.Where(x => x.Id == id).FirstOrDefault();
               if(User.IsActive == true)
                {
                    User.IsActive = false;
                    TempData["Message"] = "User Deactivated  Successfully";
                }
                else
                {
                    User.IsActive = true;
                    TempData["Message"] = "User Activated Successfully";
                }
                dbcontext.Entry(User).State = EntityState.Modified;
                dbcontext.SaveChanges();

            }
            catch (Exception)
            {
                TempData["Error"] = "An error Occured";
            }
            return RedirectToAction("Index");
        }
    }
}
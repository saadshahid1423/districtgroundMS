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
    public class FeedBackController : Controller
    {
        
        // GET: User/FeedBack
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            int UserId = (int)Session["UserId"];
            var list = dbcontext.Feedbacks.Where(X =>  X.Userid == UserId).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var Ground = dbcontext.Grounds.Where(x => x.IsDeleted == false).ToList();
            var Grounddropdown = from type in Ground
                                 select new { value = type.Id, text = type.Name };
            ViewBag.Ground = Grounddropdown;
            int UserId = (int)Session["UserId"];
            var User = dbcontext.Users.Where(x => x.IsDeleted == false && x.Id == UserId).ToList();
            var Userdropdown = from type in User
                               select new { value = type.Id, text = type.Name };
            ViewBag.User = Userdropdown;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DistrictPlayGroundManagementSystem.Feedback feedback)
        {
            try
            {


                dbcontext.Feedbacks.Add(feedback);
                dbcontext.SaveChanges();
                TempData["Message"] = "Feedback Created Successfully";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error Occured";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var model = dbcontext.Feedbacks.Where(x => x.id == Id).FirstOrDefault();
            var Ground = dbcontext.Grounds.Where(x => x.IsDeleted == false).ToList();
            var Grounddropdown = from type in Ground
                                 select new { value = type.Id, text = type.Name };
            ViewBag.Ground = Grounddropdown;
            int UserId = (int)Session["UserId"];
            var User = dbcontext.Users.Where(x => x.IsDeleted == false && x.Id == UserId).ToList();
            var Userdropdown = from type in User
                               select new { value = type.Id, text = type.Name };
            ViewBag.User = Userdropdown;
           
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.Feedback feedback)
        {
            try
            {
                DistrictPlayGroundManagementSystem.Feedback _Feedback = dbcontext.Feedbacks.Where(X => X.id == feedback.id).FirstOrDefault();
                _Feedback.Details = feedback.Details;
                _Feedback.Groundid = feedback.Groundid;
                _Feedback.Userid = feedback.Userid;
                dbcontext.Entry(_Feedback).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Feedback Updated Successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error Occured";
            }

            return RedirectToAction("Index");
        }
       

    }
}
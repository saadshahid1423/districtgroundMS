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
    public class BookingController : Controller
    {
        // GET: User/Booking
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();

        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            int UserId = (int)Session["UserId"];
            var list = dbcontext.Bookings.Where(X => X.IsDeleted == false && X.Userid == UserId).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var Team = dbcontext.Teams.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in Team
                           select new { value = type.id, text = type.Name };
            ViewBag.Team = dropdown;
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
        public ActionResult Create(DistrictPlayGroundManagementSystem.Booking booking)
        {
            try
            {


                booking.IsDeleted = false;
                booking.IsAuthorized = false;
                dbcontext.Bookings.Add(booking);
                dbcontext.SaveChanges();
                TempData["Message"] = "Booking Created Successfully";
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
            var model = dbcontext.Bookings.Where(x => x.id == Id).FirstOrDefault();
            var Team = dbcontext.Teams.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in Team
                           select new { value = type.id, text = type.Name };
            ViewBag.Team = dropdown;
            var Ground = dbcontext.Grounds.Where(x => x.IsDeleted == false).ToList();
            var Grounddropdown = from type in Ground
                                 select new { value = type.Id, text = type.Name };
            ViewBag.Ground = Grounddropdown;
            int UserId = (int)Session["UserId"];
            var User = dbcontext.Users.Where(x => x.IsDeleted == false && x.Id == UserId).ToList();
            var Userdropdown = from type in User
                               select new { value = type.Id, text = type.Name };
            ViewBag.User = Userdropdown;
            model.Date = (String.Format("{0:s}", model.Date));
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.Booking booking)
        {
            try
            {
                DistrictPlayGroundManagementSystem.Booking _booking = dbcontext.Bookings.Where(X => X.id == booking.id).FirstOrDefault();
                _booking.Date = booking.Date;
                _booking.Teamid = booking.Teamid;
                _booking.Groundid = booking.Groundid;
                _booking.Userid = booking.Userid;
                _booking.Detail = booking.Detail;
                dbcontext.Entry(_booking).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Booking Updated Successfully";
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
                var booking = dbcontext.Bookings.Where(x => x.id == id).FirstOrDefault();
                booking.IsDeleted = true;
                dbcontext.Entry(booking).State = EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Booking Deleted Successfully";

            }
            catch (Exception)
            {
                TempData["Error"] = "An error Occured";
            }
            return RedirectToAction("Index");
        }
   
    }
}
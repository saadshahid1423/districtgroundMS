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
    public class BookingController : Controller
    {
        // GET: Admin/Booking
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
   
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var list = dbcontext.Bookings.Where(X => X.IsDeleted == false).ToList();
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
                           select new { value = type.Id, text = type.Name};
            ViewBag.Ground = Grounddropdown;
            var User = dbcontext.Users.Where(x => x.IsDeleted == false).ToList();
            var Userdropdown = from type in User
                               select new { value = type.Id, text = type.Name};
            ViewBag.User = Userdropdown;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DistrictPlayGroundManagementSystem.Booking booking)
        {
            try
            {


                booking.IsDeleted = false;
                booking.IsAuthorized= false;
                booking.Date = booking.Date.Replace("T", " ");
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
            var User = dbcontext.Users.Where(x => x.IsDeleted == false).ToList();
            var Userdropdown = from type in User
                               select new { value = type.Id, text = type.Name };
            ViewBag.User = Userdropdown;
            model.Date = (String.Format("{0:yyyy-MM-dd}", model.Date));
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.Booking booking)
        {
            try
            {
                DistrictPlayGroundManagementSystem.Booking _booking = dbcontext.Bookings.Where(X => X.id == booking.id).FirstOrDefault();
                _booking.Date = booking.Date.Replace("T", " ");
                _booking.Teamid= booking.Teamid;
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
        public ActionResult Authorized(int id,string concession)
        {
            bool success = false;
            try
            {

                var booking = dbcontext.Bookings.Where(x => x.id == id).FirstOrDefault();
            if (booking != null)
            {
                    decimal totalamount = 0;
                if (concession != "" && concession != "0")
                {
                    decimal conc = Convert.ToDecimal(concession);
                    decimal price = Convert.ToDecimal(booking.Ground.Price);
                    decimal total = (conc / 100) * price;
                    totalamount = Convert.ToDecimal(price - total);
                     
                }
                else
                {
                    totalamount = Convert.ToDecimal(booking.Ground.Price);
                    concession = "0";
                }
                Account account = new Account();
                account.Date =  Convert.ToDateTime(booking.Date);
                account.Bookingid = booking.id;
                account.concession = Convert.ToInt32(concession);
                account.Amount = totalamount;
                dbcontext.Accounts.Add(account);
                dbcontext.SaveChanges();
              
                    booking.IsAuthorized = true;
                    TempData["Message"] = "Booking  Authorized Successfully";
            }
                dbcontext.Entry(booking).State = EntityState.Modified;
                dbcontext.SaveChanges();
                success = true;
            }
            catch (Exception)
            {
                TempData["Error"] = "An error Occured";
                success = false;
            }
            return Json(new { success = success });
        }
    }
}

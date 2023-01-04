using DistrictPlayGroundManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistrictPlayGroundManagementSystem.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class FeedBackController : Controller
    {
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        // GET: Admin/FeedBack
        public ActionResult Index()
        {
            var list = dbcontext.Feedbacks.ToList();
            return View(list);
        }
    }
}
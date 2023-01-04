using DistrictPlayGroundManagementSystem.Areas.Admin.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistrictPlayGroundManagementSystem.Models;

namespace DistrictPlayGroundManagementSystem.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class ReportController : Controller
    {
        // GET: Admin/Report
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OpenReport(ReportModel reportModel)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "BookingReport.rpt"));
            DateTime stardate = Convert.ToDateTime(reportModel.StartDate);
            DateTime endate = Convert.ToDateTime(reportModel.EndDate);
            var list = (from Bookings in dbcontext.Bookings
                        join Ground in dbcontext.Grounds
                        on Bookings.Groundid equals Ground.Id
                        //join User in dbcontext.Users 
                        //on Bookings.Userid equals User.Id
                        where Bookings.IsDeleted == false
                        select Bookings
                        ).ToList();
            var data = list.Where(x => Convert.ToDateTime(x.Date) >= stardate && Convert.ToDateTime(x.Date) <= endate).Select(
                x=> new
                {
                    Id = x.id,
                    Name = x.Ground.Name ,
                    Date = x.Date ?? "",
                    Detail = x.Detail ?? ""

                }
                ).ToList();
            
            rd.SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "BookingList.pdf");
        }
        public ActionResult AccountReport()
        {
            return View();
        }
        public ActionResult OpenAccountReport(ReportModel reportModel)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "AccountReport.rpt"));
            DateTime stardate = Convert.ToDateTime(reportModel.StartDate);
            DateTime endate = Convert.ToDateTime(reportModel.EndDate);
            var list = (from Accoun in dbcontext.Accounts
                        join Booking in dbcontext.Bookings
                        on Accoun.Bookingid equals Booking.id
                        
                        select Accoun
                        ).ToList();
            var data = list.Where(x => Convert.ToDateTime(x.Booking.Date) >= stardate && Convert.ToDateTime(x.Booking.Date) <= endate).Select(
                x => new
                {
                    Id = x.id,
                    Date = x.Date ?? DateTime.Now,
                    concession = x.concession ?? 0,
                    Amount = x.Amount ?? 0

                }
                ).ToList();

            rd.SetDataSource(data);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "AccountList.pdf");
        }

    }
}
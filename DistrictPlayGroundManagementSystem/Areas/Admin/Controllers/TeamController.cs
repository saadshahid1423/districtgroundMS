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
    public class TeamController : Controller
    {
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        // GET: Admin/Team
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var list = dbcontext.Teams.Where(X => X.IsDeleted == false).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var TeamType = dbcontext.TeamTypes.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in TeamType
                           select new { value = type.id, text = type.TeamType1 };
            ViewBag.TeamType = dropdown;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DistrictPlayGroundManagementSystem.Team team)
        {
            try
            {
               
                    team.IsActive = true;
                team.IsDeleted = false;
                team.AdminId = (int)Session["AdminId"];
                    dbcontext.Teams.Add(team);
                    dbcontext.SaveChanges();
                    TempData["Message"] = "Team Created Successfully";
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
            var model = dbcontext.Teams.Where(x => x.id == Id).FirstOrDefault();
            var TeamType = dbcontext.TeamTypes.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in TeamType
                           select new { value = type.id, text = type.TeamType1 };
            ViewBag.TeamType = dropdown;
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.Team team)
        {
            try
            {
                DistrictPlayGroundManagementSystem.Team _Team = dbcontext.Teams.Where(X => X.id == team.id).FirstOrDefault();
                _Team.Name = team.Name;
                _Team.TeamTypeId = team.TeamTypeId;
                _Team.President = team.President;
                _Team.Description = team.Description;
                dbcontext.Entry(_Team).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Team Updated Successfully";
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
                var team = dbcontext.Teams.Where(x => x.id == id).FirstOrDefault();
                team.IsDeleted= true;
                dbcontext.Entry(team).State = EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Team Deleted Successfully";

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
                var TEAM = dbcontext.Teams.Where(x => x.id == id).FirstOrDefault();
                if (TEAM.IsActive == true)
                {
                    TEAM.IsActive = false;
                    TempData["Message"] = "Team Deactivated  Successfully";
                }
                else
                {
                    TEAM.IsActive = true;
                    TempData["Message"] = "Team Activated Successfully";
                }
                dbcontext.Entry(TEAM).State = EntityState.Modified;
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
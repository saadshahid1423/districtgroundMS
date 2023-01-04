using DistrictPlayGroundManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistrictPlayGroundManagementSystem.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class GroundController : Controller
    {
        // GET: Admin/Ground
        public DistrictPlayGroundManagmentEntities dbcontext = new DistrictPlayGroundManagmentEntities();
        
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            var list = dbcontext.Grounds.Where(X => X.IsDeleted == false).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var GroundType = dbcontext.GroundTypes.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in GroundType
                           select new { value = type.id, text = type.GroundType1 };
            ViewBag.GroundType = dropdown;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DistrictPlayGroundManagementSystem.Ground ground)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    string FilePath = null;
                    var file = Request.Files[0];
                    if (file != null)
                    {
                        string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
                        String fileExtension = System.IO.Path.GetExtension(file.FileName);
                        string MainDirectory = string.Empty;
                        if (file.ContentType.Contains("image"))
                        {
                            if (SupportedImageFormat.Contains(fileExtension.ToLower()))
                            {
                                FilePath = string.Format("{0}{1}{2}", "/assets/images/Ground/img-Ground-", Guid.NewGuid().ToString(), fileExtension);


                                MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
                                file.SaveAs(MainDirectory);
                                ground.Picture= FilePath;
                            }
                            else
                            {
                                TempData["Error"] = "Wrong Format for Image";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ground.Picture = "/assets/images/Ground/image.png";
                        }

                    }
                    
                }
                else
                {
                    ground.Picture = "/assets/images/Ground/image.png";
                }

                ground.IsActive = true;
                ground.IsDeleted = false;
                dbcontext.Grounds.Add(ground);
                dbcontext.SaveChanges();
                TempData["Message"] = "Grounds Created Successfully";
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
            var model = dbcontext.Grounds.Where(x => x.Id == Id).FirstOrDefault();
            var GroundType = dbcontext.GroundTypes.Where(x => x.IsDeleted == false).ToList();
            var dropdown = from type in GroundType
                           select new { value = type.id, text = type.GroundType1 };
            ViewBag.GroundType = dropdown;
            ViewBag.GroundImage = model.Picture;
            TempData["Id"] = Id;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DistrictPlayGroundManagementSystem.Ground ground)
        {
            try
            {
                DistrictPlayGroundManagementSystem.Ground _Ground = dbcontext.Grounds.Where(X => X.Id == ground.Id).FirstOrDefault();
                if (Request.Files.Count > 0)
                {
                    string FilePath = null;
                    var file = Request.Files[0];
                    if (file != null)
                    {
                        string[] SupportedImageFormat = { ".jpeg", ".png", ".jpg" };
                        String fileExtension = System.IO.Path.GetExtension(file.FileName);
                        
                        string MainDirectory = string.Empty;
                        if (file.ContentType.Contains("image"))
                        {
                            if (SupportedImageFormat.Contains(fileExtension.ToLower()))
                            {
                                FilePath = string.Format("{0}{1}{2}", "/assets/images/Ground/img-Ground-", Guid.NewGuid().ToString(), fileExtension);


                                MainDirectory = Path.Combine(Server.MapPath("~" + FilePath));
                                file.SaveAs(MainDirectory);
                                _Ground.Picture = FilePath;
                            }
                            else
                            {
                                TempData["Error"] = "Wrong Format for Image";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ground.Picture = "/assets/images/Ground/image.png";
                        }

                    }
                }

                _Ground.Name = ground.Name;
                _Ground.GroundTypeID = ground.GroundTypeID;
                _Ground.Detail = ground.Detail;
                _Ground.Price = ground.Price;
                dbcontext.Entry(_Ground).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Ground Updated Successfully";
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
                var ground = dbcontext.Grounds.Where(x => x.Id == id).FirstOrDefault();
                ground.IsDeleted = true;
                dbcontext.Entry(ground).State = EntityState.Modified;
                dbcontext.SaveChanges();
                TempData["Message"] = "Ground Deleted Successfully";

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
                var ground = dbcontext.Grounds.Where(x => x.Id == id).FirstOrDefault();
                if (ground.IsActive == true)
                {
                    ground.IsActive = false;
                    TempData["Message"] = "Ground Deactivated  Successfully";
                }
                else
                {
                    ground.IsActive = true;
                    TempData["Message"] = "Ground Activated Successfully";
                }
                dbcontext.Entry(ground).State = EntityState.Modified;
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
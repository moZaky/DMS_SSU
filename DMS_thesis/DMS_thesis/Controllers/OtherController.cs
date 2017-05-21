using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMS_thesis.Models;
using System.Web.Security;
using DMS_thesis.Controllers;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using DMS_thesis.Context;
using System.IO;



namespace DMS_thesis.Controllers
{
    
    public class OtherController : Controller
    {
        public DMSContext db = new DMSContext();
        [HttpGet]
        public ActionResult FullWidth ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FullWidth(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
//string _FileName = Path.GetFileName(file.FileName);
                    string _FileName = "Testing";
                    string _path = Path.Combine(Server.MapPath("~/Files"), _FileName);
                    file.SaveAs(_path);
                    db.SaveChanges();
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
        [Authorize]
        public ActionResult SideBar()
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            return View(user);
        }
        
        public ActionResult Faq()
        {
            return View();
        }
    }
}

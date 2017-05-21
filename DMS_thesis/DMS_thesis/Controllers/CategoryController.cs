using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_thesis.Modal;
using DMS_thesis.Models;

namespace DMS_thesis.Controllers
{
    public class CategoryController : Controller
    {
        CategoryDB cDB = new CategoryDB();
        // GET: Home  
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            return Json(cDB.ListAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(Category ct)
        {
            return Json(cDB.Add(ct), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var category = cDB.ListAll().Find(x => x.Id.Equals(ID));
            return Json(category, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Category ct)
        {
            return Json(cDB.Update(ct), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(cDB.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
}
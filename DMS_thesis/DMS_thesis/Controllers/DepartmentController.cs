using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_thesis.Modal;
using DMS_thesis.Models;

namespace DMS_thesis.Controllers
{
    public class DepartmentController : Controller
    {
        DepartmentDB depDB = new DepartmentDB();
        // GET: Home  
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            return Json(depDB.ListAll(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(Department dp)
        {
            return Json(depDB.Add(dp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetbyID(int ID)
        {
            var Dept = depDB.ListAll().Find(x => x.Id.Equals(ID));
            return Json(Dept, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Department dp)
        {
            return Json(depDB.Update(dp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(depDB.Delete(ID), JsonRequestBehavior.AllowGet);
        }
    }
} 
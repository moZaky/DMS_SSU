using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMS_thesis.Models;


namespace DMS_thesis.Controllers
{
    public class TemplatesController : Controller
    {
        
       public DMSContext db = new DMSContext();
        //   TemplatesDB tDB = new TemplatesDB();

        // GET: Templates
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            //var tempp = new ViewTemplates();
            Template second = new Template();
            //User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            ViewTemplates viewtemp = new ViewTemplates();
            
            var templs = db.Templates
                .OrderByDescending(t => t.Name);
          //  templs.ToList();
            IEnumerable<Template> temp = templs as IEnumerable<Template>;
            viewtemp.Templates = temp;
            viewtemp.Template = second;// упорядочиваем по имени шаблона заявки
            return View(viewtemp);

            //return View();
        }

        [HttpPost]
        public ActionResult Create(Template template, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            // получаем текущего пользователя
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (ModelState.IsValid)
            {
                // если получен файл
                // если получен файл
               
                //var str1 = db.Categories.Find(template.CategoryId);
                //template.Name = file.FileName.Substring(file.FileName.IndexOf('.'));
                //.LastIndexOf('.'));
                //user.Last_name.ToString() + " " + str1.Name.ToString();
              
                // db.Requests.Add(request);

                //получаем время открытия
                //DateTime current = DateTime.Now;

                if (file != null)
                {
                    // Получаем расширение
                    string name = template.Name;
                    string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    //string path = current.ToString(user.Last_name + "__dd/MM/yyyy H:mm:ss__" + name).Replace(":", "_").Replace("/", ".") + ext;
                    string Path = Server.MapPath("~/Template/" + file.FileName);
                    file.SaveAs(Server.MapPath("~/Template/" + file.FileName));
                    template.Path = Path;
                }
                //  List<Files> FILES = new List<Files>();
                //for (int i = 0; i < Request.Files.Count; i++)
                //{
                //    var file = Request.Files[i];

                //    if (file != null && file.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(file.FileName);
                //        Files files = new Files()
                //        {
                //            Name = fileName,
                //            Path = Path.GetExtension(fileName)

                //        };
                //        FILES.Add(files);

                //        string path = Path.Combine(Server.MapPath("~/Files/"),+ fi);
                //        file.SaveAs(path);
                //        request.File = path;
                //    }
                //}

                //  request.Files = FILES;
                db.Templates.Add(template);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", template.CategoryId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", request.CategoryId);
            return View(template);
        }

        [HttpPost]
        public ActionResult Update(Template template, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            Template templ = db.Templates.Where(m => m.Id == template.Id).FirstOrDefault();
            templ.Name = template.Name;
            templ.CategoryId = template.CategoryId;
            // получаем текущего пользователя
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (ModelState.IsValid)
            {
                // если получен файл
                // если получен файл

                //var str1 = db.Categories.Find(template.CategoryId);
                //template.Name = file.FileName.Substring(file.FileName.IndexOf('.'));
                //.LastIndexOf('.'));
                //user.Last_name.ToString() + " " + str1.Name.ToString();

                // db.Requests.Add(request);

                //получаем время открытия
                //DateTime current = DateTime.Now;

                if (file != null)
                {
                    // Получаем расширение
                    string name = template.Name;
                    string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    //string path = current.ToString(user.Last_name + "__dd/MM/yyyy H:mm:ss__" + name).Replace(":", "_").Replace("/", ".") + ext;
                    string Path = Server.MapPath("~/Template/" + file.FileName);
                    file.SaveAs(Server.MapPath("~/Template/" + file.FileName));
                    template.Path = Path;
                    templ.Path = Path;
                }
                //  List<Files> FILES = new List<Files>();
                //for (int i = 0; i < Request.Files.Count; i++)
                //{
                //    var file = Request.Files[i];

                //    if (file != null && file.ContentLength > 0)
                //    {
                //        var fileName = Path.GetFileName(file.FileName);
                //        Files files = new Files()
                //        {
                //            Name = fileName,
                //            Path = Path.GetExtension(fileName)

                //        };
                //        FILES.Add(files);

                //        string path = Path.Combine(Server.MapPath("~/Files/"),+ fi);
                //        file.SaveAs(path);
                //        request.File = path;
                //    }
                //}

                //  request.Files = FILES;
                //db.Templates.Add(template);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", template.CategoryId);
            //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", request.CategoryId);
            return View(template);
        }

        //        public JsonResult List()
        //        {
        //            return Json(tDB.ListAll(), JsonRequestBehavior.AllowGet);
        //        }
        //        public JsonResult Add(Template tp, HttpPostedFileBase file)
        //        {
        //            tp.CategoryId = 1;
        //            tp.Path = Server.MapPath("~/Templates/" + file.FileName);
        //            file.SaveAs(tp.Path);
        //            return Json(tDB.Add(tp), JsonRequestBehavior.AllowGet);
        //        }
        //        public JsonResult getbyID(int ID)
        //        {
        //            var template = tDB.ListAll().Find(x => x.Id.Equals(ID));
        //            return Json(template, JsonRequestBehavior.AllowGet);
        //        }
        //        public JsonResult Update(Template tp, HttpPostedFileBase file)
        //        {
        //            tp.CategoryId = 1;
        //            tp.Path = Server.MapPath("~/Templates/" + file.FileName);
        //            file.SaveAs(tp.Path);
        //            return Json(tDB.Update(tp), JsonRequestBehavior.AllowGet);
        //        }
        //        public JsonResult Delete(int ID)
        //        {
        //            return Json(tDB.Delete(ID), JsonRequestBehavior.AllowGet);
        //        }

        //        //public ActionResult List()
        //        //{
        //        //    return View("_List");
        //        //}

        [HttpGet]
        [Authorize]
        public ActionResult DocFile(int? Id)
        {
            Template template1 = db.Templates.Find(Id);
            if (template1 != null && template1.Path != null)
            {
                string str = template1.Path.ToString();

                return View("DocFile", template1);

            }
            return View("Index");

        }
    }
}
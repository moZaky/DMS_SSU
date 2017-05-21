using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_thesis.Models;
using System.Web.Security;
using DMS_thesis.Controllers;
using System.Data.Entity;
using System.IO;

namespace DMS_thesis.Controllers
{
    public class RequestController : Controller
    {
        public int i = 1;
        public DMSContext db = new DMSContext();
        // GET: Request
        [Authorize]
        public ActionResult Index()
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var requests = db.Requests.Where(r => r.UserId == user.Id) //получаем заявки для текущего пользователя
                                    .Include(r => r.Category)  // добавляем категории
                                    .Include(r => r.Lifecycle)  // добавляем жизненный цикл заявок
                                    .Include(r => r.User)
                                    // добавляем данные о пользователях
                                    .OrderByDescending(r => r.Lifecycle.Opened); // упорядочиваем по дате по убыванию   
            return View(requests.ToList());

        }
        
        [HttpGet]
        public ActionResult Create()
        {
            // получаем текущего пользователя
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user != null)
            {
                // получаем набор кабинетов для департамента, в котором работает пользователь
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                ViewBag.ActivId = new SelectList(db.Activs, "Id", "Name");
               return View();
            }
            return RedirectToAction("LogOff", "Account");
        }

        // Создание новой заявки
        [HttpPost]
        public ActionResult Create(Request request, HttpPostedFileBase file)
        {
            // получаем текущего пользователя
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (ModelState.IsValid)
            {
               
                // указываем статус Открыта у заявки
                request.Status = DMS_thesis.Models.Request.RequestStatus.Open;
                //получаем время открытия
                DateTime current = DateTime.Now;

                //Создаем запись о жизненном цикле заявки
                Lifecycle newLifecycle = new Lifecycle() { Opened = current };
                request.Lifecycle = newLifecycle;

                //Добавляем жизненный цикл заявки
                db.Lifecycles.Add(newLifecycle);

                // указываем пользователя заявки
                request.UserId = user.Id;
                Category cat = new Category();
               // string str = request.CategoryId.Value.ToString();
               // string newCat = request.Category.Name;
               // request.Name = str + user.Last_name;                    
                request.Priority = (int) DMS_thesis.Models.Request.RequestPriority.Medium;
                // если получен файл
                var str1 = db.Categories.Find(request.CategoryId);
                string  str = str1.Name;
                request.Name = user.Last_name.ToString()+" " +str1.Name.ToString();
                Files fm = new Files();
                // db.Requests.Add(request);
                if (file != null)
                {
                    // Получаем расширение
                    string name = request.Name;
                    string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    // сохраняем файл по определенному пути на сервере
                    string path = current.ToString(user.Last_name+"_dd/MM/yyyy H:mm:ss_" +name).Replace(":", "_").Replace("/", ".") + ext;
                    file.SaveAs(Server.MapPath("~/Files/" + path));
                    request.File = path;
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
                db.Requests.Add(request);
                db.SaveChanges();
               // ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                //ViewBag.ActivId = new SelectList(db.Activs, "Id", "Name");
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", request.CategoryId);
           // ViewBag.ActivId = new SelectList(db.Activs, "Id", "Name");

            return View(request);
        }

        public ActionResult Details(int id)
        {
            Request request = db.Requests.Find(id);

            if (request != null)
            {
                //получаем кабинет
                var activ = db.Activs.Where(m => m.Id == request.ActivId);
                // так как кабинет у нас может быть не указан, и набор может возвращать 0 значений
                if (activ.Count() > 0)
                {
                    request.Activ = activ.First();
                }
                //получаем категорию
                //request.Category = db.Categories.Where(m => m.Id == request.CategoryId).First();
                return PartialView("_Details", request);
            }
            return View("Index");
        }

        public ActionResult Executor(int id)
        {
            User executor = db.Users.Where(m => m.Id == id).First();

            if (executor != null)
            {
                return PartialView("_Executor", executor);
            }
            return View("Index");
        }

        public ActionResult Lifecycle(int id)
        {
            Lifecycle lifecycle = db.Lifecycles.Where(m => m.Id == id).First();

            if (lifecycle != null)
            {
                return PartialView("_Lifecycle", lifecycle);
            }
            return View("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Секретар деканату")]
        public ActionResult Distribute()
        {
            var requests = db.Requests.Include(r => r.User)
                                    .Include(r => r.Lifecycle)
                                    .Include(r => r.Executor)
                                    .Where(r => r.ExecutorId == null)
                                    .Where(r => r.Status != DMS_thesis.Models.Request.RequestStatus.Closed);
            List<User> executors = db.Users.Include(e => e.Role)
                                        .Where(e => e.Role.Name == "Завідувач декана").ToList<User>();

            ViewBag.Executors = new SelectList(executors, "Id", "Last_name");
            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Секретар деканату")]
        public ActionResult Distribute(int? requestId, int? executorId)
        {
            if (requestId == null && executorId == null)
            {
                return RedirectToAction("Distribute");
            }
            Request req = db.Requests.Find(requestId);
            User ex = db.Users.Find(executorId);
            if (req == null && ex == null)
            {
                return RedirectToAction("Distribute");
            }
            req.ExecutorId = executorId;

            req.Status = DMS_thesis.Models.Request.RequestStatus.Distributed;
            Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
            lifecycle.Distributed = DateTime.Now;
            db.Entry(lifecycle).State = EntityState.Modified;

            db.Entry(req).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Distribute");
        }

        [Authorize]
        public ActionResult Detail(int id)
        {
            Request request = db.Requests.Find(id);

            if (request != null)
            {
                //получаем кабинет
                var activ = db.Activs.Where(m => m.Id == request.ActivId);
                // так как кабинет у нас может быть не указан, и набор может возвращать 0 значений
                if (activ.Count() > 0)
                {
                    request.Activ = activ.First();
                }
                //получаем категорию
                request.Category = db.Categories.Where(m => m.Id == request.CategoryId).First();
                return PartialView("_Details", request);
            }
            return View("Index");
        }

        public ActionResult Download(int id)
        {
            Request r = db.Requests.Find(id);
            if (r != null)
            {
                string filename = Server.MapPath("~/Files/" + r.File);
                string contentType = "image/jpeg";

                string ext = filename.Substring(filename.LastIndexOf('.'));
                switch (ext)
                {
                    case "txt":
                        contentType = "text/plain";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "tiff":
                        contentType = "image/tiff";
                        break;
                    case "doc":
                        contentType = "text/doc";
                        break;
                    case "docx":
                        contentType = "text/docx";
                        break;
                }
                return File(filename, contentType, filename);
            }

            return Content("Файл не найден");
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            var r = new List<UploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);

                r.Add(new UploadFilesResult()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Browse(int id)
        {
            Request request = db.Requests.Find(id);

            if (request != null)
            {
                //получаем кабинет
                var activ = db.Activs.Where(m => m.Id == request.ActivId);
                // так как кабинет у нас может быть не указан, и набор может возвращать 0 значений
                if (activ.Count() > 0)
                {
                    request.Activ = activ.First();
                }
                //получаем категорию
                var category = db.Categories.Where(c => c.Id == request.CategoryId);
                if (category.Count() > 0)
                {
                    request.Category = category.First();
                }

                var life = db.Lifecycles.Where(l => l.Id == request.LifecycleId);
                if (life.Count() > 0)
                {
                    request.Lifecycle = life.First();
                }
                // request.Category = db.Categories.Where(m => m.Id == request.CategoryId).First();
                //  request.Lifecycle = db.Lifecycles.Where(l => l.Id == request.LifecycleId).First();
                var User = db.Users.Where(u => u.Id == request.UserId)
                                    .Include(u => u.Department)
                                    .Include(u => u.Role);
                if (User.Count() > 0)
                {
                    request.User = User.First();
                }
                var Executor = db.Users.Where(e => e.Id == request.ExecutorId)
                                        .Include(e => e.Role)
                                        .Include(e => e.Department);
                if (Executor.Count() > 0)
                {
                    request.Executor = Executor.First();
                }
                return View("Browse", request);
            }
            return View("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Browse(int? id, string SubmitButton)
        {
            User user = (from u in db.Users
                         where u.Login == User.Identity.Name
                         select u).First();
            var request = db.Requests.Find(id);
            if (request == null)
            {
                return RedirectToAction("Index");
            }
            //string str = Request.Form["Submit"].ToString();
            switch (SubmitButton)
            {
                case "Submit":
                    {
                        request.Status = DMS_thesis.Models.Request.RequestStatus.Closed;
                        Lifecycle lif = db.Lifecycles.Find(request.LifecycleId);
                        lif.Proccesing = DateTime.Now;
                        lif.Checking = DateTime.Now;
                        lif.Closed = DateTime.Now;
                        db.Entry(lif).State = EntityState.Modified;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Pesonal", "Account", new { id = user.Id });

                    }
                 
                    break;
                case "Reopen":
                    {
                        if (request == null)
                        {
                            return RedirectToAction("Index");
                        }
                        Lifecycle lif1 = db.Lifecycles.Find(request.LifecycleId);
                        lif1.Opened = DateTime.Now;
                        request.Status = DMS_thesis.Models.Request.RequestStatus.Open;
                        db.Entry(lif1).State = EntityState.Modified;
                        db.Entry(request).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Pesonal", "Account", new { id = user.Id });

                    }
                    break;
                case "Cancel":
                    {

                        Lifecycle lif1 = db.Lifecycles.Find(request.LifecycleId);
                        db.Lifecycles.Remove(lif1);
                        db.Requests.Remove(request);
                        db.SaveChanges();

                        return RedirectToAction("Pesonal", "Account", new { id = user.Id });
                    }
                    break;
                case "Edit":
                    {
                        //Request req1 = db.Requests.Find(requestId);
                        //if (req1 == null)
                        //{
                        //    return RedirectToAction("Index");
                        //}
                        //Lifecycle lif1 = db.Lifecycles.Find(req1.LifecycleId);
                        //lif1.Opened = DateTime.Now;
                        //req1.Status = DMS_thesis.Models.Request.RequestStatus.Open;
                        //db.Entry(lif1).State = EntityState.Modified;
                        //db.Entry(req1).State = EntityState.Modified;
                        //db.SaveChanges();
                    }
                    break;
                default:
                    throw new Exception();

            }

            return View("Browse", request);

            }

        
        [HttpOptions]
        public ActionResult Submit(int? id)
        {
            var request = db.Requests.Find(id);
            if (request == null)
            {
              return  RedirectToAction("Index");
            }
            else
            {
                
                request.Status = DMS_thesis.Models.Request.RequestStatus.Closed;
                Lifecycle lif = db.Lifecycles.Find(request.LifecycleId);
                lif.Closed = DateTime.Now;
                db.Entry(lif).State = EntityState.Modified;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return new EmptyResult();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Test(int? Id)
        {
            Request request1 = db.Requests.Find(Id);
            if (request1 != null && request1.File!=null)
            {
                string str = "~Files/" + request1.File.ToString();

                return View("Test", request1);

            }
            return View("Index");

        }

    }
}
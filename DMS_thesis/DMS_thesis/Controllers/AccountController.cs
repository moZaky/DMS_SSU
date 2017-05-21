using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_thesis.Models;
using System.Web.Security;
using DMS_thesis.Controllers;
using System.Data.Entity;
using System.Net;



namespace DMS_thesis.Controllers
{
  //  [AllowAnonymous]
    public class AccountController : Controller
    {
        public DMSContext db = new DMSContext();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                   
                    
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            using (DMSContext _db = new DMSContext())
            {
                try
                {
                    User user = (from u in _db.Users
                                 where u.Login == login && u.Password == password
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public ActionResult Personal()
        {
                User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
                return View(user);

        }

        public ActionResult Details(int? id)
        {
            User user = db.Users.Where(m => m.Id == id).First();

            if (user != null)
            {
                return PartialView("_Details", user);
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult SendedRequest(int? id)
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var requests = db.Requests.Where(r => r.UserId == user.Id) // получаем заявки для текущего пользователя, 
                                                                           // который является исполнителем данных заявок
                                    .Include(r => r.Category)  // добавляем категории
                                    .Include(r => r.Lifecycle)  // добавляем жизненный цикл заявок
                                    .Include(r => r.User)
                                    .Include(r => r.Executor)// добавляем данные о пользователях
                                    .OrderByDescending(r => r.Lifecycle.Opened); // упорядочиваем по дате по убыванию   
            return PartialView(requests.ToList());
        }

        //public ActionResult SendedRequest(int? id, string SortOrder, string SearchString)
        //{
        //    User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();
        //    List<DMS_thesis.Models.Request> request = null;
        //    if (SortOrder != null)
        //        SortOrder = SortOrder.Contains("DSC") ? SortOrder.Split('_')[0] : string.Format("{0}_DSC", SortOrder);
        //    ViewBag.Name = "Name";
        //    ViewBag.Status = "Status";
        //    ViewBag.Priority = "Priority";
        //    switch (SortOrder)
        //    {
        //        case "Name":
        //            request = db.Requests.OrderBy(r => r.Name).ToList();
        //            ViewBag.Name = "Name";
        //            break;
        //        case "Name_DSC":
        //            request = db.Requests.OrderByDescending(r => r.Name).ToList();
        //            ViewBag.Name = "Name_DSC";
        //            break;
        //        case "Status":
        //            request = db.Requests.OrderBy(r => r.Status).ToList();
        //            ViewBag.Status = "Status";
        //            break;
        //        case "Status_DSC":
        //            request = db.Requests.OrderByDescending(s => s.Status).ToList();
        //            ViewBag.Status = "Status_DSC";
        //            break;
        //        case "Priority":
        //            request = db.Requests.OrderBy(s => s.Priority).ToList();
        //            ViewBag.Priority = "Priority";
        //            break;
        //        case "Priority_DSC":
        //            request = db.Requests.OrderBy(s => s.Priority).ToList();
        //            ViewBag.Priority = "Priority_DSC";
        //            break;
        //        default:
        //            request = db.Requests.OrderBy(s => s.Name).ToList();
        //            break;
        //    }

        //    return PartialView(request.ToList());
        //}

        [HttpGet]
        public ActionResult ReceivedRequest(int? id)
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).FirstOrDefault();

            var requests = db.Requests.Where(r => r.ExecutorId == user.Id) // получаем заявки для текущего пользователя, 
                                                                           // который является исполнителем данных заявок
                                    .Include(r => r.Category)  // добавляем категории
                                    .Include(r => r.Lifecycle)  // добавляем жизненный цикл заявок
                                    .Include(r => r.User)         // добавляем данные о пользователях
                                    .OrderByDescending(r => r.Lifecycle.Opened); // упорядочиваем по дате по убыванию   
            return PartialView(requests.ToList());
        }

        [Authorize(Roles = "Адміністратор, Секретар деканату, Завідувач декана, Декан")]
        public ActionResult Process (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            
            return PartialView("Process",request);
        }

        //Заявки для изменения статуса исполнителем
        [HttpGet]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus()
        {
            // получаем текущего пользователя
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();
            if (user != null)
            {
                var requests = db.Requests.Include(r => r.User)
                                    .Include(r => r.Lifecycle)
                                    .Include(r => r.Executor)
                                    .Where(r => r.ExecutorId == user.Id)
                                    .Where(r => r.Status != DMS_thesis.Models.Request.RequestStatus.Closed);
                return View(requests);
            }
            return RedirectToAction("LogOff", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Исполнитель")]
        public ActionResult ChangeStatus(int requestId, int status)
        {
            User user = db.Users.Where(m => m.Login == HttpContext.User.Identity.Name).First();
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            Request req = db.Requests.Find(requestId);
            if (req != null)
            {
                //req.Status =status;
                Lifecycle lifecycle = db.Lifecycles.Find(req.LifecycleId);
                if (status ==(int) DMS_thesis.Models.Request.RequestStatus.Proccesing)
                {
                    lifecycle.Proccesing = DateTime.Now;
                }
                else if (status == (int)DMS_thesis.Models.Request.RequestStatus.Checking)
                {
                    lifecycle.Checking = DateTime.Now;
                }
                else if (status == (int)DMS_thesis.Models.Request.RequestStatus.Closed)
                {
                    lifecycle.Closed = DateTime.Now;
                }
                db.Entry(lifecycle).State = EntityState.Modified;
                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ChangeStatus");
        }
        [HttpGet]
        public ActionResult PartLog ()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PartLog(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {


                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return PartialView(model);
        }
    }
    }

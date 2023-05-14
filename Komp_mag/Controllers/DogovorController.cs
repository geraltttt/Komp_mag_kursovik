using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agent.DAO;
using Agent.Models;
using Microsoft.AspNet.Identity;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Config;

namespace Agent.Controllers
{
    public class DogovorController : Controller
    {
        //private static readonly ILog logger = LogManager.GetLogger(typeof(Deadlock));
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        GroupDogDAO groupDAO = new GroupDogDAO();
        DogovorDAO dogovorDAO = new DogovorDAO();
        TarifDAO tarifDAO = new TarifDAO();

        /*     protected bool ValidateDogovor(Dogovor DogovorToValidate)
               {
                   if (DogovorToValidate.IDKl == null)
                       ModelState.AddModelError("FIO", "Поле 'Фамилия, Имя, Отчество ' обязательно для заполнения.");
                   if (DogovorToValidate.IDAg == null)
                       ModelState.AddModelError("Avtor", "Поле 'Автор книги' обязательно для заполнения.");
                   if (DogovorToValidate.B == null)
                       ModelState.AddModelError("Book", "Поле 'Название книги' обязательно для заполнения.");
                   if (DogovorToValidate.Librarian == null)
                       ModelState.AddModelError("Librarian", "Поле 'Библиотекарь' обязательно для заполнения.");
                   if (DogovorToValidate.Date == null)
                       ModelState.AddModelError("Date", "Поле 'Дата' обязательно для заполнения.");
                   return ModelState.IsValid;
               }
              

        public ActionResult Start()
        {
            return View("Start");
        }
 */
        [Authorize]
        public ActionResult Confirm(int id)
        {
            Dogovor dogovor = dogovorDAO.getDogovor(id);
            string userId = User.Identity.GetUserId();
            dogovor.IDAg = userId;
            dogovor.IDGroup = 3;
            dogovorDAO.UpdateGroup(dogovor);
            return RedirectToAction("Index");
        }
        public ActionResult Reject(int id)
        {
            Dogovor dogovor = dogovorDAO.getDogovor(id);
            string userId = User.Identity.GetUserId();
            dogovor.IDAg = userId;
            dogovor.IDGroup = 4;
            dogovorDAO.UpdateGroup(dogovor);
            return RedirectToAction("Index");
        }

        public ActionResult MyObject(int? id)
        {
            ViewData["Groups"] = groupDAO.GetAllGroups();
            var dogovor = id == null ? dogovorDAO.GetAllDogovor() : dogovorDAO.GetAllDogovor().Where(x => x.GroupDog.Id == id);
            string userid = User.Identity.GetUserId();
            var myobject = dogovorDAO.GetMyObject(userid);
            if (!Request.IsAjaxRequest())
                return View(myobject);
            else
               
                return PartialView("DogovorList", dogovor);
        }
             
        public ActionResult Index(int? id)
        {
            ViewData["Groups"] = groupDAO.GetAllGroups();
            var dogovor = id == null ? dogovorDAO.GetAllDogovor() : dogovorDAO.GetAllDogovor().Where(x => x.GroupDog.Id == id);
            if (!Request.IsAjaxRequest())
                return View(dogovor);
            else
                return PartialView("DogovorList", dogovor);

        }


        public ActionResult Details(int id)
        {
            return View(dogovorDAO.getDogovor(id));
        }

        protected bool ViewDataSelectList(int GroupId)
        {
            var groups = groupDAO.GetAllGroups();
            ViewData["GroupId"] = new SelectList(groups, "Id", "Name", GroupId);
            return groups.Count() > 0;
        }

        // Create
        [HttpGet]
        public ActionResult Create(string id)
        {
            var tarif = new SelectList(tarifDAO.GetAllTarif(), "Id", "Object");
            ViewData["IDTr"] = tarif;
            Dogovor dogovor = new Dogovor();
            return View(dogovor);
        }
        [HttpPost]
        public ActionResult Create(Dogovor model)
        {
            var tarif = new SelectList(tarifDAO.GetAllTarif(), "Id", "Object");
            ViewData["IDTr"] = tarif;
            string userId = User.Identity.GetUserId();
            model.IDKl = userId;
            try
            {
                dogovorDAO.CreateDogovor(model);
                return RedirectToAction("MyObject");
            }
            catch (Exception ex)
            {
               Logger.Log.Error("Ошибка: ", ex);
            }
            return RedirectToAction("Create");
        }


        public ActionResult Edit(int id)
        {
            Dogovor Dogov = dogovorDAO.getDogovor(id);
            if (!ViewDataSelectList(Dogov.GroupDog.Id))
                return RedirectToAction("MyObject");
            return View(dogovorDAO.getDogovor(id));
        }

        [HttpPost]
        public ActionResult Edit( Dogovor Dogov)
        {
            ViewDataSelectList(-1);
            try
            {
                if (ModelState.IsValid && dogovorDAO.updateDogovor( Dogov))
                    return RedirectToAction("MyObject");
                else
                    return View(Dogov);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return View(Dogov);
            }
        }

        public ActionResult Delete(int id)
        {
            return View(dogovorDAO.getDogovor(id));
        }

        [HttpPost]
        public ActionResult Delete(int id, Dogovor Dogov)
        {
            try
            {
                if (dogovorDAO.deleteDogovor(id))
                    return RedirectToAction("MyObject");
                else
                    return View(dogovorDAO.getDogovor(id));
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return View(dogovorDAO.getDogovor(id));
            }
        }
    }
}
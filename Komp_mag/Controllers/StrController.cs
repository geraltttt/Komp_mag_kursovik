using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agent.DAO;
using Agent.Models;
using Microsoft.AspNet.Identity;

namespace Agent.Controllers
{
    public class StrController : Controller
    {
        GroupDogDAO groupDAO = new GroupDogDAO();
        StrSlDAO strslDAO = new StrSlDAO();
        DogovorDAO dogovorDAO = new DogovorDAO();

        public ActionResult Index(int? id)
        {
            ViewData["Groups"] = groupDAO.GetAllGroups();
            var strsl = id == null ? strslDAO.GetAllStrSl() : strslDAO.GetAllStrSl().Where(x => x.GroupDog.Id == id);
            if (!Request.IsAjaxRequest())
                return View(strsl);
            else
                return PartialView("StrSlList", strsl);

        }

        public ActionResult MyObject(int? id)
        {
            ViewData["Groups"] = groupDAO.GetAllGroups();
            var strsl = id == null ? strslDAO.GetAllStrSl() : strslDAO.GetAllStrSl().Where(x => x.GroupDog.Id == id);
            string userid = User.Identity.GetUserId();
            var myobject = strslDAO.GetMyObject(userid);
            if (!Request.IsAjaxRequest())
                return View(myobject);
            else
 
                return PartialView("StrSlList", strsl);
        }

        public ActionResult Details(int id)
        {
            return View(strslDAO.getStrSl(id));
        }
        protected bool ViewDataSelectList(int GroupId)
        {
            var groups = groupDAO.GetAllGroups();
            ViewData["GroupId"] = new SelectList(groups, "Id", "Name", GroupId);
            return groups.Count() > 0;
        }

     
        [HttpGet]
        public ActionResult Create(string id)
        {
            string userId = User.Identity.GetUserId();
           
            StrSl strsl = new StrSl();
            using (Entities ent = new Entities())
            { 
                strsl.IDKl = userId;
            }
            return View(strsl);
        }
        [HttpPost]
        public ActionResult Create(StrSl model)
        {
             string userId = User.Identity.GetUserId();
            model.IDKl = userId;
            try
            {
                strslDAO.CreateStrSl(model);
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
            StrSl Str = strslDAO.getStrSl(id);
            if (!ViewDataSelectList(Str.GroupDog.Id))
                return RedirectToAction("MyObject");
            return View(strslDAO.getStrSl(id));
        }

        [HttpPost]
        public ActionResult Edit( StrSl Str)
        {
            ViewDataSelectList(-1);
            try
            {
                if (ModelState.IsValid && strslDAO.updateStrSl( Str))
                    return RedirectToAction("MyObject");
                else
                    return View(Str);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return View(Str);
            }
        }

        public ActionResult Delete(int id)
        {
            return View(strslDAO.getStrSl(id));
        }

        [HttpPost]
        public ActionResult Delete(int id, StrSl Str)
        {
            try
            {
                if (strslDAO.deleteStrSl(id))
                    return RedirectToAction("MyObject");
                else
                    return View(strslDAO.getStrSl(id));
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return View(strslDAO.getStrSl(id));
            }
        }
        [Authorize]
        public ActionResult Confirm(int id)
        {
            var dogovor = dogovorDAO.getDogovor(id: id); 
            dogovor.IDGroup = 3;
            dogovorDAO.UpdateGroup(dogovor);
            return RedirectToAction("Index");
        }
        public ActionResult Reject(int id)
        {
            Dogovor dogovor = dogovorDAO.getDogovor(id);
            dogovor.IDGroup = 4;
            dogovorDAO.UpdateGroup(dogovor);
            return RedirectToAction("Index");
        }
    }
}

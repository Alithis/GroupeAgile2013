using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteTaLoc.Models;

namespace NoteTaLoc.Controllers
{
    public class UserTableController : Controller
    {
        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /UserTable/

        public ActionResult Index()
        {
            return View(db.UserTables.ToList());
        }

        //
        // GET: /UserTable/Details/5

        public ActionResult Details(int id = 0)
        {
            UserTable usertable = db.UserTables.Find(id);
            if (usertable == null)
            {
                return HttpNotFound();
            }
            return View(usertable);
        }

        //
        // GET: /UserTable/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UserTable/Create

        [HttpPost]
        public ActionResult Create(UserTable usertable)
        {
            if (ModelState.IsValid)
            {
                db.UserTables.Add(usertable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usertable);
        }

        //
        // GET: /UserTable/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserTable usertable = db.UserTables.Find(id);
            if (usertable == null)
            {
                return HttpNotFound();
            }
            return View(usertable);
        }

        //
        // POST: /UserTable/Edit/5

        [HttpPost]
        public ActionResult Edit(UserTable usertable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usertable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usertable);
        }

        //
        // GET: /UserTable/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserTable usertable = db.UserTables.Find(id);
            if (usertable == null)
            {
                return HttpNotFound();
            }
            return View(usertable);
        }

        //
        // POST: /UserTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTable usertable = db.UserTables.Find(id);
            db.UserTables.Remove(usertable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
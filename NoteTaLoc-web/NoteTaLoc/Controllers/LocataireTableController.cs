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
    public class LocataireTableController : Controller
    {
        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /LocataireTable/

        public ActionResult Index()
        {
            var locatairetables = db.LocataireTables.Include(l => l.AdresseTable).Include(l => l.UserTable);
            return View(locatairetables.ToList());
        }

        //
        // GET: /LocataireTable/Details/5

        public ActionResult Details(int id = 0)
        {
            LocataireTable locatairetable = db.LocataireTables.Find(id);
            if (locatairetable == null)
            {
                return HttpNotFound();
            }
            return View(locatairetable);
        }

        //
        // GET: /LocataireTable/Create

        public ActionResult Create()
        {
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo");
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom");
            return View();
        }

        //
        // POST: /LocataireTable/Create

        [HttpPost]
        public ActionResult Create(LocataireTable locatairetable)
        {
            if (ModelState.IsValid)
            {
                db.LocataireTables.Add(locatairetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", locatairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", locatairetable.UserId);
            return View(locatairetable);
        }

        //
        // GET: /LocataireTable/Edit/5

        public ActionResult Edit(int id = 0)
        {
            LocataireTable locatairetable = db.LocataireTables.Find(id);
            if (locatairetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", locatairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", locatairetable.UserId);
            return View(locatairetable);
        }

        //
        // POST: /LocataireTable/Edit/5

        [HttpPost]
        public ActionResult Edit(LocataireTable locatairetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(locatairetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", locatairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", locatairetable.UserId);
            return View(locatairetable);
        }

        //
        // GET: /LocataireTable/Delete/5

        public ActionResult Delete(int id = 0)
        {
            LocataireTable locatairetable = db.LocataireTables.Find(id);
            if (locatairetable == null)
            {
                return HttpNotFound();
            }
            return View(locatairetable);
        }

        //
        // POST: /LocataireTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            LocataireTable locatairetable = db.LocataireTables.Find(id);
            db.LocataireTables.Remove(locatairetable);
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
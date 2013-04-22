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
    public class ProprietaireTableController : Controller
    {
        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /ProprietaireTable/

        public ActionResult Index()
        {
            var proprietairetables = db.ProprietaireTables.Include(p => p.AdresseTable).Include(p => p.UserTable);
            return View(proprietairetables.ToList());
        }

        //
        // GET: /ProprietaireTable/Details/5

        public ActionResult Details(int id = 0)
        {
            ProprietaireTable proprietairetable = db.ProprietaireTables.Find(id);
            if (proprietairetable == null)
            {
                return HttpNotFound();
            }
            return View(proprietairetable);
        }

        //
        // GET: /ProprietaireTable/Create

        public ActionResult Create()
        {
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo");
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom");
            return View();
        }

        //
        // POST: /ProprietaireTable/Create

        [HttpPost]
        public ActionResult Create(ProprietaireTable proprietairetable)
        {
            if (ModelState.IsValid)
            {
                db.ProprietaireTables.Add(proprietairetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", proprietairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", proprietairetable.UserId);
            return View(proprietairetable);
        }

        //
        // GET: /ProprietaireTable/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProprietaireTable proprietairetable = db.ProprietaireTables.Find(id);
            if (proprietairetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", proprietairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", proprietairetable.UserId);
            return View(proprietairetable);
        }

        //
        // POST: /ProprietaireTable/Edit/5

        [HttpPost]
        public ActionResult Edit(ProprietaireTable proprietairetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proprietairetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", proprietairetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", proprietairetable.UserId);
            return View(proprietairetable);
        }

        //
        // GET: /ProprietaireTable/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProprietaireTable proprietairetable = db.ProprietaireTables.Find(id);
            if (proprietairetable == null)
            {
                return HttpNotFound();
            }
            return View(proprietairetable);
        }

        //
        // POST: /ProprietaireTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProprietaireTable proprietairetable = db.ProprietaireTables.Find(id);
            db.ProprietaireTables.Remove(proprietairetable);
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
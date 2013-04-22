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
    public class AdresseTableController : Controller
    {
        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /AdresseTable/

        public ActionResult Index()
        {
            return View(db.AdresseTables.ToList());
        }

        //
        // GET: /AdresseTable/Details/5

        public ActionResult Details(int id = 0)
        {
            AdresseTable adressetable = db.AdresseTables.Find(id);
            if (adressetable == null)
            {
                return HttpNotFound();
            }
            return View(adressetable);
        }

        //
        // GET: /AdresseTable/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdresseTable/Create

        [HttpPost]
        public ActionResult Create(AdresseTable adressetable)
        {
            if (ModelState.IsValid)
            {
                db.AdresseTables.Add(adressetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adressetable);
        }

        //
        // GET: /AdresseTable/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AdresseTable adressetable = db.AdresseTables.Find(id);
            if (adressetable == null)
            {
                return HttpNotFound();
            }
            return View(adressetable);
        }

        //
        // POST: /AdresseTable/Edit/5

        [HttpPost]
        public ActionResult Edit(AdresseTable adressetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adressetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adressetable);
        }

        //
        // GET: /AdresseTable/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AdresseTable adressetable = db.AdresseTables.Find(id);
            if (adressetable == null)
            {
                return HttpNotFound();
            }
            return View(adressetable);
        }

        //
        // POST: /AdresseTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdresseTable adressetable = db.AdresseTables.Find(id);
            db.AdresseTables.Remove(adressetable);
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
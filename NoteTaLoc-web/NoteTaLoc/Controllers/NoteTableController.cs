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
    public class NoteTableController : Controller
    {
        private notetalocEntities db = new notetalocEntities();

        //
        // GET: /NoteTable/

        public ActionResult Index()
        {
            var notetables = db.NoteTables.Include(n => n.AdresseTable).Include(n => n.UserTable);
            return View(notetables.ToList());
        }

        //
        // GET: /NoteTable/Details/5

        public ActionResult Details(int id = 0)
        {
            NoteTable notetable = db.NoteTables.Find(id);
            if (notetable == null)
            {
                return HttpNotFound();
            }
            return View(notetable);
        }

        //
        // GET: /NoteTable/Create

        public ActionResult Create()
        {
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo");
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom");
            return View();
        }

        //
        // POST: /NoteTable/Create

        [HttpPost]
        public ActionResult Create(NoteTable notetable)
        {
            if (ModelState.IsValid)
            {
                db.NoteTables.Add(notetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", notetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", notetable.UserId);
            return View(notetable);
        }

        //
        // GET: /NoteTable/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NoteTable notetable = db.NoteTables.Find(id);
            if (notetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", notetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", notetable.UserId);
            return View(notetable);
        }

        //
        // POST: /NoteTable/Edit/5

        [HttpPost]
        public ActionResult Edit(NoteTable notetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdresseId = new SelectList(db.AdresseTables, "AdresseId", "AptNo", notetable.AdresseId);
            ViewBag.UserId = new SelectList(db.UserTables, "UserId", "Nom", notetable.UserId);
            return View(notetable);
        }

        //
        // GET: /NoteTable/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NoteTable notetable = db.NoteTables.Find(id);
            if (notetable == null)
            {
                return HttpNotFound();
            }
            return View(notetable);
        }

        //
        // POST: /NoteTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NoteTable notetable = db.NoteTables.Find(id);
            db.NoteTables.Remove(notetable);
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
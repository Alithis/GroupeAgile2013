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
            //return View(db.AdresseTables.ToList());
            return View(new List<AdresseTable>());
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

        public ActionResult SearchNoted(string searchString)
        {
            return View("SearchNoted", SearchAdresses(searchString));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SearchPhrase"></param>
        /// <returns></returns>
        public List<AdresseTable> SearchAdresses(string SearchPhrase)
        {
            List<AdresseTable> searchResult = new List<AdresseTable>();
            if (!String.IsNullOrEmpty(SearchPhrase))
            {
                string[] parts = SearchPhrase.Split(new char[] { ' ' });
                List<string> searchTerms = new List<string>();
                foreach (string part in parts)
                {
                    searchTerms.Add(part.Trim());
                }

                searchResult = (from a in db.AdresseTables
                                where searchTerms.All(term => a.Ville.ToUpper().Contains(term.ToUpper()) ||
                                                              a.CodePostal.ToUpper().Contains(term.ToUpper()) ||
                                                              a.Pays.ToUpper().Contains(term.ToUpper()) ||
                                                              a.Province.ToUpper().Contains(term.ToUpper()) ||
                                                              a.Rue.ToUpper().Contains(term.ToUpper())
                                                              )
                                select a).ToList();
            }
            return searchResult;
        }
    }
}
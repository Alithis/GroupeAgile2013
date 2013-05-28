using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

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
                SearchPhrase = normalizeString(SearchPhrase);
                List<string> searchTerms = splitTerms(SearchPhrase);

                searchResult = (from a in db.AdresseTables
                                where searchTerms.All(term => a.Ville.ToUpper().Contains(term) ||
                                                              a.CodePostal.ToUpper().Contains(term) ||
                                                              a.Pays.ToUpper().Contains(term) ||
                                                              a.Province.ToUpper().Contains(term) ||
                                                              a.Rue.ToUpper().Contains(term) ||
                                                              a.GeoCodeResponse.ToUpper().Contains(term) 
                                                              )
                                orderby a.Pays, a.Province, a.Ville, a.Rue, a.AptNo
                                select a).ToList();
            }
            return searchResult;
        }

        private String normalizeString(string text)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in text.Trim().ToUpper().ToCharArray())
            {
                string normalizedChar = c.ToString()
                    .Normalize(NormalizationForm.FormD);
                stringBuilder.Append(normalizedChar);
            }

            return stringBuilder.ToString();
        }

        private List<string> splitTerms(string text)
        {
            List<string> terms = new List<string>();
            string[] parts = text.Split(new char[] { ' ' });
            foreach (string part in parts)
            {
                terms.Add(part.Trim());
            }

            return terms;
        }

        public List<AdresseTable> searchMarkers(string northeastLat, string northeastLng, string southwestLat, string southwestLng)
        {
            List<AdresseTable> result = new List<AdresseTable>();

            var neLat = Decimal.Parse(northeastLat);
            var neLng = Decimal.Parse(northeastLng);
            var swLat = Decimal.Parse(southwestLat);
            var swLng = Decimal.Parse(southwestLng);

            decimal maxLat = Math.Max(neLat, swLat);
            decimal minLat = Math.Min(neLat, swLat);
            decimal maxLng = Math.Max(neLng, swLng);
            decimal minLng = Math.Min(neLng, swLng);

            result = (from a in db.AdresseTables
                      where (a.Lattitude < maxLat && a.Lattitude > minLat) && (a.Longitude < maxLng && a.Longitude > minLng)
                      orderby a.Pays, a.Province, a.Ville, a.Rue, a.AptNo
                      select a).ToList();

            return result;
        }
    }

}
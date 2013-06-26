using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RateYoutRent.Models;
using RateYourRent.Helper;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Entity;
using NoteTaLoc.Models;

namespace NoteTaLoc.Controllers
{
    public class SearchController : Controller
    {
        public SearchController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadMarkers()
        {
            try
            {
                List<SearchMarker> list = new List<SearchMarker>();
                using (notetalocEntities db = new notetalocEntities())
                {
                    var rq = from a in db.AdresseTables
                             select new SearchMarker()
                             {
                                 Id = a.AdresseId,
                                 Latitude = (double)a.Lattitude,
                                 Longitude = (double)a.Longitude,
                                 Address = a.GeoCodeResponse,
                             };

                    list.AddRange(rq.ToList());
                }

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                HttpContext.Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

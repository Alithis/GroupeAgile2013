using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteTaLoc.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowGeoLocation(string latitude, string longitude)
        {
            ViewBag.Latitude = latitude; 
            ViewBag.Latitude = longitude;

            return View();
        }

    }
}

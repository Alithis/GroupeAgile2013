using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace NoteTaLoc.Controllers
{
    public class Story62Controller : Controller
    {
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }
        //
        // GET: /Story62/

        public ActionResult Index(string idNote)
        {
            ViewBag.idNote = "Story 62 (jqxTree) : " + idNote;
            return View();
        }

        public ActionResult Accordeon()
        {
            ViewBag.idNote = "Story 62 (utilisateurs ayant notés avec le détail des critères)";
            return View();
        }
    }
}

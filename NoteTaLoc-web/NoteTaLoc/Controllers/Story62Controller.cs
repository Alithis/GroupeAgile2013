using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteTaLoc.Controllers
{
    public class Story62Controller : Controller
    {
        //
        // GET: /Story62/

        public ActionResult Index(string idNote)
        {
            ViewBag.idNote = "Story 62 (jqxTree) : " + idNote;
            return View();
        }

        public ActionResult Accordeon()
        {
            ViewBag.idNote = "Story 62 (Accordeon)";
            return View();
        }
    }
}

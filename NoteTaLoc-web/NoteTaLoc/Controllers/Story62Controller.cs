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
            ViewBag.idNote = "Story 62 : " + idNote;
            return View();
        }

    }
}

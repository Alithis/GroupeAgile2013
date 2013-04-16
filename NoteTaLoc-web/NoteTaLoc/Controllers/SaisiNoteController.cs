using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NoteTaLoc.Controllers
{
    public class SaisiNoteController : Controller
    {
        //
        // GET: /SaisiNote/

        public ActionResult Index()
        {
            SaisiNoteForm form = new SaisiNoteForm();
            form.Pays = "Canada";

            return View(form);
        }

        //
        // GET: /SaisiNote/5

        [HttpPost]
        public ActionResult Index(SaisiNoteForm form)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "La note a été enregistrée.";
                ViewBag.NumTimes = 1;
            }
            return View(form);
        }

        //
        // POST : /SaisiNote/Noter/
        [HttpPost]
        public ActionResult Noter(SaisiNoteForm form)
        {
            ViewBag.Message="La note a été enregistrée.";
            ViewBag.NumTimes = 1;
            return View(form);
        }
    }
}

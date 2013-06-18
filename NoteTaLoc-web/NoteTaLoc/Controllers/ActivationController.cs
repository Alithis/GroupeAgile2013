using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace NoteTaLoc.Controllers
{
    public class ActivationController : Controller
    {
        //
        // GET: /Activation/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

    }
}

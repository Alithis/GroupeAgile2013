﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteTaLoc.Controllers
{
    public class Story3Controller : Controller
    {
        //
        // GET: /Story3/

        public ActionResult Index(String idAdresse)
        {
            ViewBag.idAdresse = "Story 3 : " + idAdresse;
            return View();
        }

    }
}
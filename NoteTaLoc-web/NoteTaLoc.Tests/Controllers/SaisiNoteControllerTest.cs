﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using NoteTaLoc.Controllers;
using System.Web;
using System.Web.Mvc;
using NoteTaLoc.Models;

namespace NoteTaLoc.Tests.Controllers
{
    [TestClass]
    public class SaisiNoteControllerTest
    {
        [TestMethod]
        public void NoterAppartementWhenGood()
        {
            SaisiNoteController controller = new SaisiNoteController();
            string address = "4049 Avenue Bannantyne, Verdun, QC H4G 2N4, Canada";
            string country = "Canada";
            string zip = "Montréal";
            string provincia = "Quebec";
            string citta = "Montréal";
            string appart = "Montréal";
            string lng = "45.4633017";
            string lat = "-73.5756787";
            string nota = "3";
            var result = controller.NoterAppartement(address, country, zip, provincia, citta, appart, lng, lat, nota) as ActionResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void IndexTestWhenUserIsNotLogged()
        {
            SaisiNoteController controller = new SaisiNoteController();
            var result = controller.Index() as ViewResult; ;
            Assert.AreEqual("Login", result.ViewName);
        }
        [TestMethod]
        public void IndexTestWhenUserIsLogged()
        {
            /*
            notetalocEntities db = new notetalocEntities();
            var username = "shum";
            var userObject = db.UserTables.Single(t => t.Pseudo == username);

            HttpContext.Session.Add("UserSessionObject", userObject);
            SaisiNoteController controller = new SaisiNoteController();
            var result = controller.Index() as ViewResult; ;
            Assert.AreEqual("index", result.ViewName);
             */
        }
    }
}

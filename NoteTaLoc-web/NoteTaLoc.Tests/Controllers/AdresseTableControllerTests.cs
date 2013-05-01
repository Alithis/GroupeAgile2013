using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteTaLoc;
using NoteTaLoc.Controllers;
using NoteTaLoc.Models;
using System.Web.Mvc;


namespace NoteTaLoc.Tests.Controllers
{
    [TestClass]
    public class AdresseTableControllerTests
    {
        [TestMethod]
        public void SearchNoted()
        {
            AdresseTableController controller = new AdresseTableController();
            var result = controller.SearchNoted("Montréal") as ViewResult;
            Assert.AreEqual("SearchNoted", result.ViewName);
        }

        [TestMethod]
        public void SearchAdresses()
        {
            AdresseTableController controller = new AdresseTableController();
            var result = controller.SearchAdresses("Montréal");
            Assert.IsTrue(result.Count() > 0);
        }
    }
}

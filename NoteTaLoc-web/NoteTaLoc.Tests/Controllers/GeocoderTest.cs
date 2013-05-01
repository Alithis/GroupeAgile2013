using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoteTaLoc.Tests.Controllers
{
    [TestClass]
    public class GeocoderTest
    {
        [TestMethod]
        public void TestGoodAddressLocate()
        {
            var address = "4049 bannantyne,+Verdun,+QC+H4G1C2,+Canada";
            var geocode = new NoteTaLoc.Controllers.Geocoder();

            var location = geocode.Locate(address);

            Assert.AreEqual(45.4633017, location.Latitude);
            Assert.AreEqual(-73.5756787,  location.Longitude);
            Assert.AreEqual("4049 Avenue Bannantyne, Verdun, QC H4G 1C2, Canada", location.Formatted_address);
        }

        [TestMethod]
        public void TestWrongAddressLocate()
        {
            var address = "fffffffffffff";
            var geocode = new NoteTaLoc.Controllers.Geocoder();

            var location = geocode.Locate(address);

            Assert.IsNull(location);
        }

        [TestMethod]
        public void TestOnlyNumberAddressLocate()
        {
            var address = "245";
            var geocode = new NoteTaLoc.Controllers.Geocoder();

            var location = geocode.Locate(address);

            Assert.IsNull(location);
        }

        [TestMethod]
        public void TestApproximateAddressLocate()
        {
            var address = "4049bannntyne,+Montréal,+Canada";
            var geocode = new NoteTaLoc.Controllers.Geocoder();

            var location = geocode.Locate(address);

            Assert.IsNull(location);
        }
    }
}

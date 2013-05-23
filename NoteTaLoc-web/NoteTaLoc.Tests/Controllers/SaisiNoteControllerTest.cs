using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace NoteTaLoc.Tests.Controllers
{
    [TestClass]
    public class SaisiNoteControllerTest
    {
        [TestMethod]
        public void TestSaveNoteSaisi()
        {
            var form = new NoteTaLoc.Models.SaisiNoteForm()
            {
                Rue = "4049 bannantyne",
                Region = "Montréal",
                CodePostal = "H4G 1C2",
                Pays = "Canada"
            };
            //var address = "4049 bannantyne,+Montréal,+Canada";
            var address = new NoteTaLoc.Models.AdresseTable()
            {
                Rue = "4049 bannantyne",
                Ville = "Montréal",
                Pays = "Canada"
                //Longitude = 45.4633017,
                //Lattitude = -73.5756787,
                //GeoCodeResponse = "4049 Avenue Bannantyne, Verdun, QC H4G 2N4, Canada"
            };
            var note = new NoteTaLoc.Models.NoteTable()
            {
                Note = 1,
                AdresseId = 2
            };
            var Writer = MockRepository.GenerateMock<NoteTaLoc.Controllers.SaisiNoteWriter>();
            Writer.Expect(m => m.SaveAddresNoteSaisi(address));
            Writer.Expect(m => m.GetAddressId(address)).Return(2);
            Writer.Expect(m => m.SaveNoteSaisi(note));

            var controller = new NoteTaLoc.Controllers.SaisiNoteController();
            controller.Index(form);

            Writer.VerifyAllExpectations();
        }
    }
}

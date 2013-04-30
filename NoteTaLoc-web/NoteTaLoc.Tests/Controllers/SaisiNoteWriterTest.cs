using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace NoteTaLoc.Tests.Controllers
{
    [TestClass]
    public class SaisiNoteWriterTest
    {
        [TestMethod]
        public void TestSaveAdress()
        {
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

            };
            var mailWriter = MockRepository.GenerateMock<NoteTaLoc.Controllers.MailSender>();
            //mailWriter.Expect(m => m.SendMail("Canada", "Canada", "Canada", "Canada")).IgnoreArguments().Repeat.Never();
            var context = MockRepository.GenerateMock<NoteTaLoc.Controllers.SaisiNoteContext>();
            context.Expect(m => m.SaveAddress(address));
            //context.Expect(m => m.UpdateNoteStatus(note)).Repeat.Never();

            var saisiNoteWriter = new NoteTaLoc.Controllers.SaisiNoteWriter(mailWriter, context);
            saisiNoteWriter.SaveAddresNoteSaisi(address);

            mailWriter.VerifyAllExpectations();
            context.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestSaveNoteSaisiWithoutSendingMail()
        {
            var mailTo = "eric.foka@alithis.com";
            var mailFrom = "duc.pham@alithis.com";
            var msg = "message";
            var obj = "object";
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

            };
            var mailWriter = MockRepository.GenerateMock<NoteTaLoc.Controllers.MailSender>();
            mailWriter.Expect(m => m.SendMail(mailTo, mailFrom, obj, msg)).IgnoreArguments().Repeat.Never();
            var context = MockRepository.GenerateMock<NoteTaLoc.Controllers.SaisiNoteContext>();
            context.Expect(m => m.SaveAddress(address));
            context.Expect(m => m.UpdateNoteStatus(note));

            var saisiNoteWriter = new NoteTaLoc.Controllers.SaisiNoteWriter(mailWriter, context);
            saisiNoteWriter.SaveNoteSaisi(note);

            mailWriter.VerifyAllExpectations();
            context.VerifyAllExpectations();
        }

        [TestMethod]
        public void TestSaveNoteSaisiWithSendingMail()
        {
            var mailTo = "eric.foka@alithis.com";
            var mailFrom = "duc.pham@alithis.com";
            var msg = "message";
            var obj = "object";
            var form = new NoteTaLoc.Models.SaisiNoteForm()
            {
                Rue = "4049 bannantyne",
                Region = "Montréal",
                CodePostal = "H4G 1C2",
                Pays = "Canada"
                //Longitude = 45.4633017,
                //Lattitude = -73.5756787,
                //GeoCodeResponse = "4049 Avenue Bannantyne, Verdun, QC H4G 2N4, Canada"
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
                Note = 0,

            };
            var mailWriter = MockRepository.GenerateMock<NoteTaLoc.Controllers.MailSender>();
            mailWriter.Expect(m => m.SendMail(mailTo, mailFrom, obj, msg));
            var context = MockRepository.GenerateMock<NoteTaLoc.Controllers.SaisiNoteContext>();
            context.Expect(m => m.SaveAddress(address));
            context.Expect(m => m.UpdateNoteStatus(note)).IgnoreArguments().Repeat.Never();

            var saisiNoteWriter = new NoteTaLoc.Controllers.SaisiNoteWriter(mailWriter, context);
            saisiNoteWriter.SaveNoteSaisi(note);

            mailWriter.VerifyAllExpectations();
            context.VerifyAllExpectations();
        }
    }
}

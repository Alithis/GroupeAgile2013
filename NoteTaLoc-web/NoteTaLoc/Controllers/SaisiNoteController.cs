using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

//{
//              }
namespace NoteTaLoc.Controllers
{
    public class MailSender
    {
        public MailSender()
        {
        }
        public virtual void SendMail(string mailTo, string mailFrom, string obj, string msg)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            string serverMail = config.AppSettings.Settings["ServerMail"].Value;

            MailAddress to = new MailAddress(mailTo);
            MailAddress fromMail = new MailAddress(mailFrom);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = fromMail;
            mailMessage.To.Add(to);
            mailMessage.Subject = obj;
            mailMessage.Body = msg;

            SmtpClient client = new SmtpClient();
            //client.Host = "mail.cia.ca";
            client.Host = serverMail;
            client.Send(mailMessage);
        }
    }

    public class SaisiNoteContext
    {
        public virtual void SaveNote(NoteTable note)
        {
            using (var ctx = new notetalocEntities())
            {
                note.NoteId = GetNoteId(DateTime.Now);
                ctx.NoteTables.Add(note);
                ctx.SaveChanges();
            }
        }

        private int GetNoteId(DateTime dt)
        {

            return (dt.Year * 10000 + dt.Month * 100 + dt.Day + dt.Hour * 24 + dt.Minute * 60 + dt.Second * 60);
        }

        public virtual void SaveAddress(AdresseTable address)
        {
            using (var ctx = new notetalocEntities())
            {
                var add = ctx.AdresseTables.Where(a => a.GeoCodeResponse == address.GeoCodeResponse).FirstOrDefault();
                if (add == null)
                {
                    address.AdresseId = GetAddresseId(DateTime.Now);
                    ctx.AdresseTables.Add(address);
                    ctx.SaveChanges();
                }
            }
        }

        private int GetAddresseId(DateTime dt)
        {

            return (dt.Year * 10000 + dt.Month * 100 + dt.Day + dt.Hour * 24 + dt.Minute * 60 + dt.Second * 60);
        }

        public virtual void UpdateNoteStatus(NoteTable note)
        {
            using (var ctx = new notetalocEntities())
            {
                var noteToUpdate = ctx.NoteTables.Where(n => n.NoteId == note.NoteId).FirstOrDefault();
                noteToUpdate.StatutNote = 1;
                ctx.SaveChanges();
            }
        }

    }

    public class SaisiNoteWriter
    {
        private MailSender _MailSender;
        private SaisiNoteContext _SaisiNoteContext;
        public SaisiNoteWriter(MailSender mailSender, SaisiNoteContext context)
        {
            _MailSender = mailSender;
            _SaisiNoteContext = context;
        }
        public void SaveAddresNoteSaisi(AdresseTable address)
        {
            _SaisiNoteContext.SaveAddress(address);
        }

        public void SaveNoteSaisi(NoteTable note)
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            string receiverMail = config.AppSettings.Settings["ReceiverMail"].Value;
            string senderMail = config.AppSettings.Settings["SenderMail"].Value;
            var mailTo = receiverMail;
            var mailFrom = senderMail;
            var msg = "Verifier la note de cet appartement!";
            var obj = "NoteTaLoc";
            _SaisiNoteContext.SaveNote(note);
            if (note.Note == 0)
            {
                _MailSender.SendMail(mailTo, mailFrom, obj, msg);
            }
            else
            {
                _SaisiNoteContext.UpdateNoteStatus(note);
            }
        }


        public virtual int GetAddressId(AdresseTable address)
        {
            using (var ctx = new notetalocEntities())
            {
                return ctx.AdresseTables.SingleOrDefault(a => a.GeoCodeResponse == address.GeoCodeResponse).AdresseId;
            }
        }
    }

    [Serializable]
    public class GeocoderLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Formatted_address { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Latitude, Longitude);
        }
    }

    public class Geocoder
    {
        public GeocoderLocation Locate(string query)
        {
            string url = string.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&region=ca&sensor=false",
               HttpUtility.UrlEncode(query));
            WebRequest request = WebRequest.Create(url);

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    var result = document.Descendants("result").ToList();
                    XElement status = document.Descendants("status").FirstOrDefault();
                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();
                    XElement formattedAddressElement = document.Descendants("formatted_address").FirstOrDefault();
                    if (status.Value == "OK" && result.Count == 1)
                    {
                        if (longitudeElement != null && latitudeElement != null)
                        {
                            return new GeocoderLocation
                            {
                                Longitude = Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                                Latitude = Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture),
                                Formatted_address = formattedAddressElement.Value,
                                Status = status.Value
                            };
                        }
                    }
                }
            }

            return null;
        }
    }


    public class SaisiNoteController : Controller
    {
        private SaisiNoteWriter _SaisiNoteWriter;
        public SaisiNoteController()
        {
            _SaisiNoteWriter = new SaisiNoteWriter(new MailSender(), new SaisiNoteContext());
        }
        //
        // GET: /SaisiNote/

        public ActionResult Index()
        {
            //SaisiNoteForm form = new SaisiNoteForm();
            var userVariable = HttpContext.Session["UserSessionObject"];

            if (userVariable == null)
            {
                return RedirectToAction("LogIn", "Account", String.Format("{0}/{1}", "Index", "SaisiNote"));
                //RedirectToAction( "Index",  "SaisiNote", null);    

            }
            return View();
        }

        //
        // POST: /SaisiNote/NoterAppartement

        public ActionResult NoterAppartement(string address, string country, string zip, string provincia, string citta, string appart, string lng, string lat, string nota)
        {
            //SaisiNoteForm form = new SaisiNoteForm();

            var data = new
            {
                indirizzo = address,
                Pays = country,
                zip = zip,
                province = provincia,
                ville = citta,
                appartement = appart,
                longitude = lng,
                latitude = lat,
                note = nota
            };
            var result = new { returnValue = false };
            try
            {
                var valLng = Double.Parse(lng, CultureInfo.InvariantCulture);
                var valLat = Double.Parse(lat, CultureInfo.InvariantCulture);

                var addressToSave = new AdresseTable();
                addressToSave.RueNo = int.Parse(GetRueAndNumero(address)[0]);
                addressToSave.Rue = GetRueAndNumero(address)[1];
                addressToSave.AptNo = appart;
                addressToSave.CodePostal = zip;
                addressToSave.Ville = citta;
                addressToSave.Pays = country;
                addressToSave.Province = provincia;
                addressToSave.GeoCodeResponse = address;
                addressToSave.Lattitude = (decimal)valLat;
                addressToSave.Longitude = (decimal)valLng;

                var saisiNoteWriter = new SaisiNoteWriter(new MailSender(), new SaisiNoteContext());
                saisiNoteWriter.SaveAddresNoteSaisi(addressToSave);
                var id = saisiNoteWriter.GetAddressId(addressToSave);
                var noteToSave = new NoteTable();
                noteToSave.Note = int.Parse(nota);
                noteToSave.AdresseId = id;
                noteToSave.UserId = 1;
                noteToSave.StatutNote = 0;
                saisiNoteWriter.SaveNoteSaisi(noteToSave);
                result = new { returnValue = true };
            }
            catch (Exception ex)
            {
                result = new { returnValue = false };
            }

            return Json(result); ;
        }

        private List<string> GetRueAndNumero(string address)
        {
            List<string> result = new List<string>();
            var firstSplit = address.Split(',');
            var rueNumero = firstSplit[0];
            var secondSplit = rueNumero.Split(' ');
            var number = secondSplit[0];
            result.Add(number);
            string rue = secondSplit[1];
            for (int i = 2; i < secondSplit.Length; i++)
            {
                rue += " " + secondSplit[i];
            }
            result.Add(rue);
            return result;
        }

        //
        // GET: /SaisiNote/5

        [HttpPost]
        public ActionResult Index(SaisiNoteForm form)
        {
            if (ModelState.IsValid)
            {
                var address = ConcatenationAddress(form);
                try
                {
                    var geocoder = new Geocoder();
                    var response = geocoder.Locate(address);
                    if (response != null)
                    {
                        var longititude = response.Longitude;
                        var lat = response.Latitude;
                        var formatted_address = response.Formatted_address;
                        var addressToSave = new AdresseTable();
                        addressToSave.AptNo = form.Appartement;
                        addressToSave.CodePostal = form.CodePostal;
                        addressToSave.Ville = form.Localite;
                        addressToSave.Pays = form.Pays;
                        addressToSave.Province = form.Region;
                        addressToSave.GeoCodeResponse = formatted_address;
                        addressToSave.Lattitude = (decimal)lat;
                        addressToSave.Longitude = (decimal)longititude;

                        var saisiNoteWriter = new SaisiNoteWriter(new MailSender(), new SaisiNoteContext());
                        saisiNoteWriter.SaveAddresNoteSaisi(addressToSave);
                        var id = saisiNoteWriter.GetAddressId(addressToSave);
                        var noteToSave = new NoteTable();
                        noteToSave.Note = form.Note;
                        noteToSave.AdresseId = id;
                        noteToSave.UserId = 1;
                        noteToSave.StatutNote = 0;
                        saisiNoteWriter.SaveNoteSaisi(noteToSave);
                        ViewBag.Message = "Enregistrement reussie!";
                        ViewBag.NumTimes = 1;
                        ViewData["color"] = "green";
                    }
                    else
                    {
                        ViewBag.Message = "vous devez inserez une adresse valide!";
                        ViewBag.NumTimes = 1;
                        ViewData["color"] = "red";
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "La note n'a pas été enregistrée. Réessayer plus tard!";
                    ViewBag.NumTimes = 1;
                    ViewData["color"] = "red";
                }
            }
            return View(form);
        }

        //
        // POST : /SaisiNote/Noter/
        [HttpPost]
        public ActionResult Noter(SaisiNoteForm form)
        {
            ViewBag.Message = "La note a été enregistrée.";
            ViewBag.NumTimes = 1;
            return View(form);
        }

        private string ConcatenationAddress(SaisiNoteForm form)
        {
            string address = "";
            if (form.Rue != null)
                address += form.Rue;
            address = addField(address, form.Appartement);
            address = addField(address, form.Localite);
            address = addField(address, form.CodePostal);
            address = addField(address, form.Region);
            address = addField(address, form.Pays);

            return address;
        }


        private String addField(String src, String field)
        {
            if (!String.IsNullOrEmpty(field))
            {
                if (!String.IsNullOrEmpty(src) && src[src.Length - 1] != '+')
                {
                    src += "+";
                }
                src += field;
            }

            return src;
        }
    }
}

using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
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
            MailAddress to = new MailAddress(mailTo);
            MailAddress fromMail = new MailAddress(mailFrom);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = fromMail;
            mailMessage.To.Add(to);
            mailMessage.Subject = obj;
            mailMessage.Body = msg;

            //SmtpClient client = new SmtpClient();
            //client.Host = "mai.cia.ca";
            //client.Send(mailMessage);
        }
    }

    public class SaisiNoteContext
    {
        public virtual void SaveNote(NoteTable note)
        {
            using (var ctx = new notetalocEntities())
            {
                ctx.NoteTables.Add(note);
                ctx.SaveChanges();
            }
        }
        public virtual void SaveAddress(AdresseTable address)
        {
            using (var ctx = new notetalocEntities())
            {
                var add = ctx.AdresseTables.Where(a => a.Lattitude == address.Lattitude && a.Longitude == address.Longitude).FirstOrDefault();
                if (add != null)
                {


                }
                else
                {
                    ctx.AdresseTables.Add(address);
                }
                ctx.SaveChanges();
            }
        }




        public virtual void UpdateNoteStatus(NoteTable note)
        {
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
            var mailTo = "eric.foka@alithis.com";
            var mailFrom = "duc.pham@alithis.com";
            var msg = "message";
            var obj = "object";
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
                return ctx.AdresseTables.SingleOrDefault(a => a.Lattitude == address.Lattitude && a.Longitude == address.Longitude && a.GeoCodeResponse == address.GeoCodeResponse).AdresseId;
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

            return View();
        }

        //
        // GET: /SaisiNote/5

        [HttpPost]
        public ActionResult Index(SaisiNoteForm form)
        {
            if (ModelState.IsValid)
            {
                var address = ConcatenationAddress(form);
                //var address = "4049 bannantyne,+Montréal,+Canada";
                //address + “,” + city + “,” + state + “,” + postcode + ” “ + country
                //// string url = string.Format("http://maps.google.com/maps/api/geocode/xml?address={0}&region=ca&sensor=false",
                ////HttpUtility.UrlEncode(address)
                //);

                // var urlToValidate = string.Format(url, address);
                // WebResponse response = null;
                try
                {
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    //request.Method = "GET";
                    //response = request.GetResponse();
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
                        addressToSave.GeoCodeResponse = formatted_address;
                        //addressToSave.Lattitude = lat;
                        //addressToSave.Longitude = longititude;

                        var saisiNoteWriter = new SaisiNoteWriter(new MailSender(), new SaisiNoteContext());
                        saisiNoteWriter.SaveAddresNoteSaisi(addressToSave);

                        var noteToSave = new NoteTable();
                        noteToSave.Note = form.Note;
                        saisiNoteWriter.SaveNoteSaisi(noteToSave);

                    }
                    else
                    {
                        //ViewBag.Message = "Aucune réponse du serveur!";
                        //ViewBag.NumTimes = 1;
                        ViewBag.Message = "vous devez inserez une adresse valide!";
                        ViewBag.NumTimes = 1;
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "La note n'a pas été enregistrée. Réessayer plus tard!";
                    ViewBag.NumTimes = 1;
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
            if (form.Numero != null)
                address += form.Numero;
            address = addField(address, form.Rue);
            //addField(address, form.Rue);
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

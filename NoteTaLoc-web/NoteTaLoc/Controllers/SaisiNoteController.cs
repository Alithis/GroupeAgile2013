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
<<<<<<< HEAD
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

            SmtpClient client = new SmtpClient();
            client.Host = "mail.cia.ca";
            client.Send(mailMessage);
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
                if (add == null)
                {
                    ctx.AdresseTables.Add(address);
                }
                ctx.SaveChanges();
            }
        }
        
        public virtual void UpdateNoteStatus(NoteTable note)
        {
            using (var ctx = new notetalocEntities())
            {
                var noteToUpdate = ctx.NoteTables.SingleOrDefault(n=>n.NoteId == note.NoteId);
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
            var mailTo = "eric.foka@alithis.com";
            var mailFrom = "duc.pham@alithis.com";
            var msg = "Un usager a donné la note 0 à un appartement. Proceder aux verifications!";
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


=======
    public class EnvoyeurCourriel
    {
        private string _ServerHost;

        public EnvoyeurCourriel()
        {
            _ServerHost = "mail.cia.ca";
        }

        public void SendMail(string from, string to, string objet, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Sender = new MailAddress(to);
            mailMessage.From = new MailAddress(from);
            SmtpClient client = new SmtpClient();
            client.Host = _ServerHost;
            client.Send(mailMessage);
        }
    
    }

>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f
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
<<<<<<< HEAD
=======
                var filtering = "country:CA";// route ¦ locality ¦ locality ¦ administrative_area ¦ postal_code ¦ country 
                string url = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&components={1}&sensor=false";
                var urlToValidate = string.Format(url, address, filtering);
                WebResponse response = null;
>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f
                try
                {
                    var geocoder = new Geocoder();
                    var response = geocoder.Locate(address);
                    if (response != null)
                    {
<<<<<<< HEAD
                        var longititude = response.Longitude;
                        var lat = response.Latitude;
                        var formatted_address = response.Formatted_address;
                        var addressToSave = new AdresseTable();
                        addressToSave.AptNo = form.Appartement;
                        addressToSave.CodePostal = form.CodePostal;
                        addressToSave.GeoCodeResponse = formatted_address;
                        addressToSave.Pays = form.Pays;
                        addressToSave.Province = form.Region;
                        addressToSave.Ville = form.Localite;
                        //addressToSave.Lattitude = lat;
                        //addressToSave.Longitude = longititude;

                        var saisiNoteWriter = new SaisiNoteWriter(new MailSender(), new SaisiNoteContext());
                        saisiNoteWriter.SaveAddresNoteSaisi(addressToSave);
=======
                        XPathDocument document = new XPathDocument(response.GetResponseStream());
                        XPathNavigator navigator = document.CreateNavigator();
                        navigator.MoveToChild("GeocodeResponse", "");
                        navigator.MoveToChild("status", "");
                        XmlReader status = navigator.ReadSubtree();
                        String st = "";
                        while (status.Read())
                        {
                            st = status.ReadInnerXml();
                        }
                        if (st != "OK")
                        {
                            ViewBag.Message = "vous devez inserez une adresse valide!";
                            ViewBag.NumTimes = 1;
                        }
                        else
                        {
                            /*
                            XPathNodeIterator result = navigator.Select("/GeocodeResponse/result");
                            XPathNodeIterator formattedAddressIterator = result.Current.Select("formatted_address");
                            navigator.MoveToChild("formatted_address", "");
                             XmlReader xmlReaderFormatAddress = navigator.ReadSubtree();
                            XPathNodeIterator geometryIterator = result.Current.Select("geometry");
                            XPathNodeIterator locationIterator = geometryIterator.Current.Select("location");
                            XPathNodeIterator latIterator = locationIterator.Current.Select("lat");
                            navigator.MoveToChild("lat", "");
                            XmlReader xmlReaderLat = navigator.ReadSubtree();
                            //latIterator.MoveNext();
                            XPathNodeIterator lngIterator = locationIterator.Current.Select("lng");
                            navigator.MoveToChild("lng", "");
                            XmlReader xmlReaderLn = navigator.ReadSubtree();
                            //lngIterator.MoveNext();
                             * */
                            ViewBag.Message = "La note a été enregistrée.";
                            ViewBag.NumTimes = 1;

                            using (var ctx = new notetalocEntities())
                            {
                                AdresseTable addressTable = new AdresseTable();
                                addressTable.Rue = form.Rue;
                                addressTable.Ville = form.Localite;
                                addressTable.Pays = form.Pays;
                                addressTable.Province = form.Region;
                                addressTable.CodePostal = form.CodePostal;
                                addressTable.AptNo = form.Appartement;
                                addressTable.GeoCodeResponse = "1155 Rue Metcalfe #2121, Montréal, QC H3B 4J5, Canada";
                                //addressTable.Longitude = int.Parse(xmlReaderLat.ReadInnerXml());
                                //addressTable.Lattitude = int.Parse(xmlReaderLn.ReadInnerXml());
                                addressTable.Longitude = 76;
                                addressTable.Lattitude = 45;
                                ctx.Set<AdresseTable>().Add(addressTable);
                                ctx.SaveChanges();

                                var addId = (from a in ctx.AdresseTables
                                           //where a.GeoCodeResponse == xmlReaderFormatAddress.ReadInnerXml()
                                             where a.GeoCodeResponse == "1155 Rue Metcalfe #2121, Montréal, QC H3B 4J5, Canada"
                                           select a.AdresseId).First();
                               
                                var noteTable = new NoteTable();
                                noteTable.AdresseId = addId;
                                noteTable.Note = form.Note;
                                ctx.Set<NoteTable>().Add(noteTable);
                                ctx.SaveChanges();
                            }

                            if (form.Note == 0)
                            {
                                var from = "eric.foka@alithis.com";
                                var to = "duc.pham@alithis.com";
                                var obj = "Note d'une location";
                                var body = "L'usager a insérer la valeur 0! Verifier cette note avec le propriétaire.";
                                var senderMail = new EnvoyeurCourriel();
                                senderMail.SendMail(from, to, obj, body);
                            }
                            


                        }
                    }
                        else
                        {
                            ViewBag.Message = "Aucune réponse du serveur!";
                            ViewBag.NumTimes = 1;
                        }
>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f

                        var idAddress = saisiNoteWriter.GetAddressId(addressToSave);
                         
                        var noteToSave = new NoteTable();
                        noteToSave.Note = form.Note;
                        noteToSave.UserId = 1;
                        noteToSave.AdresseId = idAddress;
                        noteToSave.StatutNote = 0;
                        saisiNoteWriter.SaveNoteSaisi(noteToSave);

                        ViewBag.Message = "Enregistrement reussie!";
                        ViewBag.NumTimes = 1;
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
<<<<<<< HEAD
            if (form.Numero != null)
                address += form.Numero;
            address = addField(address, form.Rue);
            //addField(address, form.Rue);
=======
            address = addField(address, form.Rue);
>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f
            address = addField(address, form.Appartement);
            address = addField(address, form.Localite);
            address = addField(address, form.CodePostal);
            address = addField(address, form.Region);
            address = addField(address, form.Pays);

            return address;
        }

<<<<<<< HEAD
=======

>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f
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
<<<<<<< HEAD

=======
>>>>>>> 96ad74dd8ecd6293cac75b321eabf350b22b168f
            return src;
        }
    }
}

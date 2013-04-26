using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

namespace NoteTaLoc.Controllers
{
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

    public class SaisiNoteController : Controller
    {
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
                var filtering = "country:CA";// route ¦ locality ¦ locality ¦ administrative_area ¦ postal_code ¦ country 
                string url = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&components={1}&sensor=false";
                var urlToValidate = string.Format(url, address, filtering);
                WebResponse response = null;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    response = request.GetResponse();
                    if (Response != null)
                    {
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
            address = addField(address, form.Rue);
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

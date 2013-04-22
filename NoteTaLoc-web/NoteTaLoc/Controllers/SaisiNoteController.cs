using NoteTaLoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

namespace NoteTaLoc.Controllers
{
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
                string url = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";
                var urlToValidate = string.Format(url, address);
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
                            XPathNodeIterator result = navigator.Select("/GeocodeResponse/result");
                            //XPathNodeIterator formattedAddressIterator = result.Current.Select("formatted_address");
                            XPathNodeIterator geometryIterator = result.Current.Select("geometry");
                            XPathNodeIterator locationIterator = geometryIterator.Current.Select("location");
                            XPathNodeIterator latIterator = locationIterator.Current.Select("lat");
                            XPathNodeIterator lngIterator = locationIterator.Current.Select("lng");
                            ViewBag.Message = "La note a été enregistrée.";
                            ViewBag.NumTimes = 1;
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
            if (form.Numero != null)
                address += form.Numero;
            addField(address, form.Rue);
            addField(address, form.Rue);
            addField(address, form.Appartement);
            addField(address, form.Localite);
            addField(address, form.CodePostal);
            addField(address, form.Region);
            addField(address, form.Pays);

            return address;
        }


        private void addField(String src, String field) {
            if (String.IsNullOrEmpty(field))
            {
                if (String.IsNullOrEmpty(src) && src[src.Length-1] != '+')
                {
                    src += "+";
                }
                src += field;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteTaLoc.Models
{
    public class SaisiNoteForm
    {
        //[Required(ErrorMessage = "Le numéro est obligatoire.")]
        public int  Numero { get; set; }
        //[Required(ErrorMessage = "La rue est obligatoire.")]
        public string Rue { get; set; }
        public string Appartement { get; set; }
        public string Localite { get; set; }
        //[Required(ErrorMessage = "Le code postal est obligatoire.")]
        public string CodePostal { get; set; }
        public string Region { get; set; }

        public string Pays { get; set; }
        //[Required(ErrorMessage = "Le numéro est obligatoire.")]
        //[Range(0, 5, ErrorMessage = "La note doit être comprise entre 0 et 5")]
        public int Note { get; set; }


    }
}
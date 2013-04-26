using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteTaLoc.Models
{
    public class SaisiNoteForm
    {
        [Required(ErrorMessage = "La rue est obligatoire.")]
        public string Rue { get; set; }

        public string Appartement { get; set; }

        [Required(ErrorMessage = "La localité est obligatoire.")]
        public string Localite { get; set; }

        [Required(ErrorMessage = "Le code postal est obligatoire.")]
        public string CodePostal { get; set; }

        [Required(ErrorMessage = "La région est obligatoire.")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Le pays est obligatoire.")]
        public string Pays { get; set; }

        [Required(ErrorMessage = "Le numéro est obligatoire.")]
        [Range(0, 5, ErrorMessage = "La note doit être comprise entre 0 et 5")]
        public int Note { get; set; }
    }
}
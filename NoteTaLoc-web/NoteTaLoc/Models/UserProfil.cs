/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
*/
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace NoteTaLoc.Models
{
    public class UserProfil
    {
        public int UserId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Pseudo { get; set; }
        public string MotDePasse { get; set; }
        public string Token { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Courriel { get; set; }
    }
}
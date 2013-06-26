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

        public int Note { get; set; }

        public List<NoteTaLoc.Models.CriteriaTable> CriteriaTableList { get; set; }

        public SaisiNoteForm() {

            CriteriaTableList = new List<CriteriaTable>();
            NoteTaLoc.Utilitary.CategoryStorage tmpCategories = (NoteTaLoc.Utilitary.CategoryStorage)HttpContext.Current.Application["Categories"];

            foreach(NoteTaLoc.Utilitary.Criteria refCrit in tmpCategories.getCriteriaList("fr")){

                CriteriaTable tmpCrit = new CriteriaTable();
                tmpCrit.Criteria = refCrit.criteriaId;
                tmpCrit.Category = refCrit.categoryId;
                this.CriteriaTableList.Add(tmpCrit);

            }

        }
    }
}
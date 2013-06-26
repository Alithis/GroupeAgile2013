//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace NoteTaLoc.Models
{
    public partial class NoteTable
    {
        public NoteTable()
        {
            this.CriteriaTables = new HashSet<CriteriaTable>();
        }
    
        public int NoteId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> AdresseId { get; set; }
        public Nullable<int> Note { get; set; }
        public Nullable<int> StatutNote { get; set; }
        public string Commentaire { get; set; }
        public Nullable<int> StatutComment { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
    
        public virtual AdresseTable AdresseTable { get; set; }
        public virtual UserTable UserTable { get; set; }
        public virtual ICollection<CriteriaTable> CriteriaTables { get; set; }
    }
    
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Tickets")]
    public partial class Ticket
    {
        public Ticket()
        {
            Discussions = new HashSet<Discussion>();
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreerDate { get; set; }
        public int StatutId { get; set; }
        public int CategorieId { get; set; }
        public int PeriodeId { get; set; }
        public int EmployeeId { get; set; }
        public int? SupporterId { get; set; }

        public virtual Category Categorie { get; set; }
        public virtual Compte Employee { get; set; }
        public virtual Periode Periode { get; set; }
        public virtual Statut Statut { get; set; }
        public virtual Compte Supporter { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}

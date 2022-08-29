using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Categories")]
    public partial class Category
    {
        public Category()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public bool Statut { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

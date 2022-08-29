using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Statuts")]
    public partial class Statut
    {
        public Statut()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public bool Display { get; set; }
        public string Couleur { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

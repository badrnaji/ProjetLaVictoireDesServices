using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Comptes")]
    public partial class Compte
    {
        public Compte()
        {
            Discussions = new HashSet<Discussion>();
            TicketEmployees = new HashSet<Ticket>();
            TicketSupporters = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public string NomUtilisateur { get; set; }
        public string Password { get; set; }
        public string NomComplet { get; set; }
        public bool Statut { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Phone { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Ticket> TicketEmployees { get; set; }
        public virtual ICollection<Ticket> TicketSupporters { get; set; }
    }
}

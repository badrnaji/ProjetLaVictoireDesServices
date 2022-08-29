using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Roles")]
    public partial class Role
    {
        public Role()
        {
            Comptes = new HashSet<Compte>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }

        public virtual ICollection<Compte> Comptes { get; set; }
    }
}

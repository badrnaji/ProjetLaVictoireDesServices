using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ProjetLaVictoireDesServices.Models
{
    [Table("Photos")]
    public partial class Photo
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}

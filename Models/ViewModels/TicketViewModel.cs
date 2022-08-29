using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetLaVictoireDesServices.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Statutes { get; set; }
        public SelectList Periodes { get; set; }
    }
}

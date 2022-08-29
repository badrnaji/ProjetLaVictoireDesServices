using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetLaVictoireDesServices.Models.ViewModels
{
    public class CompteViewModel
    {
        public Compte Compte { get; set; }
        public SelectList Roles { get; set; }
    }
}

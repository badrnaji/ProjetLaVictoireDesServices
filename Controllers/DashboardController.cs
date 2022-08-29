using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System.Linq;
using System.Security.Claims;

namespace ProjetLaVictoireDesServices.Controllers
{

    [Authorize(Roles = "Administrateur,Support,Employee")]
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly HelpDeskVictoireDBEntities db;

        public DashboardController(HelpDeskVictoireDBEntities _db)
        {
            this.db = _db;
        }


        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.EmployeeId == compte.Id).ToList();
            return View();
        }
    }
}

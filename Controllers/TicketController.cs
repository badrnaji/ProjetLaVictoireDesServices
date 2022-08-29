using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProjetLaVictoireDesServices.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ProjetLaVictoireDesServices.Controllers
{
    [Route("ticket")]
    public class TicketController : Controller
    {

        private readonly HelpDeskVictoireDBEntities db;
        private IWebHostEnvironment iwebHostEnvironment;
        public TicketController(HelpDeskVictoireDBEntities _db, IWebHostEnvironment _iwebHostEnvironment)
        {
            this.db = _db;
            this.iwebHostEnvironment = _iwebHostEnvironment;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("envoyer")]
        public IActionResult Envoyer()
        {
            var ticketViewModel = new TicketViewModel();
            ticketViewModel.Ticket = new Ticket();

            var categorie = db.Categories.Where(r => r.Statut).ToList();
            ticketViewModel.Categories = new SelectList(categorie, "Id", "Nom");

            var Statut = db.Statuts.Where(r => r.Display).ToList();
            ticketViewModel.Statutes = new SelectList(Statut, "Id", "Nom");

            var periode = db.Periodes.Where(r => r.Statut).ToList();
            ticketViewModel.Periodes = new SelectList(periode, "Id", "Nom");

            return View("Envoyer",ticketViewModel);
        }

        [HttpPost]
        [Route("envoyer")]
        public IActionResult Envoyer(TicketViewModel ticketViewModel,IFormFile[] files)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name).Value;
                var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
                ticketViewModel.Ticket.CreerDate = DateTime.Now;
                ticketViewModel.Ticket.EmployeeId = compte.Id;
                db.Tickets.Add(ticketViewModel.Ticket);
                db.SaveChanges();

                //Upload photos pour ticket(optionnel)
                if(files != null && files.Length > 0)
                {
                    foreach(var file in files) 
                    {
                        var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + file.FileName;
                        var path = Path.Combine(iwebHostEnvironment.WebRootPath, "uploads", fileName);
                        var stream = new FileStream(path, FileMode.Create);
                        file.CopyToAsync(stream);

                        //Enregistrer photo dans la base de donnée

                        var photo = new Photo();
                        photo.Nom = fileName;
                        photo.TicketId = ticketViewModel.Ticket.Id;
                        db.Photos.Add(photo);
                        db.SaveChanges();
                    }
                    
                }


                TempData["msg"] = "Done";
            }
            catch
            {
                TempData["msg"] = "Failed";
            }

            return RedirectToAction("Envoyer");
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        [Route("historique")]
        public IActionResult Historique()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).Where(t => t.EmployeeId == 
            compte.Id).ToList();

            return View("Historique");
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("Liste")]
        public IActionResult Liste()
        {
            ViewBag.tickets = db.Tickets.OrderByDescending(t => t.Id).ToList();

            return View("Liste");
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("Attribuer")]
        public IActionResult Attribuer()
        {
            ViewBag.tickets = db.Tickets.Where(t => t.Supporter == null).OrderByDescending(t => t.Id).ToList();

            return View("Attribuer");
        }


        [Authorize(Roles = "Administrateur")]
        [HttpPost]
        [Route("Attribuer")]
        public IActionResult Attribuer(int id, int supporterId)
        {
            var ticket = db.Tickets.Find(id);
            ticket.SupporterId = supporterId;
            db.SaveChanges();
            return RedirectToAction("Liste");
        }

        //Les tickets de chargement sont attribués au supporter
        [Authorize(Roles = "Support")]
        [HttpGet]
        [Route("Assigned")]
        public IActionResult Assigned()
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
            ViewBag.tickets = db.Tickets.Where(t => t.SupporterId == compte.Id).OrderByDescending(t => t.Id).ToList();

            return View("Assigned");
        }


        [Authorize(Roles = "Administrateur,Support,Employee")]
        [HttpGet]
        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            ViewBag.ticket = db.Tickets.Find(id);
            ViewBag.Supporters = db.Comptes.Where(a => a.RoleId == 2 && a.Statut == true).ToList();
            ViewBag.discussions = db.Discussions.Where(d => d.TickeId == id).OrderBy(d => d.Id).ToList();
            return View("Details");
        }

        [Authorize(Roles = "Support,Employee")]
        [HttpPost]
        [Route("envoyer_discussion")]
        public IActionResult EnvoyerDiscussion(int ticketId, string message)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));

            var discussion = new Discussion();
            discussion.CreerDate = DateTime.Now;
            discussion.Contenu = message;
            discussion.TickeId = ticketId;
            discussion.CompteId = compte.Id;
            db.Discussions.Add(discussion);
            db.SaveChanges();
            return RedirectToAction("Details",new { id = ticketId});
        }

    }
}

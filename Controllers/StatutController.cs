using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ProjetLaVictoireDesServices.Controllers
{
    [Authorize(Roles = "Administrateur")]
    [Route("statut")]
    public class StatutController : Controller
    {

        private readonly HelpDeskVictoireDBEntities db;

        public StatutController(HelpDeskVictoireDBEntities _db)
        {
            this.db = _db;
        }

        
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.statutes = db.Statuts.ToList();
            return View("Index");
        }

        [HttpGet]
        [Route("ajouter")]
        public IActionResult Ajouter()
        {
            return View("Ajouter", new Statut());
        }

        [HttpPost]
        [Route("ajouter")]
        public IActionResult Ajouter(Statut statut)
        {
            try
            {
                db.Statuts.Add(statut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Ajouter", new Statut());
            }
            
        }

        [HttpGet]
        [Route("supprimer/{id}")]
        public IActionResult Supprimer(int id)
        {
            try
            {
                var statut = db.Statuts.Find(id);
                db.Statuts.Remove(statut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                ViewBag.msg = "Failed";
                ViewBag.statutes = db.Statuts.ToList();
                return View("Index");
            }
            
        }

        [HttpGet]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id)
        {
                var statut = db.Statuts.Find(id);
               
                return View("Modifier", statut);
            
        }

        [HttpPost]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id, Statut statut)
        {
            try
            {
                db.Entry(statut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Modifier", statut);
            }

        }

    }
}

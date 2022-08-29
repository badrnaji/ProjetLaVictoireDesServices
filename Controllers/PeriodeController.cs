using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ProjetLaVictoireDesServices.Controllers
{
    [Authorize(Roles = "Administrateur")]
    [Route("periode")]
    public class PeriodeController : Controller
    {

        private readonly HelpDeskVictoireDBEntities db;

        public PeriodeController(HelpDeskVictoireDBEntities _db)
        {
            this.db = _db;
        }

        
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.periodes = db.Periodes.ToList();
            return View("Index");
        }

        [HttpGet]
        [Route("ajouter")]
        public IActionResult Ajouter()
        {
            return View("Ajouter", new Periode());
        }

        [HttpPost]
        [Route("ajouter")]
        public IActionResult Ajouter(Periode periode)
        {
            try
            {
                db.Periodes.Add(periode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Ajouter", new Periode());
            }
            
        }

        [HttpGet]
        [Route("supprimer/{id}")]
        public IActionResult Supprimer(int id)
        {
            try
            {
                var periode = db.Periodes.Find(id);
                db.Periodes.Remove(periode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                ViewBag.msg = "Failed";
                ViewBag.periodes = db.Periodes.ToList();
                return View("Index");
            }
            
        }

        [HttpGet]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id)
        {
                var periode = db.Periodes.Find(id);
               
                return View("Modifier", periode);
            
        }

        [HttpPost]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id, Periode periode)
        {
            try
            {
                db.Entry(periode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Modifier", periode);
            }

        }

    }
}

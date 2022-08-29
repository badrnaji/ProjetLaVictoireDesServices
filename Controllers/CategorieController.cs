using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ProjetLaVictoireDesServices.Controllers
{
    [Authorize(Roles = "Administrateur")]
    [Route("categorie")]
    public class CategorieController : Controller
    {

        private readonly HelpDeskVictoireDBEntities db;

        public CategorieController(HelpDeskVictoireDBEntities _db)
        {
            this.db = _db;
        }

        
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.categories = db.Categories.ToList();
            return View("Index");
        }

        [HttpGet]
        [Route("ajouter")]
        public IActionResult Ajouter()
        {
            return View("Ajouter", new Category());
        }

        [HttpPost]
        [Route("ajouter")]
        public IActionResult Ajouter(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Ajouter", new Category());
            }
            
        }

        [HttpGet]
        [Route("supprimer/{id}")]
        public IActionResult Supprimer(int id)
        {
            try
            {
                var categorie = db.Categories.Find(id);
                db.Categories.Remove(categorie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                ViewBag.msg = "Failed";
                ViewBag.categories = db.Categories.ToList();
                return View("Index");
            }
            
        }

        [HttpGet]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id)
        {
                var categorie = db.Categories.Find(id);
               
                return View("Modifier", categorie);
            
        }

        [HttpPost]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id, Category category)
        {
            try
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.msg = "Failed";
                return View("Modifier", category);
            }

        }

    }
}

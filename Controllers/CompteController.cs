using Microsoft.AspNetCore.Mvc;
using ProjetLaVictoireDesServices.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProjetLaVictoireDesServices.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProjetLaVictoireDesServices.Controllers
{
    [Route("compte")]
    public class CompteController : Controller
    {

        private readonly HelpDeskVictoireDBEntities db;

        public CompteController(HelpDeskVictoireDBEntities _db)
        {
            this.db = _db;
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.comptes = db.Comptes.Where(a => a.RoleId !=1).ToList();
            return View("Index");
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("ajouter")]
        public IActionResult Ajouter()
        {
            var compteViewModel = new CompteViewModel();
            compteViewModel.Compte = new Compte();
            var Roles = db.Roles.Where(r => r.Id != 1).ToList();
            compteViewModel.Roles = new SelectList(Roles, "Id", "Nom");
            return View("Ajouter", compteViewModel);
        }

        [Authorize(Roles = "Administrateur")]
        [HttpPost]
        [Route("ajouter")]
        public IActionResult Ajouter(CompteViewModel compteViewModel)
        {
            try
            {
                compteViewModel.Compte.Password = BCrypt.Net.BCrypt.HashPassword(compteViewModel.Compte.Password,
                        BCrypt.Net.BCrypt.GenerateSalt());
                db.Comptes.Add(compteViewModel.Compte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                ViewBag.msg = "Failed";
                return View("Ajouter", compteViewModel);
            }
        }

        [Authorize(Roles = "Administrateur")]
        [Route("supprimer/{id}")]
        public IActionResult Supprimer(int id)
        {
            try
            {
                var compte = db.Comptes.SingleOrDefault(a => a.Id == id && a.RoleId != 1);
                db.Comptes.Remove(compte);
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch
            {
                ViewBag.msg = "Failed";
            }
            ViewBag.comptes = db.Comptes.Where(a => a.RoleId != 1).ToList();
            return View("Index");
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id)
        {

            var compteViewModel = new CompteViewModel();
            compteViewModel.Compte = db.Comptes.Find(id);
            var Roles = db.Roles.Where(r => r.Id != 1).ToList();
            compteViewModel.Roles = new SelectList(Roles, "Id", "Nom");
            return View("Modifier", compteViewModel);
        }

        [Authorize(Roles = "Administrateur")]
        [HttpPost]
        [Route("modifier/{id}")]
        public IActionResult Modifier(int id,CompteViewModel compteViewModel)
        {
            try
            {
                var password = db.Comptes.AsNoTracking().SingleOrDefault(a => a.Id == id).Password;
                if (!string.IsNullOrEmpty(compteViewModel.Compte.Password))
                {
                    password = BCrypt.Net.BCrypt.HashPassword(compteViewModel.Compte.Password,
                        BCrypt.Net.BCrypt.GenerateSalt());
                }
                compteViewModel.Compte.Password = password;
                db.Entry(compteViewModel.Compte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.msg = "Failed";
                return View("Modifier", compteViewModel);
            }
        }

        [Authorize(Roles = "Administrateur,Support,Employee")]
        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {

            var username = User.FindFirst(ClaimTypes.Name).Value;
            var compte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
            return View("Profile", compte);
        }

        [Authorize(Roles = "Administrateur,Support,Employee")]
        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Compte compte)
        {

            var username = User.FindFirst(ClaimTypes.Name).Value;
            var currentCompte = db.Comptes.SingleOrDefault(a => a.NomUtilisateur.Equals(username));
            try
            {
                
                currentCompte.NomUtilisateur = compte.NomUtilisateur;
                if (!string.IsNullOrEmpty(compte.Password))
                {
                    currentCompte.Password = BCrypt.Net.BCrypt.HashPassword(compte.Password,
                        BCrypt.Net.BCrypt.GenerateSalt());
                }
                currentCompte.NomComplet = compte.NomComplet;
                currentCompte.Email = compte.Email;
                db.SaveChanges();
                ViewBag.msg = "Done";
            }
            catch (Exception e)
            {

                ViewBag.msg = "Failed";
            }
            return View("Profile", currentCompte);
        }

    }
}

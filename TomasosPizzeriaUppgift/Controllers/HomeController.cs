using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;

namespace TomasosPizzeriaUppgift.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult RegisterPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterUser()
        {
            return View();
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserLogginValidation()
        {
            return View();
        }
        public IActionResult MenuPage()
        {
            var model = new MenuPage();

            using (TomasosContext db = new TomasosContext())
            {
                 model.Matratter = db.Matratt.ToList();
                model.Ingredins.MatrattProdukt = db.MatrattProdukt.ToList();
                model.Ingredienses = db.Produkt.ToList();
                model.mattratttyper = db.MatrattTyp.ToList();

            }
             
            return View(model);
        }
        public IActionResult CustomerBasket(int id)
        {
            var model = new MenuPage();
            using (TomasosContext db = new TomasosContext())
            {
               model.matratt = db.Matratt.FirstOrDefault(r => r.MatrattId == id);
                model.Matratter = db.Matratt.ToList();
                model.Ingredins.MatrattProdukt = db.MatrattProdukt.ToList();
                model.Ingredienses = db.Produkt.ToList();
                model.mattratttyper = db.MatrattTyp.ToList();


            }
            return PartialView("_MenuPage",model);
        }


    }
}

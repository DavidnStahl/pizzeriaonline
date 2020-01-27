using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomasosPizzeriaUppgift.Models;
using TomasosPizzeriaUppgift.ViewModels;
using TomasosPizzeriaUppgift.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TomasosPizzeriaUppgift.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult HomePage()
        {
            return View();
        }

        public IActionResult MenuPage()
        {
            var model = Services.Services.Instance.GetMenuInfo();
            var id = Convert.ToInt32(Request.Cookies["cookie_customer"]);
            if (id != 0)
            {
                var matratteradded = GetMatratterCacheList(id, "2");
                matratteradded.Add(model.matratt);
                model.Matratteradded = matratteradded;
                model.mattratttyper = Services.Services.Instance.GetMatratttyper();
                return View(model);
            }
            else
            {
                return RedirectToAction("LoginPage");
            }
            
            
        }
        public IActionResult RegisterPage()
        {
            ViewBag.Message = "Fyll i personlig information";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(Kund user)
        {
            var kund = Services.Services.Instance.CheckUserName(user);
            if (ModelState.IsValid && kund.AnvandarNamn == null)
            {
                Services.Services.Instance.SaveUser(user);
                ModelState.Clear();
                return RedirectToAction("LoginPage");
            }
            else
            {
                ViewBag.Message = "Användarnamn upptaget";
                return View(nameof(RegisterPage));
            }
        }
        
       
        public ActionResult CustomerInfoPage()
        {

            var id = GetCustomerCache();
            if (id == 0)
            {
                return RedirectToAction("LoginPage");
            }
            var customer = Services.Services.Instance.GetById(id);
            ViewBag.Message = "Din personliga information";
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(Kund user)
        {
            var kund = Services.Services.Instance.CheckUserName(user);
            var id = GetCustomerCache();
            var customer = Services.Services.Instance.GetById(id);

            if (ModelState.IsValid)
            {
                ModelState.Clear();
                Services.Services.Instance.UpdateUser(user,id);
                return RedirectToAction("CustomerInfoPage");
            }
            else
            {
                ViewBag.Message = "Användarnamn upptaget";
                return View(nameof(CustomerInfoPage));
            }
        }
        public IActionResult LoginPage()
        {
            ViewBag.Message = "Var vänlig logga in";
            return View();
        }
        public IActionResult LogOut()
        {
            ResetCookie();
            return RedirectToAction("LoginPage");
        }
        [HttpPost]
        public IActionResult UserLogginValidation(Kund customer)
        {

            var kund = Services.Services.Instance.GetUserId(customer);
            if (kund != null)
            {
                SetCustomerCache(kund);
                return RedirectToAction("MenuPage");
            }
            else
            {
                ViewBag.Message = "Fel inlogg försök igen";
                return View(nameof(LoginPage));
            };
        }

        public ActionResult RemoveItemCustomerBasket(int id, int count)
        {
            var matratteradded = GetMatratterCacheList(id,"2");
            matratteradded.RemoveAt(count);
            var menumodel = SetMatratterCacheList(matratteradded);
            return PartialView("MenuPage", menumodel); 
        }

        public ActionResult CustomerBasket(int id)
        {
            var matratteradded = GetMatratterCacheList(id,"1");
            var menumodel = SetMatratterCacheList(matratteradded);
            menumodel.mattratttyper = Services.Services.Instance.GetMatratttyper();
            return PartialView("MenuPage", menumodel);
        }
        public ActionResult PaymentLoggin()
        {
            ViewBag.Message = "Logga in, för att betala";
            return View();
        }
        [HttpPost]
        public ActionResult PaymentLogginValidation(Kund customer)
        {
            var cust = Services.Services.Instance.GetUserId(customer);
            var cacheid = GetCustomerCache();
            if(cacheid == cust.KundId)
            {
                return RedirectToAction("PayPage");
            }
            else
            {
                ViewBag.Message = "Fel inlogg försök igen";
                return View(nameof(PaymentLoggin));
            };
        }
        public ActionResult PayPage()
        {
            var jsonget = Request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            var model = new MenuPage();
            model.Matratteradded = matratteradded;
            model.mattratttyper = Services.Services.Instance.GetMatratttyper();
            return View(model);
        }
        public ActionResult PayUser()
        {
            var userid = GetCustomerCache();
            var jsonget = Request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            Services.Services.Instance.UserPay(matratteradded, userid);
            ResetCookie();
            return View();
        }


        public int GetCustomerCache()
        {
            var id = Convert.ToInt32(Request.Cookies["cookie_customer"]);
            return id;
        }
        public void SetCustomerCache(Kund kund)
        {
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookieKey);
            }
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);
            options.HttpOnly = true;
            Response.Cookies.Append("cookie_customer", kund.KundId.ToString(), options);
        }
        public List<Matratt> GetMatratterCacheList(int id, string options)
        {
            var model = Services.Services.Instance.GetMatratterById(id);
            var jsonget = Request.Cookies["cookie_matratter"];
            var matratteradded = new List<Matratt>();
            if (jsonget != null)
            {
                matratteradded = JsonConvert.DeserializeObject<List<Matratt>>(jsonget);
            }
            if(options == "1")
            {
                matratteradded.Add(model);
            }
            return matratteradded;
        }
        public MenuPage SetMatratterCacheList(List<Matratt> matratteradded)
        {
            string json = JsonConvert.SerializeObject(matratteradded, Formatting.Indented);
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);
            options.HttpOnly = true;
            Response.Cookies.Append("cookie_matratter", json, options);

            var menumodel = Services.Services.Instance.GetMenuInfo();
            menumodel.Matratteradded = matratteradded;

            return menumodel;
        }
        public void ResetCookie()
        {
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookieKey);
            }
        }


    }
}

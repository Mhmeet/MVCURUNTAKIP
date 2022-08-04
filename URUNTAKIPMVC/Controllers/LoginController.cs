using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace URUNTAKIPMVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        const string kullaniciadi = "admin";
        const string PASSWORD = "1234";
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string pass)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pass))
            {
                ViewBag.message = "Boşlukları Doldurunuz!";
                return View();
            }
            else if (kullaniciadi == username && PASSWORD == pass)
            {
                Session["ID"] = username;
                return RedirectToAction("Anasayfa", "Admin");
            }
            else
            {
                ViewBag.message = "Tekrar Deneyiniz";
                return View();
            }

        }
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}
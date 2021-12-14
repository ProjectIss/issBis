using Bis.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bis.Controllers
{
    public class AuthController : Controller
    {
        private BISModel db = new BISModel();
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login(string ReturnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var enUser = db.Users.FirstOrDefault(x => x.username == loginView.username && x.password == loginView.password);
                if (enUser != null)
                {
                    var enEmployee = db.Employees.FirstOrDefault(x => x.employeeId == enUser.username);
                    CustomSerializeModel userModel = new Models.CustomSerializeModel()
                    {
                        id = enUser.id,
                        name = enUser.name,
                        email = enEmployee != null ? enEmployee.email : string.Empty,
                        role = enUser.role,
                        EId = enEmployee != null ? enEmployee.id : 0
                    };
                    string userData = JsonConvert.SerializeObject(userModel);
                    FormsAuthenticationTicket authTicket =
                        new FormsAuthenticationTicket(1, loginView.username, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);

                    string enTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie("biscookie", enTicket);
                    Response.Cookies.Add(faCookie);
                }

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Something Wrong : Username or Password invalid ^_^ ");
            return View(loginView);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("biscookie", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth", null);
        }
    }
}
using Bis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bis.Controllers
{
    public class SeedController : Controller
    {
        private BISModel db = new BISModel();
        // GET: Seed
        public ActionResult Index()
        {
            User enUser = db.Users.FirstOrDefault(x => x.name == "Bis Administrator");
            if(enUser == null)
            {
                enUser = new User()
                {
                    name = "Bis Administrator",
                    username = "admin",
                    password = "1234",
                    role = "Admin",
                    status = "Active",
                };
                db.Users.Add(enUser);
                db.SaveChanges();
            }
            return View();
        }
    }
}
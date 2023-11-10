
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class DangnhapController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            ADMIN aDMIN = new ADMIN();
            return View(aDMIN);
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Login(ADMIN aDMIN)
        {
            QLSVEntities QLSVEntities = new QLSVEntities();
            var status = QLSVEntities.ADMINs.FirstOrDefault(m => m.UserName == aDMIN.UserName && m.Password == aDMIN.Password);
            if (status == null)
            {
                ViewBag.LoginFail = "Sai tài khoản hoặc mật khẩu!";
                return View("Login");
            }
            else
            {
                return RedirectToAction("DashBoard","Home");
            }
            return View(); ;
        }


    }
}

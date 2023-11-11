using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.App_Start;
using test3.Models;

namespace test3.Controllers
{
    namespace test3.Controllers
    {
        public class HomeController : Controller
        {
            
            // GET: QLSV
            QLSVController db = new QLSVController();
            [Role_User]
            public ActionResult DashBoard()
            {
                
                return View(/*danhSachTinTuc*/); 
                /*List<TinTuc> danhSachTinTuc = db.TinTucs.ToList();*/
                
            }
            public ActionResult LoiPhanQuyen() {
                return View();
            }
            /*public ActionResult Xoa(int id)
            {
                QLSVController db = new QLSVController();
                var tinTuc = db.TinTucs.Find(id);
                db.TinTucs.Remove(tinTuc);
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }*/

        }
    }
}
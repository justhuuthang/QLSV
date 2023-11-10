using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    namespace test3.Controllers
    {
        public class HomeController : Controller
        {
            // GET: QLSV
            QLSVEntities db = new QLSVEntities();
            public ActionResult DashBoard()
            {
                List<TinTuc> danhSachTinTuc = db.TinTucs.ToList();
                return View(danhSachTinTuc);
            }
            public ActionResult Xoa(int id)
            {
                QLSVEntities db = new QLSVEntities();
                var tinTuc = db.TinTucs.Find(id);
                db.TinTucs.Remove(tinTuc);
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }

        }
    }
}
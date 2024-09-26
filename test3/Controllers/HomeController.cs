using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;

namespace test3.Controllers
{
    public class HomeController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DashBoard(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var thongBao = db.Notifications.ToList();
            return View(thongBao.ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult LoiPhanQuyen() {
                return View();
            }

            [HttpGet]
            public ActionResult ThemMoiThongBao()
            {
                return View();
            }

            [HttpPost]
            public ActionResult ThemMoiThongBao(Notification thongBao)
            {
                QuanliSVEntities db = new QuanliSVEntities();
                db.Notifications.Add(thongBao);
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }

        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DashBoard");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var lop = db.Notifications.Find(id);

            if (lop == null)
            {
                return RedirectToAction("DashBoard");
            }

            return View(lop);
        }

        [HttpPost]
        public ActionResult Suathongtin(Notification lop, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingClass = db.Notifications.Find(lop.NotificationID);

            if (existingClass == null)
            {
                return RedirectToAction("DashBoard");
            }

            if (action == "Xóa")
            {
                db.Notifications.Remove(existingClass);
                db.SaveChanges();

                return RedirectToAction("DashBoard");
            }
            else if (action == "Sửa")
            {
                existingClass.NotificationName = lop.NotificationName;
                existingClass.NotificationContent = lop.NotificationContent;
                existingClass.Date = lop.Date;

                db.SaveChanges();

                return RedirectToAction("DashBoard");
            }

            return View(lop);
        }

    }
    }
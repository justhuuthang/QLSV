using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;

namespace test3.Controllers
{
    public class QLLController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DanhSachLop(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var lop = db.Classes.ToList();
            return View(lop.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoiLop()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiLop(Class lop)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            db.Classes.Add(lop);
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }

        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachLop");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var lop = db.Classes.Find(id);

            if (lop == null)
            {
                return RedirectToAction("DanhSachLop");
            }

            return View(lop);
        }

        [HttpPost]
        public ActionResult Suathongtin(Class lop, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingClass = db.Classes.Find(lop.ClassID);

            if (existingClass == null)
            {
                return RedirectToAction("DanhSachLop");
            }

            if (action == "Xóa")
            {
                db.Classes.Remove(existingClass);
                db.SaveChanges();

                return RedirectToAction("DanhSachLop");
            }
            else if (action == "Sửa")
            {
                existingClass.ClassID = lop.ClassID;
                existingClass.ClassName = lop.ClassName;
                existingClass.StartDate = lop.StartDate;
                existingClass.EndDate = lop.EndDate;
                existingClass.HeadTeacher = lop.HeadTeacher;
                existingClass.MaxStudents = lop.MaxStudents;

                db.SaveChanges();

                return RedirectToAction("DanhSachLop");
            }

            return View(lop);
        }
    }
}

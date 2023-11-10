using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class QLLController : Controller
    {
        // GET: QLL
        QLSVEntities db = new QLSVEntities();
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
            var lop = db.Lops.ToList();
            return View(lop.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoiLop()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            // Tìm kiếm sinh viên dựa trên MaLop và MaKhoa
            List<Lop> searchResults = db.Lops
              .Where(s => s.MaLop.StartsWith(search) || s.MaKhoa.StartsWith(search))
              .ToList();


            // Trả về view hiển thị danh sách sinh viên kết quả
            return View("DanhSachLop", searchResults);
        }

        [HttpPost]
        public ActionResult ThemMoiLop(Lop lop)
        {
            QLSVEntities db = new QLSVEntities();
            db.Lops.Add(lop);
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
        public ActionResult Xoa(string maLop)
        {
            QLSVEntities db = new QLSVEntities();
            var lop = db.Lops.Find(maLop);
            db.Lops.Remove(lop);
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
        [HttpGet]
        public ActionResult Suathongtin(string maLop)
        {
            QLSVEntities db = new QLSVEntities();
            var lop = db.Lops.Find(maLop);
            return View(lop);
        }
        [HttpPost]
        public ActionResult Suathongtin(Lop lop)
        {
            db.Entry(lop).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
    }
}
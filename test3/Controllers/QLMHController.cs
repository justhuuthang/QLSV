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
    public class QLMHController : Controller
    {
        // GET: QLMH
        QLSVEntities db = new QLSVEntities();
        public ActionResult DanhSachMonHoc(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var monHoc = db.MonHocs.ToList();
            return View(monHoc.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchMaMon)
        {
            List<MonHoc> searchmamon = db.MonHocs.Where(s => s.MaMon == searchMaMon).ToList();
            return View("DanhSachMonHoc", searchMaMon);
        }
        [HttpGet]
        public ActionResult ThemMonHoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMonHoc(MonHoc monHoc)
        {
            if (string.IsNullOrEmpty(monHoc.MaMon))
            {
                ViewBag.MaMonError = "Yêu cầu nhập Mã Môn.";
            }
            if (string.IsNullOrEmpty(monHoc.TenMon))
            {
                ViewBag.TenMonError = "Yêu cầu nhập Tên Môn.";
            }
            if (string.IsNullOrEmpty(monHoc.MaKhoa))
            {
                ViewBag.MaKhoaError = "Yêu cầu chọn Mã Khoa.";
            }

            if (!string.IsNullOrEmpty(monHoc.MaMon) && !string.IsNullOrEmpty(monHoc.TenMon) && !string.IsNullOrEmpty(monHoc.MaKhoa))
            {
                QLSVEntities db = new QLSVEntities();
                db.MonHocs.Add(monHoc);
                db.SaveChanges();
                return RedirectToAction("DanhSachMonHoc");
            }

            return View(monHoc);
        }






        public ActionResult Xoa(string id)
        {
            QLSVEntities db = new QLSVEntities();
            var monHoc = db.MonHocs.Find(id);
            db.MonHocs.Remove(monHoc);
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }

        [HttpGet]
        public ActionResult Suathongtinmonhoc(string id)
        {
            QLSVEntities db = new QLSVEntities();
            var monhoc = db.MonHocs.Find(id);
            return View(monhoc);
        }

        [HttpPost]
        public ActionResult Suathongtinmonhoc(MonHoc monhoc)
        {
            if (string.IsNullOrEmpty(monhoc.MaMon))
            {
                ViewBag.MaMonError = "Yêu cầu nhập Mã Môn.";
            }
            if (string.IsNullOrEmpty(monhoc.TenMon))
            {
                ViewBag.TenMonError = "Yêu cầu nhập Tên Môn.";
            }
            if (string.IsNullOrEmpty(monhoc.MaKhoa))
            {
                ViewBag.MaKhoaError = "Yêu cầu chọn Mã Khoa.";
            }

            if (!string.IsNullOrEmpty(monhoc.MaMon) && !string.IsNullOrEmpty(monhoc.TenMon) && !string.IsNullOrEmpty(monhoc.MaKhoa))
            {
                QLSVEntities db = new QLSVEntities();
                db.Entry(monhoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachMonHoc");
            }

            return View(monhoc);
        }

    }
}
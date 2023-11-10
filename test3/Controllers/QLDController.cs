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
    public class QLDController : Controller
    {
        // GET: QLD
        QLSVEntities db = new QLSVEntities();
        public ActionResult DanhSachBangDiem(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var diem = db.BangDiems.ToList();
            return View(diem.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchMSSV, string searchMaMon,string searchMaLop)
        {
            List<BangDiem> searchResults = new List<BangDiem>();

            if (!string.IsNullOrEmpty(searchMSSV) && !string.IsNullOrEmpty(searchMaMon))
            {
                searchResults = db.BangDiems
                    .Where(s => s.MSSV == searchMSSV && s.MaMon == searchMaMon)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(searchMSSV))
            {
                searchResults = db.BangDiems
                    .Where(s => s.MSSV == searchMSSV)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(searchMaMon))
            {
                searchResults = db.BangDiems
                    .Where(s => s.MaMon == searchMaMon)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(searchMaLop))
            {
                searchResults = db.BangDiems
                    .Where(s => s.MaLop == searchMaLop)
                    .ToList();
            }
            return View("DanhSachBangDiem", searchResults);
        }

        [HttpGet]
        public ActionResult ThemBangDiem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemBangDiem(BangDiem bangdiem)
        {
            QLSVEntities db = new QLSVEntities();
            db.BangDiems.Add(bangdiem);
            db.SaveChanges();
            return RedirectToAction("DanhSachBangDiem");
        }
        public ActionResult Xoa(string MSSV)
        {
            QLSVEntities db = new QLSVEntities();
            var BangDiem = db.BangDiems.Find(MSSV);
            db.BangDiems.Remove(BangDiem);
            db.SaveChanges();
            return RedirectToAction("DanhSachBangDiem");
        }
        [HttpGet]
        public ActionResult SuathongtinBangDiem(string bangDiem)
        {
            QLSVEntities db = new QLSVEntities();
            var BangDiem = db.BangDiems.Find(bangDiem);
            return View(BangDiem);
        }
        [HttpPost]
        public ActionResult SuathongtinBangDiem(BangDiem bangdiem)
        {
            db.Entry(bangdiem).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachBangDiem");
        }
    }
}
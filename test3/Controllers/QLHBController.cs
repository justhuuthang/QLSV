using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Printing;
using System.Net;
using test3.App_Start;
using System.Data.Entity;

namespace test3.Controllers
{
    public class QLHBController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DanhSachHocBong(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var diem = db.Scholarships.ToList();
            return View(diem.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchField, string searchValue, int? page)
        {
            searchValue = searchValue.ToLower();

            List<Scholarship> searchResults = new List<Scholarship>();

            switch (searchField)
            {   
                case "ScholarshipName":
                    searchResults = db.Scholarships.Where(s => s.ScholarshipName.ToLower().Contains(searchValue)).ToList();
                    break;     
            }
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Scholarship> pagedResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachHocBong", pagedResults);
        }
        [Role_User]
        [HttpGet]
        public ActionResult ThemHocBong()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemHocBong(Scholarship hocBong)
        {
            string scholarshipName = Request["ScholarshipName"];

            var existingScholarship = db.Scholarships.FirstOrDefault(s => s.ScholarshipName == scholarshipName);

            if (existingScholarship != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Đã có học bổng này.");
            }

            db.Scholarships.Add(hocBong);
            db.SaveChanges();
            return RedirectToAction("DanhSachHocBong");
        }
        public ActionResult Xoa(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var hocBong = db.Scholarships.Find(id);
            db.Scholarships.Remove(hocBong);
            db.SaveChanges();
            return RedirectToAction("DanhSachHocBong");
        }
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachHocBong");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var hocBong = db.Scholarships.Find(id);

            if (hocBong == null)
            {
                return RedirectToAction("DanhSachHocBong");
            }
            return View(hocBong);
        }

        [HttpPost]
        public ActionResult Suathongtin(Scholarship hocBong)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            string scholarshipName = Request["ScholarshipName"];

            var existingScholarship = db.Scholarships.FirstOrDefault(s => s.ScholarshipName == scholarshipName);

            if (existingScholarship != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Học bổng này đã tồn tại.");
            }
            db.Entry(hocBong).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sửa thông tin học bổng thành công.";
            return RedirectToAction("DanhSachHocBong");
        }
    }
}

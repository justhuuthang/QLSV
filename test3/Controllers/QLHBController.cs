using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Printing;

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
                case "ScholarshipID":
                    if (int.TryParse(searchValue, out int scholarshipID))
                    {
                        searchResults = db.Scholarships.Where(s => s.ScholarshipID == scholarshipID).ToList();
                    }
                    break;
                
            }
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Scholarship> pagedResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachHocBong", pagedResults);
        }
        [HttpGet]
        public ActionResult ThemHocBong()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemHocBong(Scholarship hocBong)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            db.Scholarships.Add(hocBong);
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
        public ActionResult Suathongtin(Scholarship hocBong, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingScholarship = db.Scholarships.Find(hocBong.ScholarshipID);

            if (existingScholarship == null)
            {
                return RedirectToAction("DanhSachHocBong");
            }

            if (action == "Xóa")
            {
                db.Scholarships.Remove(existingScholarship);
                db.SaveChanges();

                return RedirectToAction("DanhSachHocBong");
            }
            else if (action == "Sửa")
            {
                existingScholarship.ScholarshipID = hocBong.ScholarshipID;
                existingScholarship.ScholarshipName = hocBong.ScholarshipName;
                existingScholarship.Description = hocBong.Description;
                existingScholarship.StartDate = hocBong.StartDate;
                existingScholarship.EndDate = hocBong.EndDate;
                existingScholarship.Conditions = hocBong.Conditions;

                db.SaveChanges();

                return RedirectToAction("DanhSachHocBong");
            }

            return View(hocBong);
        }
    }
}

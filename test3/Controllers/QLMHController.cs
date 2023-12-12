using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using System.Data.Entity.Validation;
using test3.App_Start;
using System.Net;
using DocumentFormat.OpenXml.Office2010.Excel;
using ClosedXML.Excel;
using System.IO;

namespace test3.Controllers
{
    public class QLMHController : Controller
    {
        QuanliSVEntities db = new QuanliSVEntities();
        /*[Role_User(FunctionID = "Admin_XemDanhSach")]*/
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
            var monHoc = db.Courses.Include(c => c.Department).Include(c => c.Semester).ToList();
            return View(monHoc.ToPagedList((int)page, (int)pageSize));
        }
        /*[Role_User(FunctionID = "Admin_XemDanhSach")]*/
        [HttpGet]
        public ActionResult Search(string searchField, string searchValue, int? page)
        {
            List<Cours> searchResults = new List<Cours>();

            switch (searchField)
            {
                case "CourseName":
                    searchResults = db.Courses
                        .Where(s => s.CourseName.Contains(searchValue))
                        .ToList();
                    break;

                case "SemesterName":
                    searchResults = db.Courses
                        .Where(s => s.Semester.SemesterName.Contains(searchValue))
                        .ToList();
                    break;

                case "DepartmentName":
                    searchResults = db.Courses
                        .Where(s => s.Department.DepartmentName.Contains(searchValue))
                        .ToList();
                    break;

                default:
                    break;
            }

            TempData["SearchResults"] = searchResults;
            TempData["SearchField"] = searchField;
            TempData["SearchValue"] = searchValue;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IPagedList<Cours> pagedSearchResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachMonHoc", pagedSearchResults);
        }

        /*[Role_User(FunctionID = "Admin_XemDanhSach")]*/
        [HttpGet]
        public ActionResult ThemMoiMonHoc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMoiMonHoc(Cours monHoc)
        {
            string courseName = Request["CourseName"];

            var existingCours = db.Courses.FirstOrDefault(s => s.CourseName == courseName);

            if (existingCours != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Môn học đã tồn tại .");
            }

            db.Courses.Add(monHoc);
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }
        /*[Role_User(FunctionID = "Admin_XemDanhSach")]*/
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachMonHoc");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var monHoc = db.Courses.Find(id);

            if (monHoc == null)
            {
                return RedirectToAction("DanhSachMonHoc");
            }
            return View(monHoc);
        }
        [HttpPost]
        public ActionResult Suathongtin(Cours monHoc)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingCourse = db.Courses.FirstOrDefault(s => s.CourseID != monHoc.CourseID && s.CourseName == monHoc.CourseName);

            if (existingCourse != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Môn học đã tồn tại.");
            }

            db.Entry(monHoc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }
        public ActionResult Xoa(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var monHoc = db.Courses.Find(id);
            db.Courses.Remove(monHoc);
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }
   


    }
}
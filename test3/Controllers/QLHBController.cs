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
            var diem = db.Grades.ToList();
            return View(diem.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchField, string searchValue, int? page)
        {
            searchValue = searchValue.ToLower();

            List<Grade> searchResults = new List<Grade>();

            switch (searchField)
            {
                case "StudentID":
                    if (int.TryParse(searchValue, out int studentID))
                    {
                        searchResults = db.Grades.Where(s => s.StudentID == studentID).ToList();
                    }
                    break;
                case "CourseID":
                    if (int.TryParse(searchValue, out int courseID))
                    {
                        searchResults = db.Grades.Where(s => s.CourseID == courseID).ToList();
                    }
                    break;
                case "ExamDate":
                    if (DateTime.TryParse(searchValue, out DateTime dob))
                    {
                        searchResults = db.Grades.Where(s => s.ExamDate == dob).ToList();
                    }
                    break;
                case "SemesterID":
                    if (int.TryParse(searchValue, out int semesterID))
                    {
                        searchResults = db.Grades.Where(s => s.SemesterID == semesterID).ToList();
                    }
                    break;
                default:
                    break;
            }
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Grade> pagedResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachDiem", pagedResults);
        }
        [HttpGet]
        public ActionResult ThemHocBong()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemHocBong(Grade diem)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            db.Grades.Add(diem);
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
            var diem = db.Grades.Find(id);

            if (diem == null)
            {
                return RedirectToAction("DanhSachHocBong");
            }

            return View(diem);
        }

        [HttpPost]
        public ActionResult Suathongtin(Grade diem, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingGrade = db.Grades.Find(diem.GradeID);

            if (existingGrade == null)
            {
                return RedirectToAction("DanhSachDiem");
            }

            if (action == "Xóa")
            {
                db.Grades.Remove(existingGrade);
                db.SaveChanges();

                return RedirectToAction("DanhSachDiem");
            }
            else if (action == "Sửa")
            {
                existingGrade.StudentID = diem.StudentID;
                existingGrade.CourseID = diem.CourseID;
                existingGrade.ExamDate = diem.ExamDate;
                existingGrade.SemesterID = diem.SemesterID;
                existingGrade.ScoreScale10 = diem.ScoreScale10;
                existingGrade.ScoreScale4 = diem.ScoreScale4;
                existingGrade.LetterGrade = diem.LetterGrade;

                db.SaveChanges();

                return RedirectToAction("DanhSachDiem");
            }

            return View(diem);
        }
    }
}

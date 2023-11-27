using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using System.Data.Entity;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing.Printing;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Net;

namespace test3.Controllers
{
    public class QLDController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DanhSachDiem(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }

            var diem = db.Grades.Include(c => c.Cours).Include(c => c.Semester).ToList();
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
        public ActionResult ThemMoiDiem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiDiem(Grade diem)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            db.Grades.Add(diem);
            db.SaveChanges();
            return RedirectToAction("DanhSachDiem");
        }

        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachDiem");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var diem = db.Grades.Include(c => c.Cours).Include(c => c.Semester).FirstOrDefault(c => c.GradeID == id);

            if (diem == null)
            {
                return RedirectToAction("DanhSachDiem");
            }

            return View(diem);
        }

        [HttpPost]
        public ActionResult Suathongtin(List<Grade> updatedData)
        {
            try
            {
                foreach (var data in updatedData)
                {
                    var existingData = db.Grades.FirstOrDefault(d => d.StudentID == data.StudentID && d.CourseID == data.CourseID);

                    if (existingData != null)
                    {
                        existingData.ScoreScale10 = data.ScoreScale10;
                        existingData.ScoreScale4 = data.ScoreScale4;
                        existingData.LetterGrade = data.LetterGrade;

                    }
                }
                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật dữ liệu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật dữ liệu: " + ex.Message });
            }
        }

    }
}

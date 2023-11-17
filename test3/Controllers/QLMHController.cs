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

namespace test3.Controllers
{
    public class QLMHController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();

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




        [HttpGet]
        public ActionResult ThemMoiMonHoc()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ThemMoiMonHoc(Cours monHoc)
        {
            // Kiểm tra xem CourseName đã tồn tại chưa
            if (db.Courses.Any(c => c.CourseName == monHoc.CourseName))
            {
                return Json(new { success = false, message = "Tên môn học đã tồn tại. Vui lòng chọn tên khác." });
            }

            // Nếu không có lỗi, thêm môn học vào cơ sở dữ liệu
            db.Courses.Add(monHoc);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm môn học thành công." });
        }










        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachMonHoc");
            }
            QuanliSVEntities db = new QuanliSVEntities();
            var monHoc = db.Courses.Include(c => c.Department).Include(c => c.Semester).FirstOrDefault(c => c.CourseID == id);

            if (monHoc == null)
            {
                return RedirectToAction("DanhSachMonHoc");
            }


            return View(monHoc);
        }
        [HttpPost]
        public ActionResult Suathongtin(Cours monHoc, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingCours = db.Courses.Find(monHoc.CourseID);

            if (existingCours == null)
            {
                return RedirectToAction("DanhSachMonHoc");
            }

            if (action == "Xóa")
            {
                db.Courses.Remove(existingCours);
                db.SaveChanges();
                return RedirectToAction("DanhSachMonHoc");
            }
            else if (action == "Sửa")
            {
                // Kiểm tra xem CourseName đã tồn tại chưa (trừ môn học hiện tại)
                if (db.Courses.Any(c => c.CourseName == monHoc.CourseName && c.CourseID != monHoc.CourseID))
                {
                    ViewBag.Error = "Tên môn học đã tồn tại. Vui lòng chọn tên khác.";
                    return View(monHoc);
                }

                // Cập nhật thông tin môn học
                existingCours.CourseName = monHoc.CourseName;
                existingCours.Description = monHoc.Description;
                existingCours.Credits = monHoc.Credits;
                existingCours.DepartmentID = monHoc.DepartmentID;
                existingCours.SemesterID = monHoc.SemesterID;
                existingCours.ClassID = monHoc.ClassID;

                db.SaveChanges();

                return RedirectToAction("DanhSachMonHoc");
            }

            return View(monHoc);
        }








    }
}

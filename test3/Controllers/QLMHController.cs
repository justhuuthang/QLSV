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
        [Role_User(FunctionID = "Admin_XemDanhSach")]
        public ActionResult ListSubjects(int? page, int? pageSize)
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



        [Role_User]
        [HttpGet]
        public ActionResult AddNewSubject()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddNewSubject(Cours monHoc)
        {
            string courseName = Request["CourseName"];

            var existingStudent = db.Courses.FirstOrDefault(c => c.CourseName == courseName);

            if (existingStudent != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Môn học đã tồn tại với tên này.");
            }

            db.Courses.Add(monHoc);
            db.SaveChanges();
            return RedirectToAction("ListSubjects");
        }








        [Role_User]
        [HttpGet]
        public ActionResult Information(int id)
        {
            if (id == 0)
            {
                // Giá trị id không hợp lệ, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("ListSubjects");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var monHoc = db.Courses.Include(c => c.Department).Include(c => c.Semester).FirstOrDefault(c => c.CourseID == id);

            if (monHoc == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("ListSubjects");
            }

            ViewBag.KhoaCu = monHoc.DepartmentID;
            ViewBag.LopCu = monHoc.ClassID;
            return View(monHoc);
        }

        [HttpPost]
        public ActionResult Information(Cours monHoc, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingStudent = db.Courses.Find(monHoc.CourseID);

            if (existingStudent == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn
                return RedirectToAction("ListSubjects");
            }

            if (action == "Xóa")
            {
                // Xóa sinh viên
                db.Courses.Remove(existingStudent);
                db.SaveChanges();

                return RedirectToAction("ListSubjects");
            }
            else if (action == "Sửa")
            {
                // Cập nhật thông tin sinh viên
                existingStudent.CourseName = monHoc.CourseName;
                existingStudent.Description = monHoc.Description;
                existingStudent.Credits = monHoc.Credits;
                existingStudent.DepartmentID = monHoc.DepartmentID;
                existingStudent.SemesterID = monHoc.SemesterID;
                existingStudent.ClassID = monHoc.ClassID;


                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return RedirectToAction("ListSubjects");
            }

            // Nếu không phải là "Sửa" hoặc "Xóa", quay lại View với dữ liệu nhập
            return View(monHoc);
        }

        



    }
}

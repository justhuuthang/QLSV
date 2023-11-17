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
    public class QLHKController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        [Role_User(FunctionID = "Admin_XemDanhSach")]
        public ActionResult DanhSachHocKi(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var hocKi = db.Semesters.ToList();
            return View(hocKi.ToPagedList((int)page, (int)pageSize));
        }


        [Role_User]
        [HttpGet]
        public ActionResult ThemMoiHocKi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiHocKi(Semester hocKi)
        {
            string semesterName = Request["SemesterName"];

            var existingStudent = db.Semesters.FirstOrDefault(s => s.SemesterName == semesterName);

            if (existingStudent != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Đã có học kì này.");
            }

            db.Semesters.Add(hocKi);
            db.SaveChanges();
            return RedirectToAction("DanhSachHocKi");
        }


        [Role_User]
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                // Giá trị id không hợp lệ, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("DanhSachHocKi");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            //var hocKi = db.Semesters.Include(c => c.Department).Include(c => c.Semester).FirstOrDefault(c => c.CourseID == id);
            var hocKi = db.Semesters.Find(id);

            if (hocKi == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("DanhSachHocKi");
            }

            //ViewBag.KhoaCu = hocKi.DepartmentID;
            //ViewBag.LopCu = hocKi.ClassID;
            return View(hocKi);
        }

        [HttpPost]
        public ActionResult Suathongtin(Semester hocKi, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingStudent = db.Semesters.Find(hocKi.SemesterID);

            if (existingStudent == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn
                return RedirectToAction("DanhSachHocKi");
            }

            if (action == "Xóa")
            {
                // Xóa sinh viên
                db.Semesters.Remove(existingStudent);
                db.SaveChanges();

                return RedirectToAction("DanhSachHocKi");
            }
            else if (action == "Sửa")
            {
                // Cập nhật thông tin sinh viên
                existingStudent.SemesterName = hocKi.SemesterName;
                existingStudent.StartDate = hocKi.StartDate;
                existingStudent.EndDate = hocKi.EndDate;
               


                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return RedirectToAction("DanhSachHocKi");
            }

            // Nếu không phải là "Sửa" hoặc "Xóa", quay lại View với dữ liệu nhập
            return View(hocKi);
        }
    }
}

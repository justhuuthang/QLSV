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
    public class QLSVController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        [Role_User(FunctionID = "Admin_XemDanhSach")]
        public ActionResult DanhSachSinhVien(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var sinhVien = db.Students.ToList();
            return View(sinhVien.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchField, string searchValue)
        {
            searchValue = searchValue.ToLower(); // Chuyển giá trị tìm kiếm về chữ thường

            List<Student> searchResults = new List<Student>();

            switch (searchField)
            {
                case "StudentID":
                    if (int.TryParse(searchValue, out int studentID))
                    {
                        searchResults = db.Students.Where(s => s.StudentID == studentID).ToList();
                    }
                    break;
                case "FullName":
                    searchResults = db.Students.Where(s => s.FullName.ToLower().Contains(searchValue.ToLower())).ToList();
                    break;
                case "DateOfBirth":
                    // Kiểm tra nếu searchValue là ngày tháng hợp lệ
                    if (DateTime.TryParse(searchValue, out DateTime dob))
                    {
                        searchResults = db.Students.Where(s => s.DateOfBirth == dob).ToList();
                    }
                    break;
                case "ContactNumber":
                    searchResults = db.Students.Where(s => s.ContactNumber.ToLower().Contains(searchValue.ToLower())).ToList();
                    break;
                case "Email":
                    searchResults = db.Students.Where(s => s.Email.ToLower().Contains(searchValue.ToLower())).ToList();
                    break;
                default:
                    break;
            }

            return View("DanhSachSinhVien", searchResults);
        }
        [Role_User]
        [HttpGet]
        public ActionResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewStudent(Student sinhVien)
        {
            string contactNumber = Request["ContactNumber"];

            var existingStudent = db.Students.FirstOrDefault(s => s.ContactNumber == contactNumber);

            if (existingStudent != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Sinh viên đã tồn tại với số điện thoại này.");
            }

            db.Students.Add(sinhVien);
            db.SaveChanges();
            return RedirectToAction("DanhSachSinhVien");
        }

        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                // Giá trị id không hợp lệ, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("DanhSachSinhVien");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var sinhVien = db.Students.Find(id);

            if (sinhVien == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn, có thể chuyển hướng hoặc hiển thị thông báo lỗi
                return RedirectToAction("DanhSachSinhVien");
            }

            ViewBag.KhoaCu = sinhVien.DepartmentID;
            ViewBag.LopCu = sinhVien.ClassID;
            return View(sinhVien);
        }

        [HttpPost]
        public ActionResult Suathongtin(Student sinhVien, string action)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var existingStudent = db.Students.Find(sinhVien.StudentID);

            if (existingStudent == null)
            {
                // Sinh viên không tồn tại, xử lý theo ý của bạn
                return RedirectToAction("DanhSachSinhVien");
            }

            if (action == "Xóa")
            {
                // Xóa sinh viên
                db.Students.Remove(existingStudent);
                db.SaveChanges();

                return RedirectToAction("DanhSachSinhVien");
            }
            else if (action == "Sửa")
            {
                // Cập nhật thông tin sinh viên
                existingStudent.FullName = sinhVien.FullName;
                existingStudent.DateOfBirth = sinhVien.DateOfBirth;
                existingStudent.Gender = sinhVien.Gender;
                existingStudent.Address = sinhVien.Address;
                existingStudent.ContactNumber = sinhVien.ContactNumber;
                existingStudent.Email = sinhVien.Email;
                existingStudent.ClassID = sinhVien.ClassID;
                existingStudent.DepartmentID = sinhVien.DepartmentID;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();

                return RedirectToAction("DanhSachSinhVien");
            }

            // Nếu không phải là "Sửa" hoặc "Xóa", quay lại View với dữ liệu nhập
            return View(sinhVien);
        }





    }
}

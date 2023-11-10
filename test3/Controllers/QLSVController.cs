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

namespace test3.Controllers
{
    public class QLSVController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
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

        [HttpGet]
        public ActionResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewStudent(Student sinhVien)
        {
            if (string.IsNullOrEmpty(sinhVien.FullName))
            {
                ViewBag.FullNameError = "Yêu cầu nhập họ tên sinh viên.";
            }

            if (sinhVien.DateOfBirth == null || sinhVien.DateOfBirth == DateTime.MinValue)
            {
                ModelState.Remove("DateOfBirth");
                ViewBag.DateOfBirthError = "Yêu cầu nhập ngày sinh sinh viên.";
            }
            if (sinhVien.Gender == null)
            {
                ModelState.AddModelError("Gender", "Yêu cầu chọn giới tính sinh viên.");
            }



            if (sinhVien.DepartmentID == ' ')
            {
                ViewBag.DepartmentIDError = "Yêu cầu chọn mã khoa sinh viên.";
            }

            if (sinhVien.ClassID == ' ')
            {
                ViewBag.ClassIDError = "Yêu cầu chọn mã lớp sinh viên.";
            }
            if (string.IsNullOrEmpty(sinhVien.Address))
            {
                ViewBag.AddressError = "Yêu cầu nhập nơi sinh sinh viên.";
            }

            if (string.IsNullOrEmpty(sinhVien.FullName) ||
                sinhVien.DateOfBirth == null || sinhVien.Gender == null || sinhVien.DepartmentID == 0 || sinhVien.ClassID == 0 || string.IsNullOrEmpty(sinhVien.Address))
            {
                ViewBag.ErrorMessage = "Yêu cầu nhập đủ thông tin.";
                return View(sinhVien);
            }

            // Nếu dữ liệu hợp lệ, tiến hành thêm mới giảng viên
            QuanliSVEntities db = new QuanliSVEntities();
            db.Students.Add(sinhVien);
            db.SaveChanges();
            return RedirectToAction("DanhSachSinhVien");
        }


        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var sinhVien = db.Students.Find(id);
            /*if (sinhVien.Gender)
            {
                ViewBag.NamChecked = true;
                ViewBag.NuChecked = false;
            }
            else
            {
                ViewBag.NamChecked = false;
                ViewBag.NuChecked = true;
            }*/
            ViewBag.KhoaCu = sinhVien.DepartmentID;
            ViewBag.LopCu = sinhVien.ClassID;
            return View(sinhVien);
        }
        public ActionResult Xoa(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var sinhVien = db.Students.Find(id);
            db.Students.Remove(sinhVien);
            db.SaveChanges();
            return RedirectToAction("DanhSachSinhVien");
        }
       
        [HttpPost]
        public ActionResult Suathongtin(Student sinhVien)
        {
            if (string.IsNullOrEmpty(sinhVien.FullName))
            {
                ViewBag.FullNameError = "Yêu cầu nhập họ tên sinh viên.";
            }

            if (string.IsNullOrEmpty(sinhVien.Address))
            {
                ViewBag.AddressError = "Yêu cầu nhập nơi sinh sinh viên.";
            }

            if (sinhVien.DateOfBirth == null )
            {
                ModelState.Remove("DateOfBirth");
                ViewBag.DateOfBirthError = "Yêu cầu nhập ngày sinh sinh viên.";
            }

            // Kiểm tra trường "Gender"
            if (sinhVien.Gender == null)
            {
                ModelState.AddModelError("Gender", "Yêu cầu chọn giới tính sinh viên.");
            }

            if (sinhVien.DepartmentID == ' ')
            {
                ViewBag.DepartmentIDError = "Yêu cầu chọn mã khoa sinh viên.";
            }

            if (sinhVien.ClassID == ' ')
            {
                ViewBag.ClassIDError = "Yêu cầu chọn mã lớp sinh viên.";
            }


            if (ModelState.IsValid)
            {
                try
                {
                    QuanliSVEntities db = new QuanliSVEntities();
                    db.Entry(sinhVien).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DanhSachSinhVien");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // In lỗi kiểm tra cụ thể
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }

            ViewBag.ErrorMessage = "Yêu cầu nhập đủ thông tin.";
            return View(sinhVien);
        }

    }
}

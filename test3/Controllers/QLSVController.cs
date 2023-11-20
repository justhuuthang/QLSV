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
using OfficeOpenXml;
using PagedList.Mvc;
using ClosedXML.Excel;
using System.IO;

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
        public ActionResult Search(string searchField, string searchValue, int? page)
        {
            List<Student> searchResults = new List<Student>();
            bool isSearchByClass = searchField == "ClassName";
            bool isSearchByDepartment = searchField == "DepartmentName";
            switch (searchField)
            {
                case "StudentID":
                    if (int.TryParse(searchValue, out int studentID))
                    {
                        searchResults = db.Students.Where(s => s.StudentID == studentID).ToList();
                    }
                    break;
                case "ClassName":
                    searchResults = db.Students.Where(s => s.Class.ClassName.Contains(searchValue)).ToList();
                    break;
                case "DepartmentName":
                    searchResults = db.Students.Where(s => s.Class.Department.DepartmentName.Contains(searchValue)).ToList();
                    break;

                default:
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IPagedList<Student> pagedSearchResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachSinhVien", pagedSearchResults);
        }
        public ActionResult ExportSinhVien(string searchField)
        {
            List<Student> students;
            bool isSearchByClass = !string.IsNullOrEmpty(searchField);
            bool isSearchByDepartment = !string.IsNullOrEmpty(searchField);

            if (isSearchByClass)
            {
                students = GetStudentListByClassFromDatabase(searchField); 
            }
            else if (isSearchByDepartment)
            {
                students = GetStudentListByDepartmentFromDatabase(searchField);
            }
            else
            {
                students = GetStudentListFromDatabase();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachSinhVien");

                worksheet.Cell(1, 1).Value = "StudentID";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Address";
                worksheet.Cell(1, 6).Value = "ContactNumber";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "ClassID";
                worksheet.Cell(1, 9).Value = "DepartmentID";

                for (int i = 0; i < students.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = students[i].StudentID;
                    worksheet.Cell(row, 2).Value = students[i].FullName;
                    worksheet.Cell(row, 3).Value = students[i].DateOfBirth;
                    worksheet.Cell(row, 4).Value = students[i].Gender ? "Nam" : "Nữ";
                    worksheet.Cell(row, 5).Value = students[i].Address;
                    worksheet.Cell(row, 6).Value = students[i].ContactNumber;
                    worksheet.Cell(row, 7).Value = students[i].Email;
                    worksheet.Cell(row, 8).Value = students[i].ClassID;
                    worksheet.Cell(row, 9).Value = students[i].DepartmentID;
                }

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVien.xlsx");
            }
        }
        private List<Student> GetStudentListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Students.ToList();
        }
        private List<Student> GetStudentListByClassFromDatabase(string tenLop)
        {
            var db = new QuanliSVEntities();
            var students = db.Students
                     .Where(s => s.Class.ClassName.ToLower() == tenLop.ToLower())
                     .ToList();
            return db.Students.ToList();
        }
        private List<Student> GetStudentListByDepartmentFromDatabase(string tenKhoa)
        {
            var db = new QuanliSVEntities();
            var students = db.Students
                             .Where(s => s.Class.Department.DepartmentName.ToLower() == tenKhoa.ToLower())
                             .ToList();

            return students;
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
        public ActionResult Xoa(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var sinhVien = db.Students.Find(id);
            db.Students.Remove(sinhVien);
            db.SaveChanges();
            return RedirectToAction("DanhSachSinhVien");
        }
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachSinhVien");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var sinhVien = db.Students.Find(id);

            if (sinhVien == null)
            {
                return RedirectToAction("DanhSachSinhVien");
            }
            return View(sinhVien);
        }

        [HttpPost]
        public ActionResult Suathongtin(Student sinhVien)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            db.Entry(sinhVien).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachSinhVien");
        }
        [HttpGet]
        public ActionResult ImportStudents()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImportStudents(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        Student newStudent = new Student
                        {
                            FullName = worksheet.Cells[row, 2].Value?.ToString(),
                            DateOfBirth = Convert.ToDateTime(worksheet.Cells[row, 3].Value),
                            Gender = worksheet.Cells[row, 4].Value?.ToString() == "Nam" ? true : false,
                            Address = worksheet.Cells[row, 5].Value?.ToString(),
                            ContactNumber = worksheet.Cells[row, 6].Value?.ToString(),
                            Email = worksheet.Cells[row, 7].Value?.ToString(),
                            ClassID =Convert.ToInt32(worksheet.Cells[row, 8].Value?.ToString()),
                            DepartmentID = Convert.ToInt32(worksheet.Cells[row, 9].Value?.ToString())
                        };

                        db.Students.Add(newStudent);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("DanhSachSinhVien");
        }
        [HttpGet]
        public ActionResult GetClassesByDepartment(int departmentId)
        {
            var classes = db.Classes.Where(c => c.DepartmentID == departmentId).ToList();

            // Chuyển đổi danh sách lớp thành một danh sách SelectListItem
            var classList = classes.Select(c => new SelectListItem
            {
                Value = c.ClassID.ToString(), // Field ID của bảng lớp
                Text = c.ClassName // Field tên lớp của bảng lớp
            }).ToList();
            return Json(classList, JsonRequestBehavior.AllowGet);
        }

    }
}

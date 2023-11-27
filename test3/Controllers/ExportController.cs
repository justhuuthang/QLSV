using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
       
            public ActionResult ExportSinhVien()
        {
            List<Student> searchResults = TempData["SearchResults"] as List<Student>;

            List<Student> students;
            if (searchResults != null && searchResults.Any())
            {
                students = searchResults;
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
                worksheet.Cell(1, 8).Value = "Class";
                worksheet.Cell(1, 9).Value = "Department";
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
                    worksheet.Cell(row, 8).Value = students[i].Class.ClassName;
                    worksheet.Cell(row, 9).Value = students[i].Department.DepartmentName;
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

        private List<Student> GetStudentList(string searchClassName, string searchDepartmentName)
        {
            var query = db.Students.AsQueryable();

            // Nếu có thông tin về lớp, thêm điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(searchClassName))
            {
                query = query.Where(s => s.Class.ClassName.Contains(searchClassName));
            }

            // Nếu có thông tin về khoa, thêm điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(searchDepartmentName))
            {
                query = query.Where(s => s.Class.Department.DepartmentName.Contains(searchDepartmentName));
            }

            return query.ToList();
        }

        private QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult ExportMonHoc()
        {
            // Lấy thông tin về ClassName từ TempData
            string searchClassName = TempData["SearchClassName"] as string;
            List<Cours> subjects;
            if (string.IsNullOrEmpty(searchClassName))
            {
                subjects = GetSubjectListFromDatabase();
            }
            else
            {
                subjects = db.Courses.Where(c => c.CourseName.Contains(searchClassName)).ToList();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachMonHoc");
                worksheet.Cell(1, 1).Value = "CourseID";
                worksheet.Cell(1, 2).Value = "CourseName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Credits";
                worksheet.Cell(1, 5).Value = "DepartmentName";
                worksheet.Cell(1, 6).Value = "SemesterName";

                for (int i = 0; i < subjects.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = subjects[i].CourseID;
                    worksheet.Cell(row, 2).Value = subjects[i].CourseName;
                    worksheet.Cell(row, 3).Value = subjects[i].Description;
                    worksheet.Cell(row, 4).Value = subjects[i].Credits;

                    var departmentName = GetDepartmentName(subjects[i].DepartmentID);
                    var semesterName = GetSemesterName(subjects[i].SemesterID);

                    worksheet.Cell(row, 5).Value = departmentName;
                    worksheet.Cell(row, 6).Value = semesterName;
                }

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachMonHoc.xlsx");
            }
        }

        private string GetDepartmentName(int departmentID)
        {
            var department = db.Departments.Find(departmentID);
            return department != null ? department.DepartmentName : string.Empty;
        }
        private string GetSemesterName(int semesterID)
        {
            var semester = db.Semesters.Find(semesterID);
            return semester != null ? semester.SemesterName : string.Empty;
        }
        private List<Cours> GetSubjectListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Courses.ToList();
        }
        //Export DS lớp
        public ActionResult ExportLop()
        {
            List<Class> searchResults = TempData["SearchResults"] as List<Class>;

            List<Class> classes;
            if (searchResults != null && searchResults.Any())
            {
                classes = searchResults;
            }
            else
            {
                classes = GetClassListFromDatabase();
            }


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachLop");
                worksheet.Cell(1, 1).Value = "ClassID";
                worksheet.Cell(1, 2).Value = "ClassName";
                worksheet.Cell(1, 3).Value = "StartDate";
                worksheet.Cell(1, 4).Value = "EndDate";
                worksheet.Cell(1, 5).Value = "HeadTeacher";
                worksheet.Cell(1, 6).Value = "MaxStudents";
                for (int i = 0; i < classes.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = classes[i].ClassID;
                    worksheet.Cell(row, 2).Value = classes[i].ClassName;
                    worksheet.Cell(row, 3).Value = classes[i].StartDate;
                    worksheet.Cell(row, 4).Value = classes[i].EndDate;
                    worksheet.Cell(row, 5).Value = classes[i].HeadTeacher;
                    worksheet.Cell(row, 6).Value = classes[i].MaxStudents;
                }
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLop.xlsx");

            }
        }
        private List<Class> GetClassListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Classes.ToList();
        }

        private List<Student> GetClassList(string searchClassName, string searchDepartmentName)
        {
            var query = db.Students.AsQueryable();

            // Nếu có thông tin về lớp, thêm điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(searchClassName))
            {
                query = query.Where(s => s.Class.ClassName.Contains(searchClassName));
            }

            // Nếu có thông tin về khoa, thêm điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(searchDepartmentName))
            {
                query = query.Where(s => s.Class.Department.DepartmentName.Contains(searchDepartmentName));
            }

            return query.ToList();
        }

        //Export DS khoa
        public ActionResult ExportKhoa()
        {
            List<Department> searchResults = TempData["SearchResults"] as List<Department>;

            List<Department> departments;
            if (searchResults != null && searchResults.Any())
            {
                departments = searchResults;
            }
            else
            {
                departments = GetDepartmentListFromDatabase();
            }


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachLop");
                worksheet.Cell(1, 1).Value = "DepartmentID";
                worksheet.Cell(1, 2).Value = "DepartmentName";
                worksheet.Cell(1, 3).Value = "Description";
                for (int i = 0; i < departments.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = departments[i].DepartmentID;
                    worksheet.Cell(row, 2).Value = departments[i].DepartmentName;
                    worksheet.Cell(row, 3).Value = departments[i].Description;

                }
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhoa.xlsx");

            }
        }
        private List<Department> GetDepartments()
        {
            var db = new QuanliSVEntities();
            return db.Departments.ToList();
        }

        private List<Department> GetDepartments(string searchDepartmentName)
        {
            var query = db.Departments.AsQueryable();

            if (!string.IsNullOrEmpty(searchDepartmentName))
            {
                query = query.Where(s => s.DepartmentName.Contains(searchDepartmentName));
            }

            return query.ToList();
        }


        //danh sach sinh vien dat hoc bong
        public ActionResult ExportSVDHB(string MaKi)
        {
            List<sp_DanhSachSinhVienDatHocBong_Result> dssvdhb = db.sp_DanhSachSinhVienDatHocBong(MaKi).ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DanhSachSinhVienDatHocBong");


            var headers = new List<string> { "Mã Sinh Viên", "Tên Sinh Viên", "..." };
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }


            for (int i = 0; i < dssvdhb.Count; i++)
            {
                var sv = dssvdhb[i];
                worksheet.Cell(i + 2, 1).Value = sv.SemesterID;
                worksheet.Cell(i + 2, 2).Value = sv.FullName;

            }

            var memoryStream = new System.IO.MemoryStream();
            workbook.SaveAs(memoryStream);

            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVienDatHocBong.xlsx");
        }

        //export hoc ki
        public ActionResult ExportHocKi()
        {

            List<Semester> semesters = GetSemesterListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHocKi");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "SemesterID";
                worksheet.Cell(1, 2).Value = "SemesterName";
                worksheet.Cell(1, 3).Value = "StartDate";
                worksheet.Cell(1, 4).Value = "EndDate";


                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < semesters.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = semesters[i].SemesterID;
                    worksheet.Cell(row, 2).Value = semesters[i].SemesterName;
                    worksheet.Cell(row, 3).Value = semesters[i].StartDate;
                    worksheet.Cell(row, 4).Value = semesters[i].EndDate;


                }
                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHocKi.xlsx");
            }
        }

        //hocbong
        public ActionResult ExportHocBong()
        {

            List<Scholarship> scholarships = GetScholarshipListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHocBong");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "ScholarshipID";
                worksheet.Cell(1, 2).Value = "ScholarshipName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "StartDate";
                worksheet.Cell(1, 5).Value = "EndDate";
                worksheet.Cell(1, 6).Value = "Conditions";


                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < scholarships.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = scholarships[i].ScholarshipID;
                    worksheet.Cell(row, 2).Value = scholarships[i].ScholarshipID;
                    worksheet.Cell(row, 3).Value = scholarships[i].Description;
                    worksheet.Cell(row, 4).Value = scholarships[i].StartDate;
                    worksheet.Cell(row, 5).Value = scholarships[i].EndDate;
                    worksheet.Cell(row, 6).Value = scholarships[i].Conditions;


                }
                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHocBong.xlsx");
            }
        }

        private List<Department> GetDepartmentListFromDatabase()
        {
            var db = new QuanliSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Departments.ToList();
        }
        private List<Semester> GetSemesterListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Semesters.ToList();
        }
        private List<Scholarship> GetScholarshipListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Scholarships.ToList();
        }

    }
}
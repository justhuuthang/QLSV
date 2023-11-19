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
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Student> students = GetStudentListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachSinhVien");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "StudentID";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Address";
                worksheet.Cell(1, 6).Value = "ContactNumber";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "ClassID";
                worksheet.Cell(1, 9).Value = "DepartmentID";

                // Ghi danh sách sinh viên vào file Excel
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

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVien.xlsx");
            }
        }
        private QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult ExportMonHoc()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Cours> subjects = GetSubjectListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachMonHoc");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "CourseID";
                worksheet.Cell(1, 2).Value = "CourseName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Credits";
                worksheet.Cell(1, 5).Value = "DepartmentName";
                worksheet.Cell(1, 6).Value = "SemesterName";

                // Ghi danh sách sinh viên vào file Excel
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

                // Tạo tệp Excel và trả về nó cho người dùng
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

        // Hàm để lấy tên học kỳ từ SemesterID
        private string GetSemesterName(int semesterID)
        {
            var semester = db.Semesters.Find(semesterID);
            return semester != null ? semester.SemesterName : string.Empty;
        }






        /*public ActionResult ExportGiangVien()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<GiangVien> GiangViens = GetGiangVienListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachGiangVien");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "MGV";
                worksheet.Cell(1, 2).Value = "Họ tên";
                worksheet.Cell(1, 3).Value = "Ngày sinh";
                worksheet.Cell(1, 4).Value = "Giới tính";
                worksheet.Cell(1, 5).Value = "Khoa";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < GiangViens.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = GiangViens[i].MGV;
                    worksheet.Cell(row, 2).Value = GiangViens[i].HoTenGV;
                    worksheet.Cell(row, 3).Value = GiangViens[i].NgaySinhGV.ToString("dd-MM-yyyy");
                    worksheet.Cell(row, 4).Value = GiangViens[i].GioiTinh ? "Nam" : "Nữ";
                    worksheet.Cell(row, 5).Value = GiangViens[i].Khoa.TenKhoa;

                }

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachGiangVien.xlsx");
            }
        }
        public ActionResult ExportMonHoc()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<MonHoc> MonHocs = GetMonHocListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachMonHoc");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "Mã môn";
                worksheet.Cell(1, 2).Value = "Tên môn";
                worksheet.Cell(1, 3).Value = "Mã Khoa";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < MonHocs.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = MonHocs[i].MaMon;
                    worksheet.Cell(row, 2).Value = MonHocs[i].TenMon;
                    worksheet.Cell(row, 3).Value = MonHocs[i].MaKhoa;

                }

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachMonHoc.xlsx");
            }
        }
        public ActionResult ExportLop()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Lop> Lops = GetLopListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachLop");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "Mã lớp";

                worksheet.Cell(1, 2).Value = "Mã Khoa";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < Lops.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = Lops[i].MaLop;
                    worksheet.Cell(row, 2).Value = Lops[i].MaKhoa;

                }

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLop.xlsx");
            }
        }


        public ActionResult ExportKhoa()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Khoa> Khoas = GetKhoaListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachKhoa");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "Mã Khoa";

                worksheet.Cell(1, 2).Value = "Tên Khoa";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < Khoas.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = Khoas[i].MaKhoa;
                    worksheet.Cell(row, 2).Value = Khoas[i].TenKhoa;

                }

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhoa.xlsx");
            }
        }












        private List<SinhVien> GetSinhVienListFromDatabase()
        {
            var db = new QLSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.students.ToList();
        }
        private List<GiangVien> GetGiangVienListFromDatabase()
        {
            var db = new QLSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.GiangViens.ToList();
        }

        private List<MonHoc> GetMonHocListFromDatabase()
        {
            var db = new QLSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.MonHocs.ToList();
        }
        private List<Lop> GetLopListFromDatabase()
        {
            var db = new QLSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Lops.ToList();
        }
        private List<Khoa> GetKhoaListFromDatabase()
        {
            var db = new QLSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Khoas.ToList();
        }
    }*/

        private List<Student> GetStudentListFromDatabase()
        {
            var db = new QuanliSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Students.ToList();
        }
        private List<Cours> GetSubjectListFromDatabase()
        {
            var db = new QuanliSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Courses.ToList();
        }
    }
}
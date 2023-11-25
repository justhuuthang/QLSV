using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using System.Net;
using test3.App_Start;
using System.Data.Entity;

namespace test3.Controllers
{
    public class QLKController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DanhSachKhoa(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var khoa = db.Departments.ToList();
            return View(khoa.ToPagedList((int)page, (int)pageSize));
        }
        [HttpGet]
        public ActionResult Search(string searchValue, int? page)
        {
            List<Department> searchResults = new List<Department>();
            searchResults = db.Departments.Where(s => s.DepartmentName.ToUpper().StartsWith(searchValue)).ToList();
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Department> pagedResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachKhoa", pagedResults);
        }
        [Role_User]
        [HttpGet]
        public ActionResult ThemMoiKhoa()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiKhoa(Department khoa)
        {
            string departmentName = Request["DepartmentName"];

            var existingDepartmentName = db.Departments.FirstOrDefault(s => s.DepartmentName == departmentName);;

            if (existingDepartmentName != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Khoa này đã tồn tại.");
            }

            db.Departments.Add(khoa);
            db.SaveChanges();
            return RedirectToAction("DanhSachKhoa");
        }
        public ActionResult Xoa(int id)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            var khoa = db.Departments.Find(id);
            db.Departments.Remove(khoa);
            db.SaveChanges();
            return RedirectToAction("DanhSachKhoa");
        }
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachKhoa");
            }
            var khoa = db.Departments.Find(id);

            if (khoa == null)
            {
                return RedirectToAction("DanhSachKhoa");
            }
            return View(khoa);
        }

        [HttpPost]
        public ActionResult Suathongtin(Department khoa)
        {
            string departmentName = Request["DepartmentName"];

            var existingDepartmentName = db.Departments.FirstOrDefault(s => s.DepartmentName == departmentName); ;

            if (existingDepartmentName != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Khoa này đã tồn tại.");
            }
            db.Entry(khoa).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachKhoa");
        }
    }
}

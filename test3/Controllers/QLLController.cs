﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using System.Net;
using System.Data.Entity;
using test3.App_Start;

namespace test3.Controllers
{
    public class QLLController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        /*[Role_User(FunctionID = "Admin_XemDanhSach")]*/
        public ActionResult DanhSachLop(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var lop = db.Classes.ToList();
            return View(lop.ToPagedList((int)page, (int)pageSize));
        }
        [Role_User]
        [HttpGet]
        public ActionResult ThemMoiLop()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search(string searchValue, int? page)
        {
            List<Class> searchResults = new List<Class>();
            searchResults = db.Classes.Where(s => s.ClassName.ToUpper().StartsWith(searchValue)).ToList();
            TempData["SearchResults"] = searchResults;
            TempData["SearchValue"] = searchValue;
            int pageNumber = page ?? 1;
            int pageSize = 10;
            IPagedList<Class> pagedResults = searchResults.ToPagedList(pageNumber, pageSize);

            return View("DanhSachLop", pagedResults);
        }

        [HttpPost]
        public ActionResult ThemMoiLop(Class lop)
        {
            string className = Request["ClassName"];

            var existingClassName = db.Classes.FirstOrDefault(s => s.ClassName == className);

            if (existingClassName != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Tên lớp đã tồn tại.");
            }

            db.Classes.Add(lop);
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
        [Role_User]
        [HttpGet]
        public ActionResult Suathongtin(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("DanhSachLop");
            }

            QuanliSVEntities db = new QuanliSVEntities();
            var lop = db.Classes.Find(id);

            if (lop == null)
            {
                return RedirectToAction("DanhSachLop");
            }

            return View(lop);
        }

        [HttpPost]
        public ActionResult Suathongtin(Class lop)
        {
            string className = Request["ClassName"];

            var existingClassName = db.Classes.FirstOrDefault(s => s.ClassID != lop.ClassID && s.ClassName == lop.ClassName);

            if (existingClassName != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Tên lớp đã tồn tại.");
            }
            db.Entry(lop).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
        public ActionResult Xoa(int id)
        {
            var lop = db.Classes.Find(id);
            db.Classes.Remove(lop);
            db.SaveChanges();
            return RedirectToAction("DanhSachLop");
        }
    }
}

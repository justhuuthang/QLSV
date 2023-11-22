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
using System.Security;
using DocumentFormat.OpenXml.Spreadsheet;

namespace test3.Controllers
{
    public class QLSVDHBController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();

        public ActionResult DanhSachSinhVienDatHocBong(int? page, int? pageSize,string MaKi)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            ViewBag.MaKi = MaKi;
            List<sp_DanhSachSinhVienDatHocBong_Result> dssvdhb = db.sp_DanhSachSinhVienDatHocBong(MaKi).ToList();
            return View(dssvdhb.ToPagedList((int)page, (int)pageSize));

        }





    }
}

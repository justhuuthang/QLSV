using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult DanhSachTaiKhoan(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 10;
            }
            var accounts = db.Accounts.ToList();
            return View(accounts.ToPagedList((int)page, (int)pageSize));
        }
    }
}
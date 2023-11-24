using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using test3.App_Start;
using test3.Models;

namespace test3.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Account user)
        {
            using (QuanliSVEntities QLSVEntities = new QuanliSVEntities())
            {
                // Kiểm tra tên người dùng tồn tại trước khi kiểm tra mật khẩu
                var existingUser = QLSVEntities.Accounts.SingleOrDefault(m => m.Username == user.Username);

                if (existingUser == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("Sai tài khoản hoặc mật khẩu");
                }

                // Kiểm tra mật khẩu chỉ khi tên người dùng tồn tại
                if (existingUser.Password != user.Password)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Content("Sai tài khoản hoặc mật khẩu");
                }

                // Lưu thông tin người dùng vào Session
                SessionConfig.setUser(existingUser);
                return RedirectToAction("DashBoard", "Home");
            }
        }

        public ActionResult Logout()
        {
            SessionConfig.setUser(null);
            return RedirectToAction("Login", "Account");
        }
       [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Account user)
        {
            QuanliSVEntities db = new QuanliSVEntities();
            string email = Request["Email"];

            var users = db.Accounts.FirstOrDefault(s => s.Email == email);

            if (users != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Người dùng đã tồn tại");
            }
            db.Accounts.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

    }
}

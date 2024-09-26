using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using test3.Models;
using PagedList;
using System.Net;

namespace test3.Controllers
{
    public class QLTTController : Controller
    {
        // GET: QLSV
        QuanliSVEntities db = new QuanliSVEntities();
        [HttpGet]
        public ActionResult ThemMoiThongBao()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiThongBao(Notification thongBao)
        {
            string notificationName = Request["NotificationName"];

            var existingNotificationName = db.Notifications.FirstOrDefault(s => s.NotificationName == notificationName);

            if (existingNotificationName != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Thông báo đã tồn tại.");
            }

            db.Notifications.Add(thongBao);
            db.SaveChanges();
            return RedirectToAction("DashBoard", "Home");
        }
        [HttpGet]
        public ActionResult Suathongtin()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LayThongBaoTheoID(string searchValue)
        {
            if (int.TryParse(searchValue, out int notificationID))
            {
                var thongbao = db.Notifications.Find(notificationID);
                if (thongbao == null)
                {
                    ViewBag.Error = "Không tìm thấy thông báo";
                    return View("Suathongtin");
                }
                ViewBag.NotificationID = thongbao.NotificationID;
                ViewBag.NotificationName = thongbao.NotificationName;
                ViewBag.Date = thongbao.Date;
                ViewBag.NotificationContent = thongbao.NotificationContent;
                Session["SearchValue"] = searchValue;
                return View("Suathongtin", thongbao);
            }
            else
            {
                ViewBag.Error = "Không tìm thấy thông báo";
                return View("Suathongtin");
            }
        }

        [HttpPost]
        public ActionResult Suathongtin(Notification thongbao, string action)
        {
            var existingNotification = db.Notifications.Find(thongbao.NotificationID);
            if (action == "Xóa")
            {
                db.Notifications.Remove(existingNotification);
                db.SaveChanges();

                return RedirectToAction("DashBoard", "Home");
            }
            else if (action == "Sửa")
            {
                existingNotification.NotificationName = thongbao.NotificationName;
                existingNotification.NotificationContent = thongbao.NotificationContent;
                if (thongbao.Date != null)
                {
                    existingNotification.Date = thongbao.Date;
                }
                db.SaveChanges();

                return RedirectToAction("DashBoard", "Home");
            }

            return View(thongbao);
        }
    }
}
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test3.Models;

namespace test3.App_Start
{
    public class Role_User : AuthorizeAttribute
    {
        QuanliSVEntities db =  new QuanliSVEntities();
        public string FunctionID { get; set; }
        /*public bool kiemtra(int accountID, string functionID)
        {
            var dem = db.Roles.Count(m => m.AccountID == accountID & m.FunctionID == functionID);
            if (dem > 0)
            {
                return true;
            }
            else { return false; }
        }*/
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionConfig.getUser();
            if (user == null || user.UserID < 0)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Login"
                }));
                return;
            }
               if(string.IsNullOrEmpty(FunctionID) == false)
            {
                var check = new role().kiemtra(user.UserID, FunctionID);
                if(check == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "LoiPhanQuyen"
                    }));
                    return;
                }
            }
            return;
        }
    }
}
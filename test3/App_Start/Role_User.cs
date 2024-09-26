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
        QuanliSVEntities db = new QuanliSVEntities();
        public string Group { get; set; }

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

            /*var userGroup = Group.Split(',');
            bool hasValidFunction = false;

            foreach (var userGroups in userGroup)
            {
                if (!string.IsNullOrEmpty(userGroups))
                {
                    var check = new role().kiemtra(user.UserID, userGroups);
                    if (check)
                    {
                        hasValidFunction = true;
                        break;
                    }
                }
            }
            if (!hasValidFunction)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "LoiPhanQuyen"
                }));
                return;
            }*/
        }
    }

}
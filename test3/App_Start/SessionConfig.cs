using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test3.Models;

namespace test3.App_Start
{
    public static class SessionConfig
    {
        public static void setUser (Account user)
        {
            HttpContext.Current.Session["user"] = user;
        }
        public static Account getUser ()
        {
            return (Account)HttpContext.Current.Session["user"];
        }
    }
}
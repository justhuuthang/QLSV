using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test3.Models;

namespace test3
{
    public class role
    {
        QuaniSVEntities db= new QuaniSVEntities();
        public bool kiemtra(int accountID, string functionID)
        {
            var dem = db.Roles.Count(m => m.AccountID == accountID & m.FunctionID == functionID);
            if (dem > 0)
            {
                return true;
            }
            else { return false; }
        }
    }
}
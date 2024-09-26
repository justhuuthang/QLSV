using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using test3.Models;

namespace test3
{
    public class role
    {
        QuanliSVEntities db= new QuanliSVEntities();
        public bool kiemtra(int accountID, string userGroups)
        {
            var dem = db.Roles.Count(m => m.AccountID == accountID & m.Group == userGroups);
            if (dem > 0)
            {
                return true;
            }
            else { return false; }
        }
    }
}
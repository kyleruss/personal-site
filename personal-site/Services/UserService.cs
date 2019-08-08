using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using personal_site.Models;

namespace personal_site.Services
{
    public class UserService
    {
        private static UserService _instance;

        private UserService() { }

        public List<ApplicationUser> GetUserList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<ApplicationUser> userList = context.Users.ToList();
                return userList;
            }
        }

        public static UserService GetInstance()
        {
            _instance = _instance ?? new UserService();
            return _instance;
        }
    }
}
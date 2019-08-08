using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<bool> RemoveUser(string username)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                ApplicationUser user = new ApplicationUser() { Id = username };
                context.Users.Attach(user);
                context.Users.Remove(user);

                return await context.SaveChangesAsync() > 0;
            };
        }

     

        public static UserService GetInstance()
        {
            _instance = _instance ?? new UserService();
            return _instance;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using personal_site.Models;
using personal_site.ViewModels;

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

        public async Task<ApplicationUser> GetUser(string userId, ApplicationUserManager userManager)
        {
            return await userManager.FindByIdAsync(userId);
        }


        public async Task<bool> CreateUser(AdminUserEditViewModel model, ApplicationUserManager userManager)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                DisplayName = model.DisplayName,
                ProfilePicture = model.ProfilePicture        
            };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                IdentityResult roleResult = await userManager.AddToRoleAsync(user.Id, model.RoleName);
                return roleResult.Succeeded;
            }

            else return false;
        }

        public async Task<bool> EditUser(AdminUserEditViewModel model, ApplicationUserManager userManager)
        {
            ApplicationUser user = await userManager.FindByIdAsync(model.UserId);

            if (user != null)
            {
                user.Email = model.Email;
                user.DisplayName = model.DisplayName;
                user.UserName = model.Username;

                IdentityResult result = await userManager.UpdateAsync(user);
                IList<string> userRoles = await userManager.GetRolesAsync(user.Id);


                if (userRoles.Count > 0)
                    await userManager.RemoveFromRolesAsync(user.Id, userRoles.ToArray());
          

                IdentityResult roleCreateStatus = await userManager.AddToRoleAsync(user.Id, model.RoleName);
                return roleCreateStatus.Succeeded;
            }

            else return false;
        }
     

        public static UserService GetInstance()
        {
            _instance = _instance ?? new UserService();
            return _instance;
        }
    }
}
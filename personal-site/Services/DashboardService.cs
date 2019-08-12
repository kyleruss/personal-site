using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using personal_site.Models;
using personal_site.ViewModels;

namespace personal_site.Services
{
    public class DashboardService
    {
        private static DashboardService _instance;

        private DashboardService() { }

        public void ToggleShutdownMode(bool enable) { }

        public void ToggleMaintenanceMode(bool enable) { }

        public List<AdminUserMonthlyStatsModel> GetUserRegistrationData()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var result = context.Users
                    .GroupBy(g => g.DateJoined.Month)
                    .Select(x => new AdminUserMonthlyStatsModel { Month = x.Key, UserCount = x.Count() });

                return result.ToList();
            }
        }

        public int GetNumTotalusers()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Users.Count();
            }
        }

        public static DashboardService GetInstance()
        {
            _instance = _instance ?? new DashboardService();
            return _instance;
        }
    }
}
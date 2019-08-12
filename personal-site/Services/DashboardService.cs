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

        public AdminUserStatViewModel GetUserRegistrationData()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                List<AdminUserMonthlyStatsModel> monthlyUserCount = context.Users
                    .GroupBy(g => g.DateJoined.Month)
                    .Select(x => new AdminUserMonthlyStatsModel { Month = x.Key, UserCount = x.Count() }).ToList();

                int currentMonth = DateTime.Now.Month;
                int monthlyTotalCount = context.Users.Where(x => x.DateJoined.Month == currentMonth).Count();
                int totalCount = context.Users.Count();

                AdminUserCountStatsModel countStatsModel = new AdminUserCountStatsModel() { MonthlyCount = monthlyTotalCount, TotalCount = totalCount };

                return new AdminUserStatViewModel()
                {
                    MonthlyStatsModel = monthlyUserCount,
                    UserCountStats = countStatsModel
                };
            }
        }

        public static DashboardService GetInstance()
        {
            _instance = _instance ?? new DashboardService();
            return _instance;
        }
    }
}
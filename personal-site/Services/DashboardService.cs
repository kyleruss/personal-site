using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;
using personal_site.Models;
using personal_site.ViewModels;

namespace personal_site.Services
{
    public class DashboardService
    {
        private static DashboardService _instance;
        private const string SHUTDOWN_MODE = "ShutdownMode";
        private const string MAINT_MODE = "MaintenanceMode";

        private DashboardService() { }

        public bool ToggleShutdownMode(bool enable)
        {
            return SetSiteMode(SHUTDOWN_MODE, enable);
        }

        public bool ToggleMaintenanceMode(bool enable)
        {
            return SetSiteMode(MAINT_MODE, enable);
        }

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

        private bool SetSiteMode(string mode, bool enable)
        {
            try
            {
                var confDoc = new XmlDocument();
                string confPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                confDoc.Load(confPath);

                confDoc.SelectSingleNode("/configuration/appSettings/add[@key='" + mode + "']")
                    .Attributes["value"].Value = (enable ? "true" : "false");

                confDoc.Save(confPath);
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }

            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static DashboardService GetInstance()
        {
            _instance = _instance ?? new DashboardService();
            return _instance;
        }
    }
}
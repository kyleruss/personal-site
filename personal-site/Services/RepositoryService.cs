using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace personal_site.Services
{
    public class RepositoryService
    {
        private static RepositoryService _instance;

        private RepositoryService() { }

        public void GetRepositories()
        {

        }

        public static RepositoryService GetInstance()
        {
            _instance = _instance ?? new RepositoryService();
            return _instance;
        }
    }
}
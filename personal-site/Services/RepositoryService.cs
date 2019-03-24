using System;
using System.Collections.Generic;
using System.Linq;
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

        private void GetReadme()
        {

        }

        private void FilterReadme()
        {

        }

        public static RepositoryService GetInstance()
        {
            _instance = _instance ?? new RepositoryService();
            return _instance;
        }
    }
}
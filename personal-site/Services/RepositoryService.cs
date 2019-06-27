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

        private const int REPO_LOAD_TASK = 0;
        private const int COMMITS_LOAD_TASK = 1;
        private const int CODELINES_LOAD_TASK = 2;
        private const int LANG_LOAD_TASK = 3;
        private const int README_LOAD_TASK = 4;
        private const int DESC_LOAD_TASK = 5;
        private const int CHECK_DATA_TASK = 6;
        private const int FILTER_DATA_TASK = 7;
        private const int FILTER_CONTENT_TASK = 8;

        private RepositoryService() { }

        public void GetRepositories()
        {

        }

        public void RunTask(int taskID)
        {
            switch(taskID)
            {
                case REPO_LOAD_TASK:
                    break;

                case COMMITS_LOAD_TASK:
                    break;

                case CODELINES_LOAD_TASK:
                    break;

                case LANG_LOAD_TASK:
                    break;

                case README_LOAD_TASK:
                    break;

                case DESC_LOAD_TASK:
                    break;

                case CHECK_DATA_TASK:
                    break;

                case FILTER_CONTENT_TASK:
                    break;

                case FILTER_DATA_TASK:
                    break;
            }
        }

        public void UpdateRepositories()
        {

        }

        public void FetchRepositoriesList()
        {

        }

        public static RepositoryService GetInstance()
        {
            _instance = _instance ?? new RepositoryService();
            return _instance;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using personal_site.ViewModels;

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

        public async Task RemoveRepository(string name, HttpServerUtilityBase server)
        {
            dynamic repositories =  await LoadRepositories(server);
            repositories.Property(name).Remove();

            await SaveRepositories(repositories, server);
        }

        public async Task<string> EditRepository(AdminRepoEditViewModel model, HttpServerUtilityBase server)
        {
            dynamic repositories = await LoadRepositories(server);
            dynamic repo = repositories[model.Name];
            string desc = repo.description;

            repo.description = model.Description;
            repo.codeLines = model.CodeLines;

            await SaveRepositories(repositories, server);

            return desc;
        }

        public async Task SaveRepositories(dynamic repoObj, HttpServerUtilityBase server)
        {
            string repoJson = JsonConvert.SerializeObject(repoObj);
            string repoPath = GetRepoFilePath(server);
            using (StreamWriter writer = new StreamWriter(repoPath, false))
            {
                await writer.WriteLineAsync(repoJson);
            }
        }

        public async Task<dynamic> LoadRepositories(HttpServerUtilityBase server)
        {
            string repoPath = GetRepoFilePath(server);
            using (StreamReader reader = new StreamReader(repoPath))
            {
                string jsonStr = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(jsonStr);
            }
        }

        private string GetRepoFilePath(HttpServerUtilityBase server)
        {
            return server.MapPath("~/Content/resources/repository-data.json");
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

        public static RepositoryService GetInstance()
        {
            _instance = _instance ?? new RepositoryService();
            return _instance;
        }
    }
}
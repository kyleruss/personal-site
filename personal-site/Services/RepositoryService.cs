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
            repo.readme = model.Readme;

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


        public List<string> GetRepositoryTaskList()
        {
            List<string> taskList = new List<string>
            {
                "repo-load",
                "commits-load",
                "codelines-load",
                "languages-load",
                "readme-load",
                "description-load",
                "find-abnormal-data",
                "filter-abnormal-data",
                "filter-content"
            };

            return taskList;
        }

        private string GetRepoFilePath(HttpServerUtilityBase server)
        {
            return server.MapPath("~/Content/resources/repository-data.json");
        }

        public bool RunTask(string taskName)
        {
            List<string> taskList = GetRepositoryTaskList();

            if (!taskList.Contains(taskName))
                return false;
            else
            {
                string commandText = "/K grunt " + taskName;
                string projectDir = AppDomain.CurrentDomain.BaseDirectory;
                var process = new Process();
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = commandText,
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = projectDir
                };
                
                process.StartInfo = startInfo;
                process.Start();


                               
                return true;
            }

        }

        public static RepositoryService GetInstance()
        {
            _instance = _instance ?? new RepositoryService();
            return _instance;
        }
    }
}
 
 
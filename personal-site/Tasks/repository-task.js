module.exports = function(grunt)
{
    grunt.registerTask('repo-load', function()
    {
        var request = require('request');
        var fs = require('fs');

        var repoData = [];
        var index = 0;

        var userApiUrl = "https://api.github.com/users/kyleruss/";
        var repoApiUrl = "https://api.github.com/repos/kyleruss/";
        var apiContentUrl = "https://raw.githubusercontent.com/";
        var repoDataFile = "Tasks/repository-data.json";

        var done = this.async();
        getRepositories();
        

        function loadRepoDataFile()
        {
            fs.readFile(repoDataFile, 'utf-8', (err, data) =>
            {
                if(err) console.log('Repo data file load error: ' + err);
                else repoData = JSON.parse(data);
            });
        }

        function saveRepoDataFile(callback)
        {
            var repoJsonStr = JSON.stringify(repoData);
            fs.writeFile(repoDataFile, repoJsonStr, 'utf-8', callback);
        }


        function getRepositories()
        {
            var getRepositoriesApiUrl = userApiUrl + "repos";

            callApi(getRepositoriesApiUrl, (data) =>
            {
                data.forEach((repoItem) =>
                {
                    var repoName = repoItem.name;
                    var repoLink = repoItem.html_url;
                    var repoObj = { name: repoName, link: repoLink };
                    
                    repoData.push(repoObj);
                });

                saveRepoDataFile((err) =>
                {
                    console.log('save data');
                    done();
                });
            });
        }

        function callApi(apiNodeUrl, callback, options)
        {
            if (options == null) 
                options = 
                {
                    url: apiNodeUrl, 
                    json: true,
                    headers: { 'User-Agent': 'request' }
                };

            request(options, (err, res, data) =>
            {
                if(err)
                    console.log('Error: ' + err);

                else if(res.statusCode !== 200)
                    console.log('Error status code: ' + res.statusCode);

                else
                    callback(data);
            });
        };

        function getApiUrl(i, type)
        {
            var repoName = repoData[i].name;
            var url = "";

            switch(type)
            {
                case 0: //commits
                    url = repoApiUrl + repoName + "/contributors";
                    break;

                case 1: //lines of code
                    url = repoApiUrl + repoName + "/stats/contributors";
                    break;

                case 2: //languages
                    url = repoApiUrl + repoName + "/languages";
                    break;

                case 3: //readme
                    url = repoApiUrl + repoName + "/readme";
                    break;
            }

            return url;
        };

        function getRepositoryCommits()
        {
            callApi(getApiUrl(index, 0), (data) =>
            {
                var commits = data[0].contributions;
                repoData[index]["commits"] = commits;

                getRepositoryLanguages();
            });
        };

        function getRepositoryCodeLines()
        {
            callApi(getApiUrl(index, 1), (data) =>
            {
                var weeks = data[0].weeks;
                var total = 0;

                weeks.forEach(function(week)
                {
                    var additions = week.a;
                    total += additions;
                });

                callback();
            });
        };

        function getRepositoryLanguages()
        {
            callApi(getApiUrl(index, 2), (data) =>
            {
                repoData[index]["languages"] = data;
                getRepositoryReadme();
            });
        };

        function getRepositoryReadme()
        {
            var readmeApiUrl = getApiUrl(index, 3);
            var options = 
            { 
                url: readmeApiUrl, 
                headers: { 'User-Agent': 'request', 'Accept': 'application/vnd.github.VERSION.html'}
            };

            callApi(readmeApiUrl, (data) => 
            {
                repoData[index]["readme"] = data;
                updateIntervalCounter();

            }, options);
        };

        function updateIntervalCounter()
        {
            if(index == repoData.length - 1)
            {
                console.log(repoData);
                done();
            }
            else
                index++;
        }
    });
};
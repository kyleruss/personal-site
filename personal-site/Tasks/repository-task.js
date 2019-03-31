module.exports = function(grunt)
{
    var request = require('request');
    var fs = require('fs');

    var repoData = [];
    var index = 0;

    var userApiUrl = "https://api.github.com/users/kyleruss/";
    var repoApiUrl = "https://api.github.com/repos/kyleruss/";
    var apiContentUrl = "https://raw.githubusercontent.com/";
    var repoDataFile = "Tasks/repository-data.json";
    

    function loadRepoDataFile()
    {
        return new Promise((resolve, reject) =>
        {
            fs.readFile(repoDataFile, 'utf-8', (err, data) =>
            {
                if(err) 
                {
                    console.log('Repo data file load error: ' + err);
                    reject(err);
                }

                else
                {
                    repoData = JSON.parse(data);
                    resolve();
                }
            });
        });
    }

    function saveRepoDataFile(callback)
    {
        var repoJsonStr = JSON.stringify(repoData);
        fs.writeFile(repoDataFile, repoJsonStr, 'utf-8', callback);
    }

    function callApi(apiNodeUrl, callback, options)
    {
        if (options == null) 
        {
            options = 
            {
                url: apiNodeUrl, 
                json: true,
                headers: { 'User-Agent': 'request' }
            };
        };

        return new Promise((resolve, reject) =>
        {
            request(options, (err, res, data) =>
            {
                if(err || res.statusCode != 200)
                {
                    console.log('Error: ' + err + ' statusCode: ' + res.statusCode);
                    reject(err);
                }

                else
                {
                    resolve();
                    callback(data);
                }
            });
        });
    };

    function getApiUrl(index, type)
    {
        var url, repoName;
        if(index != -1) repoName = repoData[i].name;

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

            case 4: //repos
                url = userApiUrl + "repos";
                break;
        }

        return url;
    };
    
    async function runTask(task)
    {
        var done = this.async();
        
        await loadRepoDataFile();

        for(var i = 0; i < 5; i++)
            await task(i);

        await saveRepoDataFile(() =>
        {
            console.log('File saved');
            done();
        }); 
    }

    function getRepositories()
    {
        return callApi(getApiUrl(-1, 4), (data) =>
        {
            data.forEach((repoItem) =>
            {
                var repoName = repoItem.name;
                var repoLink = repoItem.html_url;
                var repoObj = { name: repoName, link: repoLink };
                
                repoData.push(repoObj);
            });
        });
    }

    function getRepositoryCommits(index)
    {
        return callApi(getApiUrl(index, 0), (data) =>
        {
            var commits = data[0].contributions;
            repoData[index]["commits"] = commits;
        });
    };

    function getRepositoryCodeLines(index)
    {
        return callApi(getApiUrl(index, 1), (data) =>
        {
            var weeks = data[0].weeks;
            var total = 0;

            weeks.forEach(function(week)
            {
                var additions = week.a;
                total += additions;
            });

            repoData[index]["codeLines"] = total;
        });
    };

    function getRepositoryLanguages(index)
    {
        return callApi(getApiUrl(index, 2), (data) =>
        {
            repoData[index]["languages"] = data;
            getRepositoryReadme();
        });
    };

    function getRepositoryReadme(index)
    {
        var readmeApiUrl = getApiUrl(index, 3);
        var options = 
        { 
            url: readmeApiUrl, 
            headers: { 'User-Agent': 'request', 'Accept': 'application/vnd.github.VERSION.html'}
        };

        return callApi(readmeApiUrl, (data) => 
        {
            repoData[index]["readme"] = data;
            
        }, options);
    };

    grunt.registerTask('repo-load', function()
    {
        runTask(getRepositories);
    });

    grunt.registerTask('commits-load', function()
    {
        runTask(getRepositoryCommits);
    });

    grunt.registerTask('codelines-load', function()
    {
        runTask(getRepositoryCodeLines);
    });

    grunt.registerTask('languages-load', function()
    {
        runTask(getRepositoryLanguages);
    });

    grunt.registerTask('readme-load', function()
    {
        runTask(getRepositoryReadme);
    });
};
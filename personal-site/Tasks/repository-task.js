module.exports = function(grunt)
{
    var request = require('request');
    var fs = require('fs');

    var repoData = [{}];
    var index = 0;

    var userApiUrl = "https://api.github.com/users/kyleruss/";
    var repoApiUrl = "https://api.github.com/repos/kyleruss/";
    var apiContentUrl = "https://raw.githubusercontent.com/";
    var repoDataFile = "Tasks/repository-data.json";

    var COMMITS_TASK = 0,
        CODE_TASK = 1,
        LANGUAGES_TASK = 2,
        README_TASK = 3,
        REPO_TASK = 4;
    

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
                    if(data != null && data.length > 0)
                        repoData = JSON.parse(data);

                    resolve();
                }
            });
        });
    };

    function saveRepoDataFile(callback)
    {
        var repoJsonStr = JSON.stringify(repoData);
        fs.writeFile(repoDataFile, repoJsonStr, 'utf-8', callback);
    };

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
    

    function getApiUrl(repoName, type)
    {
        var url;

        switch(type)
        {
            case COMMITS_TASK: //commits
                url = repoApiUrl + repoName + "/contributors";
                break;

            case CODE_TASK: //lines of code
                url = repoApiUrl + repoName + "/stats/contributors";
                break;

            case LANGUAGES_TASK: //languages
                url = repoApiUrl + repoName + "/languages";
                break;

            case README_TASK: //readme
                url = repoApiUrl + repoName + "/readme";
                break;

            case REPO_TASK: //repos
                url = userApiUrl + "repos";
                break; 
        }

        return url;
    };
    
    async function runTask(task, singleExec, taskScope)
    {
        var done = taskScope.async();

        await loadRepoDataFile();

        if(singleExec) await task();
        else
        {
            for(var index in repoData[0])
                await task(index);
        }

        await saveRepoDataFile(() =>
        {
            console.log('File saved');
            done();
        }); 
    };

    function getRepoProperty(propName)
    {
        if(repoData == null || repoData.length == 0) return null;
        else return repoData[0][propName];
    }

    function setRepoProperty(propName, propValue)
    {
        if(repoData != null)
            repoData[0][propName] = propValue;
    }

    function getRepositories()
    {
        return callApi(getApiUrl(null, REPO_TASK), (data) =>
        {
            data.forEach((repoItem) =>
            {
                var repoName = repoItem.name;
                var repoLink = repoItem.html_url;
                var repoObj = { name: repoName, link: repoLink };
                
                setRepoProperty(repoName, repoObj);
            });
        });
    };

    function getRepositoryCommits(index)
    {
        return callApi(getApiUrl(index, COMMITS_TASK), (data) =>
        {
            var commits = data[0].contributions;
            setRepoProperty("commits", commits);
        });
    };

    function getRepositoryCodeLines(index)
    {
        return callApi(getApiUrl(index, CODE_TASK), (data) =>
        {
            var weeks = data[0].weeks;
            var total = 0;

            weeks.forEach(function(week)
            {
                var additions = week.a;
                total += additions;
            });

            setRepoProperty("codeLines", total);
        });
    };

    function getRepositoryLanguages(index)
    {
        return callApi(getApiUrl(index, LANGUAGES_TASK), (data) =>
        {
            setRepoProperty("languages", data);
        });
    };

    function getRepositoryReadme(index)
    {
        var readmeApiUrl = getApiUrl(index, README_TASK);
        var options = 
        { 
            url: readmeApiUrl, 
            headers: { 'User-Agent': 'request', 'Accept': 'application/vnd.github.VERSION.html'}
        };

        return callApi(readmeApiUrl, (data) => 
        {
            setRepoProperty("readme", data);

        }, options);
    };

    grunt.registerTask('repo-load', function()
    {
        runTask(getRepositories, true, this);
    });

    grunt.registerTask('commits-load', function()
    {
        runTask(getRepositoryCommits, false, this);
    });

    grunt.registerTask('codelines-load', function()
    {
        runTask(getRepositoryCodeLines, false, this);
    });

    grunt.registerTask('languages-load', function()
    {
        runTask(getRepositoryLanguages, false, this);
    });

    grunt.registerTask('readme-load', function()
    {
        runTask(getRepositoryReadme, false, this);
    });
};
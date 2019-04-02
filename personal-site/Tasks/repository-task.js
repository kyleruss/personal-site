module.exports = function(grunt)
{
    var request = require('request');
    var fs = require('fs');
    var config = require('./api-config.json');

    var repoData = [{}];

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

    function getHeaders(apiNodeUrl)
    {
        var authTokenStr = "token " + config.authToken;
        var headers = 
        {
            url: apiNodeUrl, 
            json: true,
            headers: { 'User-Agent': 'request', 'Authorization': authTokenStr }
        };

        return headers;
    }

    function callApi(apiNodeUrl, callback, options)
    {
        if (options == null) 
            options = getHeaders(apiNodeUrl);

        return new Promise((resolve, reject) =>
        {
            request(options, (err, res, data) =>
            {
                if(err || res.statusCode != 200)
                {
                    console.log('Error: ' + err + ' statusCode: ' + res.statusCode);
                    
                    if(res.statusCode == 202)
                    {
                        resolve();
                        callback(null, res.statusCode);
                    }

                    else reject();
                }

                else
                {
                    resolve();
                    callback(data, res.statusCode);
                }
            });
        });
    };

    function getApiUrl(repoName, type)
    {
        var url;

        switch(type)
        {
            case COMMITS_TASK:
                url = repoApiUrl + repoName + "/contributors";
                break;

            case CODE_TASK:
                url = repoApiUrl + repoName + "/stats/contributors";
                break;

            case LANGUAGES_TASK:
                url = repoApiUrl + repoName + "/languages";
                break;

            case README_TASK:
                url = repoApiUrl + repoName + "/readme";
                break;

            case REPO_TASK:
                url = userApiUrl + "repos";
                break; 
        }

        return url;
    };
    
    async function runTask(task, singleExec, taskScope)
    {
        var done = taskScope.async();

        await loadRepoDataFile();
        console.log('[Repository Data File] Loaded');

        if(singleExec) await task();
        else
        {
            for(var index in repoData[0])
            {
                console.log("[Processing] Repository: " + index);
                await task(index);
            }
        }

        await saveRepoDataFile(() =>
        {
            console.log('[Repository Data File] Saved');
            done();
        }); 
    };

    function getRepoProperty(index, propName)
    {
        if(repoData == null || repoData.length == 0) return null;
        else return index == null? 
             repoData[0][propName] : repoData[0][index][propName];
    }

    function setRepoProperty(index, propName, propValue)
    {
        if(repoData != null)
        {
            if(index == null)
                repoData[0][propName] = propValue;
            else
                repoData[0][index][propName] = propValue;
        }
    }

    function getRepositories()
    {
        return callApi(getApiUrl(null, REPO_TASK), (data, status) =>
        {
            data.forEach((repoItem) =>
            {
                var repoName = repoItem.name;
                var repoLink = repoItem.html_url;
                var repoObj = { name: repoName, link: repoLink };
                
                if(getRepoProperty(null, repoName) == null)
                    setRepoProperty(null, repoName, repoObj);
            });
        });
    };

    function getRepositoryCommits(index)
    {
        return callApi(getApiUrl(index, COMMITS_TASK), (data, status) =>
        {
            var commits = data[0].contributions;
            setRepoProperty(index, "commits", commits);
        });
    };

    function getRepositoryCodeLines(index)
    {
        return callApi(getApiUrl(index, CODE_TASK), (data, status) =>
        {
            if(status == 200)
            {
                var weeks = data[0].weeks;
                var total = 0;

                weeks.forEach(function(week)
                {
                    var additions = week.a;
                    total += additions;
                });

                setRepoProperty(index, "codeLines", total);
            }
        });
    };

    function getRepositoryLanguages(index)
    {
        return callApi(getApiUrl(index, LANGUAGES_TASK), (data, status) =>
        {
            setRepoProperty(index, "languages", data);
        });
    };

    function getRepositoryReadme(index)
    {
        var readmeApiUrl = getApiUrl(index, README_TASK);
        var options = getHeaders(readmeApiUrl);
        options['headers']['Accept'] = 'application/vnd.github.VERSION.html';

        return callApi(readmeApiUrl, (data, status) => 
        {
            setRepoProperty(index, "readme", data);

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
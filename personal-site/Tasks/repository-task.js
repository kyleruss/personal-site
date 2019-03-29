module.exports = function(grunt)
{
    grunt.registerTask('repo-load', function()
    {
        var request = require('request');
        var repoData = [];

        var apiUrl = "https://api.github.com/users/kyleruss/repos";
        var languageApiUrl = "https://api.github.com/repos/kyleruss/graphi/languages";
        var commitUrl = "https://api.github.com/repos/kyleruss/byte-chat/contributors";
        var codeLineStats = "https://api.github.com/repos/kyleruss/graphi/stats/contributors";
        var apiContentUrl = "https://raw.githubusercontent.com/";

        var done = this.async();
        getRepositories();
        

        function getRepositories()
        {
            request
            ({ 
                url: apiUrl, 
                json: true,
                headers: { 'User-Agent': 'request'}
            }, (err, res, data) =>
            {
                if(err)
                    console.log('Error: ' + err);
                else if(res.statusCode !== 200)
                    console.log('Error status code: ' + res.statusCode);
                else
                {

                    data.forEach((repoItem) =>
                    {
                        var repoName = repoItem.name;
                        var repoLink = repoItem.html_url;
                        var repoObj = { name: repoName, link: repoLink };
                        
                        repoData.push(repoObj);
                    });

                    console.log(repoData);
                }

                done();
            }); 

            setTimeout(function(){ console.log('hello world')}, 2500);
        }

        function getRepositoryLanguages(repoName)
        {

        }

        function getRepositoryStats()
        {

        }

    });
};
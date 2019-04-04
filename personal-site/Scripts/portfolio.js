function Portfolio()
{
    var repoData = {};
    var currentRepo;
    var repos;
    var repoIndex;

    loadRepoData();

    function loadRepoData()
    {
        var dataUrl = $('repo-data').attr('url');
        
        $.getJSON(dataUrl, (data) =>
        {
            repoData = data;
            repos = Object.keys(repoData);
            setCurrentRepository(5);
        });
    };

    function setCurrentRepository(index)
    {
        repoIndex = index;
        repoName = repos[repoIndex];
        currentRepo = repoData[repoName];

        updatePortfolioView();
    };

    function nextRepository()
    {
        setCurrentRepository(repoIndex + 1);
    };

    function prevRepository()
    {
        setCurrentRepository(repoIndex - 1);
    };

    function updatePortfolioView()
    {
        var projectContainer = $('#project-preview');
        var commitStats = $('#commits-stats');
        var codeStats = $('#codeline-stats');
        var githubLinkBtn = $('#view-github-btn');
        var repoTitle = $('#project-title');
        var repoDesc = $('#project-description');

        projectContainer.html(currentRepo["readme"]);
        commitStats.text(currentRepo["commits"]);
        codeStats.text(currentRepo["codeLines"]);
        githubLinkBtn.attr('href', currentRepo["link"]);
        repoTitle.text(getTransformedTitle());
    };

    function getTransformedTitle()
    {
        return currentRepo["name"].replace(/-/g, " ");
    }

    $('#portf-left-arrow').click(() =>
    {
        prevRepository();
    });

    $('#portf-right-arrow').click(() =>
    {
        nextRepository();
    });
};
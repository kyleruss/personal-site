function Portfolio()
{
    var repoData = {};
    var existingRepos = [];
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
            initCarousel();
            setCurrentRepository(0);
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
        var nextIndex = repoIndex + 1;
        var n = repos.length;

        if(nextIndex >= repos.length)
            nextIndex = nextIndex % n;
        
        setCurrentRepository(nextIndex);
        $('#project-preview').slick('slickNext');
    };

    function prevRepository()
    {
        var prevIndex = repoIndex - 1;
        var n = repos.length;
        
        if(prevIndex < 0)
            prevIndex = ((prevIndex % n) + n) % n;

        setCurrentRepository(prevIndex);
        $('#project-preview').slick('slickPrev');
    };

    function initCarousel()
    {
        $('#project-preview').slick
        ({
            slidesToShow:1,
            slidesToScroll:1,
            arrows:false
        });
    };

    function pushCarouselItem()
    {
        if($.inArray(repoIndex, existingRepos) == -1)
        {
            var readmeHtml = currentRepo["readme"];
            var carouselContainer = $('#project-preview');

            carouselContainer.slick('slickAdd', readmeHtml);

            existingRepos.push(repoIndex);
        }
    };

    function updatePortfolioView()
    {
        var commitStats = $('#commits-stats');
        var codeStats = $('#codeline-stats');
        var githubLinkBtn = $('#view-github-btn');
        var repoTitle = $('#project-title');
        var repoDesc = $('#project-description');

        pushCarouselItem();
        commitStats.text(currentRepo["commits"]);
        codeStats.text(currentRepo["codeLines"]);
        githubLinkBtn.attr('href', currentRepo["link"]);
        repoTitle.text(getTransformedTitle());
        repoDesc.text(currentRepo["description"]);
        updateLanguages();
    };

    function updateLanguages()
    {
        var languages = currentRepo["languages"];
        var numLang = languages.length;
        var langIcon = $('#language-stat-icon');
        $('.language-stat').hide();

        if(numLang == 0)
            langIcon.hide();
        else
            langIcon.show();
        
        for(var i = 0; i < numLang; i++)
        {   
            var languageElement = $('.language-stat').eq(i);
            var langTextElement = languageElement.find('.language-stat-text');
            var language = languages[i];

            langTextElement.text(language);
            languageElement.show();
        }
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

    $('#project-preview').niceScroll
    ({
        horizrailenabled: false
    });
};
class PortfolioComponent
{
    constructor()
    {
        this.repoData = {};
        this.existingRepos = [];

        this.loadRepoData();
        this.initHandlers();
    };

    initDisplay()
    {
        this.setCurrentRepository(1);
    };

    loadRepoData()
    {
        var dataUrl = $('repo-data').attr('url');
        
        $.getJSON(dataUrl, (data) =>
        {
            this.repoData = data;
            this.repos = Object.keys(this.repoData);
            this.initCarousel();
        });
    };

    setCurrentRepository(index)
    {
        this.repoIndex = index;
        this.repoName = this.repos[this.repoIndex];
        this.currentRepo = this.repoData[this.repoName];

        this.updatePortfolioView();
    };

    nextRepository()
    {
        var nextIndex = this.repoIndex + 1;
        var n = this.repos.length;

        if(nextIndex >= this.repos.length)
            nextIndex = nextIndex % n;
        
        this.setCurrentRepository(nextIndex);
        $('#project-preview').slick('slickNext');
    };

    prevRepository()
    {
        var prevIndex = this.repoIndex - 1;
        var n = this.repos.length;
        
        if(prevIndex < 0)
            prevIndex = ((prevIndex % n) + n) % n;

        this.setCurrentRepository(prevIndex);
        $('#project-preview').slick('slickPrev');
    };

    initCarousel()
    {
        $('#project-preview').slick
        ({
            slidesToShow:1,
            slidesToScroll:1,
            arrows:false
        });
    };

    pushCarouselItem()
    {
        if($.inArray(this.repoIndex, this.existingRepos) == -1)
        {
            var readmeHtml = this.currentRepo["readme"];
            var carouselContainer = $('#project-preview');

            carouselContainer.slick('slickAdd', readmeHtml);

            this.existingRepos.push(this.repoIndex);
        }
    };

    updatePortfolioView()
    {
        var commitStats = $('#commits-stats');
        var codeStats = $('#codeline-stats');
        var githubLinkBtn = $('#project-view-link');
        var repoTitle = $('#project-title');
        var repoDesc = $('#project-description');

        this.pushCarouselItem();

        commitStats.text(this.currentRepo["commits"]);
        codeStats.text(this.currentRepo["codeLines"]);

        
        githubLinkBtn.attr('href', this.currentRepo["link"]);
        repoTitle.text(this.getTransformedTitle());
        repoDesc.text(this.currentRepo["description"]);
        this.updateLanguages();
    };

    updateLanguages()
    {
        var languages = this.currentRepo["languages"];
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

    getTransformedTitle()
    {
        return this.currentRepo["name"].replace(/-/g, " ");
    };

    initHandlers()
    {
        $('#prev-project-btn').click(() =>
        {
            this.prevRepository();
        });
    
        $('#next-project-btn').click(() =>
        {
            this.nextRepository();
        });
    };
};
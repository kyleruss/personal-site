class PortfolioComponent
{
    constructor()
    {
        this.repoData = {};
        this.existingRepos = [];
        this.processingAnimation = false;

        this.loadRepoData();
        this.initHandlers();
    };

    initDisplay()
    {

        this.updatePortfolioView();
    };

    toggleOffColorNavbar(show)
    {
        if(show)
            $('.side-navbar-item').addClass('side-navbar-offcolor');
        else
            $('.side-navbar-item').removeClass('side-navbar-offcolor');
    }

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
    };

    nextRepository()
    {
        if(!this.processingAnimation)
        {
            this.processingAnimation = true;
            var nextIndex = this.repoIndex + 1;
            var n = this.repos.length;

            if(nextIndex >= this.repos.length)
                nextIndex = nextIndex % n;
            
            this.setCurrentRepository(nextIndex);
            this.pushCarouselItem();
            $('#project-preview').slick('slickNext');
        }
    };

    prevRepository()
    {
        if(!this.processingAnimation && this.repoIndex > 0)
        {
            this.processingAnimation = true;
            var prevIndex = this.repoIndex - 1;

            this.setCurrentRepository(prevIndex);
            $('#project-preview').slick('slickPrev');
        }
    };

    initCarousel()
    {
        $('#project-preview').slick
        ({
            slidesToShow:1,
            slidesToScroll:1,
            arrows:false,
            swipe:true
        });

        this.setCurrentRepository(0);
        this.pushCarouselItem();
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
        var repoTitle = $('#project-title-link');
        var repoDesc = $('#project-description');

        commitStats.text(this.currentRepo["commits"]);
        codeStats.text(this.currentRepo["codeLines"]);

        githubLinkBtn.attr('href', this.currentRepo["link"]);
        repoTitle.attr('href', this.currentRepo["link"]);
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
        $('.prev-project-btn').click(() =>
        {
            this.prevRepository();
        });
    
        $('.next-project-btn').click(() =>
        {
            this.nextRepository();
        });

        $('.prev-project-btn').hover(() =>
        {
            if(this.repoIndex <= 0)
                $('#prev-project-btn').css('cursor', 'not-allowed');
            else
                $('#prev-project-btn').css('cursor', 'pointer');
        });

        $('#project-preview').on('afterChange', (event, slick, currentSlide) =>
        {
            this.updatePortfolioView();
            this.processingAnimation = false;
        });
    };
};
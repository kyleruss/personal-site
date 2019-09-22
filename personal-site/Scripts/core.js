$(function()
{
    var homeComponent = new HomeComponent();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    var portfolioComponent = new PortfolioComponent();
    var contactComponent = new ContactComponent();

    $('#module-container').fullpage
    ({
        fitToScreen: true,
        normalScrollElements: '#project-preview',
        onLeave: function(origin, destination, direction)
        {
            var navbarItem = $('.side-navbar-item');
            var destIndex = destination.index;
            var originIndex = origin.index;
            var sectionObj;

            if(destIndex < 5 && originIndex != 5) 
                $('.side-navbar-item.active').addClass('side-navbar-item-untoggled');
            
            switch(destIndex)
            {
                case 1:
                    sectionObj = aboutComponent;
                    break;

                case 2:
                    sectionObj = skillsComponent;
                    break;

                case 3:
                    sectionObj = portfolioComponent;
                    break;

                case 4:
                    sectionObj = contactComponent;
                    break;
            }

            
            setTimeout(() =>
            {

                if(destIndex == 3 || destIndex == 0)
                    portfolioComponent.toggleOffColorNavbar(true);
                else
                    portfolioComponent.toggleOffColorNavbar(false);

                if((destIndex != 5) || (originIndex == 5 && destIndex == 0))
                {
                    navbarItem.removeClass('active');
                    navbarItem.removeClass('side-navbar-item-untoggled');
                    navbarItem.eq(destIndex).addClass('active');
                }


                if(sectionObj != null) sectionObj.initDisplay();
            }, 300);
        }
    });

    $('#main-navbar a').click(function(e)
    {
        e.preventDefault();
        var linkIndex = $('#main-navbar a').index($(this)) + 1;

        if(linkIndex == 3) linkIndex++;

        fullpage_api.moveTo(linkIndex);
    });

    $('.side-navbar-item').click(function(e)
    {
        var navIndex = $('.side-navbar-item').index($(this)) + 1;
        fullpage_api.moveTo(navIndex);
    })
});
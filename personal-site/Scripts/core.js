$(function()
{
    Home();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    Portfolio();
    var contactComponent = new ContactComponent();

    $('#module-container').fullpage
    ({
        fitToScreen: true,
        onLeave: function(origin, destination, direction)
        {
            var navbarItem = $('.side-navbar-item');
            var destIndex = destination.index;
            var sectionObj;

            if(destIndex < 5) navbarItem.removeClass('active');
            
            switch(destIndex)
            {
                case 1:
                    sectionObj = aboutComponent;
                    break;

                case 2:
                    sectionObj = skillsComponent;
                    break;

                case 4:
                    sectionObj = contactComponent;
                    break;
            }

            
            setTimeout(() =>
            {
                if(destIndex < 5) navbarItem.eq(destIndex).addClass('active');

                if(sectionObj != null) sectionObj.initDisplay();
            }, 300);
        }
    });

    $('#main-navbar a').click(function(e)
    {
        e.preventDefault();
        var linkIndex = $('#main-navbar a').index($(this)) + 1;
        fullpage_api.moveTo(linkIndex);
    });

    $('.side-navbar-item').click(function(e)
    {
        var navIndex = $('.side-navbar-item').index($this);
        fullpage_api.moveTo(navIndex);
    })
});
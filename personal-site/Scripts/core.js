$(function()
{
    Home();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    Portfolio();
    var contactComponent = new ContactComponent();

    //$('body').niceScroll(scrollConfig);
    $('#module-container').fullpage({
        fitToScreen: true,
        onLeave: function(origin, destination, direction)
        {
            switch(destination.index)
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

            if(sectionObj != null)
            {
                setTimeout(() =>
                {
                    sectionObj.initDisplay();
                }, 500);
            }
        }
    });
});
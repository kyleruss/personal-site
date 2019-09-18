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

    $('#main-navbar a').click(function(e)
    {
        e.preventDefault();
        var linkIndex = $('#main-navbar a').index($(this)) + 1;
        fullpage_api.moveTo(linkIndex);
    });
});
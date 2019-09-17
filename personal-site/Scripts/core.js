$(function()
{
    Home();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    Portfolio();
    var contactComponent = new ContactComponent();

    //$('body').niceScroll(scrollConfig);
    $('#module-container').fullpage({
        fitToScreen: false
    });

    $(window).on('activate.bs.scrollspy', function()
    {
        var section = $('#main-navbar').find('a.active').attr('href');
        var sectionObj = null;

        switch(section)
        {
            case '#about-container':
                sectionObj = aboutComponent;
                break;

            case '#skills-container':
                sectionObj = skillsComponent;
                break;

            case '#contact-container':
                sectionObj = contactComponent;
                break;
        }

        if(sectionObj != null)
            sectionObj.initDisplay();
    });
});
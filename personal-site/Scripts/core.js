﻿$(function()
{
    var userScrollValue = 0;

    Home();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    Portfolio();
    var contactComponent = new ContactComponent();

    $('body').niceScroll(scrollConfig);

    $(window).scroll(function(e)
    {
        var scrollVal = $(this).scrollTop();
        checkPageNav(scrollVal);
    });


    function checkPageNav(scrollVal)
    {
        if(scrollVal > userScrollValue)
            toggleNavbar(false);
        else
            toggleNavbar(true);

            userScrollValue = scrollVal;
    };

    function toggleNavbar(show)
    {
        var navbar = $('#main-navbar');
        if(show) navbar.slideDown();
        else navbar.slideUp();
    };

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
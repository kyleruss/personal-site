$(function()
{
    var userScrollValue = 0;

    Home();
    About();
    Portfolio();
    Contact();

    $('body').niceScroll(scrollConfig);

    $(window).scroll(function(e)
    {
        var scrollVal = $(this).scrollTop();

        if(scrollVal > userScrollValue)
            toggleNavbar(false);
        else
            toggleNavbar(true);

            userScrollValue = scrollVal;
    });

    function toggleNavbar(show)
    {
        var navbar = $('#main-navbar');
        if(show) navbar.slideDown();
        else navbar.slideUp();
    };
});
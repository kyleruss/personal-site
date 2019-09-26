class HomeComponent
{
    constructor()
    {
        this.initHandlers();

        setTimeout(() =>
        {
            $('.rect-shape').addClass('rect-hover');
        }, 100);
    };

    initHandlers()
    {
        $('#scroll-bottom-btn').click(function(e)
        {   
            if(typeof fullpage_api !== 'undefined')
                fullpage_api.moveTo(2);
        });
    };
};
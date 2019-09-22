class AboutComponent
{
    constructor()
    {
        this.initHandlers();
        this.displayToggled = false;
        $('#about-image-addons').hide();
        $('#resume-btn').addClass('resume-btn-hidden');
    }

    initDisplay()
    {
        if(!this.displayToggled)
        {
            this.displayToggled = true;
            this.displayTitle();
            this.displayAboutText();
            $('#about-image-addons').fadeIn('slow');
        }
    };

    initHandlers()
    {
        $('#resume-btn').click(function(e)
        {
            fullpage_api.setAllowScrolling(false);
            $('#resume-modal').modal('show');
        });

        $('#resume-modal').on('hidden.bs.modal', function()
        {
            fullpage_api.setAllowScrolling(true);
        });
    };


    displayTitle()
    {
        $('#about-title').addClass('about-title-activated');
    }
    
    displayAboutText()
    {
        var aboutText = "Hello, I'm Kyle. Full-stack web & mobile developer based in Adelaide, AU";
        var i = 1;
        var textInterval = setInterval(() =>
        {
            if(i <= aboutText.length)
            {
                var aboutStr = aboutText.substring(0, i);
                $('#about-text').html(aboutStr);
                i++;
            }

            else 
            {
                clearInterval(textInterval);
                $('#resume-btn').animate({opacity: 1}, 200);
                var textNode = $('#about-text').contents().last().get(0);
                var lastNode = textNode.splitText(60);
                $(lastNode).wrap("<span class='location-highlight'/>");
            }
        }, 50); 

        setTimeout(() =>
        {
            
            $('#resume-btn').fadeIn('slow');
        }, 2000);
    };
};
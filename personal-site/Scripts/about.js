class AboutComponent
{
    constructor()
    {
        this.initHandlers();
        this.displayToggled = false;
        $('#resume-btn').addClass('resume-btn-hidden');
    }

    initDisplay()
    {
        if(!this.displayToggled)
        {
            this.displayToggled = true;
            this.displayTitle();
            this.displayAboutText();
        }
    };

    initHandlers()
    {
    
    };


    displayTitle()
    {
        $('#about-title').addClass('about-title-activated');
    }
    
    displayAboutText()
    {
        var aboutText = "Hello, I'm Kyle. I enjoy creating solutions for interesting problems";
        var i = 1;
        var textInterval = setInterval(() =>
        {
            if(i <= aboutText.length)
            {
                var aboutStr = aboutText.substring(0, i);
                $('#about-text').text(aboutStr); 
                i++;
            }

            else 
            {
                clearInterval(textInterval);
                $('#resume-btn').animate({opacity: 1}, 500);
            }
        }, 50); 

        setTimeout(() =>
        {
            $('#resume-btn').fadeIn('slow');
        }, 2000);
    };
};
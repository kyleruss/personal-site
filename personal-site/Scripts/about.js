function About()
{
    var aboutText = "Hello, I'm Kyle. I enjoy creating solutions for interesting problems";
    displayAboutText();

    function displayAboutText()
    {
        var i = 1;
        setInterval(() =>
        {
            if(i <= aboutText.length)
            {
                var aboutStr = aboutText.substring(0, i);
                $('#about-text').text(aboutStr); 
                i++;
            }

            else return;
        }, 100);
    };
};
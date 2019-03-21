function About()
{
    var skills =
    [
        "C++",
        "ANGULAR",
        "PHP",
        "CORDOVA",
        "IONIC",
        "ANDROID",
        "JQUERY",
        "SOCKET.IO",
        "SQL",
        "SASS/CSS",
        "HTML",
        "BULMA",
        "BOOTSTRAP",
        "C#/.NET",
        "JAVA",
        "NODE.JS"
    ];

    function rotateSkills()
    {
        var n = skills.length;
        var i = 0;

        setInterval(function()
        {
            var skillText   =   skills[i];
            var firstSkill  =   $('.skill-display').first()
            firstSkill.first().remove();

            var skillContainer = $('<div>', { 'class': 'skill-display' })
            .append($('<h1>').text(skillText));

            $('#skills-container').append(skillContainer);

            i = (i + 1) % n;

        }, 2000);
    };

    rotateSkills();
};
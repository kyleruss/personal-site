class SkillsComponent
{
    constructor()
    {
        $('.skill-shape').hide();
    };

    initDisplay()
    {
        $('#skill-title').addClass('skill-title-toggled');
        $('.skill-shape').fadeIn('slow');
    };
}
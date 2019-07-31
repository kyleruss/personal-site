$(function()
{
    function saveSocialSettings()
    {
        processForm($('#socialUpdateForm'), $('#social-update-btn'));
    };

    $('#social-update-btn').click((e) =>
    {
        e.preventDefault();
        saveSocialSettings();
    });
});
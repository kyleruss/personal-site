$(function()
{
    function verifyCredentials()
    {
        postAjaxForm($('#admin-login-form'), (data) =>
        {
            
        });
    };

    function verifyAuthCode()
    {

    };

    $('#admin-login-btn').click(function(e)
    {
        e.preventDefault();
        verifyCredentials();
    });
});
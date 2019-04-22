$(function()
{
    autoAuthCallbackReply();

    function autoAuthCallbackReply()
    {
        setTimeout(() =>
        {
            authCallbackReply();
        }, 3000);
    };

    function authCallbackReply()
    {
        var callbackKey = 'authCallbackStatus';
        var authStatus = $('#callback-container').attr('data-status');

        localStorage.removeItem(callbackKey);
        localStorage.setItem(callbackKey, authStatus);
    };  

    $('#main-navbar').hide();
    
    $('#social-callback-returnbtn').click(() =>
    {
        authCallbackReply();
    });
});
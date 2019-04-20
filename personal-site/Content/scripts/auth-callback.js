$(function()
{
    AuthCallbackReply();

    function AuthCallbackReply()
    {
        var callbackKey = 'authCallbackStatus';
        var authStatus = $('#callback-container').attr('data-status');

        localStorage.removeItem(callbackKey);
        localStorage.setItem(callbackKey, authStatus);
    };  

});
$(function()
{
    function verifyCredentials()
    {
        processForm($('#admin-login-form'), $('#admin-login-btn'), null, (data) =>
        {          
            if(data.ActionSuccess)
                $('#admin-login-modal').modal('show');
        });
    };

    function verifyAuthCode()
    {
        processForm($('#admin-auth-code-form'), $('#admin-auth-code-btn'), null, (data) =>
        {
            if(data.ActionSuccess)
            {
                var redirectUrl = data.Data.RedirectUrl;
                window.location.href = redirectUrl;
            }

        }, null, $('#auth-code-alert'));
    };

    $('#admin-auth-code-btn').click((e) =>
    {
        verifyAuthCode();
    });

    $('#admin-login-btn').click((e) =>
    {
        verifyCredentials();
    });
});
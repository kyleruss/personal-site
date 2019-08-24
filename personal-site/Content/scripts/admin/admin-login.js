$(function()
{
    function verifyCredentials()
    {
        postAjaxForm($('#admin-login-form'), (data) =>
        {
            console.log(data);
            
            if(data.ActionSuccess)
                $('#admin-login-modal').modal('show');

            else
            { 
                var alert = $('#login-alert');
                toggleAlertResponse(data, alert);
                setTimeout(() => { toggleAlertResponse(data, alert, true)}, 2000);
            }
        });
    };

    function verifyAuthCode()
    {
        postAjaxForm($('#admin-auth-code-form'), (data) =>
        {
            console.log(data);

            if(data.ActionSuccess)
            {
                var redirectUrl = data.Data.RedirectUrl;
                console.log(redirectUrl);
                window.location.href = redirectUrl;
            }

            else
            {
                var alert = $('#auth-code-alert');
                toggleAlertResponse(data, alert);
                setTimeout(() => { toggleAlertResponse(data, alert, true)}, 2000);
            }
        });
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
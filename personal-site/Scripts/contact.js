﻿function Contact()
{
    var CONTACT_STATUS_PROGRESS = 0,
        CONTACT_STATUS_SUCCESS = 1,
        CONTACT_STATUS_ERROR = 2,
        CONTACT_STATUS_RESET = 3;

    $('#contact-btn-progress').hide();
    $('#contact-btn-status').hide();

    $('#contact-form').submit(function(e)
    {
        e.preventDefault();

        var form = $(this);
        var contactUrl = form.attr('action');
        var contactData = form.serialize();
        var contactBtnSpinner = $('#contact-btn-progress');
        var contactBtnText = $('#contact-btn-text');

        updateContactButton(CONTACT_STATUS_PROGRESS);
        
        setTimeout(function ()
        {
            $.ajax
            ({
                url: contactUrl,
                method: 'POST',
                data: contactData,
                dataType: 'json',
                success: function(response)
                {
                    var respStatus = response.ActionSuccess;
                    
                    if (respStatus)
                    {
                        updateContactButton(CONTACT_STATUS_SUCCESS);
                        clearForm();
                    }

                    else
                    {
                        var errMsg  =   response.ResponseMsg;
                        updateContactButton(CONTACT_STATUS_ERROR, errMsg);
                    }

                    resetContactButton();
                },

                error: function(xhr, statusText, err)
                {
                    updateContactButton(CONTACT_STATUS_ERROR);
                    resetContactButton();
                }

            });

        }, 1500);
    });

    function updateContactButton(statusCode, errMsg)
    {
        var contactBtn = $('#contact-submit');
        var contactBtnSpinner = $('#contact-btn-progress');
        var contactBtnText = $('#contact-btn-text');
        var contactBtnStatusIcon = $('#contact-btn-status');
        var btnMsg = "";

        switch(statusCode)
        {
            case CONTACT_STATUS_PROGRESS:
                contactBtnSpinner.show();
                btnMsg = "SENDING MESSAGE";
                break;

            case CONTACT_STATUS_SUCCESS:
                contactBtnSpinner.hide();
                btnMsg = "MESSAGE HAS BEEN SENT";
                contactBtnStatusIcon.attr('class', 'fas fa-check-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-success');
                break;

            case CONTACT_STATUS_ERROR:
                contactBtnSpinner.hide();
                btnMsg = errMsg;
                contactBtnStatusIcon.attr('class', 'fas fa-times-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-error');
                break;
        }

        contactBtnText.text(btnMsg);
    }

    function resetContactButton()
    {
        setTimeout(function()
        {
            $('#contact-btn-status').hide();
            $('#contact-btn-progress').hide();
            $('#contact-btn-text').text('send message');
            $('#contact-submit').attr('class', 'btn btn-primary contact-btn-normal');
        }, 2000);
    }

    function clearForm()
    {
        $('#contact-name-field').val('');
        $('#contact-email-field').val('');
        $('#contact-message-field').val('');
    }
};
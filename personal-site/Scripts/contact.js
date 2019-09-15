class ContactComponent
{
    constructor()
    {
        this.CONTACT_STATUS_PROGRESS = 0,
        this.CONTACT_STATUS_SUCCESS = 1,
        this.CONTACT_STATUS_ERROR = 2,
        this.CONTACT_STATUS_RESET = 3;

        this.serviceRotateIndex = 0;

        $('#contact-btn-progress').hide();
        $('#contact-btn-status').hide();
        $('.slick-dots > li').slice(-2).hide();

        this.initCarousel();
        this.initHandlers();
    };

    initDisplay()
    {
        $('#contact-form-title').addClass('contact-form-title-toggled');
    };

    initHandlers()
    {
        $('#contact-form').submit(function(e)
        {
            e.preventDefault();

            var form = $(this);
            var contactUrl = form.attr('action');
            var contactData = form.serialize();
            var contactBtnSpinner = $('#contact-btn-progress');
            var contactBtnText = $('#contact-btn-text');

            this.updateContactButton(CONTACT_STATUS_PROGRESS);

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
                            this.updateContactButton(this.CONTACT_STATUS_SUCCESS);
                            this.clearForm();
                        }

                        else
                        {
                            var errMsg  =   response.ResponseMsg;
                            this.updateContactButton(this.CONTACT_STATUS_ERROR, errMsg);
                        }

                        resetContactButton();
                    },

                    error: function(xhr, statusText, err)
                    {
                        this.updateContactButton(this.CONTACT_STATUS_ERROR);
                        this.resetContactButton();
                    }

                });

            }, 1500);
        });
    };

    updateContactButton(statusCode, errMsg)
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
    };

    resetContactButton()
    {
        setTimeout(function()
        {
            $('#contact-btn-status').hide();
            $('#contact-btn-progress').hide();
            $('#contact-btn-text').text('send message');
            $('#contact-submit').attr('class', 'btn btn-primary contact-btn-normal');
        }, 2000);
    };

    clearForm()
    {
        $('#contact-name-field').val('');
        $('#contact-email-field').val('');
        $('#contact-message-field').val('');
    };

    initCarousel()
    {
        $('#service-list-container').slick
        ({
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 2,
            dots: true,
            arrows:false,
            autoplay:true,
            autoplaySpeed: 3000,
            accessibility: false
        });
    };
};
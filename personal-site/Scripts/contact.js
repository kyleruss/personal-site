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
        var context = this;
        $('#contact-form').submit(function(e)
        {
            e.preventDefault();

            var form = $(this);
            var contactUrl = form.attr('action');
            var contactData = form.serialize();

            context.updateContactButton(context.CONTACT_STATUS_PROGRESS);

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
                            context.updateContactButton(context.CONTACT_STATUS_SUCCESS);
                            context.clearForm();
                        }

                        else
                        {
                            var errMsg  =   response.ResponseMsg;
                            context.updateContactButton(context.CONTACT_STATUS_ERROR, errMsg);
                        }

                        context.resetContactButton();
                    },

                    error: function(xhr, statusText, err)
                    {
                        context.updateContactButton(context.CONTACT_STATUS_ERROR);
                        context.resetContactButton();
                    }

                });

            }, 1500);
        });

        $('.footer-info-description').hover(function()
        {
            $('#credits-heart').css('font-weight', 900);
        }, function()
        {
            $('#credits-heart').css('font-weight', 400);
        });

        $('#go-top-btn').click(function(e)
        {
            fullpage_api.moveTo(1);
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
            case this.CONTACT_STATUS_PROGRESS:
                contactBtnSpinner.show();
                btnMsg = "SENDING MESSAGE";
                break;

            case this.CONTACT_STATUS_SUCCESS:
                contactBtnSpinner.hide();
                btnMsg = "MESSAGE SENT";
                contactBtnStatusIcon.attr('class', 'fas fa-check-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-success');
                break;

            case this.CONTACT_STATUS_ERROR:
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
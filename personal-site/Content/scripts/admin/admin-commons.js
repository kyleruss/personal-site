var spinnerName = '.prog-spinner';
var alertName = '.prog-alert';

$(function()
{
    //Hide any progress spinners and alerts
    hideSpinners();
    hideAlerts();

    //Enable nice scrolling for the main component container
    $('#component-content-container').niceScroll(scrollConfig);
});

function hideSpinners()
{
    $(spinnerName).hide();
};

function hideAlerts()
{
    $(alertName).hide();
};

function startAjaxResponseOperation(element)
{
    element.children().hide();
    element.find(spinnerName).show();
};

function stopAjaxResponseOperation(responseObject, element, alert = null, alertDelay = 2000)
{
    element.find(spinnerName).hide();
    element.children().not(spinnerName).show();

    toggleAlertResponse(responseObject, alert);

    setTimeout(() => { toggleAlertResponse(responseObject, alert, true); }, alertDelay);
};

function toggleAlertResponse(responseObject, alert = null, hide = false)
{
    var alertElement = (alert != null) ? alert : $(alertName);

    if(!hide)
    {
        var alertClassName = alertName.substr(1);
        alertElement.find('.prog-alert-text').text(responseObject.ResponseMsg);
        var statusClass = responseObject.ActionSuccess ? 'alert-success' : 'alert-danger';
        alertElement.attr('class', 'alert ' +  alertClassName + ' ' + statusClass);
        alertElement.show();
    }

    else alertElement.hide();
};

function processForm(form, btn, formData = null, callback = null)
{
    startAjaxResponseOperation(btn);

    postAjaxForm(form, (data) =>
    {
        stopAjaxResponseOperation(data, btn);
        if(callback != null) callback(data);

    }, formData);
};

function postAjaxForm(form, callback, formData = null)
{
    var formUrl = form.attr('action');
    formData = formData != null? formData : form.serialize();
    
    $.post(formUrl, formData, (data) =>
    {
        callback(data);
    });
};

function delayStopAjaxResponse(responseObject, element, callback = null, alert = null, delay = 2000)
{
    setTimeout(() =>
    {
        stopAjaxResponseOperation(responseObject, element, alert);
        if(callback != null) callback();

    }, delay);
};
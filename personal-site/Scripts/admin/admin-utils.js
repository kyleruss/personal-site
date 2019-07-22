var spinnerName = '.prog-spinner';
var alertName = '.prog-alert';

$(spinnerName).hide();

function startAjaxResponseOperation(element)
{
    element.children().hide();
    element.find(spinnerName).show();
};

function stopAjaxResponseOperation(responseObject, element, alert = null, alertDelay = 2000)
{
    element.find(spinnerName).hide();
    element.children().not(spinnerName).show();

    alert = alert != null? alert : $(alertName);
    alertElement.find('.prog-alert-text').text(responseObject.ResponseMsg);
    var statusClass = responseObject.ActionSuccess ? 'alert-success' : 'alert-danger';
    alertElement.attr('class', 'alert ' + statusClass);
    alertElement.show();

    setTimeout(() => { alertElement.hide() }, alertDelay);
};

function delayStopAjaxResponse(responseObject, element, callback = null, delay = 2000)
{
    setTimeout(() =>
    {
        stopAjaxResponseOperation(responseObject, element);
        if(callback != null) callback();

    }, delay);
};
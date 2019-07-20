var spinnerName = '.prog-spinner';
var alertName = '.prog-alert';

function startAjaxResponseOperation(element)
{
    element.children().hide();
    element.find(spinnerName).show();
};

function stopAjaxResponseOperation(responseObject, element, alert = null, alertDelay = 2000)
{
    element.find(spinnerName).hide();
    element.children().not(spinnerName).show();

    var alertElement = alert != null? alert : $(alertName);
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
var spinnerName = '.prog-spinner';
var alertName = '.prog-alert';

//Hide any progress spinners and alerts
$(spinnerName).hide();
$(alertName).hide();

//Enable nice scrolling for the main component container
$('#component-content-container').niceScroll(scrollConfig);

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
    alertElement.find('.prog-alert-text').text(responseObject.ResponseMsg);
    var statusClass = responseObject.ActionSuccess ? 'alert-success' : 'alert-danger';
    alertElement.attr('class', 'alert ' + statusClass);
    alertElement.show();

    setTimeout(() => { alertElement.hide() }, alertDelay);
};

function delayStopAjaxResponse(responseObject, element, callback = null, alert = null, delay = 2000)
{
    setTimeout(() =>
    {
        stopAjaxResponseOperation(responseObject, element, alert);
        if(callback != null) callback();

    }, delay);
};
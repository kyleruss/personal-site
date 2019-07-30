$(function()
{
    function updateChannel()
    {

    };

    function addRssItem()
    {
        var btnElement = $('#rss-item-add-btn');
        startAjaxResponseOperation(btnElement);

        postAjaxForm($('#admin-rss-item-form'), (data) =>
        {
            stopAjaxResponseOperation(data, btnElement);
        });
    };

    function removeRssItem()
    {
        
    };

 /*   $('#channel-update-btn').click((e) =>
    {
        e.preventDefault();
        updateChannel();
    }); */

    $('#rss-item-add-btn').click((e) =>
    {
        e.preventDefault();
        addRssItem();
    });
});
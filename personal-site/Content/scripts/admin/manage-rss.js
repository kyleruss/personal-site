$(function()
{
    function updateChannel()
    {
        processForm($('#admin-rss-chanel-form'), $('#channel-update-btn'));
    };

    function addRssItem()
    {
        processForm($('#admin-rss-item-form'), $('#rss-item-add-btn'), null, (data) =>
        {
            var url = window.location.href;
            var listId = '#rss-items-list';

            $(listId).load(url + ' ' + listId, () =>
            {
                hideSpinners();
            });
            
        });
    };

    function removeRssItem(currentBtn)
    {
        var formData = currentBtn.attr('name') + '=' + currentBtn.attr('value');
        processForm($('#admin-rss-item-remove-form'), currentBtn, formData, (data) =>
        {
            if(data.ActionSuccess)
            {
                var currentItemElement =  currentBtn.closest('.rss-item');
                currentItemElement.remove();
            }
        });
    };

    $('.rss-remove-btn').click(function(e)
    {
        e.preventDefault();
        removeRssItem($(this));
    });

    $('#channel-update-btn').click((e) =>
    {
        e.preventDefault();
        updateChannel();
    }); 

    $('#rss-item-add-btn').click((e) =>
    {
        e.preventDefault();
        addRssItem();
    });
});
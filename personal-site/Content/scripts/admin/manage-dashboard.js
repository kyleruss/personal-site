$(function()
{
    loadStatData();

    function toggleSiteMode(toggleElement)
    {
        var toggleModeUrl = (toggleElement.attr('id') == 'maint-toggle-btn'? MaintModeUrl : ShutdownModeUrl);
        var enable = toggleElement.prop('checked');
        var toggleData = { enableMode: enable};
        
        $.post(toggleModeUrl, toggleData, (data) =>
        {
            toggleAlertResponse(data);

            setTimeout(() => { toggleAlertResponse(data, null, true)}, 1500);
        });
    }

    function loadStatData()
    {
        console.log(statisticsFetchUrl);

        $.getJSON(statisticsFetchUrl, (data) =>
        {
            console.log('DATA: ' + data);
        });
    };

    $('#maint-toggle-btn').change(function(e)
    {
        toggleSiteMode($(this));
    });

    $('#shutdown-toggle-btn').change(function(e)
    {
        toggleSiteMode($(this));
    });
});
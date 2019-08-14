$(function()
{
    var statData = {};

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

    //DATA FORMAT: {"MonthlyStatsModel":[{"Month":8,"UserCount":4}],"UserCountStats":{"MonthlyCount":4,"TotalCount":4}}
    function loadStatData()
    {
        console.log(statisticsFetchUrl);

        $.getJSON(statisticsFetchUrl, (data) =>
        {
            statData = $.parseJSON(data);
            setUserCountStats();
            console.log('DATA: ' + data);
        });
    };

    function setUserCountStats()
    {
        $('#user-total-stat-text').text(statData.UserCountStats.TotalCount);
        $('#user-monthly-stat-text').text(statData.UserCountStats.MonthlyCount);
    }

    $('#maint-toggle-btn').change(function(e)
    {
        toggleSiteMode($(this));
    });

    $('#shutdown-toggle-btn').change(function(e)
    {
        toggleSiteMode($(this));
    });
});
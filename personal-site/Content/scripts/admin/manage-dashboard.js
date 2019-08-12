$(function()
{
    loadStatData();

    function toggleShutdownMode()
    {

    };

    function toggleMaintenanceMode()
    {

    };

    function loadStatData()
    {
        console.log(statisticsFetchUrl);

        $.getJSON(statisticsFetchUrl, (data) =>
        {
            console.log('DATA: ' + data);
        });
    };
});
$(function()
{
    loadStatData();

    function loadStatData()
    {
        console.log(statisticsFetchUrl);

        $.getJSON(statisticsFetchUrl, (data) =>
        {
            console.log('DATA: ' + data);
        });
    };
});
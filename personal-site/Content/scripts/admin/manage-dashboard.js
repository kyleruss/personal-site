﻿$(function()
{
    var statData = {};
    var apiUsername = 'kyleruss';

    loadStatData();
    getGithubFollowerCount();

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
    };

    function getGithubFollowerCount()
    {
        var apiUrl = `https://api.github.com/users/${apiUsername}/followers`;

        $.getJSON(apiUrl, (data) =>
        {
            var followerCount = data.length;
            $('#github-stat-text').text(followerCount);
        });
    };

    function displayStatChart()
    {
        var chartContext = $('#user-stat-chart');
        var chartData = getChartMonthlyData();
        var chartLabels = getChartMonths();

        var chart = new Chart(chartContext,
        {
            type: 'line',
            data: 
            {
                labels: chartLabels,
                datasets: 
                [{
                    label: 'User monthly registrations',
                    borderColor: 'rgb(15, 250, 105)',
                    data: chartData
                }]
            }
        });
    };

    function getChartMonths()
    {
        return ['August', 'September', 'October', 'November', 'December'];
    }

    function getChartMonthlyData()
    {
        var monthlyStatData = [];
        var maxMonths = 5;

        $.each(statData.MonthlyStatsModel, function(key, statObj)
        {
            monthlyStatData.push(statObj.UserCount);
        });

        for(var i = monthlyStatData.length; i < maxMonths; i++)
            monthlyStatData.push(0);

        return monthlyStatData;
    };

    //DATA FORMAT: {"MonthlyStatsModel":[{"Month":8,"UserCount":4}],"UserCountStats":{"MonthlyCount":4,"TotalCount":4}}
    function loadStatData()
    {
        $.getJSON(statisticsFetchUrl, (data) =>
        {
            statData = $.parseJSON(data);
            setUserCountStats();
            displayStatChart();
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
$(function()
{
    ManageRepos();
    ManageBlog();
    ManageRss();

    $('#component-content-container').niceScroll(scrollConfig);
});
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
function ManageRepos()
{
    var repoData = {};

    loadRepos();
    $('#repo-list-body').niceScroll(scrollConfig);

    function loadRepos()
    {
        var repoTableContainer = $('#repo-table-body');

        $.getJSON(repoFetchUrl, (data) =>
        {
            repoData = data;
            $.each(repoData, (index, item) =>
            {
                var rowElement = $('<tr/>');

                createRepoTableCell('name', item, rowElement);
                createRepoTableCell('description', item, rowElement);
                createRepoTableCell('languages', item, rowElement);
                createRepoControls(rowElement);

                repoTableContainer.append(rowElement);
            });
        });
    };

    function createRepoControls(rowElement)
    {
        var cellElement = $('<td/>');
        cellElement.attr('class', 'repo-controls-cell');

        var editControlElement = $('<i/>');
        editControlElement.attr('class', 'far fa-edit repo-control-btn repo-edit-btn');
        
        var removeControlElement = $('<i/>');
        removeControlElement.attr('class', 'fas fa-times repo-control-btn repo-remove-btn');

        cellElement.append(editControlElement);
        cellElement.append(removeControlElement);
        rowElement.append(cellElement);
    }

    function createRepoTableCell(name, item, rowElement)
    {
        var itemElement = $('<td/>');
        var itemValue = (name == 'languages') ? getRepoLanguages(item) : item[name];
        
        itemElement.html(itemValue);
        rowElement.append(itemElement);

        return itemElement;
    };

    function removeRepo(cell)
    {
        var repoId = getRepoRowName(cell);
        var formData = { repoName: repoId };
        var removeModal = $('#repo-remove-modal');

        $('.repo-remove-status-icon').hide();

        $.post(repoRemoveUrl, formData, function(data)
        {
            var removeStatus = data.ActionSuccess;
            $('#removal-modal-text').text(data.ResponseMsg);

            if(removeStatus)
            {
                $('#repo-remove-success').show();

                var parentRow = cell.closest('tr');
                parentRow.remove();
                delete repoData[repoId];
            }

            else $('#repo-remove-fail').show();
            
            removeModal.modal('show');
        });
    };

    function saveRepo(btnElement)
    {
        var form = $('#repo-edit-form');
        var repoUrl = form.attr('action');
        var repoData = form.serialize();
        var alertElement = $('#repo-save-alert');

        startAjaxResponseOperation(btnElement);

        $.post(repoUrl, repoData, function(data)
        {
            stopAjaxResponseOperation(data, btnElement, alertElement);
        });
    };

    function getRepoLanguages(repo)
    {
        var lang = repo['languages'];
        return lang.length > 0 ? lang.join(", ") : lang;
    }

    function editRepo(cell)
    {
        var repoId = getRepoRowName(cell);
        var repository = repoData[repoId];
        
        $('#repo-edit-name').val(repository['name']);
        $('#repo-edit-desc').val(repository['description']);
        $('#repo-edit-lang').val(getRepoLanguages(repository));
        $('#repo-edit-code').val(repository['codeLines']);
        $('#repo-edit-readme').val(repository['readme']);

        $('#repo-edit-modal').modal('show');
    };
    
    function getRepoRowName(cell)
    {
        var parentRow = cell.closest('tr');   
        return parentRow.find('td').first().html();
    }

    $('#repo-save-btn').click(function(e)
    {
        e.preventDefault();
        saveRepo($(this));
    }); 

    $(document).on('click', '.repo-edit-btn', function(e)
    {
        editRepo($(this));
    });

    $(document).on('click', '.repo-remove-btn', function(e)
    {
        removeRepo($(this));
    });
};
function ManageBlog()
{
    $('.blog-edit-btn').click((e) =>
    {
        var blogId = $(e.target).closest('.blog-post-item').attr('data-blogId');
        var postUrl = blogPostUrl + "/" + blogId;
        
        $.getJSON(postUrl, (data) =>
        {
            var jsonData = $.parseJSON(data);
            var blogModal = $('#blog-edit-modal');

            $('#blog-edit-title').val(jsonData.Title);
            $('#blog-edit-desc').val(jsonData.Description);
            $('#blog-edit-area').val(jsonData.PostContent);
            $('#blog-edit-id').val(jsonData.PostId);

            blogModal.modal('show');
        });

    });

    $('#blog-new-btn').click((e) =>
    {
        $('#blog-edit-id').val('');
        $('#blog-edit-modal').modal('show');
    });
};
function ManageRss()
{
    function updateChannel()
    {

    }

    function addRssItem()
    {

    }

    function removeRssItem()
    {
        
    }

    $('#channel-update-btn').click((e) =>
    {
        e.preventDefault();
    });

    $('#rss-item-add-btn').click((e) => 
    {
        e.preventDefault();
    });
};
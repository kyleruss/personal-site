$(function()
{
    ManageRepos();
    ManageBlog();
    ManageRss();

    $('#component-content-container').niceScroll(scrollConfig);
});
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
        var itemValue = (name == 'languages' && item[name].length > 0) ? item[name].join(", ") : item[name];
        
        itemElement.html(itemValue);
        rowElement.append(itemElement);

        return itemElement;
    };

    function removeRepo(cell)
    {
        var repoId = getRepoRowName(cell);
        var formData = { repoName: repoId };

        $.post(repoRemoveUrl, formData, function(data)
        {
            var removeStatus = data.ActionSuccess;

            if(removeStatus)
            {
                var parentRow = cell.closest('tr');
                parentRow.remove();
                delete repoData[repoId];
            }
        });
    };

    function editRepo(cell)
    {
        var repoId = getRepoRowName(cell);
        $('#repo-edit-modal').modal('show');
    };
    
    function getRepoRowName(cell)
    {
        var parentRow = cell.closest('tr');   
        return parentRow.find('td').first().html();
    }

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
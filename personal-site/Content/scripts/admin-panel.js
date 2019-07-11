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

                repoTableContainer.append(rowElement);
            });
        });
    };

    function createRepoTableCell(name, item, rowElement)
    {
        var itemElement = $('<td/>');
        var itemValue = (name == 'languages' && item[name].length > 0) ? item[name].join(", ") : item[name];
        
        itemElement.html(itemValue);
        rowElement.append(itemElement);

        return itemElement;
    };
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
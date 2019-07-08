$(function()
{
    ManageRepos();
    ManageBlog();
    ManageRss();

    $('#component-content-container').niceScroll(scrollConfig);
});
function ManageRepos()
{
    
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
            console.log(data);
            console.log('data: ' + jsonData.Title);

            $('#blog-edit-title').val(jsonData.Title);
            $('#blog-edit-desc').val(jsonData.Description);
            $('#blog-edit-area').val(jsonData.PostContent);
            blogModal.attr('data-blogId', jsonData.PostId);

            blogModal.modal('show');
        });
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
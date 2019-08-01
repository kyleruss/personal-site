$(function()
{
    function getBlogPostInfo(target)
    {
        var blogId = $(target).closest('.blog-post-item').attr('data-blogId');
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
    };

    
    function saveBlogPost()
    {
        processForm($('#blog-edit-form'), $('#blog-save-btn'));
    };

   

    $('.blog-edit-btn').click((e) =>
    {
        getBlogPostInfo(e.target);
    });

    $('#blog-new-btn').click((e) =>
    {
        $('#blog-edit-id').val('');
        $('#blog-edit-modal').modal('show');
    });

    $('#blog-save-btn').click((e) =>
    {
        e.preventDefault();
        saveBlogPost();
    });
});
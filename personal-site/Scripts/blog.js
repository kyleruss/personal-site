function Blog()
{
    var currentBlog;
    var blogList;

    $('.blog-post-display').hover((e) =>
    {   
        e.stopImmediatePropagation();
        var innerContainer = $(e.target).parent().find('.blog-post-inner-container');
        innerContainer.toggleClass('blog-hover-effect');
    });

    $('.blog-post-display').click(() =>
    {
        var blogModal = $('#blog-modal');

        updateBlogModal();
        blogModal.modal();
    });

    $('#blog-modal-close').click(() =>
    {
        $('#blog-modal').modal('hide');
    });

    function updateBlogModal()
    {
        var modalTitle = $('#blog-modal-title');
        var modalBody = $('#blog-modal-body');

        modalTitle.text(currentBlog.title);
        modalTitle.text(currentBlog.content);
    };
};
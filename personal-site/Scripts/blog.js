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

        blogModal.modal();
    });

    $('#blog-modal-close').click(() =>
    {
        $('#blog-modal').modal('hide');
    });


};
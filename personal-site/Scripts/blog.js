function Blog()
{
    var currentBlog;
    var blogList;
    var currentPage;

    loadBlogPosts();

    function updateBlogModal()
    {
        var modalTitle = $('#blog-modal-title');
        var modalBody = $('#blog-modal-body');

        modalTitle.text(currentBlog.Title);
        modalBody.text(currentBlog.PostContent);
    };

    function loadBlogPosts()
    {
        var url = $('blog-data').attr('url');
        $.getJSON(url, (data) =>
        {
            blogList = JSON.parse(data);
            initPages();
        });
    };

    function initBlogList(page)
    {
        var blogDisplays = $('.blog-post-display');
        blogDisplays.hide();

        var totalPerPage = 3;
        var actualLength = blogList.length;
        var total = currentPage * totalPerPage;
        var startIndex = (currentPage - 1) * totalPerPage;
        total = total > actualLength? actualLength : total;

        for(var elementIndex = 0; startIndex < total; elementIndex++, startIndex++)
        {
            var blogTitle = blogList[startIndex].Title;
            var blogElement = $(blogDisplays[elementIndex]);
            
            blogElement.find('.blog-post-title').text(blogTitle);
            blogElement.attr('data-blog-index', startIndex);
            blogElement.show();
        };
    };

    function initPages()
    {
        var totalPages = getTotalPages();

        $('#post-pagination-total').text(totalPages);
        setCurrentPage(1);
    };

    function setCurrentPage(page)
    {
        currentPage = page;
        $('#post-pagination-current').text(currentPage);
        initBlogList(currentPage);
    }

    function nextPage()
    {
        var totalPages = getTotalPages();

        if(currentPage < totalPages)
            setCurrentPage(currentPage + 1);
    };

    function prevPage()
    {
        if(currentPage > 1)
            setCurrentPage(currentPage - 1);
    };

    function getTotalPages()
    {
        var n = blogList.length;
        return Math.ceil(n / 3);
    };

    $('.blog-post-display').hover((e) =>
    {   
        e.stopImmediatePropagation();
        var innerContainer = $(e.target).parent().find('.blog-post-inner-container');
        innerContainer.toggleClass('blog-hover-effect');
    });

    $('.blog-post-display').click((e) =>
    {
        var blogModal = $('#blog-modal');
        var blogIndex = $(e.target).parent().attr('data-blog-index');
        currentBlog = blogList[blogIndex];

        updateBlogModal();
        blogModal.modal();
    });

    $('#blog-modal-close').click(() =>
    {
        $('#blog-modal').modal('hide');
    });

    $('#left-post-btn').click(() =>
    {
        prevPage();
    });

    $('#right-post-btn').click(() =>
    {
        nextPage();
    });
};
var scrollConfig =
{
    horizrailenabled: false
};
$(function()
{
    Home();
    About();
    Portfolio();
    Contact();

    $('body').niceScroll(scrollConfig);
});
function Home()
{
    $('.rect-shape').toggleClass('rect-hover');
};
function About()
{
    var skills =
    [
        "C++",
        "ANGULAR",
        "PHP",
        "CORDOVA",
        "IONIC",
        "ANDROID",
        "JQUERY",
        "SOCKET.IO",
        "SQL",
        "SASS/CSS",
        "HTML",
        "BULMA",
        "BOOTSTRAP",
        "C#/.NET",
        "JAVA",
        "NODE.JS"
    ];

    function rotateSkills()
    {
        var n = skills.length;
        var i = 0;
        var enable = false;

        if (enable) {
            setInterval(function () {
                var skillText = skills[i];
                var firstSkill = $('.skill-display').first()
                firstSkill.first().remove();

                var skillContainer = $('<div>', { 'class': 'skill-display' })
                .append($('<h1>').text(skillText));

                $('#skills-container').append(skillContainer);

                i = (i + 1) % n;

            }, 2000);
        }
    };

    rotateSkills();
};
function Blog()
{
    var currentBlog;
    var blogList;
    var currentPage;

    loadBlogPosts();
    toggleCommentSpinner(false, false);
    $('#comment-alert').hide();
    $('#blog-comment-template').hide(); 

    var authWindow;
    var modalHideReady = false;

    function updateBlogModal()
    {
        var modalTitle = $('#blog-modal-title');
        var modalBody = $('#blog-modal-body');

        modalTitle.text(currentBlog.Title);
        modalBody.html(currentBlog.PostContent);
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
        //Hide the displays and re-display them 
        var blogDisplays = $('.blog-post-display');
        blogDisplays.hide();

        //Calculate and init the starting index and iterating total
        var totalPerPage = 3; //Number of posts to display per page
        var actualLength = blogList.length; //The real total number of posts
        var total = currentPage * totalPerPage; //The expected number of posts
        var startIndex = (currentPage - 1) * totalPerPage; //The index to start iterating at page 1: 0, page 2: 3 etc.
        total = total > actualLength? actualLength : total; //If the expected total is larger than the actual total use the actual total

        //The blog post background gradient
        //This style is injected into the elements
        var gradientColor = "rgba(64, 87, 122, 0.6)";
        var linearBackgroundStyle = `linear-gradient(${gradientColor}, ${gradientColor})`;

        //Display the blog posts and update their content 
        //Posts are displayed based on their current page
        //Up to {totalPerPage} posts shown per page
        for(var elementIndex = 0; startIndex < total; elementIndex++, startIndex++)
        {
            var blogTitle = blogList[startIndex].Title;
            var blogBackground = blogList[startIndex].PostImage;
            var blogElement = $(blogDisplays[elementIndex]);
            
            //Inject the background images and linear gradient
            var displayBackgroundStyle = `${linearBackgroundStyle}, url("${blogBackground}")`;
            var innerElement = blogElement.find('.blog-post-inner-container');
            innerElement.css('background', displayBackgroundStyle);

            //Update the content and display the post
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

    function toggleCommentSpinner(show, showSuccess)
    {
        var commentBtn = $('#blog-comment-btn');
        var commentBtnText = $('#blog-save-btn-text');
        var commentBtnSpin = $('#blog-save-btn-spinner');
        var successIcon = $('#blog-save-success');

        if(show)
        {
            commentBtnText.hide();
            commentBtnSpin.show();
            commentBtn.attr('disabled', true);
        }

        else
        {
            commentBtnSpin.hide();
            commentBtn.attr('disabled', false);

            if(!showSuccess)
                commentBtnText.show();
            else
            {
                successIcon.show();
                commentBtn.addClass('btn-success');
                commentBtn.removeClass('btn-primary');

                setTimeout(() => 
                {
                    commentBtn.addClass('btn-primary');
                    commentBtn.removeClass('btn-success');
                    successIcon.hide();
                    commentBtnText.show();

                }, 1500);
            }
        }
    };

    function commentAddSuccess(responseData)
    {
        var comment = responseData.Data;
        currentBlog.comments.push(comment);
        
        var commentElement = createCommentElement(comment);
        var commentContainer = $('#blog-comments');

        commentContainer.prepend(commentElement);
        commentElement.fadeIn();
    }

    function handleCommentInvalidInput(data)
    {
        var commentAlert = $('#comment-alert');
        $('#comment-alert-msg').text(data.ResponseMsg);
        commentAlert.fadeIn();
        
        setTimeout(() =>
        {
            commentAlert.fadeOut();
        }, 2000);
    }

    function postComment()
    {
        var commentForm = $('#post-comment-form');
        var contentElement = $('#comment-form-content');
        
        var commentUrl = commentForm.attr('action');
        var commentContent = contentElement.val();
        var postId = currentBlog.PostId;
        var commentData = { Content: commentContent, PostId: postId };
        commentData = JSON.stringify(commentData);

        toggleCommentSpinner(true);  

        $.ajax
        ({
            url: commentUrl,
            type: 'POST',
            data: commentData,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: (data) =>
            {
                setTimeout(() =>
                {
                    toggleCommentSpinner(false, data.ActionSuccess);

                    if(!data.ActionSuccess)
                        handleCommentInvalidInput(data);

                    else
                    {
                        contentElement.val('');
                        commentAddSuccess(data);
                    }

                    console.log(data);

                }, 2000);
            },
            error: (xhr, textStatus, err) =>
            {
                console.log('[Error]: ' + xhr.responseText);
            }
        }); 
    };

    function getBlogComments()
    {
        var commentsUrl = $('blog-comment-data').attr('url');
        var commentContainer = $('#blog-comments');
        var postId = currentBlog.PostId;

        commentContainer.empty();

        $.getJSON(commentsUrl, { PostId: postId }, (data) =>
        {
            currentBlog.comments = JSON.parse(data);

            currentBlog.comments.forEach((comment) =>
            {
                var commentElement = createCommentElement(comment);
                commentElement.show();
                commentContainer.prepend(commentElement);
            });
        });
    };

    function createCommentElement(comment)
    {
        var templateElement = $('#blog-comment-template');
        var commentElement = templateElement.clone();

        commentElement.removeAttr('id');
        commentElement.attr('data-commentId', comment.CommentId);
        commentElement.find('.blog-comment-text').text(comment.CommentContent);

        var commentUser = comment.User;

        if(commentUser != null)
        {
            commentElement.find('.blog-comment-user').text(commentUser.DisplayName);

            var profilePicture = commentUser.ProfilePicture;

            if(profilePicture != null)
            {
                commentElement.find('.user-picture-placeholder').hide();
                commentElement.find('.blog-comment-img').css('background-image', `url("${profilePicture}"`);
            }
        }
        
        return commentElement;
    };

    function closeAuthWindow()
    {
        if(authWindow != null)
            authWindow.close();
    }

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
        getBlogComments();
    });

    $('#blog-modal-close').click(() =>
    {
        $('#blog-modal').removeClass('show');
    });

    $('#blog-modal').on('transitionend', () =>
    {
        var modal = $('#blog-modal');

        if(!modal.hasClass('show'))
        {
            modalHideReady = true;
            modal.modal('hide');
            modalHideReady = false;
        }
        else
        {
            $('body').getNiceScroll().remove();

            $('#blog-comments').getNiceScroll().remove();
            $('#blog-comments').niceScroll(scrollConfig);

            $('#blog-modal').getNiceScroll().remove();
            $('#blog-modal').niceScroll(scrollConfig);
        }
    }); 

    $('#left-post-btn').click(() =>
    {
        prevPage();
    });

    $('#right-post-btn').click(() =>
    {
        nextPage();
    });

    $('#blog-comment-btn').click((e) =>
    {
        e.preventDefault();
        postComment();
    });

    $('#comment-toggle-btn').click(() =>
    {
        $('#blog-comments').slideToggle(300);
    });

    $('#blog-modal').on('hidden.bs.modal', () =>
    {
        $('body').niceScroll(scrollConfig);
    });

    $('#blog-modal').on('hide.bs.modal', (e) =>
    {
        if(!modalHideReady)
        {
            e.preventDefault();
            $('#blog-modal').removeClass('show');
        } 
    });

    $('#social-auth-form').submit((e) =>
    {
        var target = 'authFormTarget';
        var form = $(e.target);
        
        authWindow = window.open('', target, 'width=900,height=600');
        form.attr('target', target);
    });

    $(window).bind('storage', function(e)
    {
        var storageKey = 'authCallbackStatus';
        var callbackStatus = localStorage.getItem(storageKey);
        
        if(callbackStatus != null)
        {
            console.log('status: ' + callbackStatus);
            closeAuthWindow();
        }
    });
    
};
function Portfolio()
{
    var repoData = {};
    var currentRepo;
    var repos;
    var repoIndex;

    loadRepoData();

    function loadRepoData()
    {
        var dataUrl = $('repo-data').attr('url');
        
        $.getJSON(dataUrl, (data) =>
        {
            repoData = data;
            repos = Object.keys(repoData);
            setCurrentRepository(0);
        });
    };

    function setCurrentRepository(index)
    {
        repoIndex = index;
        repoName = repos[repoIndex];
        currentRepo = repoData[repoName];

        updatePortfolioView();
    };

    function nextRepository()
    {
        var nextIndex = repoIndex + 1;
        var n = repos.length;

        if(nextIndex >= repos.length)
            nextIndex = nextIndex % n;
        
        setCurrentRepository(nextIndex);
    };

    function prevRepository()
    {
        var prevIndex = repoIndex - 1;
        var n = repos.length;
        
        if(prevIndex < 0)
            prevIndex = ((prevIndex % n) + n) % n;

        setCurrentRepository(prevIndex);
    };

    function updatePortfolioView()
    {
        var projectContainer = $('#project-preview');
        var commitStats = $('#commits-stats');
        var codeStats = $('#codeline-stats');
        var githubLinkBtn = $('#view-github-btn');
        var repoTitle = $('#project-title');
        var repoDesc = $('#project-description');

        projectContainer.html(currentRepo["readme"]);
        commitStats.text(currentRepo["commits"]);
        codeStats.text(currentRepo["codeLines"]);
        githubLinkBtn.attr('href', currentRepo["link"]);
        repoTitle.text(getTransformedTitle());
        repoDesc.text(currentRepo["description"]);
        updateLanguages();
    };

    function updateLanguages()
    {
        var languages = currentRepo["languages"];
        var numLang = languages.length;
        var langIcon = $('#language-stat-icon');
        $('.language-stat').hide();

        if(numLang == 0)
            langIcon.hide();
        else
            langIcon.show();
        
        for(var i = 0; i < numLang; i++)
        {   
            var languageElement = $('.language-stat').eq(i);
            var langTextElement = languageElement.find('.language-stat-text');
            var language = languages[i];

            langTextElement.text(language);
            languageElement.show();
        }
    };

    function getTransformedTitle()
    {
        return currentRepo["name"].replace(/-/g, " ");
    }

    $('#portf-left-arrow').click(() =>
    {
        prevRepository();
    });

    $('#portf-right-arrow').click(() =>
    {
        nextRepository();
    });

    $('#project-preview').niceScroll
    ({
        horizrailenabled: false
    });
};
function Contact()
{
    var CONTACT_STATUS_PROGRESS = 0,
        CONTACT_STATUS_SUCCESS = 1,
        CONTACT_STATUS_ERROR = 2,
        CONTACT_STATUS_RESET = 3;

    var serviceRotateIndex = 0;

    $('#contact-btn-progress').hide();
    $('#contact-btn-status').hide();
    $('.service-container').slice(-2).hide();
    rotateServices();

    $('#contact-form').submit(function(e)
    {
        e.preventDefault();

        var form = $(this);
        var contactUrl = form.attr('action');
        var contactData = form.serialize();
        var contactBtnSpinner = $('#contact-btn-progress');
        var contactBtnText = $('#contact-btn-text');

        updateContactButton(CONTACT_STATUS_PROGRESS);

        setTimeout(function ()
        {
            $.ajax
            ({
                url: contactUrl,
                method: 'POST',
                data: contactData,
                dataType: 'json',
                success: function(response)
                {
                    var respStatus = response.ActionSuccess;
                    
                    if (respStatus)
                    {
                        updateContactButton(CONTACT_STATUS_SUCCESS);
                        clearForm();
                    }

                    else
                    {
                        var errMsg  =   response.ResponseMsg;
                        updateContactButton(CONTACT_STATUS_ERROR, errMsg);
                    }

                    resetContactButton();
                },

                error: function(xhr, statusText, err)
                {
                    updateContactButton(CONTACT_STATUS_ERROR);
                    resetContactButton();
                }

            });

        }, 1500);
    });

    function updateContactButton(statusCode, errMsg)
    {
        var contactBtn = $('#contact-submit');
        var contactBtnSpinner = $('#contact-btn-progress');
        var contactBtnText = $('#contact-btn-text');
        var contactBtnStatusIcon = $('#contact-btn-status');
        var btnMsg = "";

        switch(statusCode)
        {
            case CONTACT_STATUS_PROGRESS:
                contactBtnSpinner.show();
                btnMsg = "SENDING MESSAGE";
                break;

            case CONTACT_STATUS_SUCCESS:
                contactBtnSpinner.hide();
                btnMsg = "MESSAGE HAS BEEN SENT";
                contactBtnStatusIcon.attr('class', 'fas fa-check-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-success');
                break;

            case CONTACT_STATUS_ERROR:
                contactBtnSpinner.hide();
                btnMsg = errMsg;
                contactBtnStatusIcon.attr('class', 'fas fa-times-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-error');
                break;
        }

        contactBtnText.text(btnMsg);
    };

    function resetContactButton()
    {
        setTimeout(function()
        {
            $('#contact-btn-status').hide();
            $('#contact-btn-progress').hide();
            $('#contact-btn-text').text('send message');
            $('#contact-submit').attr('class', 'btn btn-primary contact-btn-normal');
        }, 2000);
    };

    function clearForm()
    {
        $('#contact-name-field').val('');
        $('#contact-email-field').val('');
        $('#contact-message-field').val('');
    };



    function setServicePage(page)
    {
        var serviceContainer = $('.service-container');

        $('.service-page').removeClass('service-page-active');
        $('.service-page').eq(page).addClass('service-page-active');
        
        while(page != serviceRotateIndex)
        {
            var nextIndex = (serviceRotateIndex + 3) % 5;
            if (page < serviceRotateIndex)
            {
                serviceContainer.eq(nextIndex - 1).hide();
                serviceContainer.eq(serviceRotateIndex - 1).show();
                serviceRotateIndex--;
            }

            else
            {
                serviceContainer.eq(serviceRotateIndex).hide();
                serviceContainer.eq(nextIndex).show();
                serviceRotateIndex++;
            }
        }
    };

    function rotateServices()
    {
        setInterval(() =>
        {
            var pageIndex = (serviceRotateIndex + 1) % 3;
            setServicePage(pageIndex);
        }, 2000);
    };

    $('.service-page').click(function()
    {
        $('.service-page').removeClass('service-page-active');
        var pageIndex = $('.service-page').index($(this));
        setServicePage(pageIndex);
    });
};
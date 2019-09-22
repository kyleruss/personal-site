var scrollConfig =
{
    horizrailenabled: false,
    cursorcolor: "rgba(114, 97, 238)",
    zindex: 999999,
    cursorborder: "1px solid #000",
    cursorwidth: 12
};
$(function()
{
    var homeComponent = new HomeComponent();
    var aboutComponent = new AboutComponent();
    var skillsComponent = new SkillsComponent();
    var portfolioComponent = new PortfolioComponent();
    var contactComponent = new ContactComponent();

    $('#module-container').fullpage
    ({
        fitToScreen: true,
        normalScrollElements: '#project-preview',
        onLeave: function(origin, destination, direction)
        {
            var navbarItem = $('.side-navbar-item');
            var destIndex = destination.index;
            var originIndex = origin.index;
            var sectionObj;

            if(destIndex < 5 && originIndex != 5) 
                $('.side-navbar-item.active').addClass('side-navbar-item-untoggled');
            
            switch(destIndex)
            {
                case 1:
                    sectionObj = aboutComponent;
                    break;

                case 2:
                    sectionObj = skillsComponent;
                    break;

                case 3:
                    sectionObj = portfolioComponent;
                    break;

                case 4:
                    sectionObj = contactComponent;
                    break;
            }

            
            setTimeout(() =>
            {

                if(destIndex == 3 || destIndex == 0)
                    portfolioComponent.toggleOffColorNavbar(true);
                else
                    portfolioComponent.toggleOffColorNavbar(false);

                if((destIndex != 5) || (originIndex == 5 && destIndex == 0))
                {
                    navbarItem.removeClass('active');
                    navbarItem.removeClass('side-navbar-item-untoggled');
                    navbarItem.eq(destIndex).addClass('active');
                }


                if(sectionObj != null) sectionObj.initDisplay();
            }, 300);
        }
    });

    $('#main-navbar a').click(function(e)
    {
        e.preventDefault();
        var linkIndex = $('#main-navbar a').index($(this)) + 1;

        if(linkIndex == 3) linkIndex++;

        fullpage_api.moveTo(linkIndex);
    });

    $('.side-navbar-item').click(function(e)
    {
        var navIndex = $('.side-navbar-item').index($(this)) + 1;
        fullpage_api.moveTo(navIndex);
    })
});
class HomeComponent
{
    constructor()
    {
        this.initHandlers();

        setTimeout(() =>
        {
            $('.rect-shape').addClass('rect-hover');
        }, 100);
    };

    initHandlers()
    {
        $('#scroll-bottom-btn').click(function(e)
        {   
            fullpage_api.moveTo(2);
        });
    };
};
class AboutComponent
{
    constructor()
    {
        this.initHandlers();
        this.displayToggled = false;
        $('#about-image-addons').hide();
        $('#resume-btn').addClass('resume-btn-hidden');
    }

    initDisplay()
    {
        if(!this.displayToggled)
        {
            this.displayToggled = true;
            this.displayTitle();
            this.displayAboutText();
            $('#about-image-addons').fadeIn('slow');
        }
    };

    initHandlers()
    {
        $('#resume-btn').click(function(e)
        {
            fullpage_api.setAllowScrolling(false);
            $('#resume-modal').modal('show');
        });

        $('#resume-modal').on('hidden.bs.modal', function()
        {
            fullpage_api.setAllowScrolling(true);
        });
    };


    displayTitle()
    {
        $('#about-title').addClass('about-title-activated');
    }
    
    displayAboutText()
    {
        var aboutText = "Hello, I'm Kyle. Full-stack web & mobile developer based in Adelaide, AU";
        var i = 1;
        var textInterval = setInterval(() =>
        {
            if(i <= aboutText.length)
            {
                var aboutStr = aboutText.substring(0, i);
                $('#about-text').html(aboutStr);
                i++;
            }

            else 
            {
                clearInterval(textInterval);
                $('#resume-btn').animate({opacity: 1}, 200);
                var textNode = $('#about-text').contents().last().get(0);
                var lastNode = textNode.splitText(60);
                $(lastNode).wrap("<span class='location-highlight'/>");
            }
        }, 50); 

        setTimeout(() =>
        {
            
            $('#resume-btn').fadeIn('slow');
        }, 2000);
    };
};
class SkillsComponent
{
    constructor()
    {
        
    };

    initDisplay()
    {
        $('#skill-title').addClass('skill-title-toggled');
    };
}
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
class PortfolioComponent
{
    constructor()
    {
        this.repoData = {};
        this.existingRepos = [];

        this.loadRepoData();
        this.initHandlers();
    };

    initDisplay()
    {
        this.setCurrentRepository(1);
    };

    toggleOffColorNavbar(show)
    {
        if(show)
            $('.side-navbar-item').addClass('side-navbar-offcolor');
        else
            $('.side-navbar-item').removeClass('side-navbar-offcolor');
    }

    loadRepoData()
    {
        var dataUrl = $('repo-data').attr('url');
        
        $.getJSON(dataUrl, (data) =>
        {
            this.repoData = data;
            this.repos = Object.keys(this.repoData);
            this.initCarousel();
        });
    };

    setCurrentRepository(index)
    {
        this.repoIndex = index;
        this.repoName = this.repos[this.repoIndex];
        this.currentRepo = this.repoData[this.repoName];

        this.updatePortfolioView();
    };

    nextRepository()
    {
        var nextIndex = this.repoIndex + 1;
        var n = this.repos.length;

        if(nextIndex >= this.repos.length)
            nextIndex = nextIndex % n;
        
        this.setCurrentRepository(nextIndex);
        $('#project-preview').slick('slickNext');
    };

    prevRepository()
    {
        var prevIndex = this.repoIndex - 1;
        var n = this.repos.length;
        
        if(prevIndex < 0)
            prevIndex = ((prevIndex % n) + n) % n;

        this.setCurrentRepository(prevIndex);
        $('#project-preview').slick('slickPrev');
    };

    initCarousel()
    {
        $('#project-preview').slick
        ({
            slidesToShow:1,
            slidesToScroll:1,
            arrows:false
        });
    };

    pushCarouselItem()
    {
        if($.inArray(this.repoIndex, this.existingRepos) == -1)
        {
            var readmeHtml = this.currentRepo["readme"];
            var carouselContainer = $('#project-preview');

            carouselContainer.slick('slickAdd', readmeHtml);

            this.existingRepos.push(this.repoIndex);
        }
    };

    updatePortfolioView()
    {
        var commitStats = $('#commits-stats');
        var codeStats = $('#codeline-stats');
        var githubLinkBtn = $('#project-view-link');
        var repoTitle = $('#project-title');
        var repoDesc = $('#project-description');

        this.pushCarouselItem();

        commitStats.text(this.currentRepo["commits"]);
        codeStats.text(this.currentRepo["codeLines"]);

        
        githubLinkBtn.attr('href', this.currentRepo["link"]);
        repoTitle.text(this.getTransformedTitle());
        repoDesc.text(this.currentRepo["description"]);
        this.updateLanguages();
    };

    updateLanguages()
    {
        var languages = this.currentRepo["languages"];
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

    getTransformedTitle()
    {
        return this.currentRepo["name"].replace(/-/g, " ");
    };

    initHandlers()
    {
        $('#prev-project-btn').click(() =>
        {
            this.prevRepository();
        });
    
        $('#next-project-btn').click(() =>
        {
            this.nextRepository();
        });
    };
};
class ContactComponent
{
    constructor()
    {
        this.CONTACT_STATUS_PROGRESS = 0,
        this.CONTACT_STATUS_SUCCESS = 1,
        this.CONTACT_STATUS_ERROR = 2,
        this.CONTACT_STATUS_RESET = 3;

        this.serviceRotateIndex = 0;

        $('#contact-btn-progress').hide();
        $('#contact-btn-status').hide();
        $('.slick-dots > li').slice(-2).hide();

        this.initCarousel();
        this.initHandlers();
    };

    initDisplay()
    {
        $('#contact-form-title').addClass('contact-form-title-toggled');
    };

    initHandlers()
    {
        var context = this;
        $('#contact-form').submit(function(e)
        {
            e.preventDefault();

            var form = $(this);
            var contactUrl = form.attr('action');
            var contactData = form.serialize();

            context.updateContactButton(context.CONTACT_STATUS_PROGRESS);

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
                            context.updateContactButton(context.CONTACT_STATUS_SUCCESS);
                            context.clearForm();
                        }

                        else
                        {
                            var errMsg  =   response.ResponseMsg;
                            context.updateContactButton(context.CONTACT_STATUS_ERROR, errMsg);
                        }

                        context.resetContactButton();
                    },

                    error: function(xhr, statusText, err)
                    {
                        context.updateContactButton(context.CONTACT_STATUS_ERROR);
                        context.resetContactButton();
                    }

                });

            }, 1500);
        });

        $('.footer-info-description').hover(function()
        {
            $('#credits-heart').css('font-weight', 900);
        }, function()
        {
            $('#credits-heart').css('font-weight', 400);
        });

        $('#go-top-btn').click(function(e)
        {
            fullpage_api.moveTo(1);
        });
    };

    updateContactButton(statusCode, errMsg)
    {
        var contactBtn = $('#contact-submit');
        var contactBtnSpinner = $('#contact-btn-progress');
        var contactBtnText = $('#contact-btn-text');
        var contactBtnStatusIcon = $('#contact-btn-status');
        var btnMsg = "";

        switch(statusCode)
        {
            case this.CONTACT_STATUS_PROGRESS:
                contactBtnSpinner.show();
                btnMsg = "SENDING MESSAGE";
                break;

            case this.CONTACT_STATUS_SUCCESS:
                contactBtnSpinner.hide();
                btnMsg = "MESSAGE SENT";
                contactBtnStatusIcon.attr('class', 'fas fa-check-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-success');
                break;

            case this.CONTACT_STATUS_ERROR:
                contactBtnSpinner.hide();
                btnMsg = errMsg;
                contactBtnStatusIcon.attr('class', 'fas fa-times-circle fa-lg').show();
                contactBtn.attr('class', 'btn btn-primary contact-btn-error');
                break;
        }

        contactBtnText.text(btnMsg);
    };

    resetContactButton()
    {
        setTimeout(function()
        {
            $('#contact-btn-status').hide();
            $('#contact-btn-progress').hide();
            $('#contact-btn-text').text('send message');
            $('#contact-submit').attr('class', 'btn btn-primary contact-btn-normal');
        }, 2000);
    };

    clearForm()
    {
        $('#contact-name-field').val('');
        $('#contact-email-field').val('');
        $('#contact-message-field').val('');
    };

    initCarousel()
    {
        $('#service-list-container').slick
        ({
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 2,
            dots: true,
            arrows:false,
            autoplay:true,
            autoplaySpeed: 3000,
            accessibility: false
        });
    };
};
$(function()
{
    function saveUser()
    {
        processForm($('#user-save-form'), $('#user-save-btn'), null, (data) =>
        {
            var url = window.location.href;
            var tableId = '#user-table';

            $(tableId).load(url + ' ' + tableId);
            hideSpinners();

            setTimeout(() =>
            {
                $('#user-edit-modal').modal('hide');
            }, 1500);
        });
    };


    function editUser(btnElement)
    {
        var userId = getUserIdForRow(btnElement);
        var fetchUserInfoUrl = userInfoUrl + "/" + userId;

        $.getJSON(fetchUserInfoUrl, (data) =>
        {
            var userJson = $.parseJSON(data);
            $('#user-edit-id').val(userJson.Id);
            $('#user-edit-name').val(userJson.DisplayName);
            $('#user-edit-username').val(userJson.UserName);
            $('#user-edit-email').val(userJson.Email);
            $('#user-edit-password').val(userJson.PasswordHash);
            $('#user-edit-picture').val(userJson.ProfilePicture);

            $('#user-edit-modal').modal('show');
        });
    };

    function getUserIdForRow(btnElement)
    {
        return btnElement.closest('tr').attr('data-userId');
    };

    $('.user-edit-btn').click(function(e)
    {
        editUser($(this));
    });

    $('.user-remove-btn').click(function(e)
    {
        removeUser($(this));
    });

    $('#user-save-btn').click(function(e)
    {
        e.preventDefault();
        saveUser();
    });

    $('#create-user-btn').click((e) =>
    {
        $('#user-edit-modal').modal('show');
    });
});
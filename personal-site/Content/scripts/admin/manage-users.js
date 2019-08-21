$(function()
{
    function saveUser()
    {
        processForm($('#user-save-form'), $('#user-save-btn'), null, (data) =>
        {
            var url = window.location.href;
            var tableId = '#user-table';

            $(tableId).load(url + ' ' + tableId, () =>
            {
                setTimeout(() =>
                {
                    hideSpinners();
                    $('#user-edit-modal').modal('hide');
    
                    if($('#user-edit-id').val() == "")
                        clearEditForm();
                }, 1500);
            });
        });
    };

    function removeUser(btnElement)
    {
        var userId = getUserIdForRow(btnElement);
        var fieldName = 'id';
        var formData = `${fieldName}=${userId}`;
        var alertElement = $('#user-remove-alert');

        processForm(null, btnElement, formData, (data) =>
        {
            toggleAlertResponse(data, alertElement);

            if(data.ActionSuccess)
            {
                var currentRowElement = btnElement.closest('tr');
                currentRowElement.remove();
            }

            setTimeout(() => toggleAlertResponse(data, alertElement, true), 2000);

        }, userRemoveUrl);
    };

    function editUser(btnElement)
    {
        var userId = getUserIdForRow(btnElement);
        var fetchUserInfoUrl = userInfoUrl + "/" + userId;

        $.getJSON(fetchUserInfoUrl, (data) =>
        {
            var jsonData = $.parseJSON(data);
            var userJson = jsonData.User;
            $('#user-edit-id').val(userJson.Id);
            $('#user-edit-name').val(userJson.DisplayName);
            $('#user-edit-username').val(userJson.UserName);
            $('#user-edit-email').val(userJson.Email);
            $('#user-edit-password').val(userJson.PasswordHash);
            $('#user-edit-picture').val(userJson.ProfilePicture);

            var roleName = jsonData.RoleName == null? 'Member' : jsonData.RoleName;
            $('#user-edit-role').val(roleName);

            $('#user-edit-modal').modal('show');
        });
    };

    function clearEditForm()
    {
        $('#user-save-form input').val('');
    }

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
        $('#user-edit-id').val('');
        $('#user-edit-modal').modal('show');
    });
});
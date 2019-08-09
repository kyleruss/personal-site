$(function()
{
    function saveUser()
    {
        processForm($('#user-save-form'), $('#user-save-btn'), null, (data) =>
        {
            var url = window.location.href;
            var tableId = '#user-table';

            $(tableId).load(url + ' ' + tableId);
            
            setTimeout(() =>
            {
                $('#user-edit-modal').modal('hide');
                hideSpinners();
            }, 1500);
        });
    };

    function removeUser(btnElement)
    {
        var userId = getUserIdForRow(btnElement);
    };

    function editUser(btnElement)
    {
        var userId = getUserIdForRow(btnElement);
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
$(document).ready(function () {
    $('.edit-user-btn').click(function () {
        var userCard = $(this).closest('.user-card');
        var userId = userCard.data('user-id');

        $.get('/Admin/GetUser', { userId: userId }, function (data) {
            userCard.find('.user-login').html('<input type="text" value="' + data.login + '"/>');
            userCard.find('.user-email').html('<input type="text" value="' + data.email + '"/>');
            userCard.find('.user-role').html('<select><option value="user">user</option><option value="admin">admin</option></select>');
            userCard.find('select').val(data.role);
            userCard.find('.edit-user-btn').hide();
            userCard.find('.save-user-btn').show();
        });
    });

    $('.save-user-btn').click(function () {
        var userCard = $(this).closest('.user-card');
        var userId = userCard.data('user-id');
        var login = userCard.find('.user-login input').val();
        var email = userCard.find('.user-email input').val();
        var role = userCard.find('.user-role select').val();

        $.post('/Admin/EditUser', { ID: userId, Login: login, Email: email, Role: role }, function () {
            userCard.find('.user-login').text(login);
            userCard.find('.user-email').text(email);
            userCard.find('.user-role').text(role);
            userCard.find('.edit-user-btn').show();
            userCard.find('.save-user-btn').hide();
        });
    });
});
function generatePassword(onPasswordReceived) {
    return $.getJSON("/api/passwordgenerator", function (data) {
        onPasswordReceived(data.Password);
    });
}
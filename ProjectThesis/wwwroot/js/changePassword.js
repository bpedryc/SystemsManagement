$("#change-password").click(function () {
    $("#changePasswordForm").toggleClass("d-none");
});

$("#confirm-change-password").click(function () {
    if ($("#password-1").val() === $("#password-2").val() && $("#password-1").val() !== "")
        $("#changePasswordForm").submit();
    else if ($("#error-message-div").length === 0)
        $('<div id="error-message-div" style="text-align: center; width: 100%; margin-top: 0.3em;"><p style="color: crimson;">Popraw dane</p></div>').insertBefore("#form-group-password");
});
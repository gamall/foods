$(function () {

    $("#btnForgottenPassword").click(function () {

        var regex = new RegExp(/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/);

        if (!regex.test($("#Email").val())) {
            $("#Email").addClass("invalid");
            $(".errorMessage").show();
        }
        else {
            $("#Email").removeClass("invalid");
            $("#forgotten-password-form").submit();
        }
        return false;
    });

    $("#butDownload").click(function () {
        $("#download-form").submit();
        return false;
    });

    $("#btnResetPassword").click(function () {

        var re = new RegExp(/((?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30})/);

        if ($("#NewPassword").val() === $("#ConfirmPassword").val()) {
            $(".errorMessage").hide();
            if (re.test($("#ConfirmPassword").val())) {
                $("#reset-password-form").submit();
            }
            else {
                $(".errorMessage.specchar").show();
            }
        }
        else {
            $(".errorMessage.notmatch").show();
        }

        return false;

    });

    $("#butAdminLogin").click(function () {
        if ($("#Email").val() === "" || $("#Password").val() === "") {
            $("p.errorMessage.hide.blank").show();
        }
        else {
            $("#login-form").submit();
        }
        return false;
    });

    $("#butSearch").click(function () {

        $("#user-search-form").submit();

        return false;
    });

    $("#butEditUser").click(function () {
        $("#edit-user-form").submit();
        return false;
    });

    if ($("p.errorMessage.hide.server").html() === "Error") {
        $("p.errorMessage.hide.server").hide();
        $("p.errorMessage.hide.incorrect").show();
    }

    $("#toggle-cookie").click(function () {

        $(".cookie").slideToggle("slow");

        return false;
    });

});
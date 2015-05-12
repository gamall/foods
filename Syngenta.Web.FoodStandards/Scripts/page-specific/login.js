(function ($, undefined) {
    var handlers = {
        validateForm: function () {

            var isValid = true;

            $(views.errorMessage).hide();

            //text, email, password
            $(views.formLogin + " input").each(function () {
                if (!$(this).val()) {
                    $(this).addClass("invalid");
                    $(views.errorMessageHighlite).show();
                    isValid = false;
                }
                else {
                    $(this).removeClass("invalid");
                }
            })

            //email validation            
            var re = new RegExp(/^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/);
            if ($(views.email).val() !== "") {
                if (!re.test($(views.email).val())) {
                    isValid = false;
                    $(views.email).addClass("invalid");
                    $(views.errorMessageEmail).show();
                }
                else {
                    $(views.email).removeClass("invalid");
                }
            }

            //check is valid
            if (isValid) {
                $(views.formLogin).submit();
            }
            //else {
            //    $(views.errorMessage).show();
            //}

            return false;
        },

        validateEnter: function (event) {
            if (event.keyCode == 13) {
                handlers.validateForm();
            }
        },

        validateServerErrorMessage: function () {
            if ($(views.errorMessageServer).html() === "Error") {
                $(views.errorMessageServer).html($(views.errorMessageInvalid).html());
            }
        }

    },
    views = {
        email: "#Email",
        password: "#Password",
        butLogin: "#butLogin",
        formLogin: "#login-form",
        errorMessage: ".errorMessage",
        errorMessageHighlite: ".errorMessage.hide.highlite",
        errorMessageEmail: ".errorMessage.hide.email",
        errorMessageInvalid: ".errorMessage.hide.invalid",
        errorMessageServer: ".errorMessage.server",
        loginPage: ".login"
    },

    init = function () {
        handlers.validateServerErrorMessage();
        $(views.loginPage).bind('keydown', handlers.validateEnter);
        $(views.butLogin).bind('click', handlers.validateForm);
    };

    init();
})(jQuery)
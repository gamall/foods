(function ($, undefined) {
    var handlers = {
        validateForm: function () {

            var isValid = true;

            $(views.errorMessageHide).hide();
            $(views.errorMessageServer).hide();

            //text, email, password
            $(views.formRegister + " input").each(function () {
                if (!$(this).val()) {
                    $(this).addClass("invalid");
                    isValid = false;
                    $(views.errorMessageHighlite).show();
                }
                else {
                    $(this).removeClass("invalid");
                }
            });

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
                $(views.formRegister).submit();
            }
            //else {
            //    $(views.errorMessage).show();
            //}

            return false;
        },

        validateServerErrorMessage: function () {
            if ($(views.errorMessageServer).html() === "Error") {
                $(views.errorMessageServer).html($(views.errorMessageInvalid).html());
            }
        }

    },
    views = {
        foreName: "#Forename",
        lastName: "#Surname",
        email: "#Email",
        butRegister: "#register",
        formRegister: "#registration",
        errorMessageHide: ".errorMessage.hide",
        errorMessageHighlite: ".errorMessage.hide.highlite",
        errorMessageEmail: ".errorMessage.hide.email",
        errorMessageInvalid: ".errorMessage.hide.invalid",
        errorMessageServer: ".errorMessage.server"
    },

    init = function () {
        handlers.validateServerErrorMessage();
        $(views.butRegister).bind('click', handlers.validateForm);
    };

    init();
})(jQuery);
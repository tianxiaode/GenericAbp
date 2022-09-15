(function ($) {

    var l = abp.localization.getResource('AbpAccount');

    var service = generic.abp.account.account;
    var registerForm = $("#RegisterForm");

    registerForm.submit(function (e) {
        e.preventDefault();

        if (!registerForm.valid()) {
            return false;
        }

        var input = registerForm.serializeFormToObject().registerInfoModel;


        if (input.password !== input.passwordConfirm || input.currentPassword === '') {
            abp.message.error(l("NewPasswordConfirmFailed"));
            return false;
        }

        if (input.currentPassword === '') {
            return false;
        }

        service.register(
            input
        ).then(function (result) {
            abp.message.success(l("RegisterSuccess"));
            window.location.href = "./login";
        });

    });


})(jQuery);

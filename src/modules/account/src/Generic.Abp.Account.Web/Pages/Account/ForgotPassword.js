(function ($) {

    var l = abp.localization.getResource('AbpAccount');

    var service = generic.abp.account.account;
    var forgotPasswordForm = $("#ForgotPasswordForm");
    var resetPasswordForm = $('#ResetPasswordForm');
    var count = 5;
    var sendCode = $('#SendCode');
    var tokenResult = {};

    var refreshCount = function () {
        count--;
        if (count <= 0) {
            sendCode.toggleClass('bg-primary');
            sendCode.html(l("Send"));
            return;
        }
        let resend = `${l('Resend') || 'Resend'}(${count})`;
        sendCode.html(resend);
        setTimeout(refreshCount, 1000);
    }

    sendCode.click(function () {
        if (!sendCode.hasClass('bg-primary')) return;
        let input = forgotPasswordForm.serializeFormToObject().forgotPasswordInfoModel;
        if (input.emailAddress === '') {
            abp.message.error(l('InvalidEmail'));
            return;
        }
        service.sendVerificationCode(input).then(function (result) {
            if (result.result === 1) {
                abp.message.success(l('VerificationCodeSent'));
                return;
            }

            if (result.result > 1) {
                let msg = result.result === 2
                    ? "VerificationCodeSentMaxCount"
                    : result.result === 3
                        ? "VerificationCodeSentError"
                        : "InvalidEmail";
                abp.message.error(l(msg));
                return;
            }
            //abp.message.success(l('InvalidEmail'));
            return;
        });
        sendCode.toggleClass('bg-primary');
        count = 60;
        let resend = `${l('Resend') || 'Resend'}(${count})`;
        sendCode.html(resend);
        setTimeout(refreshCount, 1000);
    });

    forgotPasswordForm.submit(function (e) {
        e.preventDefault();

        if (!forgotPasswordForm.valid()) {
            return false;
        }

        let input = forgotPasswordForm.serializeFormToObject().forgotPasswordInfoModel;


        if (input.emailAddress === '' || input.code === '') {
            return false;
        }

        service.checkVerificationCode(
            input
        ).then(function (result) {
            $('#SendCodeCard').toggleClass('d-none');
            $('#ResetPasswordCard').toggleClass('d-none');
            tokenResult.emailAddress = result.emailAddress;
            tokenResult.token = result.token;
        });
        return false;
    });

    resetPasswordForm.submit(function (e) {
        e.preventDefault();

        if (!resetPasswordForm.valid()) {
            return false;
        }

        let input = resetPasswordForm.serializeFormToObject().resetPasswordInfoBaseModel;

        if (input.newPassword !== input.newPasswordConfirm) {
            abp.message.error(l("NewPasswordConfirmFailed"));
            return false;
        }
        input.emailAddress = tokenResult.emailAddress;
        input.token = tokenResult.token;
        service.resetPassword(
            input
        ).then(function (result) {
            abp.message.success(l("ResetPasswordSuccess"));
            setTimeout(function () {
                window.location.href = "./login";
            },
                2000);
        });
        return false;
    });



})(jQuery);

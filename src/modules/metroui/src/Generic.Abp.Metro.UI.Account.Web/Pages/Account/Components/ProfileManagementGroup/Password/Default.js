(function () {
    $(function () {
        var l = abp.localization.getResource("AbpAccount"),
            form = $('#ChangePasswordForm');

        Metro.makePlugin(form, 'validator', {
            onSubmit: function (data) {
                if (
                    data.newPassword != data.newPasswordConfirm ||
                    data.newPassword == ''
                ) {
                    abp.message.error(l('NewPasswordConfirmFailed'));
                    return;
                }

                if (data.currentPassword && data.currentPassword == ''){
                    return;
                }
            
                if(data.currentPassword == data.newPassword) {
                    abp.message.error(l('NewPasswordSameAsOld'));
                    return;
                }

                volo.abp.account.profile.changePassword(data).then(function (result) {
                    console.log(result)
                    abp.message.success(l('PasswordChanged'));
                    abp.event.trigger('passwordChanged');
                    form.trigger("reset");
                });

            }
        })

        $('#ChangePasswordFormSubmitButton').click((e)=>{
            e.preventDefault();
            Metro.getPlugin(form, 'validator')._submit();
        })

    //    $('#ChangePasswordForm').submit(function (e) {
    //        e.preventDefault();

    //        if (!$('#ChangePasswordForm').valid()) {
    //            return false;
    //        }

    //        var input = $('#ChangePasswordForm').serializeFormToObject();

    //        if (
    //            input.newPassword != input.newPasswordConfirm ||
    //            input.newPassword == ''
    //        ) {
    //            abp.message.error(l('NewPasswordConfirmFailed'));
    //            return;
    //        }

    //        if (input.currentPassword && input.currentPassword == ''){
    //            return;
    //        }
            
    //        if(input.currentPassword == input.newPassword) {
    //            abp.message.error(l('NewPasswordSameAsOld'));
    //            return;
    //        }
            
    //        volo.abp.account.profile.changePassword(input).then(function (result) {
    //            abp.message.success(l('PasswordChanged'));
    //            abp.event.trigger('passwordChanged');
    //            $('#ChangePasswordForm').trigger("reset");
    //        });
    //    });
    });
})();

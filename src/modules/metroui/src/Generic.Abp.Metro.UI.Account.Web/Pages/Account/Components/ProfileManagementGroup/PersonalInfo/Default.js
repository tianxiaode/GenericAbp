(function ($) {
    $(function () {
        var l = abp.localization.getResource("AbpAccount"),
            form  = $('#PersonalSettingsForm');

        Metro.makePlugin(form, 'validator', {
            onSubmit: function (data) {
                volo.abp.account.profile.update(data).then(function (result) {
                    abp.notify.success(l('PersonalSettingsSaved'));
                    updateConcurrencyStamp();
                });

            }
        })

        $('#PersonalSettingsFormSubmitForm').click((e)=>{
            e.preventDefault();
            Metro.getPlugin(form, 'validator')._submit();
        })


    });

    abp.event.on('passwordChanged', updateConcurrencyStamp);
    
    function updateConcurrencyStamp(){
        volo.abp.account.profile.get().then(function(profile){
            $("#ConcurrencyStamp").val(profile.concurrencyStamp);
        });
    }
})(m4q);


$(function () {

    var tenantSwitchModal = new ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');
    $('#AbpTenantSwitchLink').click(function (e) {
        e.preventDefault();
        tenantSwitchModal.open();
    });

    tenantSwitchModal.onResult(function () {
        location.assign(location.href);
    });

});
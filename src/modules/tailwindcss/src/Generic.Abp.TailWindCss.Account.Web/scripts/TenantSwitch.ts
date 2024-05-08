import ModalManager from './ModalManager'
export default class TenantSwitch {

    private modal : ModalManager
    constructor() {
        const div = document.getElementById('AbpTenantSwitchLink');
        div?.addEventListener('click', this.onSwitch.bind(this));
        this.modal = new ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');
    }   

    onSwitch() {
        this.modal.open();
    }
}
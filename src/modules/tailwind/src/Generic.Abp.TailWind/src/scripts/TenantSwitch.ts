import ModalManager from './ModalManager'
export default class TenantSwitch {

    private modal : ModalManager
    constructor() {
        const div = document.getElementById('AbpTenantSwitchLink');
        div?.addEventListener('click', this.onSwitch.bind(this));
        this.modal = new ModalManager(abp.appPath + 'Abp/MultiTenancy/TenantSwitchModal');
        this.modal.onResult = this.onResult.bind(this);
    }   

    onSwitch(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.modal.open();
    }

    onResult() {
        location.assign(location.href);
    }
}
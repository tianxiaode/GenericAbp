export default class TenantSwitch {

    private modal : HTMLElement | null
    constructor() {
        const div = document.getElementById('AbpTenantSwitchLink');
        div?.addEventListener('click', this.onSwitch.bind(this));
        this.modal = document.getElementById('switchTenantDialog');
        console.log(div,this.modal)
    }   

    onSwitch() {
        this.modal!.setAttribute('open','open');
    }
}
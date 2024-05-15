import Form from "./Form"
import Toast from "./Toast"
import Alert from "./Alert"
import TenantSwitch from "./TenantSwitch"
import {AxiosManager} from "./http/AxiosManager"

export default class App {
    constructor() {
        this.initAxiosManager();
        new TenantSwitch();
        this.initNav();
        this.initForm();
        this.initAbpNotify();
        this.initAbpMessage();
    }

    private initAxiosManager() {
        let locale = abp.localization.currentCulture.cultureName;
        if(locale === 'zh-Hans') locale = 'zh-CN';
        if(locale === 'zh-Hant') locale = 'zh-TW';
        abp.axiosManager = new AxiosManager({
            errorMessageHandler: (message) => { Toast.error(message) }
        });
        abp.axiosManager.setLocale(locale);

    }

    private initForm() {
    //    const els = document.querySelectorAll('form');
    //    els.forEach( (el : Element) => {
    //        new Form(el as HTMLFormElement);
    //    })
    }

    private initNav() {
        document.getElementById('mobileMenuButton')?.addEventListener('click',this.toggleMobileMenu);
        document.getElementById('userMenuButton')?.addEventListener('click',this.onOpenUserMenu);
        document.getElementById('userMenuButton')?.addEventListener('mouseover',this.onOpenUserMenu);
        document.getElementById('languageMenuButton')?.addEventListener('click',this.openLanguageMenu);
        document.getElementById('languageMenuButton')?.addEventListener('mouseover',this.openLanguageMenu);
    }

    private initAbpNotify() {
        abp.notify = {
            success: (message: string) => { Toast.success(message) },
            info: (message: string) => { Toast.info(message) },
            warn: (message: string) => { Toast.warning(message) },
            error: (message: string) => { Toast.error(message) },
       }
    }

    private initAbpMessage() {
        abp.message = {
            confirm:(message: string, title: string, callback: Function)=> { Alert.confirm(message, title, callback) },
            success: (message: string, title: string) => { Alert.success(message, title) },
            info: (message: string, title: string) => { Alert.info(message,title) },
            warn: (message: string, title: string) => { Alert.warning(message,title) },
            error: (message: string, title: string) => { Alert.error(message,title) },
        }
    }

    private onOpenUserMenu() {
        const el = document.getElementById('userMenu') as HTMLElement;
        el.style.display = "block";
    }

    private openLanguageMenu(){
        const el = document.getElementById('languageMenu') as HTMLElement;
        el.style.display = "block";
    }


    private toggleMobileMenu() {
        const menu = document.getElementById('mobileMenu') as HTMLElement;
        const current = menu.style.display;
        menu.style.display = current == 'none' ? 'block' : 'none';
    }
}


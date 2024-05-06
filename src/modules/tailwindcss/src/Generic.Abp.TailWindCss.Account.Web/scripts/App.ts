import Form from "./From"
export default class App {
    constructor() {
        this.initNav();
        this.initForm();
    }

    private initForm() {
        const els = document.querySelectorAll('form');
        els.forEach( (el : Element) => {
            new Form(el as HTMLFormElement);
        })
    }

    private initNav() {
        document.getElementById('mobileMenuButton')?.addEventListener('click',this.toggleMobileMenu);
        document.getElementById('userMenuButton')?.addEventListener('click',this.onOpenUserMenu);
        document.getElementById('userMenuButton')?.addEventListener('mouseover',this.onOpenUserMenu);
        document.getElementById('languageMenuButton')?.addEventListener('click',this.openLanguageMenu);
        document.getElementById('languageMenuButton')?.addEventListener('mouseover',this.openLanguageMenu);
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


import App from "./App"

    function onOpenUserMenu() {
        const el = document.getElementById('userMenu') as HTMLElement;
        el.style.display = "block";
    }

    function openLanguageMenu(){
        const el = document.getElementById('languageMenu') as HTMLElement;
        el.style.display = "block";
    }
    function toggleMobileMenu() {
        const menu = document.getElementById('mobile-menu') as HTMLElement;
        const current = menu.style.display;
        menu.style.display = current == 'none' ? 'block' : 'none';
    }

document.addEventListener('DOMContentLoaded', function() {
    new App();
});
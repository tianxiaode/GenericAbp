    function onOpenUserMenu() {
        const el = document.getElementById('userMenu');
        el.style.display = "block";
    }

    function openLanguageMenu(){
        const el = document.getElementById('languageMenu');
        el.style.display = "block";
    }
    function toggleMobileMenu() {
        const menu = document.getElementById('mobile-menu');
        const current = menu.style.display;
        menu.style.display = current == 'none' ? 'block' : 'none';
    }

    function clearInputValue(e) {
        let target = e.target;
        if(target.tagName === "I") target = target.parentNode;
        const input = target.parentNode.querySelector('input');
        input.value = '';
        const event = new Event('change');
		input.dispatchEvent(event);
    }

    function onInputValueChange(e) {
        const target = e.target;
        const button = target.parentNode.querySelector('button');
        console.log(target.value)
        if (target.value) {
            console.log(button.className)
            button.className = button.className.replace(' hidden', '');
            return;
        };
        button.className += ' hidden';
    }  

    let keypressTask;
    function onInputKyepress(event) {
         if (keypressTask) {
            clearTimeout(keypressTask);
        }
        keypressTask = setTimeout(function() {
            keypressTask = null;
            onInputValueChange(event);
        }, 500);
    }


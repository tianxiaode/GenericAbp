export default class Alert {
    static icons: Record<string,string> = { info: 'info-circle',error: 'times-circle', warning: 'exclamation-triangle', success:'check-circle', confirm: 'info-circle'  };

    static showMessage(type: string, message: string, title: string, callback: Function | null = null) {
        const alert = document.createElement('dialog');
        const icon = this.icons[type];
        const resource = abp.localization.getResource('AbpUi');
        const color = type === 'confirm' ? 'info' : type;
        let buttons = `<button class="btn text-white btn-primary  btn-ok min-w-20">${resource('Yes')}</button>`;
        const titleDiv = title ? `<h3 class="font-bold text-lg">${title}</h3>` : '';
        alert.className = "modal modal-bottom sm:modal-middle";
        alert.role = "alert";
        alert.setAttribute('open','open');

        if (type === 'confirm') {
            buttons = `<button class="btn btn-cancel min-w-20">${resource('Cancel')}</button>` + buttons;
        }
        alert.innerHTML = `
            <div class="modal-box">
                <button class="btn btn-sm  btn-ghost absolute right-2 top-2 btn-times hover:bg-transparent"><i class="fas fa-times h-6 w-6 text-2xl"></i></button>
                ${titleDiv}
                <p class="py-4 "><i class="fas fa-${icon} text-${color} stroke-info shrink-0 w-6 h-6"></i> ${message}</p>
                <div class="modal-action">
                   ${buttons}
                </div>
            </div>
            `;
        const cb = (event:Event) => {
            const target = event.target as HTMLElement;
            alert.remove();
            callback && callback(target.className.includes('btn-ok') ? 'ok' : 'cancel');
        }
        alert.querySelector('button.btn-ok')?.addEventListener('click', cb) ;
        alert.querySelector('button.btn-cancel')?.addEventListener('click', cb);
        alert.querySelector('button.btn-times')?.addEventListener('click', cb );
        document.body.appendChild(alert);
    }

    static confirm(message: string, title: string,callback: Function) {
        this.showMessage('confirm', message, title ,callback);
    }

    static error(message: string, title: string) {
        this.showMessage('error', message, title);
    }

    static info(message: string, title: string) {
        this.showMessage('info', message, title);

    }

    static warning(message: string, title: string) {
        this.showMessage('warning', message, title);
    }

    static success(message: string, title: string) {
        this.showMessage('success', message, title);
    }

}
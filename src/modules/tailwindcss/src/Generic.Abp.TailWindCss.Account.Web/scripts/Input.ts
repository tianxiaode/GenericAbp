export default class Input {
    public rules:  Record<string, any>
    private input: HTMLInputElement | null = null
    private keypressTask: string | number | NodeJS.Timeout | undefined = undefined
    private clearButton : HTMLButtonElement | null = null;
    private isPasswordField: boolean = false;
    private currentPasswordType: string  = "password"
    private eyeButton : HTMLButtonElement | null = null;
    private errorElement: HTMLElement | null = null;
    constructor(el: HTMLInputElement) {
        this.rules = {};
        this.getRules(el)
        this.createInput(el);
    }

    private getRules(el: HTMLInputElement) {
        const hasValidate = el.getAttribute('data-val');
        const attributeNames = el.getAttributeNames();
        const rules : any = {};
        if(hasValidate === 'false') return;

        // 遍历每个属性名
        attributeNames.forEach(attrName => {
            // 如果属性名以'data-val'开头
            if (!attrName.startsWith('data-val-')) return;

            const name = attrName.replace('data-val-', '');
            const value = el.getAttribute(attrName);
            if (!name.includes('-')) {
                rules[name] = { message: value};
                return;
            }
            const validateValueName = name.split("-");
            rules[validateValueName[0]] = {  };
            rules[validateValueName[0]][validateValueName[1]] = value;
        });
        this.rules = rules;
    }

    private createInput(el: HTMLInputElement) {
        const me = this;
        const root = document.createElement('div');
        let innderHtml = "";
        const attachCls = el.getAttribute('controlCls') || '';
        const labelAlign = el.getAttribute('labelAlign');
        const label = el.getAttribute('label');
        const id = el.id;
        const iconCls = el.getAttribute('iconCls');
        const clearable = el.getAttribute('clearable') !== "false";
        const value = el.value;
        const clearableHidden = !!value ? '' : 'hidden';
        const type = el.type;
        let autocomplete = el.autocomplete;
        me.isPasswordField = type === 'password';
        autocomplete = autocomplete ? autocomplete : 'off';
        
        root.className = `form-control w-full max-w-xs mb-4 ${attachCls}`;
        if (labelAlign === 'top' && label) {
           innderHtml += `<label class="label" for="${id}">${label}</label>`;
        }
        innderHtml += `<div class="input input-bordered focus:border-primary focus-within:border-primary flex items-center gap-2">`;
        if (labelAlign === 'left' && label) {
           innderHtml += `<label class="label" for="${id}">${label}</label>`;
        }
        if (iconCls) {
           innderHtml += `<i class="${iconCls} w-4 h-4  opacity-70}"></i>`;
        }
           innderHtml += `<input id="${id}" name="${el.name}" type="${type}" value="${el.value}" autocomplete="${autocomplete}"  class="${el.className} grow" pattern=".*" />`;
        if (clearable) {
           innderHtml += `<button tabindex="-1" type="button"  class="clear-button ${clearableHidden}" ><i class="fas fa-times w-5 h-5 text-base opactity-70 "></i></button>`;
        }
        if (me.isPasswordField) {
           innderHtml += `<button tabindex="-1" type="button"  class="show-password-button ${clearableHidden}" ><i class="fas fa-eye w-5 h-5 text-base opactity-70"></i></button>`;
        }
        innderHtml += "</div>"
        innderHtml += `<div class="label hidden"><span class="flabel-text-alt text-error"></span></div>`;
        innderHtml += "</div>";
        root.innerHTML = innderHtml;
        el.parentElement?.replaceChild(root, el);
        me.input = root.querySelector('input');
        me.input?.addEventListener('keypress', me.onKeypress.bind(me));
        me.input?.addEventListener('change', me.onChange.bind(me));
        me.clearButton = root.querySelector('button.clear-button');
        me.clearButton?.addEventListener('click', me.onClearInputValue.bind(me));
        me.eyeButton = root.querySelector('button.show-password-button');
        me.eyeButton?.addEventListener('click', me.onSwitchInputType.bind(me));
        me.errorElement = root.querySelector('.text-error');
    }

    private onClearInputValue(event: Event) {
        const input = this.input!  as HTMLInputElement;
        input.value = "";
        const e = new Event('change');
		input.dispatchEvent(e);
    }

    private onSwitchInputType(event: Event) {
        const input = this.input!;
        const currentType = this.currentPasswordType;
        this.currentPasswordType = currentType === 'password' ? "text": "password";
        input.setAttribute('type', this.currentPasswordType);
        this.switchEyeButtonState(true);
    }

    private onChange(event: Event) {
        const me = this;
        const value = me.input!.value;
        const isPassword = me.isPasswordField;
        if (value) {
            if (isPassword) {
                me.switchEyeButtonState(true);
            }
            return me.clearButton!.classList.remove('hidden');
        }
        me.clearButton?.classList.add('hidden');
        me.switchEyeButtonState(false);
    }

    private onKeypress(event: Event) {
        const me = this;
        const keypressTask = me.keypressTask;
        
         if (keypressTask) {
            clearTimeout(keypressTask);
            me.keypressTask = undefined;
        }
        me.keypressTask = setTimeout(function() {
            me.keypressTask = undefined;
            me.onChange(event);
        }, 500);
    }

    private switchEyeButtonState(isShow: boolean) {
        const button = this.eyeButton as HTMLElement;
        if (isShow) {
            const icon = button.querySelector('i') as HTMLElement;
            button.classList.remove('hidden');
            icon.classList.remove('fa-eye');
            icon.classList.remove('fa-eye-slash')
            icon.classList.add(this.currentPasswordType === 'text' ? 'fa-eye-slash' : 'fa-eye');
            return 

        }
        button.classList.add('hidden');
    }

    get value(): string | undefined {
        return this.input?.value;
    }

    set error(error: string) {
        const el = this.errorElement!;
        const parent = el.parentElement;
        el.innerHTML = error;
        error && parent!.classList.remove('hidden');
        !error && parent!.classList.add('hidden');
    }
}
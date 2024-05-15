import Validation from './Validation'
import zxcvbn from 'zxcvbn-lite';

const PASSWORD_SETTING_PRIFIX = "Abp.Identity.Password";
const PASSWORD_SETTING = ['RequireDigit', 'RequireLowercase', 'RequireNonAlphanumeric', 'RequireUppercase', 'RequiredLength'].map(m => `${PASSWORD_SETTING_PRIFIX}.${m}`);
export default class Input {
    public id: string
    public rules: Record<string, any>
    private input: HTMLInputElement | null = null
    private changeTask: string | number | NodeJS.Timeout | undefined = undefined
    private clearButton: HTMLButtonElement | null = null;
    private isPasswordField: boolean = false;
    private currentPasswordType: string = "password"
    private eyeButton: HTMLButtonElement | null = null;
    private errorElement: HTMLElement | null = null;
    private indicatorElement: HTMLElement | null = null;
    constructor(el: HTMLElement) {
        this.input = el.querySelector('input') as HTMLInputElement;
        this.id = this.input.id;

        this.rules = {};
        this.getRules();
        this.bindEventAndElement(el);
    }

    isValid() {
        let isValid = true,
            input = this.input!,
            message = '',
            value = input.value,
            rules = this.rules;
        for (let [name, rule] of Object.entries(rules)) {
            let result = Validation.validate(name, value, rule);
            if (result) continue;
            isValid = false;
            message += `${rule.message}<br>`;
        }
        this.error = message;
        return isValid;
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

    private getRules() {
        let me = this,
            el = this.input!,
            hasValidate = el.getAttribute('data-val'),
            attributeNames = el.getAttributeNames(),
            id = el.id,
            rules: any = {},
            indexUnderlineById = id.indexOf('_');
        if (hasValidate === 'false') return;

        // 遍历每个属性名
        attributeNames.forEach(attrName => {
            // 如果属性名以'data-val'开头
            if (!attrName.startsWith('data-val-')) return;

            let name = attrName.replace('data-val-', '');
            let value = el.getAttribute(attrName);
            if (!name.includes('-')) {
                rules[name] = { message: value };
                return;
            }
            let validateValueName = name.split("-");
            rules[validateValueName[0]] = rules[validateValueName[0]] || {};
            if (value?.startsWith('*.')) {
                value = value?.replace('*.', id.substring(0, indexUnderlineById + 1));
            }
            rules[validateValueName[0]][validateValueName[1]] = value;
        });

        me.createPassworRule(rules, id, el.autocomplete);
        me.rules = rules;
    }

    private createPassworRule(rules: Record<string, any>, id: string, autocomplete: string) {
        if (autocomplete !== 'new-password' || id.toLowerCase().includes('confirm')) return;
        let resouce = abp.localization.getResource('AbpIdentity'),
            settings = abp.setting.values;
        for (let setting of PASSWORD_SETTING) {
            let name = setting.replace(`${PASSWORD_SETTING_PRIFIX}.Require`, '').toLowerCase(),
                value = settings[setting];
            if (name.startsWith('uni')) continue;
            if (name === 'length') {
                rules.length['min'] = value;
                continue;
            }
            if (value !== 'True') continue
            rules[name] = { message: resouce(`DisplayName:${setting}`) }

        }

    }

    private bindEventAndElement(el: HTMLElement) {
        let me = this;
        me.input!.addEventListener('input', me.onChange.bind(me));
        me.clearButton = el.querySelector('button.clear-button');
        me.clearButton?.addEventListener('click', me.onClearInputValue.bind(me));
        me.eyeButton = el.querySelector('button.show-password-button');
        me.eyeButton?.addEventListener('click', me.onSwitchInputType.bind(me));
        me.errorElement = el.querySelector('.text-error');
        console.log(this.errorElement)
        me.indicatorElement = el.querySelector('.indicator');

    }

    private onClearInputValue(event: Event) {
        let input = this.input! as HTMLInputElement;
        input.value = "";
        let e = new Event('input');
        input.dispatchEvent(e);
    }

    private onSwitchInputType(event: Event) {
        let input = this.input!;
        let currentType = this.currentPasswordType;
        this.currentPasswordType = currentType === 'password' ? "text" : "password";
        input.setAttribute('type', this.currentPasswordType);
        this.switchEyeButtonState(true);
    }

    private onChange(event: Event) {
        const me = this;
        const changeTask = me.changeTask;

        if (changeTask) {
            clearTimeout(changeTask);
            me.changeTask = undefined;
        }
        me.changeTask = setTimeout(function () {
            me.changeTask = undefined;
            me.onValueChange(event);
        }, 500);

    }

    private onValueChange(event: Event) {
        const me = this,
            value = me.input!.value,
            indicatorElement = me.indicatorElement as HTMLElement;
        if (indicatorElement) {
            me.refreshIndicator(indicatorElement, value);
        }

        me.isValid();

        if (value) {
            me.switchEyeButtonState(true);
            return me.clearButton!.classList.remove('hidden');
        }
        me.clearButton?.classList.add('hidden');
        me.switchEyeButtonState(false);
    }

    private refreshIndicator(el: HTMLElement, value: string) {
        let me = this,
            nodes = el.children,
            result = zxcvbn(value),
            score = result.guesses_log10,
            present = Math.floor(score / 12 * 100),
            mainClors = present <= 50 ? 'error' : present <= 75 ? 'warning' : 'success';
        present = present > 100 ? 100 : present;
        for (let i = 0; i <= 4; i++) {
            if (present < 0) {
                me.setIndicatorValue(nodes[i] as HTMLProgressElement, present, 'gray-200');
                continue;
            }
            let value = present % 20;
            if (value === present) {
                value = value * 5;
            } else {
                value = 100;
            }
            present -= 20;
            me.setIndicatorValue(nodes[i] as HTMLProgressElement, value, mainClors);
        }
    }

    private setIndicatorValue(el: HTMLProgressElement, value: number, classname: string) {
        let classList = el.classList;
        el.value = value;
        classList.remove('progress-error', 'progress-warning', 'progress-success', 'progress-gray-200');
        classList.add(`progress-${classname}`);

    }

    private switchEyeButtonState(isShow: boolean) {
        const button = this.eyeButton as HTMLElement;
        if (!button) return;
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

}
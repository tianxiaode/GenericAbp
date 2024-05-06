import Input from "./Input";
import Validation from './Validation'
export default class Form {

    private form : HTMLFormElement
    private inputs : Input[];
    constructor(el: HTMLFormElement) {
        this.form = el;
        this.inputs = [];
        this.initInput()
        el.addEventListener('submit', this.onSubmit.bind(this));
    }

    private initInput() {
        const els = this.form.querySelectorAll('input[data-role=input]');
        els.forEach( (el : Element) => {
            this.inputs.push(new Input(el as HTMLInputElement));
        })
    }

    private onSubmit(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        const inputs = this.inputs;
        let isValid = true;
        inputs.forEach(i => {
            isValid = this.validate(i);
        })
        if(!isValid) return;
        this.form.submit();
    }

    private validate(input: Input) : boolean{
        const rules = input.rules;
        const ruleNames = Object.keys(rules);
        const value = input.value || '';
        let isValid = true;
        let message = '';
        if(ruleNames.length === 0) return true;
        ruleNames.forEach(name => {
            const rule = rules[name];
            const result = Validation.validate(name, value, rule);
            if(result) return;
            isValid = false;
            message += rule.message;
        })
        input.error = message;
        return isValid
    }

}
import Input from "./Input";
import Validation from './Validation'
export default class Form {

    private form : HTMLFormElement
    private inputs : Input[];
    private compares: Record<string, Input>;
    constructor(el: HTMLFormElement) {
        this.form = el;
        this.inputs = [];
        this.compares = {};
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
            let reuslt = i.isValid();
            isValid = isValid && reuslt; 
        })
        if(!isValid) return;
        this.form.submit();
    }

}
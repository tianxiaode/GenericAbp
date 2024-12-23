﻿import Input from "./Input";
export default class Form {

    private form : HTMLFormElement
    private inputs : Input[];
    private submit: Function | null
    constructor(el: HTMLFormElement, submint: Function | null = null) {
        this.form = el;
        this.inputs = [];
        this.initInput()
        this.submit = submint;
        el.addEventListener('submit', this.onSubmit.bind(this));
    }

    private initInput() {
        const els = this.form.querySelectorAll('.input-group');
        els.forEach( (el : Element) => {
            this.inputs.push(new Input(el as HTMLElement));
        })
    }

    private onSubmit(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        const inputs = this.inputs;
        let allValid = true;
        for (let input of inputs){
            let isValid = input.isValid();
            if(isValid)continue;
            allValid = false;
            break;
        }
        if(!allValid) return;
        if (this.submit) {
            this.submit(event);
            return;
        }
        this.form.submit();
    }

}
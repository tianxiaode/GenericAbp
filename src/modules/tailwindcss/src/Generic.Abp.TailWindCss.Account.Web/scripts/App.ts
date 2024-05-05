import Input from "./Input"
export default class App {
    constructor() {
        this.initInput();
    }

    initInput() {
        const els = document.querySelectorAll('input[data-role=input]');
        els.forEach( (el : Element) => {
            new Input(el as HTMLInputElement);
        })
    }
}
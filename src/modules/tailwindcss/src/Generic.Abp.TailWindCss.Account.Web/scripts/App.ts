import Input from "./Input"
export default class App {
    constructor() {
        this.initInput();
    }

    initInput() {
        const els = document.querySelectorAll('input[data-role=input]');
        console.log('测试生成22222224444444444555555555')
        els.forEach( (el : Element) => {
            new Input(el as HTMLInputElement);
        })
    }
}
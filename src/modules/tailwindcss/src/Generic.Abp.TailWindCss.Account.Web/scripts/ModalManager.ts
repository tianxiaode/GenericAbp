export default class ModalManager {

    private viewUrl: string = ''
    private config: Record<string,any>
    private modalContainer: HTMLDivElement | null = null
    private containerId: string
    private onOpenCallbacks: Function[]
    private onCloseCallbacks: Function[]
    private onResultCallbacks: Function[]
    constructor(config: any) {
        this.config = {};
        if(typeof config === 'string') {
            this.viewUrl = config;
            config = { viewUrl: config }
        }
        this.config = config
        this.containerId = 'Abp_Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
        this.onOpenCallbacks = [];
        this.onCloseCallbacks = [];
        this.onResultCallbacks = [];
        this.createContainer();
    }

    open(data:any) {
        data = data || {};
        let me = this;
        for(var a in )
        fetch(me.viewUrl, data).then(me.getModalSuccess.bind(me), me.getModalFailure.bind(me))
    }

    onResult(callback: Function) {
        this.onResultCallbacks.push(callback);
    }

    onClose(callback: Function) {
        this.onCloseCallbacks.push(callback);
    }

    onOpen(callback: Function) {
        this.onOpenCallbacks.push(callback);
    }

    private getModalSuccess(response: any) {
        console.log(response);
        this.initAndShowModal()
    }

    private getModalFailure(response: any) {
        console.log(response)
        //abp.notify.error();
    }

    private createContainer() {
        let div = document.createElement('div');
        div.id = this.containerId;
        this.modalContainer = div;
        document.body.appendChild(div);
    }

    private removeContainer(container: HTMLDivElement) {
        container.remove();
    }

    private initAndShowModal() {
        let me = this,
            container = me.modalContainer! as HTMLDivElement;
        
    }

    private triggerAll(me: any, callbacks: Function[], argumentList: any) {
        
        callbacks.forEach(f => {
            f.apply(me, argumentList);
        })
        
    }
}
import Form from './Form'
import LoadingMask from './LoadingMask'
export default class ModalManager {

    private viewUrl: string = ''
    private config: Record<string, any>
    private modalContainer: HTMLDivElement | null = null
    private containerId: string
    private modal: HTMLElement | null = null
    private onOpenCallbacks: Function[]
    private onCloseCallbacks: Function[]
    private onResultCallbacks: Function[]
    private mask: LoadingMask | null = null
    constructor(config: any) {
        this.config = {};
        if (typeof config === 'string') {
            this.viewUrl = config;
            config = { viewUrl: config }
        }
        this.config = config
        this.containerId = 'Abp_Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
        this.onOpenCallbacks = [];
        this.onCloseCallbacks = [];
        this.onResultCallbacks = [];
    }

    open(data?: any | undefined) {
        data = data || {};
        let me = this;
        abp.axiosManager.get(me.viewUrl, data).then(me.getModalSuccess.bind(me), me.getModalFailure.bind(me))
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

    private getModalSuccess(data: any) {
        let me = this;
        me.createContainer();
        me.initAndShowModal(data)
    }

    private getModalFailure(response: any) {
        console.log(response)
        //abp.notify.error();
    }

    private createContainer() {
        let me = this,
            div = document.createElement('div');
        if (me.modalContainer) {
            me.removeContainer(me.modalContainer);
        }
        div.id = this.containerId;
        me.modalContainer = div;
        document.body.appendChild(div);
    }

    private removeContainer(container: HTMLDivElement) {
        container.remove();
    }

    private initAndShowModal(data: string) {
        let me = this,
            container = me.modalContainer! as HTMLDivElement;
        container.innerHTML = data;
        let modal = container.querySelector('dialog.modal') as HTMLElement;
        me.modal = modal;
        new Form(container.querySelector('form') as HTMLFormElement, me.onSubmit.bind(me));
        me.modal?.classList.add('modal-open');
        container.querySelector('.btn-times')?.addEventListener('click', me.onCancel.bind(me));
        container.querySelector('.btn-cancel')?.addEventListener('click', me.onCancel.bind(me));
        me.triggerAll(null, me.onOpenCallbacks, null);
    }

    private triggerAll(me: any, callbacks: Function[], argumentList: any) {

        callbacks.forEach(f => {
            f.apply(me, argumentList);
        })

    }

    private onCancel(event: Event) {
        let me = this;
        me.modal?.classList.remove('modal-open');
        me.modal = null;
        me.removeContainer(me.modalContainer!);
        me.modalContainer = null;
        me.triggerAll(null, me.onCloseCallbacks, null)
    }

    private onSubmit(event: Event) {
        let me = this,
            form = event.target as HTMLFormElement,
            formData = new FormData(form);
        me.showMask();
        abp.axiosManager.post(me.viewUrl, formData).then(me.onPostSuccess.bind(me), me.onPostFailure.bind(me));
    }

    private onPostSuccess(response: any) {
        let me = this;
        me.hideMask();
        console.log('onPostSuccess',response)

    }

    private onPostFailure(response: any) {
        this.hideMask();
        console.log('onPostFailure',response)
    }

    private showMask() {
        let resource = abp.localization.getResource('TailWindCssAccountWeb'),
            mask = new LoadingMask(resource("Submitting"));
        this.mask = mask;
    }

    private hideMask() {
        this.mask?.hide();
        this.mask = null;
    }

}
export default class Deferred<T> {
    public resolve!: (value: T | PromiseLike<T>) => void;
    public reject!: (reason?: any) => void;
    public promise: Promise<T>;
    public requestId: string;
    public cancel: () => void;

    constructor() {
        this.promise = new Promise<T>((resolve, reject) => {
            this.resolve = resolve;
            this.reject = reject;
        });
        this.requestId = this.generateRequestId();
        this.cancel = () => {};
    }

    generateRequestId() {
        return Date.now() + '-' + Math.random().toString(36).substr(2, 9);
    }
}

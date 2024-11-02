import { Deferred, getId } from "../utils";

export class HttpDeferred<T> extends Deferred<T>  {
    public requestId: string;
    public cancel:()=>void;

    constructor(){
        super();
        this.requestId = getId('request');
        this.cancel = () => {};
    }
}
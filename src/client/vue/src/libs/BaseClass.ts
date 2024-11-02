
export interface BaseClassInterface {
    $className: string;
}


export class BaseClass extends EventTarget implements BaseClassInterface {
    $className: string = 'BaseClass';
    private callbacks: Map<string, Array<Function>> = new Map();
    private callbackToWrappedCallback: Map<Function, Function> = new Map();


    on(eventName: string, callback: Function): void {
        const wrappedCallback = (event: any) => {
            let detail = (event as CustomEvent).detail;
            callback(...detail);
        };

        if (!this.callbacks.has(eventName)) {
            this.callbacks.set(eventName, []);
        }

        const callbacks = this.callbacks.get(eventName)!;
        callbacks.push(callback);
        this.callbackToWrappedCallback.set(callback, wrappedCallback);
        this.addEventListener(eventName, wrappedCallback);
    }

    un(eventName: string, callback: Function): void {
        const callbacks = this.callbacks.get(eventName);
        if (callbacks) {
            const index = callbacks.indexOf(callback);
            if (index !== -1) {
                const wrappedCallback = this.callbackToWrappedCallback.get(callback);
                if (wrappedCallback) {
                    this.removeEventListener(eventName, wrappedCallback as EventListenerOrEventListenerObject);
                    this.callbackToWrappedCallback.delete(callback);
                }
                callbacks.splice(index, 1);
            }
        }
    }

    fireEvent(eventName: string, detail: any): void {
        this.dispatchEvent(new CustomEvent(eventName, { detail }));
    }

    removeAllListeners(): void {
        for (const [eventName, callbacks] of this.callbacks.entries()) {
            for (const callback of callbacks) {
                const wrappedCallback = this.callbackToWrappedCallback.get(callback);
                if (wrappedCallback) {
                    this.removeEventListener(eventName, wrappedCallback as EventListenerOrEventListenerObject);
                }
            }
        }
        this.callbacks.clear();
        this.callbackToWrappedCallback.clear();
    }   

    destroy(): void {
        this.removeAllListeners();
    }
}

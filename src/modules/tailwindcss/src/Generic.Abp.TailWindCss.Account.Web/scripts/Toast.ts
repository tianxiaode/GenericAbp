export default class Toast {
     static toasts: { type: string, message: string, duration: number }[] = [];
    static showMessage(type: string, message: string, duration: number = 3000) {

        const toast = document.createElement('div');
        toast.className = "toast toast-top toast-center";
        toast.innerHTML = `
            <div class="toast toast-top toast-center ">
	            <div class="body p-2 bg-${type} font-bold text-white rounded opactity-70">
		            <span>${message}</span>
	            </div>
            </div>
        `;
        document.body.appendChild(toast);
        setTimeout(() => {
            toast.remove();
            this.toasts.shift(); // Remove the current toast from the queue
            if (this.toasts.length > 0) {
                const nextToast = this.toasts[0]; // Get the next toast in the queue
                this.showMessage(nextToast.type, nextToast.message, nextToast.duration);
            }
        }, duration + 1000);    }

    static addToast(type: string, message: string, duration: number = 3000) {
        this.toasts.push({ type, message, duration });

        // If there is only one toast in the queue, show it immediately
        if (this.toasts.length === 1) {
            this.showMessage(type, message, duration);
        }
    }

    static success(message: string, duration: number = 3000) {
        this.addToast("success", message, duration); 
    }

    static error(message: string, duration: number = 3000) {
        this.addToast("error", message, duration);
    }


    static warning(message: string, duration: number = 3000) {
        this.addToast("warning", message, duration);   
    }


    static info(message: string, duration: number = 3000) {
        this.addToast("info", message, duration);   
    }
}
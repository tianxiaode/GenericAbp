
export default class LoadingMask {
    private readonly element: HTMLDivElement;
    constructor(message: string, element: HTMLElement | null = null) {
        this.element = document.createElement('div');
        this.element.className = 'loading-mask';
        this.element.innerHTML = `
            <div class="join join-vertical">
                <div class="join-item mx-auto loading loading-lg loading-spinner text-gray-200 opacity-80"></div>
                <h4 class="join-item pt-4 text-gray-200 opacity-80 text-center mt-2">${message}</h4>
            </div>
        `;

        if(element) {
            element.appendChild(this.element);
            return;
        }

        document.body.appendChild(this.element);
    }

    hide() {
        this.element.remove();
    }
}
export function getLocaleFromNavigator(): string {
    try {
        return navigator.language || 'zh-CN';
    } catch (error) {
        console.error('Failed to get locale from navigator:', error);
        return 'zh-CN';
    }
}

export function defer(callback: Function, delay: number = 0, scope: any = null, args: any[] = [], appendArgs: boolean = false ): void {
    try {
        const executeCallback = appendArgs ? callback.bind(scope, ...args) : ()=>{ callback.apply(scope, args) };

        if (delay === 0) {
            executeCallback();            
        } else {
            setTimeout(executeCallback, delay);
        }
    } catch (error) {
        console.error('Failed to defer callback:', error);
    }
}
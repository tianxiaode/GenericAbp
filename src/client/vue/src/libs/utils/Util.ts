export function getLocaleFromNavigator(): string {
    try {
        return navigator.language || "zh-CN";
    } catch (error) {
        console.error("Failed to get locale from navigator:", error);
        return "zh-CN";
    }
}

export function defer(
    callback: Function,
    delay: number = 0,
    scope: any = null,
    args: any[] = [],
    appendArgs: boolean = false
): void {
    try {
        const executeCallback = appendArgs
            ? callback.bind(scope, ...args)
            : () => {
                  callback.apply(scope, args);
              };

        if (delay === 0) {
            executeCallback();
        } else {
            setTimeout(executeCallback, delay);
        }
    } catch (error) {
        console.error("Failed to defer callback:", error);
    }
}

export const debounce = (
    func: Function,
    wait: number = 0,
    immediate: boolean = false
) => {
    let timeout: any;
    let result: any;

    const debounced = (...args: any[]) => {
        // 确保 `this` 上下文的正确性
        const context = this;

        const later = () => {
            timeout = null;
            if (!immediate) {
                try {
                    result = func.apply(context, args);
                } catch (error) {
                    console.error("Debounced function error:", error);
                }
            }
        };

        const callNow = immediate && !timeout;

        clearTimeout(timeout);

        // 确保 `wait` 至少为 4ms
        timeout = setTimeout(later, Math.max(wait, 4));

        if (callNow) {
            try {
                result = func.apply(context, args);
            } catch (error) {
                console.error("Debounced function error:", error);
            }
        }

        return result;
    };

    return debounced;
};

const safeApply = (func: Function, context: any, args: any[]) => {
    try {
        func.apply(context, args);
    } catch (error) {
        console.error("Throttled function error:", error);
    }
};

export const throttle = (func: Function, wait: number = 0) => {
    let timeout: any;
    let lastInvokeTime: number = 0;
    const throttled = (...args: any[]) => {
        const now = Date.now();
        const elapsed = now - lastInvokeTime;
        if (elapsed > wait) {
            lastInvokeTime = now;
            safeApply(func, throttled, args);
        } else if (!timeout) {
            try {
                timeout = setTimeout(() => {
                    timeout = null;
                    lastInvokeTime = now;
                    safeApply(func, throttled, args);
                }, wait - elapsed);
            } catch (error) {
                console.error("SetTimeout error:", error);
            }
        }
    };
    return throttled;
};
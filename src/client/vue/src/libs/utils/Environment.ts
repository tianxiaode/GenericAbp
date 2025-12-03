/**
 * 🎯 环境检测工具
 * 安全地检测当前运行环境，避免未定义变量错误
 */
export const Environment = {
    /**
     * 是否是浏览器环境
     */
    get isBrowser(): boolean {
        try {
            return (
                typeof window !== 'undefined' &&
                typeof window.EventTarget !== 'undefined' &&
                typeof window.CustomEvent !== 'undefined'
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 Node.js 环境
     */
    get isNode(): boolean {
        try {
            return (
                typeof process !== 'undefined' &&
                process.versions?.node != null &&
                !this.isReactNative // React Native 也有 process 对象
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 React Native 环境
     */
    get isReactNative(): boolean {
        try {
            return (
                typeof navigator !== 'undefined' &&
                (navigator as any).product === 'ReactNative'
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 Deno 环境（安全检测）
     */
    get isDeno(): boolean {
        try {
            const globalObj = globalThis as any;
            return (
                typeof globalObj.Deno !== 'undefined' &&
                typeof globalObj.Deno.version !== 'undefined'
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 Bun 环境（安全检测）
     */
    get isBun(): boolean {
        try {
            const globalObj = globalThis as any;
            return (
                typeof globalObj.Bun !== 'undefined' &&
                globalObj.Bun.version !== undefined
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 Electron 环境
     */
    get isElectron(): boolean {
        try {
            const globalObj = globalThis as any;
            return (
                typeof globalObj.process !== 'undefined' &&
                globalObj.process.versions &&
                typeof globalObj.process.versions.electron !== 'undefined'
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是 Web Worker 环境
     */
    get isWebWorker(): boolean {
        try {
            return (
                typeof self !== 'undefined' &&
                typeof (self as any).importScripts !== 'undefined'
            );
        } catch {
            return false;
        }
    },

    /**
     * 是否是服务器端环境
     */
    get isServer(): boolean {
        return !this.isBrowser && !this.isWebWorker;
    },

    /**
     * 🎯 检测是否支持控制台颜色
     */
    get supportsColor(): boolean {
        try {
            // 检查 NO_COLOR 环境变量（标准：https://no-color.org/）
            if (typeof process !== 'undefined' && process.env && process.env.NO_COLOR) {
                return false;
            }

            // 检查浏览器环境
            if (this.isBrowser) {
                // 现代浏览器控制台都支持颜色
                // 也可以检查用户代理，但不太可靠
                return true;
            }

            // 检查 Node.js 环境
            if (this.isNode) {
                // Node.js 中检查 TTY
                if (process.stdout && process.stdout.isTTY) {
                    // 检查 Node.js 版本和色彩支持
                    const nodeVersion = process.versions.node.split('.').map(Number);
                    // Node.js 4.0.0+ 支持基本颜色
                    // Node.js 8.0.0+ 支持 256 色
                    // Node.js 10.0.0+ 支持 1600 万色
                    if (nodeVersion[0] >= 10) {
                        return true; // 支持丰富颜色
                    } else if (nodeVersion[0] >= 8) {
                        return true; // 支持 256 色
                    } else if (nodeVersion[0] >= 4) {
                        return true; // 支持基本颜色
                    }
                    return false;
                }
                return false;
            }

            // 检查 Deno 环境
            if (this.isDeno) {
                const globalObj = globalThis as any;
                return globalObj.Deno && globalObj.Deno.noColor !== true;
            }

            // 检查 Bun 环境
            if (this.isBun) {
                // Bun 通常支持颜色
                return true;
            }

            // 检查终端颜色支持（通用方法）
            if (typeof process !== 'undefined' && process.platform) {
                // 检查常见的不支持颜色的环境
                const platform = process.platform;
                if (platform === 'win32') {
                    // Windows 终端支持情况较复杂，但现代 Windows Terminal 支持颜色
                    // 检查是否在 CI 环境（如 GitHub Actions）
                    if (process.env.CI || process.env.TERM === 'dumb') {
                        return false;
                    }
                    // Windows 10+ 的终端通常支持颜色
                    return true;
                }

                // Unix-like 系统：检查 TERM 环境变量
                if (process.env.TERM) {
                    const term = process.env.TERM.toLowerCase();
                    // 支持颜色的终端
                    const colorTerms = [
                        'xterm', 'xterm-256color', 'xterm-color',
                        'screen', 'screen-256color',
                        'tmux', 'tmux-256color',
                        'rxvt', 'rxvt-unicode',
                        'linux', 'cygwin'
                    ];
                    return colorTerms.some(t => term.includes(t));
                }
            }

            // 默认情况：假设不支持颜色
            return false;
        } catch (error) {
            // 出错时返回 false 以保持安全
            return false;
        }
    },

    /**
     * 获取当前环境类型
     */
    getEnvironment(): 'browser' | 'node' | 'react-native' | 'deno' | 'bun' | 'electron' | 'web-worker' | 'unknown' {
        if (this.isBrowser && !this.isElectron) return 'browser';
        if (this.isNode) return 'node';
        if (this.isReactNative) return 'react-native';
        if (this.isDeno) return 'deno';
        if (this.isBun) return 'bun';
        if (this.isElectron) return 'electron';
        if (this.isWebWorker) return 'web-worker';
        return 'unknown';
    },

    /**
     * 检查是否支持 DOM API
     */
    get supportsDom(): boolean {
        try {
            return (
                this.isBrowser &&
                typeof document !== 'undefined' &&
                typeof HTMLElement !== 'undefined'
            );
        } catch {
            return false;
        }
    },

    /**
     * 检查是否支持 Worker API
     */
    get supportsWorker(): boolean {
        try {
            return (
                typeof Worker !== 'undefined' ||
                (typeof (self as any).importScripts !== 'undefined')
            );
        } catch {
            return false;
        }
    },

    /**
     * 检查是否支持 WebSocket
     */
    get supportsWebSocket(): boolean {
        try {
            return typeof WebSocket !== 'undefined';
        } catch {
            return false;
        }
    },

    /**
     * 检查是否支持 localStorage
     */
    get supportsLocalStorage(): boolean {
        try {
            return typeof localStorage !== 'undefined';
        } catch {
            return false;
        }
    },

    /**
     * 检查是否支持 sessionStorage
     */
    get supportsSessionStorage(): boolean {
        try {
            return typeof sessionStorage !== 'undefined';
        } catch {
            return false;
        }
    },

    /**
     * 检查是否支持 fetch API
     */
    get supportsFetch(): boolean {
        try {
            return typeof fetch !== 'undefined';
        } catch {
            return false;
        }
    },

    /**
     * 获取环境信息（用于调试）
     */
    getInfo(): Record<string, any> {
        return {
            environment: this.getEnvironment(),
            isBrowser: this.isBrowser,
            isNode: this.isNode,
            isReactNative: this.isReactNative,
            isDeno: this.isDeno,
            isBun: this.isBun,
            isElectron: this.isElectron,
            isWebWorker: this.isWebWorker,
            isServer: this.isServer,
            supportsColor: this.supportsColor,
            supportsDom: this.supportsDom,
            supportsWebSocket: this.supportsWebSocket,
            supportsFetch: this.supportsFetch,
            userAgent: this.isBrowser ? navigator.userAgent : undefined,
            platform: typeof process !== 'undefined' ? process.platform : undefined,
            versions: this.isNode ? process.versions : undefined
        };
    },

    /**
     * 在控制台打印环境信息
     */
    logInfo(): void {
        if (this.isBrowser && console.group) {
            console.group('Environment Info');
            const info = this.getInfo();
            Object.entries(info).forEach(([key, value]) => {
                console.log(`${key}:`, value);
            });
            console.groupEnd();
        } else {
            console.log('Environment Info:', this.getInfo());
        }
    }
};

export default Environment;
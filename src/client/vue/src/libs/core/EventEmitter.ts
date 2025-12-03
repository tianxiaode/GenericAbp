import { Environment } from "../utils";
/**
 * 🎯 可销毁接口
 * 表示对象可以被销毁和清理资源
 */
export interface IDisposable {
    /**
     * 销毁对象，清理所有资源
     */
    dispose(): void;

    /**
     * 是否已销毁
     */
    readonly isDisposed: boolean;
}

/**
 * 🎯 事件发射器接口
 * 提供统一的事件发布订阅接口，支持跨平台使用
 */
export interface IEventEmitter extends IDisposable {
    /**
     * 监听事件
     * @param event 事件名称
     * @param listener 事件监听器函数
     * @returns 返回 EventEmitter 实例，支持链式调用
     */
    on(event: string, listener: Function): this;

    /**
     * 监听事件（只触发一次）
     * @param event 事件名称
     * @param listener 事件监听器函数
     * @returns 返回 EventEmitter 实例，支持链式调用
     */
    once(event: string, listener: Function): this;

    /**
     * 取消监听事件
     * @param event 事件名称
     * @param listener 事件监听器函数
     * @returns 返回 EventEmitter 实例，支持链式调用
     */
    off(event: string, listener: Function): this;

    /**
     * 触发事件
     * @param event 事件名称
     * @param args 传递给监听器的参数
     * @returns 如果有监听器返回 true，否则返回 false
     */
    emit(event: string, ...args: any[]): boolean;

    /**
     * 移除所有事件监听器
     * @param event 可选，指定事件名称，如果不传则移除所有事件
     * @returns 返回 EventEmitter 实例，支持链式调用
     */
    removeAllListeners(event?: string): this;

    /**
     * 获取指定事件的监听器数量
     * @param event 事件名称
     * @returns 监听器数量
     */
    listenerCount(event: string): number;

    /**
     * 获取所有事件名称
     * @returns 事件名称数组
     */
    eventNames(): string[];
}

/**
 * 🎯 基于 EventTarget 的浏览器事件发射器
 * 在浏览器环境中使用，与 DOM 事件兼容
 */
export class DomEventEmitter implements IEventEmitter {
    private target: EventTarget;
    private listeners = new Map<string, Map<Function, EventListener>>();
    private onceListeners = new Map<string, Set<Function>>();
    private disposed = false;

    constructor() {
        this.target = new EventTarget();
    }

    get isDisposed(): boolean {
        return this.disposed;
    }

    on(event: string, listener: Function): this {
        if (this.disposed) return this;

        const wrappedListener: EventListener = (eventObj: Event) => {
            const customEvent = eventObj as CustomEvent;
            if (customEvent.detail && Array.isArray(customEvent.detail)) {
                listener(...customEvent.detail);
            } else {
                listener(customEvent.detail);
            }
        };

        if (!this.listeners.has(event)) {
            this.listeners.set(event, new Map());
        }

        this.listeners.get(event)!.set(listener, wrappedListener);
        this.target.addEventListener(event, wrappedListener);

        return this;
    }

    once(event: string, listener: Function): this {
        if (this.disposed) return this;

        if (!this.onceListeners.has(event)) {
            this.onceListeners.set(event, new Set());
        }

        this.onceListeners.get(event)!.add(listener);

        const onceWrapper = (...args: any[]) => {
            listener(...args);
            this.off(event, onceWrapper);
            this.onceListeners.get(event)?.delete(listener);
        };

        return this.on(event, onceWrapper);
    }

    off(event: string, listener: Function): this {
        if (this.disposed) return this;

        // 从普通监听器中移除
        const eventListeners = this.listeners.get(event);
        if (eventListeners) {
            const wrappedListener = eventListeners.get(listener);
            if (wrappedListener) {
                this.target.removeEventListener(event, wrappedListener);
                eventListeners.delete(listener);
            }
        }

        // 从一次性监听器中移除
        this.onceListeners.get(event)?.delete(listener);

        return this;
    }

    emit(event: string, ...args: any[]): boolean {
        if (this.disposed) return false;

        const customEvent = new CustomEvent(event, {
            detail: args.length === 0 ? undefined :
                args.length === 1 ? args[0] : args,
            bubbles: false,
            cancelable: true
        });

        return this.target.dispatchEvent(customEvent);
    }

    removeAllListeners(event?: string): this {
        if (this.disposed) return this;

        if (event) {
            // 移除指定事件的所有监听器
            const eventListeners = this.listeners.get(event);
            if (eventListeners) {
                for (const [listener, wrapped] of eventListeners.entries()) {
                    this.target.removeEventListener(event, wrapped);
                }
                eventListeners.clear();
            }

            // 移除一次性监听器
            this.onceListeners.delete(event);
        } else {
            // 移除所有事件的所有监听器
            for (const [eventName, eventListeners] of this.listeners.entries()) {
                for (const [listener, wrapped] of eventListeners.entries()) {
                    this.target.removeEventListener(eventName, wrapped);
                }
                eventListeners.clear();
            }

            this.listeners.clear();
            this.onceListeners.clear();
        }

        return this;
    }

    listenerCount(event: string): number {
        if (this.disposed) return 0;

        const normalCount = this.listeners.get(event)?.size || 0;
        const onceCount = this.onceListeners.get(event)?.size || 0;
        return normalCount + onceCount;
    }

    eventNames(): string[] {
        if (this.disposed) return [];

        const names = new Set<string>();
        for (const event of this.listeners.keys()) {
            names.add(event);
        }
        for (const event of this.onceListeners.keys()) {
            names.add(event);
        }
        return Array.from(names);
    }

    dispose(): void {
        if (this.disposed) return;

        // 移除所有事件监听器
        this.removeAllListeners();

        // 清理所有引用
        this.listeners.clear();
        this.onceListeners.clear();

        // 标记为已销毁
        this.disposed = true;
    }

    /**
     * 🎯 DOM 事件系统特有方法：获取底层的 EventTarget
     */
    getEventTarget(): EventTarget {
        return this.target;
    }
}

/**
 * 🎯 纯 TypeScript 的跨平台事件发射器
 * 在非浏览器环境中使用，或者作为轻量级事件系统
 */
export class CustomEventEmitter implements IEventEmitter {
    private events = new Map<string, Set<Function>>();
    private onceListeners = new Map<string, Set<Function>>();
    private disposed = false;

    get isDisposed(): boolean {
        return this.disposed;
    }

    on(event: string, listener: Function): this {
        if (this.disposed) return this;

        if (!this.events.has(event)) {
            this.events.set(event, new Set());
        }

        this.events.get(event)!.add(listener);
        return this;
    }

    once(event: string, listener: Function): this {
        if (this.disposed) return this;

        if (!this.onceListeners.has(event)) {
            this.onceListeners.set(event, new Set());
        }

        this.onceListeners.get(event)!.add(listener);

        const onceWrapper = (...args: any[]) => {
            listener(...args);
            this.off(event, onceWrapper);
            this.onceListeners.get(event)?.delete(listener);
        };

        return this.on(event, onceWrapper);
    }

    off(event: string, listener: Function): this {
        if (this.disposed) return this;

        this.events.get(event)?.delete(listener);
        this.onceListeners.get(event)?.delete(listener);
        return this;
    }

    emit(event: string, ...args: any[]): boolean {
        if (this.disposed) return false;

        let hasListeners = false;

        // 触发普通监听器
        const listeners = this.events.get(event);
        if (listeners && listeners.size > 0) {
            hasListeners = true;
            const listenersCopy = new Set(listeners);
            listenersCopy.forEach(listener => {
                try {
                    listener(...args);
                } catch (error) {
                    console.error(`Error in event listener for "${event}":`, error);
                }
            });
        }

        // 触发一次性监听器
        const onceListeners = this.onceListeners.get(event);
        if (onceListeners && onceListeners.size > 0) {
            hasListeners = true;
            const onceListenersCopy = new Set(onceListeners);
            onceListeners.clear();

            onceListenersCopy.forEach(listener => {
                try {
                    listener(...args);
                } catch (error) {
                    console.error(`Error in once event listener for "${event}":`, error);
                }
            });
        }

        return hasListeners;
    }

    removeAllListeners(event?: string): this {
        if (this.disposed) return this;

        if (event) {
            this.events.delete(event);
            this.onceListeners.delete(event);
        } else {
            this.events.clear();
            this.onceListeners.clear();
        }

        return this;
    }

    listenerCount(event: string): number {
        if (this.disposed) return 0;

        const normalCount = this.events.get(event)?.size || 0;
        const onceCount = this.onceListeners.get(event)?.size || 0;
        return normalCount + onceCount;
    }

    eventNames(): string[] {
        if (this.disposed) return [];

        const names = new Set<string>();
        for (const event of this.events.keys()) {
            names.add(event);
        }
        for (const event of this.onceListeners.keys()) {
            names.add(event);
        }
        return Array.from(names);
    }

    dispose(): void {
        if (this.disposed) return;

        // 移除所有事件监听器
        this.removeAllListeners();

        // 清理所有引用
        this.events.clear();
        this.onceListeners.clear();

        // 标记为已销毁
        this.disposed = true;
    }
}

/**
 * 🎯 事件系统工厂
 * 根据环境自动创建合适的事件发射器
 */
export class EventEmitterFactory {
    /**
     * 创建事件发射器
     * @param type 指定事件系统类型：'auto' | 'dom' | 'custom'
     * @returns 事件发射器实例
     */
    static create(type: 'auto' | 'dom' | 'custom' = 'auto'): IEventEmitter {
        switch (type) {
            case 'dom':
                return new DomEventEmitter();
            case 'custom':
                return new CustomEventEmitter();
            case 'auto':
            default:
                return this.createAuto();
        }
    }

    /**
     * 根据环境自动创建事件发射器
     * @returns 适合当前环境的事件发射器
     */
    private static createAuto(): IEventEmitter {
        // 检测是否是浏览器环境
        const isBrowser = typeof window !== 'undefined' &&
            typeof window.EventTarget !== 'undefined' &&
            typeof window.CustomEvent !== 'undefined';

        if (isBrowser) {
            return new DomEventEmitter();
        } else {
            return new CustomEventEmitter();
        }
    }

    /**
     * 判断当前环境是否是浏览器
     * @returns 如果是浏览器环境返回 true
     */
    static isBrowser(): boolean {
        return typeof window !== 'undefined' &&
            typeof window.EventTarget !== 'undefined' &&
            typeof window.CustomEvent !== 'undefined';
    }
}

// 导出类型别名
export type EventEmitterType = IEventEmitter;

// 默认导出工厂类
export default EventEmitterFactory;
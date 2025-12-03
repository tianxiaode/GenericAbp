// libs/core/EventBase.ts
import { IEventEmitter, EventEmitterFactory, IDisposable } from './EventEmitter';
import { logger, Environment } from '../utils';

/**
 * 🎯 事件基类 - 所有需要事件功能的类的基类
 * 自动根据环境选择合适的事件系统，提供统一的事件 API
 */
export abstract class EventBase implements IDisposable {
    protected $className: string = 'EventBase';

    // 🎯 内部事件系统实例
    private _events: IEventEmitter;
    private _disposed = false;

    constructor() {
        // 🎯 自动创建适合当前环境的事件发射器
        this._events = EventEmitterFactory.create('auto');
        this.initialize();

        logger.debug(this, "[constructor]", `${this.$className} initialized`);
    }

    // 🎯 初始化方法，子类可以覆盖
    protected initialize(): void {
        // 可以在这里设置默认的事件监听
    }

    // 🎯 统一的事件 API -------------------------------------------------

    /**
     * 监听事件
     */
    on(event: string, listener: Function): this {
        this._events.on(event, listener);
        return this;
    }

    /**
     * 监听事件（只触发一次）
     */
    once(event: string, listener: Function): this {
        this._events.once(event, listener);
        return this;
    }

    /**
     * 取消监听事件
     */
    off(event: string, listener: Function): this {
        this._events.off(event, listener);
        return this;
    }

    /**
     * 触发事件
     */
    emit(event: string, ...args: any[]): boolean {
        return this._events.emit(event, ...args);
    }

    /**
     * 移除所有事件监听器
     */
    removeAllListeners(event?: string): this {
        this._events.removeAllListeners(event);
        return this;
    }

    /**
     * 获取指定事件的监听器数量
     */
    listenerCount(event: string): number {
        return this._events.listenerCount(event);
    }

    /**
     * 获取所有事件名称
     */
    eventNames(): string[] {
        return this._events.eventNames();
    }

    // 🎯 生命周期管理 -------------------------------------------------

    get isDisposed(): boolean {
        return this._disposed || this._events.isDisposed;
    }

    /**
     * 🎯 销毁对象，清理所有资源
     * 子类应该覆盖此方法，并调用 super.dispose()
     */
    dispose(): void {
        if (this._disposed) return;

        // 清理事件监听器
        this._events.dispose();

        this._disposed = true;
        logger.debug(this, "[dispose]", `${this.$className} disposed`);
    }

    // 🎯 保护的方法，供子类使用 -----------------------------------------

    /**
     * 获取底层事件发射器（用于高级操作）
     */
    protected getEventEmitter(): IEventEmitter {
        return this._events;
    }

    /**
     * 🎯 DOM 事件绑定辅助方法（仅在浏览器环境有效）
     * 将 DOM 事件转发到内部事件系统
     */
    protected bindDomEvent(
        element: HTMLElement | Window | Document,
        eventName: string,
        internalEventName?: string,
        options?: AddEventListenerOptions
    ): () => void {
        // 检查是否是浏览器环境
        if (!Environment.isBrowser) {
            logger.warn(this, "[bindDomEvent]", "Not in browser environment");
            return () => { };
        }

        const eventToEmit = internalEventName || eventName;
        const handler = (event: Event) => {
            this.emit(eventToEmit, event);
        };

        element.addEventListener(eventName, handler, options);

        // 返回清理函数
        return () => {
            element.removeEventListener(eventName, handler, options);
        };
    }

    /**
     * 🎯 批量绑定 DOM 事件
     */
    protected bindDomEvents(
        element: HTMLElement | Window | Document,
        events: Array<{
            domEvent: string;
            internalEvent?: string;
            options?: AddEventListenerOptions;
        }>
    ): () => void {
        const unbindFunctions: Array<() => void> = [];

        events.forEach(({ domEvent, internalEvent, options }) => {
            const unbind = this.bindDomEvent(
                element,
                domEvent,
                internalEvent,
                options
            );
            unbindFunctions.push(unbind);
        });

        // 返回批量清理函数
        return () => {
            unbindFunctions.forEach(unbind => unbind());
        };
    }

    /**
     * 🎯 自动绑定组件事件（用于 UI 组件）
     * 组件可以覆盖此方法来自定义事件绑定
     */
    protected setupEventBindings(): void {
        // 默认实现为空，子类可以覆盖
    }
}

// 导出 EventBase
export default EventBase;
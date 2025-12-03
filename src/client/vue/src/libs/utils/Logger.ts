// libs/core/utils/Logger.ts
import { Environment } from './Environment';

export type LogLevel = 'debug' | 'info' | 'warn' | 'error' | 'trace' | 'silent';

export interface LogEntry {
    timestamp: string;
    level: LogLevel;
    className?: string;
    method?: string;
    message: string;
    data?: any;
    source?: string;
}

export interface LoggerOptions {
    level?: LogLevel;
    enableColors?: boolean;
    showTimestamp?: boolean;
    showClassName?: boolean;
    showMethod?: boolean;
    showLevel?: boolean;
    maxDataDepth?: number;
    customFormatter?: (entry: LogEntry) => string;
}

/**
 * 🎯 增强版日志工具类
 * 提供结构化、可配置的日志输出，支持两种调用方式：
 * 1. 带上下文：logger.debug(this, "methodName", "message", ...data)
 * 2. 不带上下文：logger.debug("message", ...data)
 */
export class Logger {
    private static instance: Logger;
    private options: LoggerOptions;
    private levels: Record<LogLevel, number> = {
        debug: 0,
        trace: 1,
        info: 2,
        warn: 3,
        error: 4,
        silent: 5
    };

    private colors = {
        reset: '\x1b[0m',
        dim: '\x1b[2m',
        // 前景色
        black: '\x1b[30m',
        red: '\x1b[31m',
        green: '\x1b[32m',
        yellow: '\x1b[33m',
        blue: '\x1b[34m',
        magenta: '\x1b[35m',
        cyan: '\x1b[36m',
        white: '\x1b[37m',
        // 背景色
        bgBlack: '\x1b[40m',
        bgRed: '\x1b[41m',
        bgGreen: '\x1b[42m',
        bgYellow: '\x1b[43m',
        bgBlue: '\x1b[44m',
        bgMagenta: '\x1b[45m',
        bgCyan: '\x1b[46m',
        bgWhite: '\x1b[47m'
    };

    private levelColors: Record<LogLevel, string> = {
        debug: this.colors.magenta,
        trace: this.colors.cyan,
        info: this.colors.green,
        warn: this.colors.yellow,
        error: this.colors.red,
        silent: this.colors.white
    };

    private constructor(options: LoggerOptions = {}) {
        this.options = {
            level: Environment.isBrowser ? 'info' : 'debug',
            enableColors: !Environment.isBrowser || Environment.supportsColor,
            showTimestamp: true,
            showClassName: true,
            showMethod: true,
            showLevel: true,
            maxDataDepth: 2,
            ...options
        };
    }

    // 🎯 单例模式 -------------------------------------------------

    /**
     * 获取 Logger 实例（单例模式）
     */
    public static getInstance(options?: LoggerOptions): Logger {
        if (!Logger.instance) {
            Logger.instance = new Logger(options);
        }
        if (options) {
            Logger.instance.configure(options);
        }
        return Logger.instance;
    }

    /**
     * 重置单例实例（主要用于测试）
     */
    public static resetInstance(): void {
        Logger.instance = undefined as any;
    }

    // 🎯 配置管理 -------------------------------------------------

    /**
     * 配置 Logger
     */
    public configure(options: Partial<LoggerOptions>): void {
        this.options = { ...this.options, ...options };
    }

    /**
     * 设置日志级别
     */
    public setLevel(level: LogLevel): void {
        this.options.level = level;
    }

    /**
     * 获取当前日志级别
     */
    public getLevel(): LogLevel {
        return this.options.level!;
    }

    // 🎯 日志方法 - 带上下文的重载版本 ------------------------------

    /**
     * 🎯 调试日志
     */
    public debug(context: any, method: string, message?: any, ...data: any[]): void;
    public debug(message: string, ...data: any[]): void;
    public debug(...args: any[]): void {
        this.logWithContext('debug', ...args);
    }

    /**
     * 🎯 信息日志
     */
    public info(context: any, method: string, message?: any, ...data: any[]): void;
    public info(message: string, ...data: any[]): void;
    public info(...args: any[]): void {
        this.logWithContext('info', ...args);
    }

    /**
     * 🎯 警告日志
     */
    public warn(context: any, method: string, message?: any, ...data: any[]): void;
    public warn(message: string, ...data: any[]): void;
    public warn(...args: any[]): void {
        this.logWithContext('warn', ...args);
    }

    /**
     * 🎯 错误日志
     */
    public error(context: any, method: string, message?: any, ...data: any[]): void;
    public error(message: string, ...data: any[]): void;
    public error(...args: any[]): void {
        this.logWithContext('error', ...args);
    }

    /**
     * 🎯 跟踪日志（详细调试）
     */
    public trace(context: any, method: string, message?: any, ...data: any[]): void;
    public trace(message: string, ...data: any[]): void;
    public trace(...args: any[]): void {
        this.logWithContext('trace', ...args);
    }

    /**
     * 🎯 抛出错误（记录日志后抛出异常）
     */
    public raise(context: any, method: string, message?: any, ...data: any[]): never;
    public raise(message: string, ...data: any[]): never;
    public raise(...args: any[]): never {
        const entry = this.parseLogEntry('error', ...args);
        this.output(entry);

        // 构建错误消息
        const errorMessage = [
            entry.className && entry.className !== 'Global' ? `[${entry.className}]` : '',
            entry.method && entry.method !== 'global' ? `[${entry.method}]` : '',
            entry.message,
            ...(entry.data || []).map((item: any) =>
                typeof item === 'object' ? JSON.stringify(item) : String(item)
            )
        ].filter(Boolean).join(' ');

        throw new Error(errorMessage);
    }

    // 🎯 核心日志方法 -------------------------------------------------

    /**
     * 🎯 带上下文的通用日志方法
     */
    private logWithContext(level: LogLevel, ...args: any[]): void {
        // 检查日志级别
        if (this.shouldSkip(level)) return;

        // 解析参数
        const entry = this.parseLogEntry(level, ...args);

        // 输出日志
        this.output(entry);
    }

    /**
     * 解析日志条目
     */
    private parseLogEntry(level: LogLevel, ...args: any[]): LogEntry {
        if (args.length === 0) {
            return {
                timestamp: new Date().toISOString(),
                level,
                className: 'Global',
                method: 'unknown',
                message: '',
                data: []
            };
        }

        const firstArg = args[0];
        const secondArg = args[1];

        // 判断是否传入了上下文对象（检查是否包含 $className 或构造函数名）
        const hasContext = typeof firstArg === 'object' && firstArg !== null &&
            (firstArg.$className || firstArg.constructor?.name);

        if (hasContext) {
            // 方式1: logger.debug(this, "method", "message", ...data)
            const context = firstArg;
            const method = typeof secondArg === 'string' ? secondArg : '';
            const messageIndex = method ? 2 : 1;
            const message = args[messageIndex] || '';
            const data = args.slice(messageIndex + 1);

            return {
                timestamp: new Date().toISOString(),
                level,
                className: context.$className || context.constructor?.name || 'UnknownClass',
                method: this.sanitizeMethodName(method),
                message: typeof message === 'string' ? message : String(message),
                data,
                source: this.getSourceInfo()
            };
        } else {
            // 方式2: logger.debug("message", ...data)
            const message = typeof firstArg === 'string' ? firstArg : String(firstArg);
            const data = args.slice(1);

            return {
                timestamp: new Date().toISOString(),
                level,
                className: 'Global',
                method: 'global',
                message,
                data,
                source: this.getSourceInfo()
            };
        }
    }

    /**
     * 清理方法名（移除中括号和特殊字符）
     */
    private sanitizeMethodName(method: string): string {
        if (!method) return '';

        // 移除前后中括号，如"[constructor]" -> "constructor"
        // 同时移除其他可能用于装饰的字符
        return method
            .replace(/^\[|\]$/g, '')
            .replace(/^\<|\>$/g, '')
            .trim();
    }

    /**
     * 判断是否应该跳过当前级别的日志
     */
    private shouldSkip(level: LogLevel): boolean {
        const currentLevel = this.levels[this.options.level!];
        const targetLevel = this.levels[level];
        return targetLevel < currentLevel;
    }

    /**
     * 输出日志
     */
    private output(entry: LogEntry): void {
        const message = this.options.customFormatter
            ? this.options.customFormatter(entry)
            : this.formatEntry(entry);

        // 根据级别选择输出方法
        const logMethod = this.getLogMethod(entry.level);
        logMethod(message);

        // 如果有额外数据，单独输出（保持可读性）
        if (entry.data && entry.data.length > 0) {
            entry.data.forEach((data: any) => {
                if (typeof data === 'object' && data !== null) {
                    logMethod(this.formatData(data));
                } else if (data !== undefined && data !== null) {
                    logMethod(`  ↳ ${String(data)}`);
                }
            });
        }
    }

    /**
     * 格式化日志条目
     */
    private formatEntry(entry: LogEntry): string {
        const parts: string[] = [];

        // 时间戳
        if (this.options.showTimestamp) {
            const time = new Date(entry.timestamp).toLocaleTimeString();
            parts.push(this.colorize(`[${time}]`, this.colors.dim));
        }

        // 日志级别
        if (this.options.showLevel) {
            const levelStr = entry.level.toUpperCase().padEnd(5);
            parts.push(this.colorize(`[${levelStr}]`, this.levelColors[entry.level]));
        }

        // 类名
        if (this.options.showClassName && entry.className && entry.className !== 'Global') {
            parts.push(this.colorize(`[${entry.className}]`, this.colors.cyan));
        }

        // 方法名
        if (this.options.showMethod && entry.method && entry.method !== 'global') {
            parts.push(this.colorize(`[${entry.method}]`, this.colors.blue));
        }

        // 消息
        if (entry.message) {
            parts.push(entry.message);
        }

        return parts.join(' ');
    }

    /**
     * 格式化数据对象
     */
    private formatData(data: any, depth = 0): string {
        if (depth >= (this.options.maxDataDepth || 2)) {
            return this.colorize('...', this.colors.dim);
        }

        if (Array.isArray(data)) {
            const items = data.map(item => this.formatData(item, depth + 1));
            return `[\n  ${items.join(',\n  ')}\n]`;
        }

        if (data && typeof data === 'object') {
            const entries = Object.entries(data);
            const lines = entries.map(([key, value]) => {
                const formattedValue = this.formatData(value, depth + 1);
                return `  ${key}: ${formattedValue}`;
            });
            return `{\n${lines.join('\n')}\n}`;
        }

        return String(data);
    }

    /**
     * 获取合适的日志输出方法
     */
    private getLogMethod(level: LogLevel): (...args: any[]) => void {
        switch (level) {
            case 'error':
                return console.error.bind(console);
            case 'warn':
                return console.warn.bind(console);
            case 'info':
                return console.info.bind(console);
            case 'debug':
            case 'trace':
                return console.debug.bind(console);
            default:
                return console.log.bind(console);
        }
    }

    /**
     * 获取源文件信息（仅开发环境）
     */
    private getSourceInfo(): string | undefined {
        if (!Environment.isBrowser || (this.options as any).level === 'production') {
            return undefined;
        }

        try {
            const error = new Error();
            const stack = error.stack?.split('\n');
            // 跳过前3行（Error、Logger.log、Logger.debug等）
            if (stack && stack.length > 3) {
                return stack[3].trim();
            }
        } catch {
            // 忽略错误
        }
        return undefined;
    }

    /**
     * 添加颜色（如果启用）
     */
    private colorize(text: string, color: string): string {
        if (this.options.enableColors && color) {
            return `${color}${text}${this.colors.reset}`;
        }
        return text;
    }

    // 🎯 高级功能 -------------------------------------------------

    /**
     * 创建子日志器（用于特定模块）
     */
    public createChild(context: any, className?: string): LoggerChild {
        const classNameToUse = className || context.$className || context.constructor?.name || 'Anonymous';

        return {
            debug: (method: string, message?: any, ...data: any[]) =>
                this.debug(context, method, message, ...data),
            info: (method: string, message?: any, ...data: any[]) =>
                this.info(context, method, message, ...data),
            warn: (method: string, message?: any, ...data: any[]) =>
                this.warn(context, method, message, ...data),
            error: (method: string, message?: any, ...data: any[]) =>
                this.error(context, method, message, ...data),
            trace: (method: string, message?: any, ...data: any[]) =>
                this.trace(context, method, message, ...data),
            raise: (method: string, message?: any, ...data: any[]) =>
                this.raise(context, method, message, ...data),
            getClassName: () => classNameToUse
        };
    }

    /**
     * 创建命名日志器（无上下文）
     */
    public createNamedLogger(className: string): LoggerChild {
        const context = { $className: className };
        return this.createChild(context, className);
    }

    /**
     * 记录性能指标
     */
    public perf(context: any, method: string, startTime: number, ...extraData: any[]): void {
        const duration = performance.now() - startTime;
        const className = context.$className || context.constructor?.name || 'Unknown';

        let level: LogLevel = 'debug';
        let color = this.colors.cyan;

        if (duration > 1000) {
            level = 'warn';
            color = this.colors.yellow;
        } else if (duration > 100) {
            level = 'info';
            color = this.colors.green;
        }

        const formattedDuration = this.colorize(`${duration.toFixed(2)}ms`, color);
        const message = `Performance: ${formattedDuration}`;

        this.logWithContext(level, context, method, message, ...extraData);
    }

    /**
     * 开始性能计时
     */
    public startTimer(): number {
        return performance.now();
    }

    /**
     * 结束性能计时并记录
     */
    public endTimer(context: any, method: string, startTime: number, ...extraData: any[]): void {
        this.perf(context, method, startTime, ...extraData);
    }

    /**
     * 🎯 销毁 Logger
     */
    public destroy(): void {
        // 清理资源
        this.options = {} as LoggerOptions;
    }

    /**
     * 🎯 清空控制台（仅浏览器环境）
     */
    public clear(): void {
        if (Environment.isBrowser && (this.options as any).level !== 'production') {
            console.clear();
        }
    }

    /**
     * 🎯 分组日志（仅浏览器环境）
     */
    public group(label: string, collapsed = false): void {
        if (Environment.isBrowser) {
            if (collapsed) {
                console.groupCollapsed(label);
            } else {
                console.group(label);
            }
        }
    }

    /**
     * 🎯 结束分组
     */
    public groupEnd(): void {
        if (Environment.isBrowser) {
            console.groupEnd();
        }
    }
}

/**
 * 🎯 子日志器接口
 */
export interface LoggerChild {
    debug(method: string, message?: any, ...data: any[]): void;
    info(method: string, message?: any, ...data: any[]): void;
    warn(method: string, message?: any, ...data: any[]): void;
    error(method: string, message?: any, ...data: any[]): void;
    trace(method: string, message?: any, ...data: any[]): void;
    raise(method: string, message?: any, ...data: any[]): never;
    getClassName(): string;
}

// 🎯 导出单例实例 ------------------------------------------------
export const logger = Logger.getInstance();
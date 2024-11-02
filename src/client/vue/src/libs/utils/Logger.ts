export class Logger {

    private logger: any;
    private level: string;

    constructor() {
        this.logger = console;
        this.level = 'info';
    }

    setLevel(level: string) {
        this.level = level;
    }

    getLevel() {
        return this.level;
    }

    setLogger(logger: any){
        this.logger = logger;
    }

    getLogger() {
        return this.logger;
    }


    log(prefix: string, logFn: Function, ...args: any[]){
        if(logFn === undefined){
            throw new Error(`Logger is not defined or does not have a ${prefix} method`);
        }

         args = this.normalizeArgs(prefix, args);

        logFn(...args);
    }


    debug(...args: any[]) {
        if(this.level !== 'debug') return;
        this.log('[DEBUG]', this.logger.debug, ...args)       
    }

    info(...args: any[]) {
        if(this.level === 'warn' || this.level === 'error') return;
        this.log('[INFO]', this.logger.info, ...args);
    }

    warn(...args: any[]) {
        if(this.level === 'error') return;
        this.log('[WARN]', this.logger.warn, ...args);
    }

    error(...args: any[]) {
        this.log('[ERROR]', this.logger.error, ...args);
    }

    raise(...args: any[]) {
        const logger = this.logger;
        if (logger && logger.trace) {
            logger.trace(...args);
        }
        throw new Error('[RAISE]: ' + args.join(' '));
    }

    trace(...args: any[]) {
        if (this.logger && this.logger.trace) {
            this.log('[TRACE]', this.logger.trace, ...args);
        }
    }

    private normalizeArgs(prefix: string, args: any[]) {
        const first = args[0];
        if (first && first.$className ) {
            const name = first.$className;
            args = args.slice(1);
            return [`${prefix}[${name}]`, ...args];
        }
        return [`${prefix}`, ...args];
    }



    destroy() {
        this.logger = null;
    }
}

export const logger = new Logger();

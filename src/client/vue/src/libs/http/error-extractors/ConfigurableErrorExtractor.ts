import { ErrorExtractor, ErrorExtractorConfig} from '../types'

export class ConfigurableErrorExtractor implements ErrorExtractor {
    private config: Required<ErrorExtractorConfig>;

    constructor(config: ErrorExtractorConfig = {}) {
        this.config = {
            messageFields: config.messageFields || ['message', 'msg', 'error.message', 'error.msg', 'err_msg', 'detail', 'title'],
            codeFields: config.codeFields || ['code', 'status', 'error.code', 'error.status', 'statusCode'],
            validationFields: config.validationFields || ['validationErrors', 'errors', 'error.validationErrors', 'error.errors', 'details'],
            errorObjectField: config.errorObjectField || 'error',
            defaultMessage: config.defaultMessage || 'An error occurred'
        };
    }

    canHandle(data: any, status: number): boolean {
        // 这个提取器可以处理任何数据
        return true;
    }

    extractMessage(data: any, status: number): string {
        if (!data) {
            return this.getDefaultMessage(status);
        }

        // 尝试从配置的字段中提取消息
        for (const field of this.config.messageFields) {
            const value = this.getNestedValue(data, field);
            if (value && typeof value === 'string') {
                return value;
            }
        }

        // 如果数据本身就是字符串
        if (typeof data === 'string') {
            return data;
        }

        return this.getDefaultMessage(status);
    }

    extractCode(data: any, status: number): number {
        if (!data) {
            return status;
        }

        // 尝试从配置的字段中提取代码
        for (const field of this.config.codeFields) {
            const value = this.getNestedValue(data, field);
            if (value !== undefined && value !== null) {
                const numValue = Number(value);
                if (!isNaN(numValue)) {
                    return numValue;
                }
            }
        }

        return status;
    }

    extractValidationErrors(data: any, status: number): any {
        if (!data) {
            return null;
        }

        // 尝试从配置的字段中提取验证错误
        for (const field of this.config.validationFields) {
            const value = this.getNestedValue(data, field);
            if (value) {
                return value;
            }
        }

        return null;
    }

    extractRawError(data: any, status: number): any {
        // 如果有错误对象字段，提取错误对象
        if (this.config.errorObjectField && data && typeof data === 'object') {
            const errorObj = data[this.config.errorObjectField];
            if (errorObj) {
                return errorObj;
            }
        }

        return data;
    }

    private getNestedValue(obj: any, path: string): any {
        return path.split('.').reduce((current, key) => {
            return current && current[key] !== undefined ? current[key] : undefined;
        }, obj);
    }

    private getDefaultMessage(status: number): string {
        if (status === 0) {
            return 'Network Error';
        }

        const statusMessages: Record<number, string> = {
            400: 'Bad Request',
            401: 'Unauthorized',
            403: 'Forbidden',
            404: 'Not Found',
            500: 'Internal Server Error',
            502: 'Bad Gateway',
            503: 'Service Unavailable'
        };

        return statusMessages[status] || `HTTP Error ${status}`;
    }
}

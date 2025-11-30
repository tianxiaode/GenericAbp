import { BaseResponseHandler } from "./BaseResponseHandler";
import { HttpError, HttpDeferred, HttpRequestOptions } from "../types";
import { CompositeErrorExtractor, AbpErrorExtractor } from '../error-extractors'
/**
 * ABP vNext 验证错误格式化选项
 */
export interface AbpValidationErrorOptions {
    /**
     * 验证错误格式化方式
     * - 'raw': 返回原始 ABP 格式的验证错误数组
     * - 'field-keyed': 转换为以字段名为键的对象格式
     * - 'both': 同时包含两种格式
     */
    format?: 'raw' | 'field-keyed' | 'both';

    /**
     * 当字段有多个验证错误时的处理方式
     * - 'first': 只取第一个错误
     * - 'all': 保留所有错误（数组）
     */
    multipleErrors?: 'first' | 'all';

    /**
     * 未指定字段的错误使用的键名
     */
    defaultFieldKey?: string;
}

/**
 * ABP vNext 响应处理器配置
 */
export interface AbpResponseHandlerConfig {
    successField?: string;
    dataField?: string;
    errorField?: string;
    validationErrorsField?: string;
    unAuthorizedRequestField?: string;
    enableDetailedLogging?: boolean;
    validationErrorOptions?: AbpValidationErrorOptions;
}

/**
 * 优化后的 ABP vNext 响应处理器
 */
export class AbpResponseHandler extends BaseResponseHandler {
    private config: Required<AbpResponseHandlerConfig>;

    constructor(config: AbpResponseHandlerConfig = {}) {
        super();

        this.config = {
            successField: config.successField || 'success',
            dataField: config.dataField || 'result',
            errorField: config.errorField || 'error',
            validationErrorsField: config.validationErrorsField || 'validationErrors',
            unAuthorizedRequestField: config.unAuthorizedRequestField || 'unAuthorizedRequest',
            enableDetailedLogging: config.enableDetailedLogging || false,
            validationErrorOptions: {
                format: config.validationErrorOptions?.format || 'both',
                multipleErrors: config.validationErrorOptions?.multipleErrors || 'all',
                defaultFieldKey: config.validationErrorOptions?.defaultFieldKey || '_form',
                ...config.validationErrorOptions
            }
        };

        this.setErrorExtractor(new CompositeErrorExtractor([
            new AbpErrorExtractor()
        ]));        
    }

    isSuccessResponse(data: any, status: number): boolean {
        const httpSuccess = status >= 200 && status < 300;

        // ABP 特定逻辑：检查 success 字段
        const abpSuccess = data &&
            typeof data === 'object' &&
            data.success === true;

        return httpSuccess && abpSuccess;
    }

    
    extractSuccessData(data: any): any {
        if (data && typeof data === 'object' && this.config.dataField in data) {
            return data[this.config.dataField];
        }

        return data;
    }

    
    createError(data: any, status: number): HttpError {
        if (status === 0) {
            return new HttpError("Network Error", status);
        }

        if (data && typeof data === 'object') {
            const errorData = data[this.config.errorField] || data;

            if (errorData && typeof errorData === 'object') {
                const errorMessage = this.buildErrorMessage(errorData);
                const errorCode = errorData.code || status;
                const validationErrors = this.extractValidationErrors(errorData);

                return new HttpError(errorMessage, errorCode, validationErrors);
            }

            if (data.message) {
                return new HttpError(data.message, status);
            }
        }

        return new HttpError(`HTTP Error ${status}`, status);
    }

    private buildErrorMessage(errorData: any): string {
        let message = '';

        if (errorData.details) {
            message = errorData.details;
        } else if (errorData.message) {
            message = errorData.message;
        } else {
            message = 'An error occurred';
        }

        // 记录详细错误信息（开发环境）
        if (this.config.enableDetailedLogging && console) {
            console.warn('ABP Error Details:', errorData);
        }

        return message;
    }

    /**
     * 提取并格式化 ABP 验证错误
     * 支持多种格式输出
     */
    private extractValidationErrors(errorData: any): any {
        const rawValidationErrors = errorData[this.config.validationErrorsField];

        if (!rawValidationErrors || !Array.isArray(rawValidationErrors)) {
            return null;
        }

        const { format, multipleErrors, defaultFieldKey } = this.config.validationErrorOptions;

        // 根据配置返回不同格式
        switch (format) {
            case 'raw':
                // 返回原始 ABP 格式
                return {
                    _raw: rawValidationErrors
                };

            case 'field-keyed':
                // 返回以字段为键的格式
                return this.formatFieldKeyedErrors(rawValidationErrors, multipleErrors || 'all', defaultFieldKey || '');

            case 'both':
            default:
                // 同时返回两种格式
                return {
                    _raw: rawValidationErrors,
                    ...this.formatFieldKeyedErrors(rawValidationErrors, multipleErrors || 'all', defaultFieldKey || '')
                };
        }
    }

    /**
     * 将 ABP 验证错误转换为以字段名为键的格式
     */
    private formatFieldKeyedErrors(
        rawErrors: any[],
        multipleErrors: 'first' | 'all',
        defaultFieldKey: string
    ): Record<string, string | string[]> {
        const fieldKeyedErrors: Record<string, string | string[]> = {};

        rawErrors.forEach((error: any) => {
            const { message, members } = error;

            if (members && Array.isArray(members) && members.length > 0) {
                // 为每个成员字段添加错误
                members.forEach((member: string) => {
                    this.addFieldError(fieldKeyedErrors, member, message, multipleErrors);
                });
            } else {
                // 没有指定成员，使用默认字段
                this.addFieldError(fieldKeyedErrors, defaultFieldKey, message, multipleErrors);
            }
        });

        return fieldKeyedErrors;
    }

    /**
     * 为字段添加错误信息
     */
    private addFieldError(
        errors: Record<string, string | string[]>,
        field: string,
        message: string,
        multipleErrors: 'first' | 'all'
    ): void {
        if (!errors[field]) {
            // 字段第一次出现错误
            if (multipleErrors === 'all') {
                errors[field] = [message];
            } else {
                errors[field] = message;
            }
        } else {
            // 字段已有错误
            if (multipleErrors === 'all') {
                if (Array.isArray(errors[field])) {
                    (errors[field] as string[]).push(message);
                } else {
                    // 从单个错误转换为数组
                    errors[field] = [errors[field] as string, message];
                }
            }
            // 如果 multipleErrors === 'first'，则忽略后续错误
        }
    }

    protected handleSuccessResponse<T>(
        xhr: XMLHttpRequest,
        options: HttpRequestOptions,
        deferred: HttpDeferred<T>
    ): void {
        const data = this.parseResponse(xhr, options);

        if (options.isNoNEncapsulationData) {
            deferred.resolve(data);
            return;
        }

        // ABP 特定逻辑：检查未授权请求
        if (data && data.unAuthorizedRequest === true) {
            const error = new HttpError("Unauthorized request", 401);
            this.handleErrorMessage(error);
            deferred.reject(error);
            return;
        }

        if (this.isSuccessResponse(data, xhr.status)) {
            const successData = this.extractSuccessData(data);
            deferred.resolve(successData);
        } else {
            const error = this.createError(data, xhr.status);
            this.handleErrorMessage(error);
            deferred.reject(error);
        }
    }
}
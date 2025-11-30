// 重新导出其他文件中定义的类，以便统一导入
import { HttpError } from './HttpError';
import { HttpDeferred } from './HttpDeferred';
/**
 * HTTP 配置选项
 */
export interface HttpOptions {
    baseUrl?: string;
    tokenStorageName?: string;
    authHeaderName?: string;
    authHeaderGenerator?: (token: string) => string;
    isNestResponse?: boolean;
    nestResponseSuccessCode?: number;
    responseDataName?: string;
    languageName?: string;
    xsrfCookieName?: string;
    xsrfHeaderName?: string;
    timeout?: number;
    errorMessageHandler?: (error: HttpError) => void;
    customErrorHandler?: CustomErrorHandler;
    createHeaders?: HeadersCreator;
    responseHandler?: ResponseHandler;
}

/**
 * HTTP 请求选项
 */
export interface HttpRequestOptions {
    method?: string;
    url?: string;
    headers?: Record<string, string>;
    data?: any;
    params?: any;
    responseType?: XMLHttpRequestResponseType;
    timeout?: number;
    withCredentials?: boolean;
    onUploadProgress?: (progressEvent: any) => void;
    onDownloadProgress?: (progressEvent: any) => void;
    retry?: number;
    retryInterval?: number;
    isNoNEncapsulationData?: boolean;
}

/**
 * 响应处理器接口
 */
export interface ResponseHandler {
    handleResponse<T>(
        xhr: XMLHttpRequest,
        options: HttpRequestOptions,
        deferred: HttpDeferred<T>
    ): void;

    parseResponse(xhr: XMLHttpRequest, options: HttpRequestOptions): any;

    isSuccessResponse(data: any, status: number, xhr: XMLHttpRequest): boolean;

    extractSuccessData(data: any, status: number, xhr: XMLHttpRequest): any;

    createError(data: any, status: number, xhr: XMLHttpRequest): HttpError;

    handleRetry<T>(deferred: HttpDeferred<T>, options: HttpRequestOptions): boolean;

    /**
     * 设置错误提取器
     */
    setErrorExtractor(extractor: ErrorExtractor): void;

    /**
     * 获取错误提取器
     */
    getErrorExtractor(): ErrorExtractor;    
}

/**
 * 头部创建器函数类型
 */
export type HeadersCreator = (options: HttpRequestOptions) => Record<string, string>;

/**
 * 自定义错误处理器函数类型
 */
export type CustomErrorHandler = (
    deferred: HttpDeferred<any>,
    options: HttpRequestOptions,
    request: XMLHttpRequest
) => void;

/**
 * 错误消息处理器函数类型
 */
export type ErrorMessageHandler = (error: HttpError) => void;

/**
 * 上传进度回调函数类型
 */
export type UploadProgressHandler = (progressEvent: any) => void;

/**
 * 下载进度回调函数类型
 */
export type DownloadProgressHandler = (progressEvent: any) => void;

/**
 * 基于 code 的成功判断配置
 */
export interface CodeBasedSuccessConfig {
    successCodes?: number[];
    dataField?: string;
    messageField?: string;
    codeField?: string;
}

/**
 * 混合成功判断配置
 */
export interface HybridSuccessConfig {
    code?: number[];
    success?: boolean;
    status?: string[];
    dataField?: string;
    messageField?: string;
    codeField?: string;
}

/**
 * 错误提取器接口
 */
export interface ErrorExtractor {
    /**
     * 判断是否能够处理该错误数据
     */
    canHandle(data: any, status: number): boolean;

    /**
     * 提取错误消息
     */
    extractMessage(data: any, status: number): string;

    /**
     * 提取错误代码
     */
    extractCode(data: any, status: number): number;

    /**
     * 提取验证错误
     */
    extractValidationErrors(data: any, status: number): any;

    /**
     * 提取原始错误数据
     */
    extractRawError(data: any, status: number): any;
}

/**
 * 错误提取器配置
 */
export interface ErrorExtractorConfig {
    /**
     * 消息字段的优先级列表
     */
    messageFields?: string[];

    /**
     * 代码字段的优先级列表
     */
    codeFields?: string[];

    /**
     * 验证错误字段的优先级列表
     */
    validationFields?: string[];

    /**
     * 错误对象字段名
     */
    errorObjectField?: string;

    /**
     * 默认错误消息
     */
    defaultMessage?: string;
}

export { HttpError } from './HttpError';
export { HttpDeferred } from './HttpDeferred';

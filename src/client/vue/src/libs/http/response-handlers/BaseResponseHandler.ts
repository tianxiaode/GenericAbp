// BaseResponseHandler.ts
import { ResponseHandler, HttpDeferred, HttpError, HttpRequestOptions, ErrorExtractor } from "../types";
import { CompositeErrorExtractor  } from '../error-extractors'

export abstract class BaseResponseHandler implements ResponseHandler {
    abstract isSuccessResponse(data: any, status: number, xhr: XMLHttpRequest): boolean;
    abstract extractSuccessData(data: any, status: number, xhr: XMLHttpRequest): any;
    protected errorExtractor: ErrorExtractor = new CompositeErrorExtractor();

    setErrorExtractor(extractor: ErrorExtractor): void {
        this.errorExtractor = extractor;
    }

    getErrorExtractor(): ErrorExtractor {
        return this.errorExtractor;
    }

    handleResponse<T>(
        xhr: XMLHttpRequest,
        options: HttpRequestOptions,
        deferred: HttpDeferred<T>
    ): void {
        if (xhr.status >= 200 && xhr.status < 300) {
            this.handleSuccessResponse(xhr, options, deferred);
        } else {
            this.handleErrorResponse(xhr, options, deferred);
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

        if (this.isSuccessResponse(data, xhr.status, xhr)) {
            const successData = this.extractSuccessData(data, xhr.status, xhr);
            deferred.resolve(successData);
        } else {
            const error = this.createError(data, xhr.status, xhr);
            this.handleErrorMessage(error);
            deferred.reject(error);
        }
    }

    protected handleErrorResponse<T>(
        xhr: XMLHttpRequest,
        options: HttpRequestOptions,
        deferred: HttpDeferred<T>
    ): void {
        if (this.handleRetry(deferred, options)) {
            return;
        }

        const data = this.parseResponse(xhr, options);
        const error = this.createError(data, xhr.status, xhr);
        this.handleErrorMessage(error);
        deferred.reject(error);
    }

    createError(data: any, status: number, xhr: XMLHttpRequest): HttpError {
        // 使用错误提取器来创建错误
        const message = this.errorExtractor.extractMessage(data, status);
        const code = this.errorExtractor.extractCode(data, status);
        const validationErrors = this.errorExtractor.extractValidationErrors(data, status);

        return new HttpError(message, code, validationErrors);
    }

    parseResponse(xhr: XMLHttpRequest, options: HttpRequestOptions): any {
        const responseType = options.responseType || "json";

        switch (responseType) {
            case "json":
                if (!xhr.responseText) return null;
                try {
                    return JSON.parse(xhr.responseText);
                } catch {
                    return xhr.responseText;
                }
            case "text":
                return xhr.responseText;
            case "blob":
                return { data: xhr.response };
            default:
                return xhr.response;
        }
    }

    handleRetry<T>(deferred: HttpDeferred<T>, options: HttpRequestOptions): boolean {
        const { retry, retryInterval = 1000 } = options;

        if (!retry || retry <= 0) return false;

        options.retry = retry - 1;
        setTimeout(() => {
            // 这里需要访问原始请求进行重试
            // 由于设计限制，这部分逻辑可能需要调整
        }, retryInterval);

        return true;
    }

    protected handleErrorMessage(error: HttpError): void {
        // 调用全局错误处理器
        if ((globalThis as any).BaseHttp?.errorMessageHandler) {
            (globalThis as any).BaseHttp.errorMessageHandler(error);
        }
    }
}
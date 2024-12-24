import { isEmpty, logger } from "../utils";
import { BaseHttp, HttpRequestOptions } from "./BaseHttp";
import { HttpDeferred } from "./HttpDeferred";
import { HttpError } from "./HttpError";

export class Http extends BaseHttp {
    $className = "Http";
    protected requests: Map<string, XMLHttpRequest> = new Map();

    public request<T = any>(options: HttpRequestOptions): Promise<T> {
        if (!BaseHttp.initialized) {
            throw new Error("Http is not initialized");
        }

        const deferred = new HttpDeferred();
        const method = options.method || "GET";
        const url = this.createURL(options.url!, options.params);

        options.headers = BaseHttp.createHeaders
            ? BaseHttp.createHeaders(options)
            : this.defaultCreateHeaders(options);

        if (isEmpty(options.responseType)) {
            options.responseType = "json";
        }

        const xhr = this.createXHR(method, url, options);
        xhr.withCredentials = options.withCredentials || false;
        xhr.timeout = options.timeout || BaseHttp.timeout;

        xhr.onload = () => {
            if (xhr.status >= 200 && xhr.status < 300) {
                this.handleSuccess(deferred, options, xhr);
            } else {
                this.handleError(deferred, options, xhr);
            }
        };

        xhr.onerror = () => {
            this.handleError(deferred, options, xhr);
        };

        xhr.onprogress = (event) => {
            if (event.lengthComputable && options.onUploadProgress) {
                const percentComplete = (event.loaded / event.total) * 100;
                options.onUploadProgress(percentComplete);
            }
        };

        deferred.cancel = () => {
            xhr.abort();
            this.requests.delete(deferred.requestId);
        };

        this.requests.set(deferred.requestId, xhr);
        // logger.debug(this, options);
        // xhr.send(options.data);
        this.sendData(xhr, options.data);
        return deferred.promise as Promise<T>;
    }

    public get<T = any>(
        url: string,
        params?: any,
        options?: HttpRequestOptions
    ): Promise<T> {
        return this.request<T>({ ...options, method: "GET", url, params });
    }

    public post<T = any>(
        url: string,
        data?: any,
        options?: HttpRequestOptions
    ): Promise<T> {
        return this.request<T>({ ...options, method: "POST", url, data });
    }

    public put<T = any>(
        url: string,
        data?: any,
        options?: HttpRequestOptions
    ): Promise<T> {
        return this.request<T>({ ...options, method: "PUT", url, data });
    }

    public delete<T = any>(
        url: string,
        options?: HttpRequestOptions
    ): Promise<T> {
        return this.request<T>({ ...options, method: "DELETE", url });
    }

    public patch<T = any>(
        url: string,
        data?: any,
        options?: HttpRequestOptions
    ): Promise<T> {
        return this.request<T>({ ...options, method: "PATCH", url, data });
    }

    public upload<T = any>(
        url: string,
        file: File,
        options?: HttpRequestOptions
    ): Promise<T> {
        const data = new FormData();
        data.append("file", file);
        return this.request<T>({ ...options, method: "POST", url, data });
    }

    public download<T>(url: string, options = {}): Promise<T> {
        return this.request<T>({
            method: "GET",
            url,
            responseType: "blob",
            ...options,
        });
    }

    public downloadFile(url: string, fileName: string, options = {}): void {
        this.download(url, options)
            .then((response: any) => {
                const url = window.URL.createObjectURL(
                    new Blob([response.data])
                );
                const link = document.createElement("a");
                link.href = url;
                link.setAttribute("download", fileName);
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            })
            .catch((error) => {
                logger.error("Download failed:", error);
            });
    }

    public createXHR(
        method: string,
        url: string,
        options: HttpRequestOptions
    ): XMLHttpRequest {
        const xhr = new XMLHttpRequest();
        xhr.open(method, url, true);
        if (options.headers) {
            Object.keys(options.headers).forEach((key) => {
                xhr.setRequestHeader(key, options.headers[key]);
            });
        }
        return xhr;
    }

    protected handleSuccess<T>(
        deferred: HttpDeferred<any>,
        options: HttpRequestOptions,
        xhr: XMLHttpRequest
    ) {
        let data = this.parseResponse(xhr, options);
        if(options.isNoNEncapsulationData){
            return deferred.resolve(data);
        }
        if (
            BaseHttp.responseDataName &&
            data &&
            typeof data === "object" &&
            data.hasOwnProperty(BaseHttp.responseDataName)
        ) {
            data = data[BaseHttp.responseDataName];
        }
        if (!BaseHttp.isNestResponse) {
            deferred.resolve(data as T);
            return;
        }
        if (data.code === BaseHttp.nestResponseSuccessCode) {
            deferred.resolve(data as T);
        } else {
            const err = new HttpError(data.msg || data.message || data.err_msg, data.code, data.data);
            if (BaseHttp.errorMessageHandler) {
                BaseHttp.errorMessageHandler(err);
            }
            deferred.reject(err);
            //this.handleError(deferred, options, xhr);
        }
    }

    protected handleError(
        deferred: HttpDeferred<any>,
        options: HttpRequestOptions,
        request: XMLHttpRequest
    ) {
        const { retry } = options;
        if (retry) {
            return this.retry(deferred, options);
        }

        if (BaseHttp.customErrorHandler) {
            return BaseHttp.customErrorHandler(deferred, options, request);
        }

        let err: HttpError;
        const code = request.status;
        if (code === 0) {
            err = new HttpError("Network Error", code);
        } else {
            let data = this.parseResponse(request, options);
            if (
                BaseHttp.responseDataName &&
                data &&
                typeof data === "object" &&
                data.hasOwnProperty(BaseHttp.responseDataName)
            ) {
                data = data[BaseHttp.responseDataName];
            }

            let errorText =
                data?.error?.message ||
                data?.msg ||
                data?.message ||
                data ||
                request.statusText ||
                request.status ||
                "unknownError";
            if(errorText === request.status){
                errorText = "Http." + errorText;
            }
            if (data?.error?.validationErrors) {
                errorText = data.error.details.replaceAll("\r\n", "<br>");
            }
            err = new HttpError(errorText, code, data?.error?.validationErrors);
        }

        if (BaseHttp.errorMessageHandler) {
            BaseHttp.errorMessageHandler(err);
        }
        deferred.reject(err);
    }

    protected parseResponse(
        xhr: XMLHttpRequest,
        options: HttpRequestOptions
    ): any {
        let responseType = options.responseType || "json";
        let response: any;
        switch (responseType) {
            case "json":
                if (xhr.responseText === "" || xhr.responseText === null) {
                    response = null;
                    break;
                }
                try {
                    response = JSON.parse(xhr.responseText) || xhr.responseText;                    
                } catch (error) {
                    response = xhr.responseText;
                }
                break;
            case "text":
                response = xhr.responseText;
                break;
            case "blob":
                response = { data: xhr.response };
                break;
            default:
                response = xhr.response;
                break;
        }
        return response;
    }

    protected retry<T>(deferred: HttpDeferred<T>, options: HttpRequestOptions) {
        const retry = options.retry || 0;
        const retryInterval = options.retryInterval || 1000;
        if (retry === 0) {
            deferred.reject("retryFailed");
            return;
        }
        options.retry = retry - 1;
        setTimeout(() => {
            const request = this.requests.get(deferred.requestId);
            request?.send(options.data);
        }, retryInterval);
        deferred.reject("retrying");
    }

    protected sendData(xhr: XMLHttpRequest, data: any) {
        if (typeof data === "object" && data !== null && !Array.isArray(data)) {
            // 如果是对象，转换为 JSON 字符串
            xhr.send(JSON.stringify(data));
        } else {
            // 否则直接发送
            xhr.send(data);
        }
    }

    destroy(): void {
        this.requests.forEach((xhr) => {
            xhr.abort();
        });
        this.requests.clear();
        super.destroy();
    }
}

export const http = new Http();

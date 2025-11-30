import { isEmpty, logger } from "../utils";
import { BaseHttp } from "./BaseHttp";
import { HttpDeferred } from "./HttpDeferred";
import { HttpRequestOptions } from "./types";

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
            BaseHttp.responseHandler?.handleResponse(xhr, options, deferred);
        };

        xhr.onerror = () => {
            BaseHttp.responseHandler?.handleResponse(xhr, options, deferred);
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
                xhr.setRequestHeader(key,  options.headers![key]);
            });
        }
        return xhr;
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

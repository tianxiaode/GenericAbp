import { LocalStorage } from "../LocalStorage";
import { logger } from "../utils";
import { HttpDeferred } from "./HttpDeferred";
import { HttpError } from "./HttpError";

export interface HttpOptions {
    baseUrl?: string;
    tokenStorageName?: string;
    authHeaderName?: string;
    authHeaderGenerator?: (token: string) => string; // 添加自定义授权头部生成函数
    isNestResponse?: boolean;
    responseDataName?: string;
    languageName?: string;
    xsrfCookieName?: string;
    xsrfHeaderName?: string;
    timeout?: number;
    errorMessageHandler?: (error: HttpError) => void;
    customErrorHandler?: (deferred: HttpDeferred<any>, options: HttpRequestOptions, request: XMLHttpRequest) => void ;
    createHeaders?: (options: HttpRequestOptions) => { [key: string]: string };
}

export interface HttpRequestOptions {
    method?: string;
    url?: string;
    headers?: any;
    data?: any;
    params?: any;
    responseType?: XMLHttpRequestResponseType;
    timeout?: number;
    withCredentials?: boolean;
    onUploadProgress?: (progressEvent: any) => void;
    onDownloadProgress?: (progressEvent: any) => void;
    retry?: number;
    retryInterval?: number;
}

export class BaseHttp {
    static baseUrl: string = '';
    static tokenStorageName: string = 'token';
    static tokenStorage: any = localStorage;
    static authHeaderName: string = 'Authorization';
    static authHeaderGenerator?: (token: string) => string; // 可选的自定义授权头部生成函数
    static isNestResponse: boolean = false;
    static responseDataName: string = 'items';
    static errorMessageHandler: ((error: HttpError) => void) | null = null;
    static customErrorHandler: ((deferred: HttpDeferred<any>, options: HttpRequestOptions, request: XMLHttpRequest) => void)  | null = null;
    static languageName: string = 'language';
    static xsrfValue: string = '';
    static xsrfHeaderName: string = 'X-XSRF-TOKEN';
    static initialized = false;
    static timeout: number = 30000;
    static createHeaders: ((options: HttpRequestOptions) => { [key: string]: string }) | null = null;

    static init(options: HttpOptions) {
        Object.assign(BaseHttp, options);
        BaseHttp.initialized = true;
    }

    public createURL(url: string, params?: any): string {
        if (!url.startsWith('http')) {
            url = (BaseHttp.baseUrl.endsWith('/') ? BaseHttp.baseUrl.slice(0, -1) : BaseHttp.baseUrl) + (url.startsWith('/') ? url : `/${url}`);
        }
        if (params) {
            const queryString = new URLSearchParams(params).toString();
            url += `?${queryString}`;
        }
        return url;
    }

    public defaultCreateHeaders(options?: HttpRequestOptions): { [key: string]: string } {
        options = options || {};
        const me = BaseHttp;        
        const token = LocalStorage.getToken();

        const headers: { [key: string]: string } = {
            ...options.headers,
            'Accept-Language': LocalStorage.getLanguage(),
            'Content-Type': options.data instanceof FormData ? 'multipart/form-data' : 'application/json',
        };

        if (token) {
            headers[me.authHeaderName || 'Authorization'] = me.authHeaderGenerator ? me.authHeaderGenerator(token) : `Bearer ${token}`;
        }

        if (me.xsrfValue) {
            headers[me.xsrfHeaderName] = me.xsrfValue;
        }

        return headers;
    }

    destroy() {
        BaseHttp.errorMessageHandler = () => {};
        BaseHttp.customErrorHandler = () => {};
    }
}

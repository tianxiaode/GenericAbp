import { account } from "../Account";
import { LocalStorage } from "../LocalStorage";
import { HttpOptions, HttpRequestOptions, HttpDeferred, HttpError, ResponseHandler } from "./types"

export class BaseHttp {
    static baseUrl: string = '';
    static tokenStorageName: string = 'token';
    static tokenStorage: any = localStorage;
    static authHeaderName: string = 'Authorization';
    static authHeaderGenerator?: (token: string) => string; // 可选的自定义授权头部生成函数
    static isNestResponse: boolean = false;
    static nestResponseSuccessCode: number = 0;
    static responseDataName: string = '';
    static errorMessageHandler: ((error: HttpError) => void) | null = null;
    static customErrorHandler: ((deferred: HttpDeferred<any>, options: HttpRequestOptions, request: XMLHttpRequest) => void)  | null = null;
    static languageName: string = 'language';
    static xsrfValue: string = '';
    static xsrfHeaderName: string = 'X-XSRF-TOKEN';
    static initialized = false;
    static timeout: number = 30000;
    static createHeaders: ((options: HttpRequestOptions) => { [key: string]: string }) | null = null;
    static responseHandler?: ResponseHandler;
    static init(options: HttpOptions) {
        if (options.responseHandler) {
            BaseHttp.responseHandler = options.responseHandler;
        }
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
        const token = account.getAccessToken();
        const headers: { [key: string]: string } = {
            'Accept-Language': LocalStorage.getLanguage(),
            'Content-Type': options.data instanceof FormData ? 'multipart/form-data' : 'application/json',
            '__tenant': LocalStorage.getTenant() || '',
            ...options.headers
        };


        if (token) {
            headers[me.authHeaderName || 'Authorization'] = me.authHeaderGenerator ? me.authHeaderGenerator(token) : `${account.getTokenType()} ${token}`;
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

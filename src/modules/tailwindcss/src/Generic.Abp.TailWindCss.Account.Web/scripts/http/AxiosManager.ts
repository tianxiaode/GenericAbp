import axios, { type AxiosInstance, type AxiosRequestConfig, type AxiosResponse, AxiosError } from 'axios';
import Deferred from './Deferred';
import { HttpStatusMessage } from './HttpStatusMessage';
import { HttpStatusText } from './HttpStatusText';

export interface AxiosManagerConfig extends AxiosRequestConfig {
    tokenStorageKey?: string; // token 存储的 key
    tokenStorage?: any; // token 存储的对象
    authHeaderName?: string; // 认证头名称
    //是否嵌套响应，也就是在AxiosResponse的基础上又嵌套一层code和msg
    isNestResponse?: boolean;
    responseDataKey?: string; // 响应数据在响应体中的key
    //显示错误信息的函数或对象，可以自定义显示信息的样式
    errorMessageHandler?: (error: string) => void;
    httpStatusMessages?: HttpStatusMessage;
    loginHandler?: any; // 登录处理函数
    languageKey?: string; // 语言key
}

export interface AxiosManagerRequestConfig extends AxiosRequestConfig {
    retry?: number; // 重试次数
    retryInterval?: number; // 重试间隔
    
}

export class AxiosManager {
    protected axiosInstance: AxiosInstance;
    protected requests: { [requestId: string]: { request: Promise<any> } };
    protected isNestResponse: boolean;
    protected responseDataKey: string;
    protected errorMessageHandler?: (error: string) => void;
    protected httpStatusMessages: HttpStatusMessage;
    protected loginHandler: any;
    protected tokenStorageKey: string | undefined;
    protected tokenStorage: any;
    protected authHeaderName: string | undefined;
    languageKey: string;

    constructor(config: AxiosManagerConfig) {
        const {
            tokenStorageKey,
            tokenStorage,
            authHeaderName,
            isNestResponse,
            responseDataKey,
            errorMessageHandler,
            httpStatusMessages,
            ...axiosConfig
        } = config;
        this.isNestResponse = isNestResponse || false;
        this.responseDataKey = responseDataKey || 'data';
        this.errorMessageHandler = errorMessageHandler;
        this.httpStatusMessages = httpStatusMessages || new HttpStatusMessage(HttpStatusText);
        this.loginHandler = config.loginHandler;
        this.tokenStorageKey = tokenStorageKey;
        this.tokenStorage = tokenStorage || localStorage;
        this.authHeaderName = authHeaderName;
        this.languageKey = config.languageKey || 'language';


        this.axiosInstance = axios.create(axiosConfig);
        this.requests = {};
    }

    public setLocale(locale: string) {
        this.httpStatusMessages.locale = locale;
    }

    public sendRequest<T>(config: AxiosManagerRequestConfig): Promise<T> {
        const me = this;
        const deferred = me.getDeferred<T>(); // 创建 Deferred 对象

        const controller = new AbortController();
        config.signal = controller.signal;

        this.setHeaders(config); // 设置请求头

        const request = me
            .axiosInstance(config)
            .then((response: AxiosResponse) => {
                me.handelSuccess(deferred, config, response);
                
            })
            .catch((error: any) => {
                me.handlerError(deferred, config, error); // 处理错误信息
            }).finally(() => {
                me.deleteRequest(deferred.requestId);
            });

        deferred.cancel = () => {
            controller.abort();
            me.deleteRequest(deferred.requestId);
        };

        me.requests[deferred.requestId] = {
            request,
        };

        return deferred.promise; // 返回 Deferred 对象
    }

    public get<T>(url: string, params = {}, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'get',
            url,
            params,
            ...config,
        });
    }

    public post<T>(url: string, data = {}, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'post',
            url,
            data,
            ...config,
        });
    }

    public put<T>(url: string, data = {}, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'put',
            url,
            data,
            ...config,
        });
    }

    public delete<T>(url: string, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'delete',
            url,
            ...config,
        });
    }

    public patch<T>(url: string, data = {}, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'patch',
            url,
            data,
            ...config,
        });
    }

    public upload<T>(url: string, data = {}, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'post',
            url,
            data,
            headers: {
                'Content-Type': 'multipart/form-data',
            },
            ...config,
        });
    }

    public download<T>(url: string, config = {}): Promise<T> {
        return this.sendRequest<T>({
            method: 'get',
            url,
            responseType: 'blob',
            ...config,
        });
    }

    public downloadFile(url: string, fileName: string, config = {}): void {
        this.download(url, config).then((response: any) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName);
            document.body.appendChild(link);
            link.click();
        });
    }

    protected getDeferred<T>() : Deferred<T> {
        return new Deferred();
    }

    protected retry<T>(deferred: Deferred<T>, config: AxiosManagerRequestConfig) {
        let retry = config.retry || 0;
        let retryInterval = config.retryInterval || 1000;
        if (retry === 0) {
            deferred.reject(this.httpStatusMessages.get('retryFailed'));
        }
        config.retry = retry - 1;
        setTimeout(() => {
            this.requests[deferred.requestId];
        }, retryInterval);
        deferred.reject(
            this.httpStatusMessages
                .get('retry')
                .replace('{retry}', retry.toString())
                .replace('{interval}', (retryInterval || 1000).toString()),
        );
    }

    protected handelSuccess<T>(deferred: Deferred<any>, config: AxiosManagerRequestConfig, response: AxiosResponse) {
        const { retry } = config;
        const me = this;
        if (!me.isNestResponse) {
            deferred.resolve(response.data as T); // 解决 Deferred 对象的 Promise
        }
        const data = response.data;
        if (data.code === 200) {
            deferred.resolve(data[me.responseDataKey] as T); // 解决 Deferred 对象的 Promise
        } else {
            if (retry) {
                return me.retry(deferred, config);
            }
            me.handlerErrorMessage(data.code, data.msg); // 处理错误信息
            deferred.reject(data); // 拒绝 Deferred 对象的 Promise
        }

    }

    protected handlerError(deferred: Deferred<any>, config: AxiosManagerRequestConfig, error: any) {
        const { retry } = config;
        const me = this;
        const { response } = error;
        let errorMessage = '';
        if (retry) {
            return me.retry(deferred, config);
        }
        if (error.response) {
            // The request was made and the server responded with a status code
            // that falls out of the range of 2xx
            const data = response.data;
            const dataError = data?.error;
            errorMessage = me.handlerErrorMessage( data?.code || response.status,
                data?.msg 
                || `${dataError?.message}${dataError?.details ? ':' + dataError?.details : ''}` 
                || response.statusText 
                || error.message ); // 处理错误信息
            deferred.reject(errorMessage); // 拒绝 Deferred 对象的 Promise
        } else if (error) {
            // The request was made but no response was received
            // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
            // http.ClientRequest in node.js
            console.log(error.request);
            errorMessage = me.handlerErrorMessage(500, '服务器错误'); // 处理错误信息
            deferred.reject(errorMessage); // 拒绝 Deferred 对象的 Promise
        } 
        deferred.reject(error); // 拒绝 Deferred 对象的 Promise

    }

    protected handlerErrorMessage(code: number, message: string) : string {
        const { errorMessageHandler, httpStatusMessages } = this;
        let messageText = message;

        if (code >= 400 && code <= 505) {
            if (code === 401 && this.loginHandler) {
                this.loginHandler();
            }
            let statusText = httpStatusMessages.get(code);
            if (!statusText) {
                statusText = httpStatusMessages.get('unknownError');
            }
            if (statusText) {
                messageText = `${statusText}${message ? ': ' + message : ''}`;
            }
        }

        if (messageText && errorMessageHandler) {
            errorMessageHandler(messageText);
        }
        return messageText;
    }


    protected deleteRequest(id: string) {
        delete this.requests[id];
    }

    protected setHeaders(config: AxiosManagerRequestConfig) {
        let me = this,
            token = '';
        if(me.tokenStorageKey && me.tokenStorage){
            token = me.tokenStorage.getItem(me.tokenStorageKey);
        }
        config.headers = config.headers || {};
        config.headers['Accept-Language'] = localStorage.getItem(me.languageKey) || navigator.language;
        if (token && me.authHeaderName) {
            config.headers[me.authHeaderName] = `${token}`;
        } else {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
    }
}

export const createAxiosManager = (config: AxiosManagerConfig) => new AxiosManager(config);

import { BaseResponseHandler } from "./BaseResponseHandler";
import { ConfigurableErrorExtractor } from "../error-extractors";

export interface UniversalResponseHandlerConfig {
    /**
     * 成功判断逻辑
     */
    successChecker?: (data: any, status: number, xhr: XMLHttpRequest) => boolean;

    /**
     * 数据提取逻辑
     */
    dataExtractor?: (data: any, status: number, xhr: XMLHttpRequest) => any;

    /**
     * 错误提取器配置
     */
    errorExtractorConfig?: any;
}

/**
 * 通用的响应处理器，可通过配置适应各种系统
 */
export class UniversalResponseHandler extends BaseResponseHandler {
    private successChecker: (data: any, status: number, xhr: XMLHttpRequest) => boolean;
    private dataExtractor: (data: any, status: number, xhr: XMLHttpRequest) => any;

    constructor(config: UniversalResponseHandlerConfig = {}) {
        super();

        this.successChecker = config.successChecker || this.defaultSuccessChecker;
        this.dataExtractor = config.dataExtractor || this.defaultDataExtractor;

        if (config.errorExtractorConfig) {
            this.setErrorExtractor(new ConfigurableErrorExtractor(config.errorExtractorConfig));
        }
    }

    isSuccessResponse(data: any, status: number, xhr: XMLHttpRequest): boolean {
        return this.successChecker(data, status, xhr);
    }

    extractSuccessData(data: any, status: number, xhr: XMLHttpRequest): any {
        return this.dataExtractor(data, status, xhr);
    }

    private defaultSuccessChecker(data: any, status: number): boolean {
        return status >= 200 && status < 300;
    }

    private defaultDataExtractor(data: any): any {
        return data;
    }
}
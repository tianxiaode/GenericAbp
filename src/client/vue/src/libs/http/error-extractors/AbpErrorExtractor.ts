import { ErrorExtractor } from "../types";

/**
 * ABP vNext 错误提取器
 */
export class AbpErrorExtractor implements ErrorExtractor {
    canHandle(data: any, status: number): boolean {
        return data &&
            typeof data === 'object' &&
            (data.error !== undefined || data.__abp === true);
    }

    extractMessage(data: any, status: number): string {
        const errorObj = this.extractRawError(data, status);

        if (errorObj && typeof errorObj === 'object') {
            // ABP 格式：优先使用 details，然后是 message
            if (errorObj.details) {
                return errorObj.details;
            }
            if (errorObj.message) {
                return errorObj.message;
            }
        }

        return 'An error occurred in ABP system';
    }

    extractCode(data: any, status: number): number {
        const errorObj = this.extractRawError(data, status);

        if (errorObj && typeof errorObj === 'object') {
            if (errorObj.code) {
                const code = Number(errorObj.code);
                if (!isNaN(code)) {
                    return code;
                }
            }
        }

        return status;
    }

    extractValidationErrors(data: any, status: number): any {
        const errorObj = this.extractRawError(data, status);

        if (errorObj && typeof errorObj === 'object' && errorObj.validationErrors) {
            return errorObj.validationErrors;
        }

        return null;
    }

    extractRawError(data: any, status: number): any {
        return data?.error || data;
    }
}
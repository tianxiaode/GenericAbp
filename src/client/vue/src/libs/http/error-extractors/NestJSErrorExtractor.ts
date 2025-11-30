import { ErrorExtractor } from "../types";
/**
 * NestJS 错误提取器
 */
export class NestJSErrorExtractor implements ErrorExtractor {
    canHandle(data: any, status: number): boolean {
        return data && typeof data === 'object';
    }

    extractMessage(data: any, status: number): string {
        if (data.message) {
            return data.message;
        }
        if (data.error) {
            return data.error;
        }
        return `HTTP Error ${status}`;
    }

    extractCode(data: any, status: number): number {
        if (data.statusCode) {
            return data.statusCode;
        }
        if (data.code) {
            return data.code;
        }
        return status;
    }

    extractValidationErrors(data: any, status: number): any {
        if (data.errors) {
            return data.errors;
        }
        if (data.message && Array.isArray(data.message)) {
            return data.message;
        }
        return null;
    }

    extractRawError(data: any, status: number): any {
        return data;
    }
}
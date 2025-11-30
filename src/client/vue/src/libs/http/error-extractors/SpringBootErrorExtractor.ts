import { ErrorExtractor } from "../types";

/**
 * Spring Boot 错误提取器
 */
export class SpringBootErrorExtractor implements ErrorExtractor {
    canHandle(data: any, status: number): boolean {
        return data && typeof data === 'object';
    }

    extractMessage(data: any, status: number): string {
        if (data.error) {
            return data.error;
        }
        if (data.message) {
            return data.message;
        }
        return `HTTP Error ${status}`;
    }

    extractCode(data: any, status: number): number {
        if (data.status) {
            return data.status;
        }
        return status;
    }

    extractValidationErrors(data: any, status: number): any {
        if (data.errors) {
            return data.errors;
        }
        return null;
    }

    extractRawError(data: any, status: number): any {
        return data;
    }
}

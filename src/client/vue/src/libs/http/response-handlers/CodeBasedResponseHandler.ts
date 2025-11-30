// CodeBasedResponseHandler.ts - 基于 code 字段的处理器
import { BaseResponseHandler } from "./BaseResponseHandler";
import { HttpError } from "../types"

export class CodeBasedResponseHandler extends BaseResponseHandler {
    private successCodes: number[];

    constructor(successCodes: number[] = [0, 200]) {
        super();
        this.successCodes = successCodes;
    }

    isSuccessResponse(data: any, status: number): boolean {
        const httpSuccess = status >= 200 && status < 300;
        const businessSuccess = data && this.successCodes.includes(data.code);
        return httpSuccess && businessSuccess;
    }

    extractSuccessData(data: any): any {
        return data?.data || data;
    }

    createError(data: any, status: number): HttpError {
        const message = data?.msg || data?.message || `Business Error: ${data?.code}`;
        return new HttpError(message, data?.code || status, data?.errors);
    }
}
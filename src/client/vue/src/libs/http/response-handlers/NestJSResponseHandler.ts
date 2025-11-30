// NestJSResponseHandler.ts - NestJS 风格处理器
import { BaseResponseHandler } from "./BaseResponseHandler";
import { HttpError } from "../types";

export class NestJSResponseHandler extends BaseResponseHandler {
    isSuccessResponse(data: any, status: number): boolean {
        return status >= 200 && status < 300;
    }

    extractSuccessData(data: any): any {
        // NestJS 通常返回 { data: any, message: string, status: number }
        return data?.data || data;
    }

    createError(data: any, status: number): HttpError {
        const message = data?.message ||
            data?.error?.message ||
            `HTTP Error ${status}`;

        const errorCode = data?.statusCode ||
            data?.error?.statusCode ||
            status;

        return new HttpError(message, errorCode, data?.error?.details);
    }
}
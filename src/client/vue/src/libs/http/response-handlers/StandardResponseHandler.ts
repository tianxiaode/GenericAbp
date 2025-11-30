// StandardResponseHandler.ts - 标准 REST API 处理器
import { HttpError } from "../types";
import { BaseResponseHandler } from "./BaseResponseHandler";

export class StandardResponseHandler extends BaseResponseHandler {
    isSuccessResponse(data: any, status: number): boolean {
        return status >= 200 && status < 300;
    }

    extractSuccessData(data: any): any {
        return data;
    }

    createError(data: any, status: number): HttpError {
        if (status === 0) {
            return new HttpError("Network Error", status);
        }

        const message = data?.message || data?.error || `HTTP Error ${status}`;
        return new HttpError(message, status, data?.errors);
    }
}
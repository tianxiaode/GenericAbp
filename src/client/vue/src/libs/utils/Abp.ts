// AbpValidationUtils.ts
/**
 * ABP 验证错误工具类
 * 提供静态方法处理 ABP 验证错误
 */
export class AbpValidationUtils {
    /**
     * 将 ABP 原始验证错误转换为字段键值对格式
     */
    static toFieldKeyedErrors(
        rawValidationErrors: any[],
        options: {
            multipleErrors?: 'first' | 'all';
            defaultFieldKey?: string;
        } = {}
    ): Record<string, string | string[]> {
        const {
            multipleErrors = 'all',
            defaultFieldKey = '_form'
        } = options;

        const fieldKeyedErrors: Record<string, string | string[]> = {};

        if (!rawValidationErrors || !Array.isArray(rawValidationErrors)) {
            return fieldKeyedErrors;
        }

        rawValidationErrors.forEach((error) => {
            const { message, members } = error;

            if (members && Array.isArray(members) && members.length > 0) {
                members.forEach((member: string) => {
                    AbpValidationUtils.addFieldError(fieldKeyedErrors, member, message, multipleErrors);
                });
            } else {
                AbpValidationUtils.addFieldError(fieldKeyedErrors, defaultFieldKey, message, multipleErrors);
            }
        });

        return fieldKeyedErrors;
    }

    /**
     * 获取指定字段的错误信息
     */
    static getFieldErrors(
        validationErrors: any,
        fieldName: string
    ): string | string[] | null {
        if (!validationErrors) {
            return null;
        }

        // 如果是字段键值对格式
        if (validationErrors[fieldName]) {
            return validationErrors[fieldName];
        }

        // 如果是原始格式
        if (validationErrors._raw && Array.isArray(validationErrors._raw)) {
            const fieldErrors = validationErrors._raw
                .filter((error: any) =>
                    error.members &&
                    Array.isArray(error.members) &&
                    error.members.includes(fieldName)
                )
                .map((error: any) => error.message);

            return fieldErrors.length > 0 ? fieldErrors : null;
        }

        return null;
    }

    /**
     * 检查是否有任何验证错误
     */
    static hasValidationErrors(validationErrors: any): boolean {
        if (!validationErrors) {
            return false;
        }

        if (validationErrors._raw && Array.isArray(validationErrors._raw)) {
            return validationErrors._raw.length > 0;
        }

        return Object.keys(validationErrors).length > 0;
    }

    /**
     * 获取所有验证错误的扁平数组
     */
    static getAllErrorMessages(validationErrors: any): string[] {
        if (!validationErrors) {
            return [];
        }

        const messages: string[] = [];

        // 处理原始格式
        if (validationErrors._raw && Array.isArray(validationErrors._raw)) {
            validationErrors._raw.forEach((error: any) => {
                if (error.message) {
                    messages.push(error.message);
                }
            });
        }

        // 处理字段键值对格式
        Object.keys(validationErrors).forEach(key => {
            if (key !== '_raw') {
                const error = validationErrors[key];
                if (Array.isArray(error)) {
                    messages.push(...error);
                } else if (typeof error === 'string') {
                    messages.push(error);
                }
            }
        });

        return messages;
    }

    /**
     * 为字段添加错误信息（内部方法）
     */
    private static addFieldError(
        errors: Record<string, string | string[]>,
        field: string,
        message: string,
        multipleErrors: 'first' | 'all'
    ): void {
        if (!errors[field]) {
            if (multipleErrors === 'all') {
                errors[field] = [message];
            } else {
                errors[field] = message;
            }
        } else {
            if (multipleErrors === 'all') {
                if (Array.isArray(errors[field])) {
                    (errors[field] as string[]).push(message);
                } else {
                    errors[field] = [errors[field] as string, message];
                }
            }
        }
    }
}
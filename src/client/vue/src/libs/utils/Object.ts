import { InvalidInputError } from "./Error";
import { logger } from "./Logger";

export function isObject(obj: any): boolean {
    return obj !== null && typeof obj === "object" && !Array.isArray(obj);
}

export function copyIfDefined(source: any, target: any): any {
    if (!isObject(source)) {
        return target;
    }

    for (const key in source) {
        if (source.hasOwnProperty(key)) {
            if (isObject(source[key])) {
                if (!target.hasOwnProperty(key)) {
                    target[key] = {};
                }
                copyIfDefined(source[key], target[key]);
            } else if (source[key] !== undefined) {
                target[key] = source[key];
            }
        }
    }
    return target;
}

export function clone(obj: any): any {
    if (!isObject(obj)) {
        return obj;
    }

    const result: any = Array.isArray(obj) ? [] : {};
    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            result[key] = clone(obj[key]);
        }
    }
    return result;
}

export function deepMerge(
    target: Record<string, any>,
    source: Record<string, any>
): Record<string, any> {
    if (!isObject(target) || !isObject(source)) {
        throw new Error("Both target and source must be objects");
    }

    const output = { ...target };
    for (const key in source) {
        if (source.hasOwnProperty(key)) {
            if (isObject(source[key]) && isObject(target[key])) {
                output[key] = deepMerge(target[key], source[key]);
            } else {
                output[key] = source[key];
            }
        }
    }
    return output;
}

export function destroyMembers(obj: any, members: string[]): void {
    if (!isObject(obj)) {
        throw new Error("The first argument must be an object");
    }

    members.forEach((member) => {
        if (obj.hasOwnProperty(member)) {
            let memberValue = obj[member];
            if (
                isObject(memberValue) &&
                typeof memberValue.destroy === "function"
            ) {
                memberValue.destroy();
            }
            if (isObject(memberValue)) {
                destroyMembers(memberValue, Object.keys(memberValue));
            }
            delete obj[member];
        }
    });
}

/**
 * 递归替换目标对象的成员
 * 
 * 该函数用于将源对象中的所有成员递归地替换到目标对象中如果遇到相同名称的成员，
 * 且两者都是对象，则会递归地进行替换；否则，会直接用源对象的成员覆盖目标对象的成员
 * 
 * @param source 源对象，其成员将被复制到目标对象中
 * @param target 目标对象，其成员将被源对象的成员替换
 * @param maxDepth 最大递归深度，默认为10，以防止无限递归
 * @param currentDepth 当前递归深度，初始调用时通常不需要设置
 * @throws 如果超过最大递归深度或源和目标不是对象，将抛出错误
 */
export function replaceMembers(
    source: Record<string, any>,
    target: Record<string, any>,
    maxDepth: number = 10,
    currentDepth: number = 0
): void {
    // 检查是否超过最大递归深度
    if (currentDepth > maxDepth) {
        throw new InvalidInputError("Maximum recursion depth exceeded");
    }

    // 检查参数类型
    if (!isObject(source) || !isObject(target)) {
        throw new InvalidInputError("Both source and target must be objects");
    }

    try {
        for (const key in source) {
            if (source.hasOwnProperty(key)) {
                // 调试模式下输出日志
                logger.debug('[replaceMembers]', key);

                // 如果源和目标的当前成员都是对象，则递归替换成员
                if (isObject(source[key]) && isObject(target[key])) {
                    replaceMembers(source[key], target[key], maxDepth, currentDepth + 1);
                } else {
                    // 直接用源对象的成员覆盖目标对象的成员
                    target[key] = source[key];
                }
            }
        }
    } catch (error) {
        // 捕获并处理异常
        console.error("An error occurred during member replacement:", error);
        throw error;
    }
}


export function getNestedValue(obj: any, path: string): any {
    if (!isObject(obj)) {
        throw new Error("The first argument must be an object");
    }

    const keys = path.split(".");
    let result = obj;
    for (const key of keys) {
        if (result.hasOwnProperty(key)) {
            result = result[key];
        } else {
            return undefined;
        }
    }
    return result;
}

export function setNestedValue(obj: any, path: string, value: any): void {
    if (!isObject(obj)) {
        throw new Error("The first argument must be an object");
    }

    const keys = path.split(".");
    let result = obj;
    for (let i = 0; i < keys.length - 1; i++) {
        const key = keys[i];
        if (!result.hasOwnProperty(key)) {
            result[key] = {};
        }
        result = result[key];
    }
    result[keys[keys.length - 1]] = value;
}

export function isUndefine(obj: any): boolean {
    return typeof obj === "undefined" || obj === null;
}

export function isFunction(obj: any): boolean {
    return typeof obj === "function";
}

export function objectEach(
    obj: any,
    callback: (value: any, key: string) => boolean,
    scope?: any
): void {
    if (!isObject(obj)) {
        throw new Error("The first argument must be an object");
    }

    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            if (callback.call(scope, obj[key], key) === false) {
                return;
            }
        }
    }
}

export function isEqual(obj1: any, obj2: any): boolean {
    if (obj1 === obj2) {
        return true;
    }

    if (typeof obj1 !== typeof obj2) {
        return false;
    }

    if (Array.isArray(obj1)) {
        if (obj1.length !== obj2.length) {
            return false;
        }
        for (let i = 0; i < obj1.length; i++) {
            if (!isEqual(obj1[i], obj2[i])) {
                return false;
            }
        }
        return true;
    }

    if (isObject(obj1)) {
        if (Object.keys(obj1).length !== Object.keys(obj2).length) {
            return false;
        }
        for (const key in obj1) {
            if (obj1.hasOwnProperty(key)) {
                if (!isEqual(obj1[key], obj2[key])) {
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    return false;
}

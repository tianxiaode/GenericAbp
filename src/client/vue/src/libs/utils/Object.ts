export function isObject(obj: any): boolean {
    return obj !== null && typeof obj === "object" && !Array.isArray(obj);
}

export function copyIfDefined(source: any, target: any): any {
    if (!isObject(source)) {
        return target;
    }

    for (const key in source) {
        console.log(key);
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

import { InvalidInputError } from "./Error";

/**
 * 移除数组中的重复元素
 * @param arr 输入数组
 * @returns 去重后的数组
 */
export function removeDuplicates(arr: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new InvalidInputError("Input must be an array");
    }

    // 检查数组中的元素类型
    for (const item of arr) {
        if (
            typeof item !== "string" &&
            typeof item !== "number" &&
            typeof item !== "boolean" &&
            item !== null &&
            item !== undefined
        ) {
            throw new Error(
                "Array elements must be of type string, number, boolean, null, or undefined"
            );
        }
    }

    try {
        const seen = new Map();
        const result = [];

        for (const item of arr) {
            if (!seen.has(item)) {
                seen.set(item, true);
                result.push(item);
            }
        }

        return result;
    } catch (error) {
        console.error("An error occurred while removing duplicates:", error);
        throw error;
    }
}

/**
 * 合并多个数组中的对象，并根据指定字段去重
 *
 * 该函数接受一个数组的数组，将所有数组中的对象合并到一个新数组中。
 * 如果对象在不同数组中具有相同的指定字段值，则后面的数组中的对象会覆盖前面的数组中的对象。
 *
 * @param arrays 一个包含多个数组的数组，每个数组中的对象将被合并
 * @param field 用于去重的字段名
 * @returns 合并后的数组，其中对象根据指定字段去重
 * @throws 如果输入不是数组或对象中不存在指定字段，将抛出错误
 */
export function mergeArray<T extends Record<K, unknown>, K extends keyof T>(
    arrays: T[][],
    field: K
): T[] {
    // 检查输入是否为数组，如果不是则抛出错误
    if (!Array.isArray(arrays)) {
        throw new InvalidInputError("Input must be an array of arrays");
    }

    // 创建一个Map用于存储不重复的对象，以指定字段的值为键
    const sourceMap = new Map<T[K], T>();

    // 遍历每个数组
    arrays.forEach((array) => {
        // 检查当前数组是否为数组，如果不是则抛出错误
        if (!Array.isArray(array)) {
            throw new InvalidInputError("Each element in the input must be an array");
        }

        // 遍历当前数组中的每个对象
        array.forEach((item) => {
            // 检查对象中是否包含指定字段，如果包含则存入Map，否则抛出错误
            if (field in item) {
                sourceMap.set(item[field], item);
            } else {
                throw new InvalidInputError(
                    `Field ${String(field)} does not exist in one of the objects`
                );
            }
        });
    });

    // 将Map中的值转换为数组并返回，这些值是根据指定字段去重后的对象
    return Array.from(sourceMap.values());
}

/**
 * 移除数组中指定的值
 * @param arr 输入数组
 * @param valuesToRemove 需要移除的值数组
 * @returns 过滤后的数组
 */
export function removeValues<T>(arr: T[], valuesToRemove: T[]): T[] {
    if (!Array.isArray(arr)) {
        throw new InvalidInputError("Input must be an array");
    }

    // 处理空数组的情况
    if (arr.length === 0) {
        return arr;
    }

    // 使用 Set 来提高查找效率
    const valuesSet = new Set(valuesToRemove);

    // 使用 for 循环进行过滤，以提高性能
    const result: T[] = [];
    for (let i = 0; i < arr.length; i++) {
        if (!valuesSet.has(arr[i])) {
            result.push(arr[i]);
        }
    }

    return result;
}



/**
 * Splits an array into two arrays based on a given condition.
 * 
 * This function takes an array and a condition function as parameters. It iterates through the array,
 * and uses the condition function to determine whether each element should be placed in the first or second
 * resulting array. If the condition function returns true for an element, that element is placed in the first array;
 * otherwise, it is placed in the second array.
 * 
 * @param arr The array to be split.
 * @param condition The condition function, which takes an element and its index as parameters, and returns a boolean value.
 * @returns A tuple containing two arrays, where the first array includes elements for which the condition is true,
 * and the second array includes elements for which the condition is false.
 * @throws {InvalidInputError} Throws an error if the input is not an array or the condition is not a function.
 */
export function splitArray<T>(
    arr: T[],
    condition: (item: T, index: number) => boolean
): [T[], T[]] {
    // Check if the input is an array, throw an error if not
    if (!Array.isArray(arr)) {
        throw new InvalidInputError("Input must be an array");
    }

    // Check if the condition is a function, throw an error if not
    if (typeof condition !== 'function') {
        throw new InvalidInputError("Condition must be a function");
    }

    // Initialize two arrays for holding elements that meet and do not meet the condition, respectively
    const arr1: T[] = [];
    const arr2: T[] = [];

    // Iterate through the input array, distributing elements to the two arrays based on the condition
    for (let i = 0; i < arr.length; i++) {
        try {
            // Use the condition function to determine which array the current element should be placed in
            if (condition(arr[i], i)) {
                arr1.push(arr[i]);
            } else {
                arr2.push(arr[i]);
            }
        } catch (error) {
            // If an error occurs during the execution of the condition function, log an error message
            console.error(`Error in condition function at index ${i}:`, error);
        }
    }

    // Return the two arrays as a tuple
    return [arr1, arr2];
}

/**
 * 根据指定字段和顺序对数组进行排序
 * 
 * @param data 待排序的数组
 * @param field 排序依据的字段名
 * @param order 排序顺序，可以是'asc'（升序）或'desc'（降序）
 * @throws 如果字段名不是合法的属性名，则抛出 InvalidInputError 异常
 */
export function sortBy(data: any[], field: string, order: string) {
    // 验证 field 是否是合法的属性名
    if (typeof field !== 'string' || !/^[a-zA-Z_$][0-9a-zA-Z_$]*$/.test(field)) {
        throw new InvalidInputError('Invalid field name');
    }

    // 将排序顺序转换为小写，以支持大小写不敏感的比较
    order = order.toLowerCase();
    // 判断排序顺序是升序还是降序
    const isAscending = order.startsWith("asc") || order.startsWith("ascending");

    // 使用 Array.prototype.sort() 方法进行排序
    data.sort((a: any, b: any) => {
        // 获取两个元素在指定字段的值，如果值不存在，则默认为空字符串
        let value1 = a[field] ?? '';
        let value2 = b[field] ?? '';

        // 如果两个值都是数字类型，则直接进行数值比较
        if (typeof value1 === "number" && typeof value2 === "number") {
            return isAscending ? value1 - value2 : value2 - value1;
        }

        // 如果两个值都是字符串类型，则使用 localeCompare 方法进行字符串比较
        if (typeof value1 === "string" && typeof value2 === "string") {
            return isAscending ? value1.localeCompare(value2) : value2.localeCompare(value1);
        }

        // 如果类型不一致，或都不是数字/字符串类型，则将值转换为字符串后再进行比较
        return isAscending ? String(value1).localeCompare(String(value2)) : String(value2).localeCompare(String(value1));
    });
}

/**
 * Finds the intersection of two arrays based on a specified field.
 * @param arr1 The first array.
 * @param arr2 The second array.
 * @param field The field name used for comparison.
 * @returns Returns an array containing the intersection of the two input arrays based on the specified field.
 * @throws Throws an error if the input is not an array or if the field is not a non-empty string.
 */
export function intersectionBy(arr1: any[], arr2: any[], field: string): any[] {
    // Check if the inputs are arrays
    if (!Array.isArray(arr1) || !Array.isArray(arr2)) {
        throw new InvalidInputError("Input must be an array");
    }
    // Check if the field is a non-empty string
    if (typeof field !== 'string' || field.trim() === '') {
        throw new InvalidInputError("Field must be a non-empty string");
    }

    // Create sets of the specified field values from both arrays
    const set1 = new Set(arr1.map((item) => item[field]));
    const set2 = new Set(arr2.map((item) => item[field]));

    // Find the intersection of the two sets
    const intersection = new Set([...set1].filter(x => set2.has(x)));

    // Filter the original array to include only items with field values in the intersection
    return arr1.filter((item) => intersection.has(item[field]));
}

/**
 * 在数组中查找具有指定字段和值的项
 * @param arr 数组，包含要搜索的项
 * @param field 字段名，用于匹配项
 * @param value 要匹配的值，可以是任何类型
 * @returns 如果找到匹配的项，则返回该项，否则返回undefined
 */
export function findItemInArray<T>(arr: T[], field: keyof T, value: any): T | undefined {
    // 如果值是对象，则获取field的值再进行比较
    if (typeof value === 'object' && value !== null && field in value) {
        value = value[field];
    }

    // 处理空数组的情况
    if (arr.length === 0) {
        return undefined;
    }

    // 查找匹配的项
    return arr.find((item) => item[field] === value);
}
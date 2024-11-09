export function removeDuplicates(arr: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    return [...new Set(arr)];
}

export function removeUndefined(arr: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    return arr.filter((item) => item !== undefined);
}

export function removeNull(arr: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    return arr.filter((item) => item !== null);
}

export function removeEmptyString(arr: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    return arr.filter((item) => item !== "");
}

// 增加一个通用的函数来处理多种情况
export function removeItems(arr: any[], ...itemsToRemove: any[]): any[] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    return arr.filter((item) => !itemsToRemove.includes(item));
}

//根据指定条件，将数组拆分为两个数组
export function splitArray(
    arr: any[],
    condition: (item: any, index: number) => boolean
): [any[], any[]] {
    if (!Array.isArray(arr)) {
        throw new Error("Input must be an array");
    }
    const arr1 = [];
    const arr2 = [];
    for (let i = 0; i < arr.length; i++) {
        if (condition(arr[i], i)) {
            arr1.push(arr[i]);
        } else {
            arr2.push(arr[i]);
        }
    }
    return [arr1, arr2];
}

export function sortBy(data: any[], field: string, order: string) {
    order = order.toLowerCase().startsWith("asc") ? "asc" : "desc";
    data.sort((a: any, b: any) => {
        let value1 = a[field];
        let value2 = b[field];
        if (typeof value1 === "number") {
            return order === "asc" ? value1 - value2 : value2 - value1;
        }
        return order === "asc"
            ? (value1 || "").localeCompare(value2 || "")
            : (value2 || "").localeCompare(value1 || "");
    });
}


//根据指定字段，计算两个数组的交集
export function intersectionBy(arr1: any[], arr2: any[], field: string): any[] {
    if (!Array.isArray(arr1) ||!Array.isArray(arr2)) {
        throw new Error("Input must be an array");
    }
    //const set1 = new Set(arr1.map((item) => item[field]));
    const set2 = new Set(arr2.map((item) => item[field]));
    return [...arr1].filter((item) => set2.has(item[field]));
}
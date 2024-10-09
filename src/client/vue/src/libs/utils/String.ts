export type CssUnitType = string | number | null | undefined;

export function isEmpty(str: any): boolean {
    if (str === "\u00A0") return true;
    return typeof str === "string" ? str.trim().length === 0 : str == null;
}

export function trim(str: string): string {
    return str.trim();
}

export function isString(str: any): boolean {
    return typeof str === "string" || str instanceof String;
}

export function capitalize(str: string): string {
    return isEmpty(str) ? "" : str.charAt(0).toUpperCase() + str.slice(1);
}

export function uncapitalize(str: string): string {
    return isEmpty(str) ? "" : str.charAt(0).toLowerCase() + str.slice(1);
}

export function normalizeCssUnit(value: CssUnitType): string {
    if (typeof value === "number") return value + "px";
    if (isEmpty(value)) return "";
    return value as string;
}


// String.ts
export function cssUnitTypeToNumber(value: CssUnitType): number {
    if (typeof value === "number") return value;
    if (isEmpty(value)) return 0;
    const match = value!.match(/^(\d+(\.\d+)?)(px|em|rem|%|pt|pc|ex|ch|vw|vh|vmin|vmax)?$/);
    if (match) {
        const num = parseFloat(match[1]);
        return num;
    }
    return 0;
}


export function replace(str: string, options: { [key: string]: string }): string {
    if (isEmpty(str)) return "";
    for (const key in options) {
        if (options.hasOwnProperty(key)) {
            const value = options[key];
            str = replaceAll(str,`{${key}}`, value);
        }
    }
    return str;
}

export function replaceAll(str: string, search: string, replace: string): string {
    return str.replace(new RegExp(search, "g"), replace);
}

export function highlightText(text: string, search: string): string {
    if (isEmpty(text) || isEmpty(search)) return text;
    return replaceAll(text,search, `<span class="highlight">${search}</span>`);
}

let idSeed = 0;

export function getId(prefix: string = "id"): string {
    if (typeof prefix !== "string") {
        throw new Error("Prefix must be a string");
    }
    return `${prefix}-${idSeed++}`;
}

export const emptyString = "\u00A0";

export function camelCase(str: string): string {
    // 将字符串转换为小写并按空格、连字符、下划线分割
    if(isEmpty(str)) return "";
    if(!str.includes('-') && !str.includes('_') && !str.includes(' ')) return uncapitalize(str);
    const words = str.toLowerCase().split(/[-_\s]+/);
    let result = '';

    for (let i = 0; i < words.length; i++) {
        let word = words[i].toLocaleLowerCase();
        if (i === 0) {
            result += word; // 第一个单词保持小写
        } else {
            result += capitalize(word);
        }
    }

    return result;
}

//将驼峰式命名的字符串转换为连接符命名的全小写字符串
export function camelCaseToDash(str: string): string {
    if(isEmpty(str)) return "";
    let result = str.replace(/([A-Z])/g, '-$1').toLowerCase();
    if(result.startsWith('-')) result = result.slice(1);
    return result;
}

// 用于将字符串按指定分隔符分割，并支持分隔符的转义
// 例如：splitWithEscaping("a,b,,c,d", ",") => ["a", "b,c", "d"]
export function splitWithEscaping(str: string, separator: string): string[] {
    // 此函数用于将字符串按指定分隔符分割，并支持分隔符的转义
    if (isEmpty(str)) return [];
    if (separator === '') return [str];
    const result = [];
    let start = 0;
    let index = 0;
    const escapedSeparator = separator + separator;
 
    while (index < str.length) {
        if (str.startsWith(escapedSeparator, index)) {
            // 将两个重复的分隔符替换为一个分隔符
            str = str.slice(0, index) + separator + str.slice(index + escapedSeparator.length);
            index += separator.length;
        } else if (str.startsWith(separator, index)) {
            result.push(str.slice(start, index));
            index += separator.length;
            start = index;
        } else {
            index++;
        }
    }
 
    result.push(str.slice(start));
    return result;
}

// 将文本转换为html格式，每个换行使用P标签包裹
export function textToHtml(text: string): string {
    if (isEmpty(text)) return "";
    return "<p>" + text.replace(/\n/g, "</p><p>") + "</p>";
}
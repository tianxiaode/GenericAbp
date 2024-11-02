import { isEmpty } from "./String";

/**
 * 0 - 用对应的数字（如果存在）替换零；否则，将在结果字符串中显示零。
 * # - 用对应的数字（如果存在）替换“#”符号；否则，不会在结果字符串中显示任何数字
 * . - 确定小数点分隔符在结果字符串中的位置。
 * , - 用作组分隔符和数字比例换算说明符。 作为组分隔符时，它在各个组之间插入本地化的组分隔符字符。 作为数字比例换算说明符，对于每个指定的逗号，它将数字除以 1000
 * % - 将数字乘以 100，并在结果字符串中插入本地化的百分比符号
 * @param number 
 * @param format 
 * @returns 
 */
export function formatNumber(number: number | string | null | undefined, format: string): string {
    if (isEmpty(number)) return "";
    let num = Number(number);
    if (isNaN(num)) return "";

    let isPresent = format.includes('%');

    if(isPresent){
        num *= 100;
    }

    // 拆分格式字符串为整数部分和小数部分
    const [integerFormat, decimalFormat] = format.split('.');
    const [integerPart, decimalPart] = num.toString().split('.');

    // 处理整数部分
    let formattedIntegerPart = formatIntegerPart(integerPart, integerFormat.replace(/,/g, ''));

    // 处理小数部分
    let formattedDecimalPart = decimalPart ? formatDecimalPart(decimalPart, decimalFormat.replace('%', '')) : '';

    // 处理分隔符
    if (integerFormat.includes(',')) {
        formattedIntegerPart = formatSeparator(formattedIntegerPart);
    }

    let result = formattedDecimalPart ? `${formattedIntegerPart}.${formattedDecimalPart}` : formattedIntegerPart;

    // 处理百分号
    if (isPresent) {
        result += '%' ;
    }

    // 合并字符串并返回
    return result;
}

function formatIntegerPart(part: string, format: string): string {
    let result = '';
    let partIndex = 0;
    let formatIndex = 0;

    // 如果数字比格式短，直接返回数字
    if (part.length <= format.length) {
        return part;
    }

    // 如果数字比格式长，处理多余的数字
    while (partIndex < part.length || formatIndex < format.length) {
        if (formatIndex < format.length && (format[formatIndex] === '0' || format[formatIndex] === '#')) {
            if (partIndex < part.length) {
                result += part[partIndex];
                partIndex++;
            } else if (format[formatIndex] === '0') {
                result += '0';
            }
            formatIndex++;
        } else if (formatIndex < format.length) {
            result += format[formatIndex];
            formatIndex++;
        } else {
            result += part[partIndex];
            partIndex++;
        }
    }

    return result;
}
function formatDecimalPart(part: string, format: string): string {
    let result = '';
    let partIndex = 0;
    let formatIndex = 0;

    while (partIndex < part.length && formatIndex < format.length) {
        if (format[formatIndex] === '0' || format[formatIndex] === '#') {
            result += part[partIndex];
            partIndex++;
        }
        formatIndex++;
    }

    // 四舍五入处理
    if (partIndex < part.length) {
        let nextDigit = parseInt(part[partIndex], 10);
        if (nextDigit >= 5) {
            let last = result[result.length - 1];
            result = result.slice(0, -1) + (parseInt(last, 10) + 1).toString();
        }
    }

    return result;
}

function formatSeparator(part: string): string {
    const parts = part.split('').reverse();
    let result = '';
    for (let i = 0; i < parts.length; i++) {
        if (i > 0 && i % 3 === 0) {
            result = ',' + result;
        }
        result = parts[i] + result;
    }
    return result;
}


export function formatCurrency(number: number | string | null | undefined, currency: string, format: string): string {
    return currency + formatNumber(number, format);
}

export function percent(number: number | string | null | undefined): string {
    let num = Number(number);
    if (isNaN(num)) return "";
    if (num === 0) return "0%";
    num *= 100;
    return num + "%";
}

export function isNumber(value: any): boolean {
    return typeof value === 'number' && !isNaN(value);
}

export function isInteger(value: any): boolean {
    return isNumber(value) && value % 1 === 0;
}

export function isFloat(value: any): boolean {
    return isNumber(value) && value % 1 !== 0;
}

export function isPositive(value: any): boolean {
    return isNumber(value) && value > 0;
}

export function isNegative(value: any): boolean {
    return isNumber(value) && value < 0;
}

export function isZero(value: any): boolean {
    return isNumber(value) && value === 0;
}

export function isNotZero(value: any): boolean {
    return isNumber(value) && value !== 0;
}

export function isPositiveOrZero(value: any): boolean {
    return isNumber(value) && value >= 0;
}

export function isNegativeOrZero(value: any): boolean {
    return isNumber(value) && value <= 0;
}

export function isInRange(value: any, min: number, max: number): boolean {
    return isNumber(value) && value >= min && value <= max;
}

export function isNotInRange(value: any, min: number, max: number): boolean {
    return isNumber(value) && (value < min || value > max);
}

export function isOdd(value: any): boolean {
    return isInteger(value) && value % 2 !== 0;
}

export function isEven(value: any): boolean {
    return isInteger(value) && value % 2 === 0;
}

export function isMultipleOf(value: any, multiple: number): boolean {
    return isNumber(value) && value % multiple === 0;
}

export function isNotMultipleOf(value: any, multiple: number): boolean {
    return isNumber(value) && value % multiple !== 0;
}

export function isDivisibleBy(value: any, divisor: number): boolean {
    return isNumber(value) && value % divisor === 0;
}

export function isNotDivisibleBy(value: any, divisor: number): boolean {
    return isNumber(value) && value % divisor !== 0;
}

export function toFixed(value: any, precision: number): string {
    if (isNumber(value)) {
        return value.toFixed(precision);
    } else {
        return value;
    }
}

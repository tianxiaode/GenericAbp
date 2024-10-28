import { isEmpty } from "./String";

export function formatDate(date: Date | string, format: string): string {
    if (isEmpty(date)) return "";
    const d = new Date(date);
    if (isNaN(d.getTime())) throw new Error("Invalid date");

    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    const hours = d.getHours();
    const minutes = d.getMinutes().toString().padStart(2, '0');
    const seconds = d.getSeconds().toString().padStart(2, '0');
    const isTwelveHourFormat = format.includes("hh");

    let formattedDate = format
        .replace("yyyy", year.toString())
        .replace("yy", year.toString().slice(2))
        .replace("MM", month)
        .replace("M", month.replace(/^0+/, ''))
        .replace("dd", day)
        .replace("d", day.replace(/^0+/, ''))
        .replace("HH", hours.toString().padStart(2, '0'))
        .replace("H", hours.toString())
        .replace("hh", (hours % 12 || 12).toString().padStart(2, '0'))
        .replace("h", (hours % 12 || 12).toString())
        .replace("mm", minutes)
        .replace("m", minutes.replace(/^0+/, ''))
        .replace("ss", seconds)
        .replace("s", seconds.replace(/^0+/, ''));

    if (isTwelveHourFormat) {
        formattedDate += hours >= 12 ? " PM" : " AM";
    }

    return formattedDate;
}

export function isDate(date: any): boolean {
    if (typeof date === 'string') {
        return !isNaN(Date.parse(date));
    }
    return Object.prototype.toString.call(date) === "[object Date]" && !isNaN(date.getTime());
}

export function isToday(date: Date | string): boolean {
    const today = new Date();
    const d = new Date(date);
    return d.toDateString() === today.toDateString();
}

export function isYesterday(date: Date | string): boolean {
    const yesterday = new Date();
    yesterday.setDate(yesterday.getDate() - 1);
    const d = new Date(date);
    return d.toDateString() === yesterday.toDateString();
}

export function isThisWeek(date: Date | string): boolean {
    const today = new Date();
    const d = new Date(date);
    const diff = today.getDate() - d.getDate() + (today.getDay() - d.getDay());
    return diff >= 0 && diff < 7 && today.getMonth() === d.getMonth() && today.getFullYear() === d.getFullYear();
}

export function isThisMonth(date: Date | string): boolean {
    const today = new Date();
    const d = new Date(date);
    return d.getMonth() === today.getMonth() && d.getFullYear() === today.getFullYear();
}

export function isThisYear(date: Date | string): boolean {
    const today = new Date();
    const d = new Date(date);
    return d.getFullYear() === today.getFullYear();
}

export function isBefore(date: Date | string, dateToCompare: Date | string): boolean {
    const d = new Date(date);
    const dToCompare = new Date(dateToCompare);
    return d < dToCompare;
}

export function isAfter(date: Date | string, dateToCompare: Date | string): boolean {
    const d = new Date(date);
    const dToCompare = new Date(dateToCompare);
    return d > dToCompare;
}

export function isBetween(date: Date | string, dateFrom: Date | string, dateTo: Date | string): boolean {
    const d = new Date(date);
    const dFrom = new Date(dateFrom);
    const dTo = new Date(dateTo);
    return d >= dFrom && d <= dTo;
}

export function addDays(date: Date | string, days: number): Date {
    const d = new Date(date);
    d.setDate(d.getDate() + days);
    return d;
}

export function addMonths(date: Date | string, months: number): Date {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = d.getMonth() + months;
    const day = Math.min(d.getDate(), getDaysInMonth(year, month));
    d.setFullYear(year, month, day);
    return d;
}

export function addYears(date: Date | string, years: number): Date {
    const d = new Date(date);
    d.setFullYear(d.getFullYear() + years);
    return d;
}

export function getDaysInMonth(year: number, month: number): number {
    return new Date(year, month + 1, 0).getDate();
}

export function getFirstDayOfMonth(date: Date | string): Date {
    const d = new Date(date);
    return new Date(d.getFullYear(), d.getMonth(), 1);
}

export function getLastDayOfMonth(date: Date | string): Date {
    const d = new Date(date);
    return new Date(d.getFullYear(), d.getMonth() + 1, 0);
}

export function getFirstDayOfWeek(date: Date | string): Date {
    const d = new Date(date);
    const diff = d.getDay() === 0 ? 6 : d.getDay() - 1;
    return addDays(d, -diff);
}

export function getFirstDayOfYear(date: Date | string): Date {
    const d = new Date(date);
    return new Date(d.getFullYear(), 0, 1);
}

export function getLastDayOfYear(date: Date | string): Date {
    const d = new Date(date);
    return new Date(d.getFullYear(), 11, 31);
}

export function getWeekNumber(date: Date | string): number {
    const d = new Date(date);
    const firstDayOfYear = getFirstDayOfYear(d);
    const diff = Math.round((d.getTime() - firstDayOfYear.getTime()) / (7 * 24 * 60 * 60 * 1000));
    return Math.ceil(diff / 7);
}

export function getQuarter(date: Date | string): number {
    const d = new Date(date);
    return Math.floor((d.getMonth() + 3) / 3);
}

export function getISOWeek(date: Date | string): number {
    const d = new Date(date);
    const year = d.getFullYear();
    const firstDayOfYear = new Date(year, 0, 1);
    const dayOfWeek = firstDayOfYear.getDay();
    const diff = d.getTime() - firstDayOfYear.getTime();
    const daysSinceFirstMonday = Math.floor(diff / (24 * 60 * 60 * 1000)) + (dayOfWeek === 0 ? -6 : dayOfWeek - 1);
    return Math.floor(daysSinceFirstMonday / 7) + 1;
}

export function getISOWeeksInYear(date: Date | string): number {
    const d = new Date(date);
    const year = d.getFullYear();
    const firstDayOfYear = new Date(year, 0, 1);
    const dayOfWeek = firstDayOfYear.getDay();
    const diff = new Date(year + 1, 0, 1).getTime() - firstDayOfYear.getTime();
    const daysSinceFirstMonday = Math.floor(diff / (24 * 60 * 60 * 1000)) + (dayOfWeek === 0 ? -6 : dayOfWeek - 1);
    return Math.ceil(daysSinceFirstMonday / 7);
}

export function now(): Date {
    return new Date();
}

export function today(): Date {
    const d = new Date();
    d.setHours(0, 0, 0, 0);
    return d;
}

export function yesterday(): Date {
    const d = new Date();
    d.setDate(d.getDate() - 1);
    d.setHours(0, 0, 0, 0);
    return d;
}

export function tomorrow(): Date {
    const d = new Date();
    d.setDate(d.getDate() + 1);
    d.setHours(0, 0, 0, 0);
    return d;
}

export function thisWeek(): Date {
    const d = new Date();
    d.setDate(d.getDate() - d.getDay());
    d.setHours(0, 0, 0, 0);
    return d;
}

export function thisMonth(): Date {
    const d = new Date();
    d.setDate(1);
    d.setHours(0, 0, 0, 0);
    return d;
}

export function thisYear(): Date {
    const d = new Date();
    d.setMonth(0);
    d.setDate(1);
    d.setHours(0, 0, 0, 0);
    return d;
}

export function parse(date: string, format: string): Date | null {
    const parts = format.split(/(yyyy|MM?|dd?|HH?|mm?|ss?|S{1,6})/);
    const regex = new RegExp(parts.map(part => part.match(/\w+/) ? "(\\d{" + part.length + "})" : part).join(""));
    const matches = date.match(regex);
    if (!matches) return null;
    const values = matches.slice(1).map(value => parseInt(value, 10));
    const year = values.shift() || 0;
    const month = values.length > 1 ? values.shift() || 0 - 1 : 0;
    const day = values.length > 1 ? values.shift() : 1;
    const hours = values.length > 1 ? values.shift() : 0;
    const minutes = values.length > 1 ? values.shift() : 0;
    const seconds = values.length > 1 ? values.shift() : 0;
    const milliseconds = values.length > 1 ? values.shift() : 0;
    return new Date(year, month, day, hours, minutes, seconds, milliseconds);
}

export const calculateAge = (dateOfBirth: string | Date) => {
    if(isEmpty(dateOfBirth)) return 0;
    const birthDate = new Date(dateOfBirth);
    const ageDifMs = Date.now() - birthDate.getTime();
    const ageDate = new Date(ageDifMs);
    return Math.abs(ageDate.getUTCFullYear() - 1970);
}

export function parseTimeSpanToSeconds(timeSpan: string): number {
    if(isEmpty(timeSpan)) return 0;
    // 匹配 d.hh:mm:ss 或 hh:mm:ss 格式
    const timeSpanPattern = /^(?:(\d+)\.)?(\d{1,2}):(\d{2}):(\d{2})$/;
    const match = timeSpan.match(timeSpanPattern);

    if (!match) {
        throw new Error("Invalid TimeSpan format");
    }

    // 提取天、小时、分钟和秒的值
    const days = parseInt(match[1] || "0", 10);
    const hours = parseInt(match[2], 10);
    const minutes = parseInt(match[3], 10);
    const seconds = parseInt(match[4], 10);

    // 计算总秒数
    return days * 86400 + hours * 3600 + minutes * 60 + seconds;
}

export function convertSecondsToTimeSpan(seconds: number) {
    const days = Math.floor(seconds / (24 * 60 * 60));
    seconds %= (24 * 60 * 60);
    const hours = Math.floor(seconds / (60 * 60));
    seconds %= (60 * 60);
    const minutes = Math.floor(seconds / 60);
    seconds %= 60;

    return `${days}.${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
}
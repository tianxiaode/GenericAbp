import { isDate } from "./Date";
import { isInteger, isNumber } from "./Number";
import { isEmpty, isString } from "./String";

export type CustomValidator = (
    element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement,
    value: any
) => boolean;
export type RuleType = string | number | Date | true | CustomValidator;

export interface ValidateConfig {
    equalTo?: string;
    required?: true;
    number?: true;
    date?: true;
    minDate?: Date;
    maxDate?: Date;
    integer?: true;
    min?: number;
    max?: number;
    maxLength?: number;
    minLength?: number;
    length?: number;
    regex?: string;
    email?: true;
    requireDigit?: true;
    requireLowercase?: true;
    requireUppercase?: true;
    requireNonAlphanumeric?: true;
    url?: true;
    custom?: CustomValidator;
}

export const ValidateNames = ['equalTo','required', 'number', 'date','minDate','maxDate', 'integer','min','max','maxLength','minLength', 'length','regex', 'email','requireDigit','requireLowercase','requireUppercase','requireNonAlphanumeric', 'url', 'custom'] ;

export class Validator {
    static validate(
        ruleName: string,
        value: string | number,
        rule: RuleType,
        otherValue: string | number | null
    ): boolean {
        const validate = Validator.validates[ruleName];
        const isValid =
            ruleName === "equalTo"
                ? Validator.validates.equalTo(value, otherValue, rule)
                : validate(value, rule);
        return isValid;
    }

    static validates: Record<string, Function> = {
        equalTo(
            value: string | number,
            otherValue: string | number,
            _: RuleType
        ): boolean {
            return value === otherValue;
        },

        required(value: string | number): boolean {
            return !isEmpty(value);
        },

        number(value: string | number): boolean {
            return isNumber(value);
        },

        date(value: string | number): boolean {
            return isDate(value);
        },

        minDate(value: string | Date, rule: Date): boolean {
            const compareDate =
                typeof value === "string" ? new Date(value) : value;
            return compareDate >= rule;
        },

        maxDate(value: string | Date, rule: string | Date | true): boolean {
            const compareDate =
                typeof value === "string" ? new Date(value) : value;
            return compareDate <= rule;
        },

        integer(value: string | number): boolean {
            return isInteger(value);
        },

        min(value: string | number, rule: RuleType): boolean {
            const numValue = Number(value);
            const numRule = Number(rule);
            return numValue >= numRule;
        },

        max(value: string | number, rule: RuleType): boolean {
            const numValue = Number(value);
            const numRule = Number(rule);
            return numValue <= numRule;
        },

        minLength(value: string | number, rule: RuleType): boolean {
            if(isEmpty(value)) return false;
            const lengthRule = Number(rule);
            return value.toString().length >= lengthRule;
        },

        maxLength(value: string | number, rule: RuleType): boolean {
            if(isEmpty(value)) return false;
            const lengthRule = Number(rule);
            return value.toString().length <= lengthRule;
        },

        length(value: string | number, rule: RuleType): boolean {
            const lengthRule = Number(rule);
            return value.toString().length === lengthRule;
        },

        regex(value: string | number, rule: RuleType): boolean {
            const regex = new RegExp(rule.toString());
            return regex.test(value.toString());
        },

        email(value: string | number): boolean {
            if(isEmpty(value)) return true;
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(
                value.toString()
            );
        },

        requireDigit(value: string | number): boolean {
            return /[0-9]/.test(value.toString());
        },

        requireLowercase(value: string | number): boolean {
            return /[a-z]/.test(value.toString());
        },

        requireUppercase(value: string | number): boolean {
            return /[A-Z]/.test(value.toString());
        },

        requireNonAlphanumeric(value: string | number): boolean {
            return /[^a-zA-Z0-9]/.test(value.toString());
        },

        url(value: string | number): boolean {
            return /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/.test(
                value.toString()
            );
        },
    };
}

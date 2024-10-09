import { isDate } from "./Date";
import { isInteger, isNumber } from "./Number";
import { isEmpty } from "./String";

export type CustomValidator = (element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement, value: any) => ValidationResult;
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

export interface ValidationResult {
    isValid: boolean;
    message?: string;
    messageOptions?: any;
}

export class Validator {

    static validate(
        element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement,
        ruleName: string,
        value: string | number,
        rule: RuleType): ValidationResult {
        const validate = Validator.validates[ruleName];
        const isValid = ruleName === 'equalTo' ? validate(element, value, rule) : validate(value, rule);
        return isValid ? { isValid: true } : { isValid: false, ...ValidationMessage.get(element, ruleName, rule) };
    }

    static getEqualToElement(element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement, rule:RuleType): HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement | null {
        let parent = element.parentElement;
        while (parent && parent.tagName.toLowerCase() !== 'body') {
            const other = parent.querySelector(`input[name="${rule}"]`);
            if (other) return other as HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement;
            parent = parent.parentElement;
        }
        return null;
    }

    static validates: Record<string, Function> = {
        equalTo(
            element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement,
            value: string | number,
            rule:RuleType): boolean {
            const other = Validator.getEqualToElement(element, rule);
            if (!other) throw new Error(`Cannot find element with name ${rule}`);
            return value === other.value;
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
            const compareDate = typeof value === 'string' ? new Date(value) : value;
            return compareDate >= rule;
        },

        maxDate(value: string | Date, rule: string | Date | true): boolean {
            const compareDate = typeof value === 'string' ? new Date(value) : value;
            return compareDate <= rule;
        },

        integer(value: string | number): boolean {
            return isInteger(value);
        },

        min(value: string | number, rule:RuleType): boolean {
            const numValue = Number(value);
            const numRule = Number(rule);
            return numValue >= numRule;
        },

        max(value: string | number, rule:RuleType): boolean {
            const numValue = Number(value);
            const numRule = Number(rule);
            return numValue <= numRule;
        },

        minLength(value: string | number, rule:RuleType): boolean {
            const lengthRule = Number(rule);
            return value.toString().length >= lengthRule;
        },

        maxLength(value: string | number, rule:RuleType): boolean {
            const lengthRule = Number(rule);
            return value.toString().length <= lengthRule;
        },

        length(value: string | number, rule:RuleType): boolean {
            const lengthRule = Number(rule);
            return value.toString().length === lengthRule;
        },

        regex(value: string | number, rule:RuleType): boolean {
            const regex = new RegExp(rule.toString());
            return regex.test(value.toString());
        },

        email(value: string | number): boolean {
            return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(value.toString());
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
            return /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/.test(value.toString());
        }
    }
}

class ValidationMessage {

    static get(element: HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement, ruleName: string, rule:RuleType): Omit<ValidationResult, 'isValid'> {
        let result: Omit<ValidationResult, 'isValid'> = {
            message: `validation.${ruleName}`,
            messageOptions: { '{0}': rule.toString() }
        };
        if (ruleName === 'equalTo') {
            const other = Validator.getEqualToElement(element, rule);
            result.messageOptions = { '{0}': element.dataset.label ?? '', '{1}': other?.dataset.label ?? '' };
        }
        if (ruleName === 'url') {
            result.messageOptions = { '{0}': element.dataset.label ?? '' };
        }
        return result;
    }
}

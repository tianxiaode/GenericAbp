import isEmpty from "validator/es/lib/isEmpty"
import isLength from "validator/es/lib/isLength"
import isNumeric from "validator/es/lib/isNumeric"
import isInt from "validator/es/lib/isInt"
import isEmail from "validator/es/lib/isEmail"


const Validation: Record<string, Function> = {

    validate(name: string,value: string, rule: any): boolean {
        let validate;
        if (name.includes('length')) {
            validate = Validation.length;
        } else {
            validate = Validation[name];
        }
        return validate(value, rule);
    },

    equalto(value: any, rule: any): boolean {
        const other = document.getElementById(rule.other) as HTMLInputElement;
        const otherValue = other?.value;
        return value === otherValue;
    },

    required(value: any): boolean {
        return !isEmpty(value);
    },

    number(value: string): boolean {
        return isNumeric(value)
    },

    range(value: string, rule: any): boolean {
        return isInt(value, { min: rule.min, max: rule.max})
    },

    regex(value: string, rule: any): boolean {
        var regex = new RegExp(rule.pattern);
        return regex.test(value);
    },

    length(value: string, rule: any): boolean {
        return isLength(value, {min: rule.min, max: rule.max});
    },

    email(value: string, rule: any): boolean {
        return isEmail(value);
    },

    digit(value: string, rule: any): boolean {
       return (/[0-9]/gi).test(value);
    },

    lowercase(value: string, rule: any): boolean {
        return (/[a-z]/g).test(value);
    },

    uppercase(value: string, rule: any): boolean {
        return (/[A-Z]/g).test(value);
    },

    nonalphanumeric(value: string, rule: any): boolean {
        return (/[A-Z]/g).test(value);
    }    
}

export default Validation;
import isEmpty from "validator/es/lib/isEmpty"
import isLength from "validator/es/lib/isLength"
import isNumeric from "validator/es/lib/isNumeric"
import isInt from "validator/es/lib/isInt"


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

    equalto(value: any, otherValue: any): boolean {
        return value === otherValue;
    },

    required(value: any): boolean {
        return !isEmpty(value);
    },

    number(value: string): boolean {
        return isNumeric(value)
    },

    range(value: string, min: number, max: number): boolean {
        return isInt(value, { min: min, max: max})
    },

    regex(value: string, pattern: string): boolean {
        var regex = new RegExp(pattern);
        return regex.test(value);
    },

    length(value: string, min: number, max: number): boolean {
        return isLength(value, {min: undefined, max: max});
    }
}

export default Validation;
import { reactive } from "vue";
import {
    i18n,
    Validator,
    isEmpty,
    isFunction
} from "~/libs";

export function useFormRules(initialRules: any, formRef: any) {
    // 初始化 rules 为一个响应式对象
    const rules = reactive<any>({});

    // 定义验证函数
    const validateField = (fieldName: string, rule: any, value: any, callback: any) => {
        const errors = [];
        const fieldRules = rules[fieldName];

        for (const name in fieldRules) {
            let isValid = true;
            const otherField = name === "equalTo" ?  fieldRules['equalTo'] : "";
            if (Validator.validates.hasOwnProperty(name)) {
                isValid =
                    name === "custom"
                        ? fieldRules["custom"](fieldName, rule, value, callback, formRef.value)
                        : name === "equalTo"
                        ? value === getFieldValue(otherField)
                        : Validator.validate(
                              name,
                              value,
                              fieldRules[name],
                              formRef.value.fields
                          );
            }
            if (!isValid) {
                errors.push(
                    i18n.get(`Validation.${name}`, {
                        "0": name === "equalTo" ? getFieldLabel(fieldName) : fieldRules[name],
                        "1": name === "equalTo" ? getFieldLabel(otherField) : fieldRules[name],
                    })
                );
            }
        }
        if (errors.length > 0) {
            callback(new Error(errors.join("")));
        } else {
            callback(); // 验证通过时调用callback
        }
    };

    // 将初始规则赋值给 rules
    const setupInitialRules = () => {
        for (const fieldName in initialRules) {
            rules[fieldName] = { ...initialRules[fieldName] }; // 初始化规则
            rules[fieldName].validator = (rule: any, value: any, callback: any) => {
                if (rules[fieldName].validator.timer) {
                    clearTimeout(rules[fieldName].validator.timer);
                }
                rules[fieldName].validator.timer = setTimeout(() => {
                    rules[fieldName].validator.timer = null;
                    validateField(fieldName, rule, value, callback);
                }, 500);
            };
        }
    };

    // 允许外部更新动态规则
    const updateRules = (fieldName: string, newRules: any) => {
        if (rules[fieldName]) {
            rules[fieldName] = { ...rules[fieldName], ...newRules }; // 更新特定字段的动态规则
        }
    };

    // 执行初始规则设置
    setupInitialRules();

    const getField = (field: string) => {

        let fields = formRef.value.fields;        
        if(isFunction(fields)){
            fields = fields(fields);
        }
        console.log(fields);
        return fields.find((f: any) => f.prop === field);
    };

    const getFieldValue = (fieldName: string) => {
        const field = getField(fieldName);
        return field && field.fieldValue;
    };

    const getFieldLabel = (fieldName: string) => {
        if (isEmpty(fieldName)) return "";
        const field = getField(fieldName);
        const el = field.$el;
        const inputEl = el.querySelector("input, textarea, select");
        return field.label || inputEl.getAttribute("placeholder") || fieldName;
    };

    return {
        rules,
        updateRules, // 提供更新动态规则的方法
    };
}

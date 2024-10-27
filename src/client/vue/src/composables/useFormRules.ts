import { reactive } from "vue";
import { i18n, Validator, isEmpty, isFunction } from "~/libs";

export function useFormRules(initialRules: any, formRef: any) {
    // 初始化 rules 为一个响应式对象
    const rules = reactive<any>({});

    // 定义验证函数
    const validateField = (
        fieldName: string,
        rule: any,
        value: any,
        callback: any
    ) => {
        if (isEmpty(formRef.value)) return;

        const errors = [];
        const fieldRules = rules[fieldName];

        for (const name in fieldRules) {
            let isValid = true;
            const otherField = name === "equalTo" ? fieldRules["equalTo"] : "";
            if (fieldRules[name] === false) continue;
            if (name === "custom") {
                isValid = fieldRules[name](
                    value,
                    fieldName,
                    rule,
                    formRef.value
                );
                if (isValid !== true) {
                    errors.push(isValid);
                    continue;
                }
            } else if (name === "equalTo") {
                isValid = value === getFieldValue(otherField);
            } else if (Validator.validates.hasOwnProperty(name)) {
                if(isEmpty(value) && fieldRules['required'] === false) continue;
                isValid = Validator.validate(
                    name,
                    value,
                    fieldRules[name],
                    getFields() as any
                );
            }
            if (!isValid) {
                errors.push(
                    i18n.get(`Validation.${name}`, {
                        "0":
                            name === "equalTo"
                                ? getFieldLabel(fieldName)
                                : fieldRules[name],
                        "1":
                            name === "equalTo"
                                ? getFieldLabel(otherField)
                                : fieldRules[name],
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
            rules[fieldName].validator = (
                rule: any,
                value: any,
                callback: any
            ) => {
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
    const updateRules = (...args: any[]) => {
        let newRules = args[0];
        if (typeof newRules === "string") {
            newRules = { [args[0]]: args[1] };
        }
        for (const fieldName in newRules) {
            rules[fieldName] = { ...rules[fieldName], ...newRules[fieldName] };
        }
    };

    // 执行初始规则设置
    setupInitialRules();

    const getFields = () => {
        let fields = formRef.value.fields;
        if (isFunction(fields)) {
            fields = fields(fields);
        }
        return fields;
    };

    const getField = (field: string) => {
        const fields = getFields() as any;
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

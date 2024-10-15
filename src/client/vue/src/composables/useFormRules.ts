import {
    i18n,
    Validator,
    ValidateNames,
    isEmpty
} from "~/libs";

export function useFormRules(formRules: any, formRef: any) {
    const rules = {} as any;

    const getField = (field: string) => {
        return formRef.value.fields().find((f: any) => f.prop === field);
    };

    const getFieldLabel = (fieldName: string) => {
        if (isEmpty(fieldName)) return "";
        const field = getField(fieldName);
        const el = field.$el;
        const inputEl = el.querySelector("input, textarea, select");
        return field.label || inputEl.getAttribute("placeholder") || fieldName;
    };

    //为formRules中的每一个字段添加validator方法
    for (const fieldName in formRules) {
        rules[fieldName] = {
            ...formRules[fieldName],
            validator: (rule: any, value: any, callback: any) => {
                //创建一个缓冲时间，防止短时间内多次触发validator
                if (rules[fieldName].validator.timer) {
                    clearTimeout(rules[fieldName].validator.timer);
                }
                rules[fieldName].validator.timer = setTimeout(() => {
                    rules[fieldName].validator.timer = null;
                    validateField(fieldName, rule, value, callback);
                }, 500);

                const validateField = (
                    fieldName: string,
                    rule: any,
                    value: any,
                    callback: any
                ) => {
                    const errors = [];
                    for (const name in rule) {
                        let isValid = true;
                        const otherField = name === "equalTo" ? rule[name] : "";
                        if (ValidateNames.includes(name)) {
                            isValid =
                                name === "custom"
                                    ? rule["custom"](value, formRef.value.model)
                                    : name === "equalTo"
                                    ? value === otherField.value
                                    : Validator.validate(
                                          name,
                                          value,
                                          rule[name],
                                          formRef.value.fields
                                      );
                        }
                        if (!isValid) {
                            errors.push(
                                i18n.get(`Validation.${name}`, {
                                    "0":
                                        name === "equalTo"
                                            ? getFieldLabel(fieldName)
                                            : rule[name],
                                    "1":
                                        name === "equalTo"
                                            ? getFieldLabel(otherField)
                                            : rule[name],
                                })
                            );
                        }
                    }
                    if (errors.length > 0) {
                        callback(new Error(errors.join("")));
                    }
                };
            },
        };
    }

    return {
        rules,
    };
}

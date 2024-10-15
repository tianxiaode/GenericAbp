import { FormInstance } from 'element-plus';
import { ref } from 'vue';

export type FormExposeType = {
    form: FormInstance;
    validate: (callback: (valid: boolean) => void) => void;
    resetFields: () => void;
    clearValidate: () => void;
    fields: () => any;
    scrollToField: (field: string) => void;
    validateField: (field: string, callback: (valid: boolean) => void) => void;
    setMessage: (msg: string, type: string) => void;
    clearMessage: () => void;
    isValid: () => Promise<boolean>;
};

export function useFormExpose() {
    const formRef = ref<FormInstance>();

    // 暴露给父组件的功能
    const formExpose = () => {
        return {
            form: formRef,
            validate: (callback: (valid: boolean) => void) => formRef.value?.validate(callback),
            resetFields: () => formRef.value?.resetFields(),
            clearValidate: () => formRef.value?.clearValidate(),
            fields : () => formRef.value?.fields,
            scrollToField: (field: string) => formRef.value?.scrollToField(field),
            validateField: (field: string, callback: (valid: boolean) => void) => formRef.value?.validateField(field, callback),
            isValid: async() =>{
                const form = formRef.value;
                const promise = new Promise<boolean>((resolve, _) => {
                    if(!form) return resolve(false);

                    form.validate((valid: boolean) => {
                        resolve(valid);
                    });
                });
                return promise;
            },
        };
    };


    return {
        formRef,
        formExpose
    };
}

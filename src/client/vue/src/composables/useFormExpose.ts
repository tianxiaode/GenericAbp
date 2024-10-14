import { FormInstance } from 'element-plus';
import { ref } from 'vue';

export function useFormExpose() {
    const formRef = ref<FormInstance>();

    // 暴露给父组件的功能
    const exposeForm = () => {
        return {
            formRef,
            validate: (callback: (valid: boolean) => void) => formRef.value?.validate(callback),
            resetFields: () => formRef.value?.resetFields(),
            clearValidate: () => formRef.value?.clearValidate(),
            fields : () => formRef.value?.fields,
            scrollToField: (field: string) => formRef.value?.scrollToField(field),
            validateField: (field: string, callback: (valid: boolean) => void) => formRef.value?.validateField(field, callback),
        };
    };

    return {
        formRef,
        exposeForm,
    };
}

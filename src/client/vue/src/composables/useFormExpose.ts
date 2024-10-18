import { FormInstance } from 'element-plus';
import { ref } from 'vue';
import { clone } from '~/libs';

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

export function useFormExpose(parentFormData: any) {
    const formRef = ref<FormInstance>();
    const formData = ref<any>(parentFormData);
    const initValues = ref<any>({});


    const setInitValues = (data: any) => {
        formData.value = { ...formData.value, ...clone(data) };
        initValues.value = { ...initValues.value,  ...clone(data) };
    };

    const hasChange = () => {
        return JSON.stringify(formData.value) !== JSON.stringify(initValues.value);
    };    

    const resetForm = () => {
        formRef.value?.resetFields();
        formData.value = {...initValues.value };
    };

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
            setInitValues,
            hasChange
        };
    };


    return {
        formRef,
        formData,
        setInitValues,
        hasChange,
        resetForm,
        formExpose
    };
}

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
    submit:()=>void;
};

export function useFormExpose(handleSubmit: Function) {
    const formRef = ref<FormInstance>();
    const message = ref('');
    const messageType = ref('');

    // 暴露给父组件的功能
    const exposeForm = () => {
        return {
            form: formRef,
            validate: (callback: (valid: boolean) => void) => formRef.value?.validate(callback),
            resetFields: () => formRef.value?.resetFields(),
            clearValidate: () => formRef.value?.clearValidate(),
            fields : () => formRef.value?.fields,
            scrollToField: (field: string) => formRef.value?.scrollToField(field),
            validateField: (field: string, callback: (valid: boolean) => void) => formRef.value?.validateField(field, callback),
            setMessage: (msg: string, type: string) => {
                message.value = msg;
                messageType.value = type;
            },
            clearMessage: () => clearMessage(),
            submit:async ()=>{
                if(!formRef.value) return;
                formRef.value.validate(async (valid) => {
                    if(!valid) return;
                    await handleSubmit();
                });                
            }
        };
    };

    const clearMessage = () => {
        message.value = '';
        messageType.value = '';
    };

    return {
        formRef,
        message,
        messageType,
        clearMessage,
        exposeForm,
    };
}

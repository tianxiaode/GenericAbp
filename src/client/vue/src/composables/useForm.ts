// useForm.ts
import {  ref, reactive } from "vue";

export function useForm<T>(onSubmit: Function) {
    const formRef = ref<any>(null as any);
    const formData = reactive<any>({});
    const handleSubmit = async () => {
        const form = formRef.value;
        if (!form) return;
        if (!(await form.isValid())) return;
        await onSubmit();
    };

    return {
        formRef,
        formData,
        handleSubmit,
    };
}

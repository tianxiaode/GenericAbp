// useForm.ts
import {  ref, reactive } from "vue";

export function useForm(onSubmit: Function) {
    const formRef = ref<any>(null as any);
    const formData = reactive<any>({});
    const handleSubmit = async () => {
        const form = formRef.value;
        if (!form) return;
        
        form.validate((valid:boolean)=>{
            if(!valid) return;
            return onSubmit();
        })
        
    };    

    return {
        formRef,
        formData,
        handleSubmit,
    };
}

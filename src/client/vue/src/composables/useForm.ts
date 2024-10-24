// useForm.ts
import {  ref } from "vue";

export function useForm(onSubmit: Function) {
    const formRef = ref<any>(null as any);
    const formData = ref<any>({});
    const handleSubmit = async () => {
        const form = formRef.value;
        if (!form) return;
        
        form.validate((valid:boolean)=>{
            console.log('valid', valid)
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

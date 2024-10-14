import { FormInstance } from "element-plus";
import { ref } from "vue";

export function useAccountForm<T>(submitFn: Function) {
    const formRef = ref<any>();
    const formData = ref<T>({} as T);
    

    const handleSubmit = async (event: Event) => {
        event.preventDefault();
        const form = formRef.value as any;
        if (!form) return;
        form.validate((valid:boolean)=>{
            if(!valid) return;
            submitFn();
        });
    };


    return {
        formRef,
        formData,
        handleSubmit
    };
}

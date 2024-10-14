import { FormInstance } from "element-plus";
import { ref } from "vue";

export function useAccountForm<T>(submitFn: Function) {
    const formRef = ref<FormInstance>();
    const formData = ref<T>({} as T);
    const message = ref('');
    const alertType = ref('');
    

    const handleSubmit = async (event: Event) => {
        event.preventDefault();
        const form = formRef.value;
        if (!form) return;
        form.validate((valid:boolean)=>{
            console.log(valid);
            if(!valid) return;
            submitFn();
        });
    };

    const clearMessage = () => {
        message.value = '';
        alertType.value = '';
    };

    return {
        formRef,
        formData,
        message,
        alertType,
        clearMessage,
        handleSubmit
    };
}

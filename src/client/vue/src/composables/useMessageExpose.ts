import { ref } from "vue";

export type FormMessageTypeType = "success" | "error" | "info" | "warning" | "";

export function useFormMessageExpose() {

    const message = ref("");
    const formMessageType = ref<FormMessageTypeType>("");
    const formMessageParams = ref<any>();

    const setFormMessage = (
        message: string,
        messageType: FormMessageTypeType,
        params?: any,
    ) => {
        message.value = message;
        formMessageType.value = messageType;
        if(params){
            formMessageParams.value = params;
        }        
    };

    const success = (message: string, params?:any) => {
        setFormMessage(message, "success", params);
    };

    const error = (message: string, params?:any) => {
        setFormMessage(message, "error", params);
    };

    const info = (message: string, params?:any) => {
        setFormMessage(message, "info", params);
    };

    const warning = (message: string, params?:any) => {
        setFormMessage(message, "warning", params);
    };

    const formMessageExpose = ()=> {
        return {
            setFormMessage,
            success,
            error,
            info,
            warning
        }
    };

    return {
        formMessage: message,
        formMessageType,
        formMessageParams,
        formMessageExpose,
        setFormMessage,
        success,
        error,
        info,
        warning
    };
}

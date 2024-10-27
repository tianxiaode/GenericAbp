import { provide, ref } from "vue";

export type FormMessageTypeType = "success" | "error" | "info" | "warning" | "";

export function useFormMessageExpose() {

    const formMessage = ref("");
    const formMessageType = ref<FormMessageTypeType>("");

    const setFormMessage = (
        message: string,
        messageType: FormMessageTypeType
    ) => {
        formMessage.value = message;
        formMessageType.value = messageType;
    };

    const success = (message: string) => {
        setFormMessage(message, "success");
    };

    const error = (message: string) => {
        setFormMessage(message, "error");
    };

    const info = (message: string) => {
        setFormMessage(message, "info");
    };

    const warning = (message: string) => {
        setFormMessage(message, "warning");
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
        formMessage,
        formMessageType,
        formMessageExpose,
        setFormMessage,
        success,
        error,
        info,
        warning
    };
}

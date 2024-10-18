import { ref } from "vue";

export type FormMessageTypeType = "success" | "error" | "info" | "warning" | "";

export function useFormMessage() {
    const formMessage = ref("");
    const formMessageType = ref<FormMessageTypeType>("");

    const clearFormMessage = () => {
        formMessage.value = "";
        formMessageType.value = "";
    };

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

    const formMessageExposed = () => {
        return {
            success,
            error,
            info,
            warning,
            clearFormMessage,
        };
    };

    return {
        formMessage,
        formMessageType,
        setFormMessage,
        success,
        error,
        info,
        warning,
        clearFormMessage,
        formMessageExposed,
    };
}

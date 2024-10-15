import { ref } from "vue";

export type FormMessageTypeType = "success" | "error" | "info" | "warning" | "";

export function useFormMessage() {
    const formMessage = ref("");
    const formMessageType = ref<FormMessageTypeType>("");

    const clearFormMessage = () => {
        formMessage.value = "";
        formMessageType.value = "";
    };

    const setFormMessage = (message: string, messageType: FormMessageTypeType) => {
        formMessage.value = message;
        formMessageType.value = messageType;
    };

    const formMessageExposed = () => {
        return {
            formMessage,
            formMessageType,
            success(message: string){
                setFormMessage(message, "success");
            },
            error(message: string){
                setFormMessage(message, "error");
            },
            info(message: string){
                setFormMessage(message, "info");
            },
            warning(message: string){
                setFormMessage(message, "warning");
            },
            setFormMessage(message: string, messageType: FormMessageTypeType) {
                setFormMessage(message, messageType);
            },
            clearFormMessage() {
                clearFormMessage();
            },
        };
    };

    return {
        formMessage,
        formMessageType,
        clearFormMessage,
        formMessageExposed,
    };
}

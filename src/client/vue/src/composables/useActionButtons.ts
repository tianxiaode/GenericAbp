import { ref } from "vue";
import { deepMerge } from "~/libs";

export type ActionButtonType = {
    type: string;
    icon: string;
    order: number;
    action?: Function;
    title?: string;
    isVisible?: Function | boolean;
    isDisabled?: Function | boolean;
};

export function useActionButtons(
    defaultButtons: Record<string, ActionButtonType>,
    buttons: Record<string, ActionButtonType>
) {
    const buttonsList = ref<Record<string, ActionButtonType>>(deepMerge(defaultButtons, buttons));

    const handleButtonClick = (button: ActionButtonType, row?: any) => {
        if (button.action) {
            button.action(row);
        }
    };

    const handleButtonVisibility = (button: ActionButtonType, row?: any) => {
        if (typeof button.isVisible === "function") {
            return button.isVisible(row);
        }
        return button.isVisible || true;
    };

    const handleButtonDisabled = (button: ActionButtonType, row?: any) => {
        if (typeof button.isDisabled === "function") {
            return button.isDisabled(row);
        }
        return button.isDisabled || false;
    };

    return {
        buttonsList,
        handleButtonClick,
        handleButtonVisibility,
        handleButtonDisabled,
    };
}

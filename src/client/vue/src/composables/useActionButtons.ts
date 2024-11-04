import { ref } from "vue";
import { deepMerge } from "~/libs";

export type ActionButtonType = {
    type: string;
    icon: string;
    order: number;
    action?: Function;
    tooltip?: string;
    visible?: Function | boolean;
    disabled?: Function | boolean;
};

export function useActionButtons(
    defaultButtons: Record<string, ActionButtonType>,
    buttons: Record<string, ActionButtonType>
) {
    const buttonsList = ref<Record<string, ActionButtonType>>(
        deepMerge(defaultButtons, buttons)
    );

    const handleButtonClick = (button: ActionButtonType, row?: any) => {
        if (button.action) {
            button.action(row);
        }
    };

    const handleButtonVisibility = (button: ActionButtonType, row?: any) => {
        if (typeof button.visible === "function") {
            return button.visible(row);
        }
        return button.visible === false ? false : true;
    };

    const handleButtonDisabled = (button: ActionButtonType, row?: any) => {
        if (typeof button.disabled === "function") {
            return button.disabled(row);
        }
        return button.disabled === true ? true : false;
    };

    return {
        buttonsList,
        handleButtonClick,
        handleButtonVisibility,
        handleButtonDisabled,
    };
}

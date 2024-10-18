import { ref } from "vue";
import { isFunction } from "~/libs";

export const dialogProps = () => {
    return {
        visible: Boolean,
        dialog: Object,
        cancelButtonText: {
            type: String,
            default: "Components.Cancel",
        },
        okButtonText: {
            type: String,
            default: "Components.Save",
        },
        onCancel: Function,
        onOk: Function,
        beforeClose: Function,
        close: Function,
        reset: Function,
    };
};

export function useDialogExpose(props: any, beforeClose?: any, afterClose?: any) {
    const dialogRef = ref(null);
    const dialogVisible = ref<any>(props.visible);

    const beforeDialogClose = async (done: any) => {
        console.log("before dialog close");
        if (beforeClose && !await beforeClose() ) {
            console.log("before dialog close", "before close return false");
            return false;
        }
        if (props.onBeforeClose && await props.onBeforeClose() === false) {
            return false;
        }
        console.log("before dialog close", "done");
        isFunction(done) ? done() : dialogClose();
    };

    const dialogClose = () => {
        // do something after dialog close
        console.log("dialog close");
        if (props.onClosed) {
            props.onClosed();
        }
        dialogVisible.value = false;
        afterClose && afterClose();
    };

    const dialogExpose = () => {
        return {
            dialogRef,
            beforeDialogClose,
            dialogClose,
        };
    };

    return {
        dialogRef,
        dialogVisible,
        beforeDialogClose,
        dialogClose,
        dialogExpose,
    };
}

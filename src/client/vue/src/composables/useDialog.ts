
export const dialogProps = () => {
    return {
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

export function useDialog(
    props: any,
    emit: any,
    afterClose?: any,
) {


    const dialogBeforeClose = () => {
        if(props.beforeClose){
            props.beforeClose().then(async (allowClose: boolean) => {
                if(!allowClose) return;
                close();
            });
        }else{
            close();
        }
    };

    const close = () => {
        if (props.close) {
            props.close();
        }
        emit("close", true);
        afterClose && afterClose();
    };

    const handleReset = () => {
        if (props.reset) {
            props.reset();
        }
    };


    return {
        dialogBeforeClose,
        handleReset,
        close
    };
}
